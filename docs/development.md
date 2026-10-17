# Developing Lightstream

## Repository scope

On `main`, the active Swift package consists of `LightstreamMath` and `LightstreamCore`, plus their test targets. Inspect [Package.swift](../Package.swift) before documenting or adding a module.

Scalar math and `Vec2` have implementations. The presence of engine-core source files does not imply those systems are implemented: `EngineLoop.swift`, `Events.swift`, and the ECS test file are currently scaffolds.

## Validation

From the repository root:

```sh
swift build
swift test
swift build -c release
```

Record your Swift version and operating system with the results. If a baseline build is failing before your change, record that separately. A documentation-only change does not establish that the package builds or supports an additional platform.

## Working conventions

- Keep reusable math and engine-core responsibilities separate.
- Add focused tests for new behavior; retain the existing test structure.
- Keep future architecture plans explicitly labeled as plans.
- Keep source-file licensing notices intact.
- Do not commit `.build/`, personal IDE state, or operating-system metadata.

The organization contribution guide and pull request template provide the general review workflow.
