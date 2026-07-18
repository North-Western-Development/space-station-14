using Content.Server.Maps;
using Content.Server.Station.Systems;
using Content.Shared._Sol.Medical.Virology.Components;
using Content.Shared.Maps;
using Content.Shared.Roles;
using Robust.Shared.GameObjects;
using Robust.Shared.Prototypes;

namespace Content.IntegrationTests.Tests._Sol.Medical.Virology;

[TestFixture]
public sealed class VirologyStationStaffingTest
{
    private static readonly ProtoId<GameMapPrototype> CorkMap = "SolCork";
    private static readonly ProtoId<GameMapPrototype> SalternMap = "SolSaltern";
    private static readonly ProtoId<GameMapPoolPrototype> VirologyPool = "VirologyMapPool";
    private static readonly ProtoId<JobPrototype> VirologistJob = "Virologist";

    [Test]
    public async Task CorkAndSalternHaveConfiguredVirologistSlots()
    {
        await using var pair = await PoolManager.GetServerClient();
        var server = pair.Server;
        var proto = server.ResolveDependency<IPrototypeManager>();
        var factory = server.ResolveDependency<IComponentFactory>();
        var stationSystem = server.System<StationSystem>();
        var stationJobs = server.System<StationJobsSystem>();

        await server.WaitAssertion(() =>
        {
            var viroName = factory.GetComponentName<VirologyStationComponent>();

            AssertStation(proto, stationSystem, stationJobs, CorkMap, viroName, expectedSlots: 1);
            AssertStation(proto, stationSystem, stationJobs, SalternMap, viroName, expectedSlots: 3);

            Assert.That(proto.TryIndex(VirologyPool, out var pool), Is.True);
            Assert.That(pool!.Maps, Does.Contain(CorkMap.Id));
            Assert.That(pool.Maps, Does.Contain(SalternMap.Id));
        });

        await pair.CleanReturnAsync();
    }

    private static void AssertStation(
        IPrototypeManager proto,
        StationSystem stationSystem,
        StationJobsSystem stationJobs,
        ProtoId<GameMapPrototype> mapId,
        string viroName,
        int expectedSlots)
    {
        Assert.That(proto.TryIndex(mapId, out var map), Is.True, mapId.Id);
        Assert.That(map!.Stations, Is.Not.Empty, mapId.Id);

        var found = false;
        foreach (var (stationId, stationConfig) in map.Stations)
        {
            Assert.That(stationConfig.StationComponentOverrides.TryGetComponent(viroName, out _),
                Is.True,
                $"{mapId.Id}/{stationId} missing VirologyStation");

            var station = stationSystem.InitializeNewStation(stationConfig, null, $"{mapId.Id}-{stationId}");
            var jobs = stationJobs.GetRoundStartJobs(station);
            Assert.That(jobs.TryGetValue(VirologistJob, out var slots), Is.True, $"{mapId.Id}/{stationId} missing Virologist");
            Assert.That(slots, Is.EqualTo(expectedSlots), $"{mapId.Id} round-start slots");
            found = true;
        }

        Assert.That(found, Is.True, mapId.Id);
    }
}
