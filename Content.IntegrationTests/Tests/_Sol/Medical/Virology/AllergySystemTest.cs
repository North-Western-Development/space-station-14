using Content.Server._Sol.Medical.Allergy;
using Content.Shared._Sol.Medical.Allergy;
using Robust.Shared.GameObjects;

namespace Content.IntegrationTests.Tests._Sol.Medical.Virology;

[TestFixture]
public sealed class AllergySystemTest
{
    [Test]
    public async Task FreeTextMapsToMechanicalAllergy()
    {
        await using var pair = await PoolManager.GetServerClient();
        var server = pair.Server;
        var entMan = server.ResolveDependency<IEntityManager>();

        await server.WaitAssertion(() =>
        {
            var human = entMan.Spawn("MobHuman");
            entMan.System<AllergySystem>().ApplyFromFreeText(human, "Peanut", "None");
            Assert.That(entMan.TryGetComponent(human, out AllergyComponent allergy), Is.True);
            Assert.That(allergy!.Allergies.Contains("SolAllergyPeanut"), Is.True);
        });

        await pair.CleanReturnAsync();
    }
}
