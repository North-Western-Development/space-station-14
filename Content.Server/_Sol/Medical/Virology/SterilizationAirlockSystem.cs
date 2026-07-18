using Content.Shared._Sol.Medical.Virology.Components;
using Content.Shared.Doors;
using Content.Shared.Doors.Components;
using Content.Shared.Popups;
using Content.Shared.Power.EntitySystems;
using Robust.Shared.Map.Components;
using Robust.Shared.Timing;

namespace Content.Server._Sol.Medical.Virology;

/// <summary>
/// Timed sterilization cycle for dedicated virology sterilization airlocks.
/// Incomplete / interrupted cycles do not sterilize.
/// </summary>
public sealed class SterilizationAirlockSystem : EntitySystem
{
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly SharedPowerReceiverSystem _power = default!;
    [Dependency] private readonly GridPathogenAtmosphereSystem _gridPathogen = default!;
    [Dependency] private readonly SurgicalAsepsisSystem _asepsis = default!;
    [Dependency] private readonly SharedMapSystem _map = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<SterilizationAirlockComponent, DoorStateChangedEvent>(OnDoorStateChanged);
        SubscribeLocalEvent<SterilizationAirlockComponent, ComponentStartup>(OnStartup);
    }

    public override void Update(float frameTime)
    {
        var query = EntityQueryEnumerator<SterilizationAirlockComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out var sterilizer, out _))
        {
            if (!sterilizer.CycleInProgress)
                continue;

            if (sterilizer.RequiresPower && !_power.IsPowered(uid))
            {
                Interrupt(uid, sterilizer, "sol-sterilizer-unpowered");
                continue;
            }

            if (TryComp<DoorComponent>(uid, out var door) && door.State != DoorState.Closed)
            {
                Interrupt(uid, sterilizer, "sol-sterilizer-interrupted");
                continue;
            }

            if (_timing.CurTime < sterilizer.CycleEndsAt)
                continue;

            CompleteCycle((uid, sterilizer));
        }
    }

    private void OnStartup(Entity<SterilizationAirlockComponent> ent, ref ComponentStartup args)
    {
        EnsureComp<AirborneContaminantComponent>(ent);
    }

    private void OnDoorStateChanged(Entity<SterilizationAirlockComponent> ent, ref DoorStateChangedEvent args)
    {
        if (args.State == DoorState.Closed)
        {
            TryBeginCycle(ent);
            return;
        }

        if (ent.Comp.CycleInProgress)
            Interrupt(ent, ent.Comp, "sol-sterilizer-interrupted");
    }

    public bool TryBeginCycle(Entity<SterilizationAirlockComponent> ent)
    {
        if (ent.Comp.CycleInProgress)
            return false;

        if (ent.Comp.RequiresPower && !_power.IsPowered(ent.Owner))
        {
            _popup.PopupEntity(Loc.GetString("sol-sterilizer-unpowered"), ent);
            return false;
        }

        if (TryComp<DoorComponent>(ent, out var door) && door.State != DoorState.Closed)
        {
            _popup.PopupEntity(Loc.GetString("sol-sterilizer-doors-open"), ent);
            return false;
        }

        ent.Comp.CycleInProgress = true;
        ent.Comp.CycleEndsAt = _timing.CurTime + ent.Comp.CycleDuration;
        Dirty(ent);
        _popup.PopupEntity(Loc.GetString("sol-sterilizer-started"), ent);
        return true;
    }

    private void Interrupt(EntityUid uid, SterilizationAirlockComponent sterilizer, string locale)
    {
        sterilizer.CycleInProgress = false;
        sterilizer.CycleEndsAt = TimeSpan.Zero;
        Dirty(uid, sterilizer);
        _popup.PopupEntity(Loc.GetString(locale), uid);
    }

    private void CompleteCycle(Entity<SterilizationAirlockComponent> ent)
    {
        ent.Comp.CycleInProgress = false;
        ent.Comp.CycleEndsAt = TimeSpan.Zero;
        Dirty(ent);

        var strength = 100f * ent.Comp.SterilizationStrength;
        var xform = Transform(ent);

        if (xform.GridUid is { } gridUid && TryComp<MapGridComponent>(gridUid, out var grid))
        {
            var tile = _map.GetTileRef(gridUid, grid, xform.Coordinates).GridIndices;
            _gridPathogen.RemoveAirborneLoad(gridUid, tile, strength);
            foreach (var offset in new[] { Vector2i.Zero, new Vector2i(1, 0), new Vector2i(-1, 0), new Vector2i(0, 1), new Vector2i(0, -1) })
                _gridPathogen.RemoveAirborneLoad(gridUid, tile + offset, strength * 0.5f);
        }

        var query = EntityQueryEnumerator<SurfaceContaminationComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out var surface, out var otherXform))
        {
            if (!otherXform.Coordinates.TryDistance(EntityManager, xform.Coordinates, out var dist) || dist > 1.5f)
                continue;

            surface.Contaminants.Clear();
            surface.IsDirty = false;
            Dirty(uid, surface);

            if (TryComp<SurgicalToolSterilityComponent>(uid, out var sterility))
                _asepsis.TryWash((uid, sterility), ent, sterilize: true);
        }

        _popup.PopupEntity(Loc.GetString("sol-sterilizer-complete"), ent);
    }
}
