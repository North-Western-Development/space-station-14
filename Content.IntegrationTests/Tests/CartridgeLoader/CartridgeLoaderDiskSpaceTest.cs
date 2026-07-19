#nullable enable
using System.Linq;
using Content.IntegrationTests.Fixtures;
using Content.Server.CartridgeLoader;
using Content.Shared.CartridgeLoader;
using Robust.Shared.GameObjects;

namespace Content.IntegrationTests.Tests.CartridgeLoader;

[TestFixture]
[TestOf(typeof(CartridgeLoaderSystem))]
public sealed class CartridgeLoaderDiskSpaceTest : GameTest
{
    [TestPrototypes]
    private const string Prototypes = @"
- type: entity
  parent: BasePDACartridge
  id: DiskSpaceTestCartridge1
  components:
  - type: Cartridge
    programName: disk-space-test-1

- type: entity
  parent: BasePDACartridge
  id: DiskSpaceTestCartridge2
  components:
  - type: Cartridge
    programName: disk-space-test-2

- type: entity
  parent: BasePDACartridge
  id: DiskSpaceTestCartridge3
  components:
  - type: Cartridge
    programName: disk-space-test-3

- type: entity
  parent: BasePDACartridge
  id: DiskSpaceTestCartridge4
  components:
  - type: Cartridge
    programName: disk-space-test-4

- type: entity
  parent: BasePDACartridge
  id: DiskSpaceTestCartridge5
  components:
  - type: Cartridge
    programName: disk-space-test-5

- type: entity
  parent: BasePDACartridge
  id: DiskSpaceTestCartridge6
  components:
  - type: Cartridge
    programName: disk-space-test-6

- type: entity
  parent: BasePDACartridge
  id: DiskSpaceTestCartridge7
  components:
  - type: Cartridge
    programName: disk-space-test-7

- type: entity
  parent: BasePDACartridge
  id: DiskSpaceTestCartridge8
  components:
  - type: Cartridge
    programName: disk-space-test-8

- type: entity
  parent: BasePDACartridge
  id: DiskSpaceTestCartridge9
  components:
  - type: Cartridge
    programName: disk-space-test-9

- type: entity
  parent: BasePDACartridge
  id: DiskSpaceTestCartridge10
  components:
  - type: Cartridge
    programName: disk-space-test-10

- type: entity
  parent: BasePDA
  id: DiskSpaceTestPDA
  components:
  - type: CartridgeLoader
    uiKey: enum.PdaUiKey.Key
    preinstalled:
    - DiskSpaceTestCartridge1
    - DiskSpaceTestCartridge2
    - DiskSpaceTestCartridge3
    - DiskSpaceTestCartridge4
    - DiskSpaceTestCartridge5
    - DiskSpaceTestCartridge6
    - DiskSpaceTestCartridge7
    - DiskSpaceTestCartridge8
    - DiskSpaceTestCartridge9
";

    [Test]
    public async Task PreinstalledProgramsExceedFormerEightCap()
    {
        var map = await Pair.CreateTestMap();
        var entMan = Server.EntMan;
        var loaderSystem = Server.System<CartridgeLoaderSystem>();

        EntityUid pda = default;
        await Server.WaitAssertion(() =>
        {
            pda = entMan.SpawnEntity("DiskSpaceTestPDA", map.GridCoords);

            Assert.That(entMan.TryGetComponent(pda, out CartridgeLoaderComponent? loader), Is.True);
            Assert.That(loaderSystem.GetInstalled(pda).Count, Is.EqualTo(9));
            Assert.That(loaderSystem.GetAvailablePrograms(pda, loader).Count, Is.EqualTo(9));

            Assert.That(loaderSystem.InstallProgram(pda, "DiskSpaceTestCartridge10", loader: loader), Is.True);

            var installed = loaderSystem.GetInstalled(pda);
            Assert.That(installed.Count, Is.EqualTo(10));
            Assert.That(
                installed.Select(uid => entMan.GetComponent<CartridgeComponent>(uid).ProgramName),
                Is.EquivalentTo(new[]
                {
                    "disk-space-test-1",
                    "disk-space-test-2",
                    "disk-space-test-3",
                    "disk-space-test-4",
                    "disk-space-test-5",
                    "disk-space-test-6",
                    "disk-space-test-7",
                    "disk-space-test-8",
                    "disk-space-test-9",
                    "disk-space-test-10",
                }));
        });
    }
}
