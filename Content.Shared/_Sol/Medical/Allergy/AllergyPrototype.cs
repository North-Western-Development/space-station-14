using Content.Shared.Damage;
using Robust.Shared.Prototypes;

namespace Content.Shared._Sol.Medical.Allergy;

[Prototype]
public sealed partial class AllergyPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    [DataField(required: true)]
    public LocId Name = default!;

    [DataField]
    public LocId Description = "sol-allergy-unknown-description";

    /// <summary>
    /// Reagent IDs that trigger this allergy.
    /// </summary>
    [DataField]
    public List<string> TriggerReagents = new();

    /// <summary>
    /// Food/entity prototype IDs that trigger this allergy when ingested.
    /// </summary>
    [DataField]
    public List<EntProtoId> TriggerFoods = new();

    [DataField]
    public AllergySeverity DefaultSeverity = AllergySeverity.Mild;

    [DataField]
    public DamageSpecifier MildDamage = new()
    {
        DamageDict = new() { { "Poison", 0.5 } },
    };

    [DataField]
    public DamageSpecifier SevereDamage = new()
    {
        DamageDict = new()
        {
            { "Poison", 2 },
            { "Asphyxiation", 1 },
        },
    };

    [DataField]
    public bool CausesSneezing = true;

    [DataField]
    public bool CausesAnaphylaxis;
}

public enum AllergySeverity : byte
{
    Mild = 0,
    Moderate = 1,
    Severe = 2,
    Anaphylaxis = 3,
}
