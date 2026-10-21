# Lightstream Engine

**Studio Prism's in-house game engine and development SDK, built as a fork of [Facepunch's s&box](https://github.com/Facepunch/sbox-public).**

Lightstream brings engine development, reusable gameplay systems, and studio authoring tools into a shared foundation for Studio Prism's games. Its direction combines responsive sandbox gameplay, deeper physics interaction, and fast iteration through C# hot reload and integrated tooling.

[Studio Prism](https://github.com/StudioPrism) · [Issues](https://github.com/StudioPrism/Lightstream-Engine/issues) · [Upstream s&box](https://github.com/Facepunch/sbox-public) · [s&box documentation](https://sbox.game/dev/doc/)

## What Lightstream is

Lightstream builds on s&box's managed engine, editor, and tooling, with Valve's Source 2 native foundation supplied through upstream binaries. Studio Prism's work will extend that base with shared systems and workflows for developing projects such as **Projekt Alpha9**.

The intended development experience is simple to get started with and expandable as projects grow: open a project, change a system, test it quickly, and keep reusable work available across the studio.

Our priorities are:

- **Responsive sandbox gameplay:** consistent interaction between movement, weapons, physics, AI, vehicles, and equipment.
- **Performance:** measure changes, understand their cost, and keep iteration practical.
- **Fast iteration:** build on the inherited hot-reload and editor workflow.
- **Integrated authoring:** connect content creation, encounter design, and mission logic.
- **Flexible workflows:** support studio-specific tools and conventions as needs become concrete.

## Development status

**Early development.** The `Dev` branch contains the s&box-derived engine source and is the integration branch for Lightstream development. The inherited engine functionality should not be read as a list of features authored by Studio Prism.

The Lightstream-specific systems and tools described below are planned direction. This README does not establish that they are implemented or validated.

| Area | Intended role |
| --- | --- |
| Lightstream SDK | Reusable engine extensions, gameplay foundations, and editor tools shared across studio projects |
| Weapon authoring | Configure weapon behavior, handling, and associated content |
| Encounter and mission design | Author encounters, objectives, checkpoints, and mission structure |
| Mission scripting | Implement custom mission logic alongside the authoring tools |
| przm.DEV integration | Access projects, repositories, tasks, documentation, and installed tools through the studio developer suite |

**Projekt Alpha9** is a game project built using this foundation. Its campaign, game-specific rules, and content belong in its own repository; reusable engine and SDK work belongs here.

The earlier standalone Swift engine work is a separate development direction. This branch follows the s&box-based Lightstream architecture.

## Relationship to s&box

Lightstream is maintained by Studio Prism and derives from Facepunch's s&box project. Upstream code, authorship, and license notices remain part of this repository.

The source here covers the **managed C# engine, editor, tooling, and included content**. The native Source 2 core is supplied as prebuilt binaries; this repository does not provide that core's C++ source. A fork of the managed engine does not imply control over every native rendering or platform feature.

Use upstream documentation for the inherited APIs and editor behavior. Lightstream-specific documentation should accompany each implemented extension.

## Getting started

### Prerequisites

- [Git](https://git-scm.com/downloads)
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- A C# IDE such as [JetBrains Rider](https://www.jetbrains.com/rider/)

### Clone Lightstream

```bash
git clone --branch Dev https://github.com/StudioPrism/Lightstream-Engine.git
cd Lightstream-Engine
```

### Set up the engine

Run the setup entry point for your platform from the repository root:

| Platform | Command |
| --- | --- |
| Windows | `Setup.bat` |
| Linux / macOS | `./Setup.sh` |

The inherited setup downloads required native artifacts and builds the managed engine, shaders, and content. Availability of a setup script does not establish that a platform has been tested by Studio Prism; native artifact availability and upstream requirements still apply.

Open `engine/Sandbox-Engine.slnx` in your IDE after setup. Follow setup output for executable locations and launch instructions.

### Build and checks

The inherited build workflow uses `SboxBuild`:

```bash
dotnet run --project engine/Tools/SboxBuild/SboxBuild.csproj -- build --config Developer
dotnet run --project engine/Tools/SboxBuild/SboxBuild.csproj -- test --filter "TestCategory!=LiveBackend"
dotnet run --project engine/Tools/SboxBuild/SboxBuild.csproj -- format --verify
```

Engine and integration tests require the native artifacts and the appropriate engine environment. See [AGENTS.md](AGENTS.md) for repository build conventions.

## Repository layout

| Path | Purpose |
| --- | --- |
| `engine/` | Managed engine, editor, launcher, and build tools |
| `engine/Tests/` | Managed, engine, and integration test projects |
| `game/addons/` | Included addons and editor tooling |
| `game/core/` | Included shaders, materials, models, and other core content |
| `game/bin/` | Native artifacts and build output; do not commit generated output |

Keep generated interop bindings and build products out of manual edits. Preserve upstream structure where practical so future upstream changes can be reviewed and integrated.

## Development and contributions

Use `Dev` for ongoing integration and focused feature branches for individual changes. Describe whether a change is Lightstream-specific or an upstream fix, and include relevant validation.

Report Lightstream issues and proposals in [this repository](https://github.com/StudioPrism/Lightstream-Engine/issues). When an issue is confirmed to affect unmodified upstream s&box, consult [Facepunch's issue tracker](https://github.com/Facepunch/sbox-public/issues) as well.

Read [CONTRIBUTING.md](CONTRIBUTING.md) and [AGENTS.md](AGENTS.md) before making changes. Some inherited guidance still refers to upstream s&box; follow it in that context.

## Documentation

- [s&box developer documentation](https://sbox.game/dev/doc/)
- [s&box API reference](https://sbox.game/api)
- [Upstream repository](https://github.com/Facepunch/sbox-public)

## License and attribution

The s&box engine source code is licensed under the [MIT License](LICENSE.md). Preserve the applicable copyright and license notices.

Certain native binaries in `game/bin` are **not** covered by the MIT license and are distributed under the s&box EULA. You must agree to its terms to use those binaries.

Third-party components retain their separate licenses, as indicated in `game/thirdpartylegalnotices` and the applicable component notices. Lightstream branding does not replace those terms.
