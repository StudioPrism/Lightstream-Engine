# Lightstream Engine

Experimental Swift engine foundations by [Studio Prism](https://github.com/StudioPrism).

This repository contains a Swift package for math utilities and early engine-core scaffolding. It is research and development work, not a complete engine or a production-ready SDK.

## Default-branch implementation

| Area | What is present |
| --- | --- |
| `LightstreamMath` | Scalar utilities and a `Vec2` implementation backed by `SIMD2<Float>` |
| `LightstreamCore` | Source-file scaffolds for ECS, an engine loop, and events |
| Tests | Math test sources and an ECS test scaffold |

This status describes `main`. The [`Dev` branch](https://github.com/StudioPrism/Lightstream-Engine/tree/Dev) is a separate development branch; check the selected revision before comparing features or test results.

On `main`, the package currently exposes only `LightstreamMath` and `LightstreamCore`. Renderer, simulation, networking, and game-layer targets appear as commented plans in [Package.swift](Package.swift); they are not active package products.

The engine-loop and event source files are currently stubs. Graphics backends, a complete ECS runtime, and supported cross-platform releases should not be assumed from the repository layout.

## Get started

Install a Swift 6.0-or-newer toolchain with Swift Package Manager, then clone the repository:

```sh
git clone https://github.com/StudioPrism/Lightstream-Engine.git
cd Lightstream-Engine
swift build
swift test
```

For an optimized build:

```sh
swift build -c release
```

These are the repository's build and test commands. Platform compatibility and test results depend on the toolchain and revision; the existence of the commands is not a claim that every platform has been verified.

## Source map

| Path | Purpose |
| --- | --- |
| [Package.swift](Package.swift) | Active products, targets, and Swift settings |
| [Sources/LightstreamMath/](Sources/LightstreamMath/) | Scalar and vector math |
| [Sources/LightstreamCore/](Sources/LightstreamCore/) | Engine-core scaffolding |
| [Tests/LightstreamMathTests/](Tests/LightstreamMathTests/) | Math tests |
| [Tests/LightstreamCoreTests/](Tests/LightstreamCoreTests/) | Core test scaffold |
| [docs/development.md](docs/development.md) | Scope, verification, and contribution notes |

## Relationship to other Prism work

This is the standalone Swift package. [Projekt Alpha](https://github.com/StudioPrism/Projekt-Alpha) is a separate s&box project. The s&box-based Lightstream tooling workflow and the przm.DEV launcher are not implemented by this package.

Keep architecture ideas separate from implementation status when extending this repository.

## Contributing

Use [Issues](https://github.com/StudioPrism/Lightstream-Engine/issues) for bounded tasks and bug reports. Follow the [Studio Prism contribution guide](https://github.com/StudioPrism/.github/blob/main/CONTRIBUTING.md) and record the build/test commands and environment used in your pull request.

## License

Copyright Studio Prism. Licensed under [PolyForm Shield 1.0.0](https://polyformproject.org/licenses/shield/1.0.0).

> Required Notice: Copyright Studio Prism (https://github.com/studioprism)

PolyForm Shield allows free use for any purpose **except competing with Studio Prism's commercial products**. You may study, modify, and build on Lightstream — you may not use it to build and sell a competing game engine.
