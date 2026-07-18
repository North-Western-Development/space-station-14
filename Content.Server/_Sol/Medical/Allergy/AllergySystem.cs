using Content.Server.Chat.Systems;
using Content.Shared._CD.Records;
using Content.Shared._Sol.Medical.Allergy;
using Content.Shared.Body.Components;
using Content.Shared.Chat;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Damage;
using Content.Shared.Damage.Systems;
using Content.Shared.GameTicking;
using Content.Shared.Nutrition;
using Content.Shared.Popups;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Server._Sol.Medical.Allergy;

/// <summary>
/// Mechanical allergy reactions to reagents and foods, seeded from CD character records.
/// </summary>
public sealed class AllergySystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _prototypes = default!;
    [Dependency] private readonly DamageableSystem _damageable = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly ChatSystem _chat = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _solutions = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    private readonly Dictionary<EntityUid, TimeSpan> _lastReaction = new();

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<AllergyComponent, IngestingEvent>(OnIngesting);
        SubscribeLocalEvent<PlayerSpawnCompleteEvent>(OnPlayerSpawn);
    }

    private void OnPlayerSpawn(PlayerSpawnCompleteEvent args)
    {
        if (args.Profile?.CDCharacterRecords is not { } records)
            return;

        ApplyFromFreeText(args.Mob, records.Allergies, records.DrugAllergies);
    }

    /// <summary>
    /// Maps free-text CD allergy fields onto mechanical allergy prototypes by name/id match.
    /// </summary>
    public void ApplyFromFreeText(EntityUid mob, string allergies, string drugAllergies)
    {
        var combined = $"{allergies};{drugAllergies}";
        var scrubbed = combined.Replace("None", "", StringComparison.OrdinalIgnoreCase).Trim(';', ' ', '\n', '\t');
        if (string.IsNullOrWhiteSpace(scrubbed))
            return;

        var tokens = combined.Split(new[] { ';', ',', '/', '\n' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var comp = EnsureComp<AllergyComponent>(mob);
        var dirty = false;

        foreach (var token in tokens)
        {
            if (token.Equals("None", StringComparison.OrdinalIgnoreCase))
                continue;

            foreach (var proto in _prototypes.EnumeratePrototypes<AllergyPrototype>())
            {
                var name = Loc.GetString(proto.Name);
                if (!proto.ID.Contains(token, StringComparison.OrdinalIgnoreCase) &&
                    !name.Contains(token, StringComparison.OrdinalIgnoreCase) &&
                    !token.Contains(name, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (comp.Allergies.Contains(proto.ID))
                    continue;

                comp.Allergies.Add(proto.ID);
                dirty = true;
            }
        }

        if (dirty)
            Dirty(mob, comp);
    }

    private void OnIngesting(Entity<AllergyComponent> eater, ref IngestingEvent args)
    {
        CheckFoodAllergy(eater, args.Food);
    }

    public override void Update(float frameTime)
    {
        var query = EntityQueryEnumerator<AllergyComponent, BloodstreamComponent>();
        while (query.MoveNext(out var uid, out var allergy, out var bloodstream))
        {
            if (_lastReaction.TryGetValue(uid, out var last) && _timing.CurTime < last + TimeSpan.FromSeconds(5))
                continue;

            if (!_solutions.TryGetSolution(uid, bloodstream.BloodSolutionName, out _, out var solution))
                continue;

            foreach (var allergyId in allergy.Allergies)
            {
                if (!_prototypes.TryIndex(allergyId, out AllergyPrototype? proto))
                    continue;

                foreach (var reagent in proto.TriggerReagents)
                {
                    if (solution.GetTotalPrototypeQuantity(reagent) <= 0)
                        continue;

                    TriggerAllergy(uid, proto);
                    _lastReaction[uid] = _timing.CurTime;
                    return;
                }
            }
        }
    }

    public void CheckFoodAllergy(EntityUid eater, EntityUid food)
    {
        if (!TryComp<AllergyComponent>(eater, out var allergy))
            return;

        var foodId = MetaData(food).EntityPrototype?.ID;
        if (foodId == null)
            return;

        foreach (var allergyId in allergy.Allergies)
        {
            if (!_prototypes.TryIndex(allergyId, out AllergyPrototype? proto))
                continue;

            foreach (var trigger in proto.TriggerFoods)
            {
                if (trigger != foodId)
                    continue;

                TriggerAllergy(eater, proto);
                _lastReaction[eater] = _timing.CurTime;
                return;
            }
        }
    }

    public void TriggerAllergy(EntityUid uid, AllergyPrototype allergy)
    {
        var damage = allergy.DefaultSeverity >= AllergySeverity.Severe
            ? allergy.SevereDamage
            : allergy.MildDamage;

        if (damage.GetTotal() > 0)
            _damageable.TryChangeDamage(uid, damage, interruptsDoAfters: false);

        _popup.PopupEntity(Loc.GetString("sol-allergy-reaction", ("allergy", Loc.GetString(allergy.Name))), uid, uid);

        if (allergy.CausesSneezing && _random.Prob(0.35f))
            _chat.TryEmoteWithChat(uid, "Sneeze", ChatTransmitRange.GhostRangeLimit);

        if (allergy.CausesAnaphylaxis || allergy.DefaultSeverity >= AllergySeverity.Anaphylaxis)
            _popup.PopupEntity(Loc.GetString("sol-allergy-anaphylaxis"), uid, uid, PopupType.LargeCaution);
    }

    public bool HasAllergy(EntityUid uid, ProtoId<AllergyPrototype> allergyId)
    {
        return TryComp<AllergyComponent>(uid, out var comp) &&
               comp.Allergies.Contains(allergyId);
    }

    public IEnumerable<string> GetAllergyDisplayNames(EntityUid uid)
    {
        if (!TryComp<AllergyComponent>(uid, out var comp))
            yield break;

        foreach (var id in comp.Allergies)
        {
            if (_prototypes.TryIndex(id, out AllergyPrototype? proto))
                yield return Loc.GetString(proto.Name);
            else
                yield return id;
        }
    }
}
