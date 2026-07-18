using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._Sol.Medical.Allergy;

/// <summary>
/// Mechanical allergies on a character.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class AllergyComponent : Component
{
    [DataField, AutoNetworkedField]
    public List<ProtoId<AllergyPrototype>> Allergies = new();
}
