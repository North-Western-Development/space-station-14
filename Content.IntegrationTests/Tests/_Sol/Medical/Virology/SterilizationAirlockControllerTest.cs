using System.Collections.Generic;
using Content.Server._Sol.Medical.Virology;
using Content.Server.DeviceNetwork;
using Content.Server.Power.Components;
using Content.Shared._Sol.Medical.Virology;
using Content.Shared._Sol.Medical.Virology.Components;
using Content.Shared.DeviceLinking;
using Content.Shared.DeviceLinking.Events;
using Content.Shared.DeviceNetwork;
using Content.Shared.Doors;
using Content.Shared.Doors.Components;
using Content.Shared.Doors.Systems;
using Robust.Shared.GameObjects;
using Robust.Shared.Map;
using Robust.Shared.Maths;
using Robust.Shared.Timing;

namespace Content.IntegrationTests.Tests._Sol.Medical.Virology;

[TestFixture]
[TestOf(typeof(SterilizationAirlockSystem))]
public sealed class SterilizationAirlockControllerTest
{
    [TestPrototypes]
    private const string Prototypes = @"
- type: entity
  id: SolSterilizerTestDoor
  parent: Airlock
  components:
  - type: ApcPowerReceiver
    needsPower: false

- type: entity
  id: SolSterilizerTestController
  parent: SolSterilizationAirlockController
  components:
  - type: ApcPowerReceiver
    needsPower: false
  - type: SterilizationAirlockController
    fogDuration: 0.2
    fadeDuration: 0.1
    closingTimeout: 2
";

    [Test]
    public async Task CycleSterilizesChamberAndOpensExit()
    {
        await using var pair = await PoolManager.GetServerClient();
        var server = pair.Server;
        var testMap = await pair.CreateTestMap();
        var entMan = server.ResolveDependency<IEntityManager>();
        var timing = server.ResolveDependency<IGameTiming>();
        var map = entMan.System<SharedMapSystem>();
        var doors = entMan.System<SharedDoorSystem>();
        var sterilizer = entMan.System<SterilizationAirlockSystem>();
        var gridPathogen = entMan.System<GridPathogenAtmosphereSystem>();

        await server.WaitAssertion(() =>
        {
            var gridUid = testMap.Grid.Owner;
            var gridComp = testMap.Grid.Comp;
            var tiles = new List<(Vector2i Index, Tile Tile)>
            {
                new(new Vector2i(0, 0), new Tile(1)),
                new(new Vector2i(1, 0), new Tile(1)),
                new(new Vector2i(2, 0), new Tile(1)),
            };
            map.SetTiles(gridUid, gridComp, tiles);

            var doorA = entMan.SpawnEntity("SolSterilizerTestDoor", new EntityCoordinates(gridUid, 0.5f, 0.5f));
            var controller = entMan.SpawnEntity("SolSterilizerTestController", new EntityCoordinates(gridUid, 1.5f, 0.5f));
            var doorB = entMan.SpawnEntity("SolSterilizerTestDoor", new EntityCoordinates(gridUid, 2.5f, 0.5f));
            var tool = entMan.SpawnEntity("Scalpel", new EntityCoordinates(gridUid, 1.5f, 0.5f));

            doors.SetState(doorA, DoorState.Closed);
            doors.SetState(doorB, DoorState.Closed);

            var controllerComp = entMan.GetComponent<SterilizationAirlockControllerComponent>(controller);
            controllerComp.DoorA = doorA;
            controllerComp.DoorB = doorB;
            controllerComp.EntranceDoor = doorA;
            controllerComp.ExitDoor = doorB;
            controllerComp.RequiresPower = false;

            var sterility = entMan.EnsureComponent<SurgicalToolSterilityComponent>(tool);
            sterility.State = SurgicalSterilityState.Dirty;
            sterility.Contaminants.Add(new PathogenContaminationEntry
            {
                PathogenId = "SolPathogenFlu",
                Load = 3f,
            });
            var surface = entMan.EnsureComponent<SurfaceContaminationComponent>(tool);
            surface.IsDirty = true;
            surface.Contaminants.Add(new PathogenContaminationEntry
            {
                PathogenId = "SolPathogenFlu",
                Load = 3f,
            });

            var midTile = new Vector2i(1, 0);
            gridPathogen.AddAirborneLoad(controller, "SolPathogenFlu", 5f);
            Assert.That(gridPathogen.GetAirborneLoad(gridUid, midTile), Is.GreaterThan(0f));

            Assert.That(sterilizer.TryBeginCycle((controller, controllerComp)), Is.True);
            Assert.That(controllerComp.Phase, Is.EqualTo(SterilizationControllerPhase.Closing));
            Assert.That(entMan.HasComponent<SterilizationDoorLockComponent>(doorA), Is.True);
            Assert.That(entMan.HasComponent<SterilizationDoorLockComponent>(doorB), Is.True);

            controllerComp.Phase = SterilizationControllerPhase.Fading;
            controllerComp.PhaseEndsAt = timing.CurTime;
            sterilizer.Update(0.1f);

            doors.SetState(doorB, DoorState.Open);
            controllerComp = entMan.GetComponent<SterilizationAirlockControllerComponent>(controller);
            if (controllerComp.Phase == SterilizationControllerPhase.OpeningExit)
            {
                controllerComp.PhaseEndsAt = timing.CurTime;
                sterilizer.Update(0.1f);
            }

            controllerComp = entMan.GetComponent<SterilizationAirlockControllerComponent>(controller);
            Assert.That(controllerComp.Phase, Is.EqualTo(SterilizationControllerPhase.Idle));
            Assert.That(gridPathogen.GetAirborneLoad(gridUid, midTile), Is.EqualTo(0f));
            Assert.That(entMan.GetComponent<SurgicalToolSterilityComponent>(tool).State,
                Is.EqualTo(SurgicalSterilityState.Sterile));
            Assert.That(entMan.GetComponent<SurfaceContaminationComponent>(tool).Contaminants, Is.Empty);
            Assert.That(entMan.HasComponent<SterilizationDoorLockComponent>(doorA), Is.False);
            Assert.That(entMan.HasComponent<SterilizationDoorLockComponent>(doorB), Is.False);
        });

        await pair.CleanReturnAsync();
    }

    [Test]
    public async Task RejectsOpenDuringCycleAndAbortsWithoutPower()
    {
        await using var pair = await PoolManager.GetServerClient();
        var server = pair.Server;
        var testMap = await pair.CreateTestMap();
        var entMan = server.ResolveDependency<IEntityManager>();
        var map = entMan.System<SharedMapSystem>();
        var sterilizer = entMan.System<SterilizationAirlockSystem>();

        await server.WaitAssertion(() =>
        {
            var gridUid = testMap.Grid.Owner;
            var gridComp = testMap.Grid.Comp;
            var tiles = new List<(Vector2i Index, Tile Tile)>
            {
                new(new Vector2i(0, 0), new Tile(1)),
                new(new Vector2i(1, 0), new Tile(1)),
                new(new Vector2i(2, 0), new Tile(1)),
            };
            map.SetTiles(gridUid, gridComp, tiles);

            var doorA = entMan.SpawnEntity("SolSterilizerTestDoor", new EntityCoordinates(gridUid, 0.5f, 0.5f));
            var controller = entMan.SpawnEntity("SolSterilizerTestController", new EntityCoordinates(gridUid, 1.5f, 0.5f));
            var doorB = entMan.SpawnEntity("SolSterilizerTestDoor", new EntityCoordinates(gridUid, 2.5f, 0.5f));

            var controllerComp = entMan.GetComponent<SterilizationAirlockControllerComponent>(controller);
            controllerComp.DoorA = doorA;
            controllerComp.DoorB = doorB;
            controllerComp.EntranceDoor = doorA;
            controllerComp.RequiresPower = false;
            entMan.GetComponent<ApcPowerReceiverComponent>(doorA).Powered = true;

            var quarantineOn = new NetworkPayload
            {
                [DeviceNetworkConstants.LogicState] = SignalState.High,
            };
            var lockSignal = new SignalReceivedEvent(
                SterilizationAirlockSystem.QuarantineLockPort,
                Data: quarantineOn);
            entMan.EventBus.RaiseLocalEvent(controller, ref lockSignal);
            Assert.That(controllerComp.QuarantineLocked, Is.True);
            Assert.That(entMan.GetComponent<DoorBoltComponent>(doorA).BoltsDown, Is.True);

            var quarantineOff = new NetworkPayload
            {
                [DeviceNetworkConstants.LogicState] = SignalState.Low,
            };
            var unlockSignal = new SignalReceivedEvent(
                SterilizationAirlockSystem.QuarantineLockPort,
                Data: quarantineOff);
            entMan.EventBus.RaiseLocalEvent(controller, ref unlockSignal);
            Assert.That(controllerComp.QuarantineLocked, Is.False);
            Assert.That(entMan.GetComponent<DoorBoltComponent>(doorA).BoltsDown, Is.False);

            Assert.That(sterilizer.TryBeginCycle((controller, controllerComp)), Is.True);

            Assert.That(entMan.HasComponent<SterilizationDoorLockComponent>(doorA), Is.True);
            var before = new BeforeDoorOpenedEvent();
            entMan.EventBus.RaiseLocalEvent(doorA, before);
            Assert.That(before.Cancelled, Is.True);

            controllerComp.RequiresPower = true;
            Assert.That(entMan.TryGetComponent(controller, out ApcPowerReceiverComponent power), Is.True);
            power!.Powered = false;

            sterilizer.Update(0.1f);
            Assert.That(controllerComp.Phase, Is.EqualTo(SterilizationControllerPhase.Idle));
        });

        await pair.CleanReturnAsync();
    }
}
