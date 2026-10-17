# Lightstream

Studio Prism's framework and SDK work built in and around [s&box](https://sbox.game/).

Lightstream is being reorganized around reusable runtime systems, editor extensions, and authoring tools for Studio Prism projects. s&box supplies the underlying engine and editor. Game-specific content stays in its own repository.

## Current status

This branch is the s&box foundation and documentation layout. It does not yet contain a runnable s&box project, C# implementation, or packaged SDK. The previous standalone Swift prototype is preserved on dedicated branches; it is no longer the direction of `main`.

## Repository layout

| Path | Purpose |
| --- | --- |
| [Code/](Code/README.md) | Shared runtime systems |
| [Editor/](Editor/README.md) | s&box editor extensions |
| [Tools/](Tools/README.md) | Authoring workflows and supporting tools |
| [docs/architecture.md](docs/architecture.md) | Project boundaries and intended integration |
| [docs/development.md](docs/development.md) | Current development workflow and next steps |
| [docs/swift-history.md](docs/swift-history.md) | Preserved Swift branches and snapshots |

## Related projects

- [Projekt Alpha](https://github.com/StudioPrism/Projekt-Alpha): a separate s&box game project. Lightstream integration is not yet declared here.
- **przm.DEV**: the separate local development hub for launching tools and accessing repositories and documentation.

## Contributing

Keep reusable systems separate from game rules and content. Document new dependencies and validate changes in an actual s&box host project before describing an integration as working. See the [development guide](docs/development.md).

## License

Copyright Studio Prism. Licensed under [PolyForm Shield 1.0.0](https://polyformproject.org/licenses/shield/1.0.0).

> Required Notice: Copyright Studio Prism (https://github.com/studioprism)

PolyForm Shield allows free use for any purpose **except competing with Studio Prism's commercial products**. You may study, modify, and build on Lightstream — you may not use it to build and sell a competing game engine.
