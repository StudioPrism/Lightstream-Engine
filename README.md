# Lightstream

> A cross-platform Swift game engine developed by Studio Prism.

Lightstream is a modular, Swift-native game engine targeting macOS (Metal), Windows (Vulkan), and Linux (Vulkan). It is the foundation powering Studio Prism's Future titles.

---

## Status

Lightstream is in active early development. The public API is unstable and subject to change.

| Module | Status |
| --- | --- |
| `LightstreamMath` | 🟢 In Progress |
| `LightstreamCore` | 🟡 Scaffolded |
| `LightstreamSim` | ⚪ Planned |
| `LightstreamNet` | ⚪ Planned |
| `LightstreamGame` | ⚪ Planned |
| `LightstreamGfx` | ⚪ Planned |

---

## Architecture

Lightstream is organized into a strict dependency tier system. Lower tiers have zero knowledge of higher tiers — this boundary is enforced at compile time by SPM.

```
┌─────────────────────────────────────┐
│           LightstreamGfx            │  Metal (macOS) · Vulkan (Win/Linux)
├─────────────────────────────────────┤
│           LightstreamGame           │  Archetypes · Weapons · Match State
├──────────────────┬──────────────────┤
│  LightstreamSim  │  LightstreamNet  │  Physics · Collision · Networking
├──────────────────┴──────────────────┤
│           LightstreamCore           │  ECS · Engine Loop · Asset Registry
├─────────────────────────────────────┤
│           LightstreamMath           │  Vec2/3/4 · Quat · Mat4 · Scalar
└─────────────────────────────────────┘
```

| Tier | Modules | Rule |
| --- | --- | --- |
| **Foundation** | `LightstreamMath` | Zero platform imports. Compiles unchanged on all targets. |
| **Core** | `LightstreamCore` | Pure Swift runtime. No graphics, no sockets. Runs headless. |
| **Shared** | `LightstreamSim`, `LightstreamNet` | Deterministic simulation. Both server and client import this. No graphics dependency. |
| **Client** | `LightstreamGfx`, `LightstreamGame` | Graphics and input. Never imported by server targets. |

---

## Modules

### `LightstreamMath`

The numerical foundation of the engine. Zero dependencies. Every other module depends on this.

| Type | Description | Status |
| --- | --- | --- |
| `Scalar` | Epsilon, clamping, lerp, smoothstep, bit utilities | ✅ Done |
| `Vec2` | 2D vector — screen space, UV, input, 2D physics | ✅ Done |
| `Vec3` | 3D vector — position, velocity, normals, forces | 🔨 In Progress |
| `Vec4` | Homogeneous coordinates, RGBA color | ⚪ Planned |
| `Quaternion` | Rotation representation | ⚪ Planned |
| `Matrix4x4` | Transform math | ⚪ Planned |

All SIMD backing types (`SIMD2<Float>`, `SIMD3<Float>`, etc.) are Swift standard library types — fully cross-platform with no Apple framework dependency.

### `LightstreamCore`

Pure Swift engine runtime. No graphics, no sockets, no game rules. Must compile and run on a headless Linux server with no GPU.

- **EngineLoop** — fixed + variable tick, accumulator pattern
- **ECS World** — entity-component-system runtime
- **Event Bus** — typed event dispatch
- **Asset Registry** — handle-based asset management

---

## Platform Support

| Platform | Graphics Backend | Status |
| --- | --- | --- |
| macOS | Metal | 🟡 Planned |
| Windows | Vulkan | 🟡 Planned |
| Linux | Vulkan | 🟡 Planned |

Platform-specific code is gated with `#if os(macOS)` and `#if os(Windows)` inside source files. The Foundation and Core tiers compile identically on all platforms.

---

## Requirements

- Swift 6.0+
- Swift Package Manager

No third-party dependencies.

---

## Building

```bash
# Build all active targets
swift build

# Run all test suites
swift test

# Optimized release build
swift build -c release
```

---

## Testing

Lightstream uses [Swift Testing](https://developer.apple.com/xcode/swift-testing/) (`@Test` macro, Swift 5.9+). Tests live in `Tests/` alongside their corresponding module.

```
Tests/
  LightstreamMathTests/
    ScalarTests.swift
    Vec2Tests.swift
  LightstreamCoreTests/
```

```bash
swift test
```

---

## Project Structure

```
Lightstream/
├── Sources/
│   ├── LightstreamMath/
│   │   ├── Scalar/
│   │   │   └── Scalar.swift
│   │   └── Vec2/
│   │       └── Vec2.swift
│   └── LightstreamCore/
├── Tests/
│   ├── LightstreamMathTests/
│   │   ├── ScalarTests.swift
│   │   └── Vec2Tests.swift
│   └── LightstreamCoreTests/
├── Package.swift
└── README.md
```

---

## License

Copyright Studio Prism. Licensed under [PolyForm Shield 1.0.0](https://polyformproject.org/licenses/shield/1.0.0).

> Required Notice: Copyright Studio Prism (https://github.com/studioprism)

PolyForm Shield allows free use for any purpose **except competing with Studio Prism's commercial products**. You may study, modify, and build on Lightstream — you may not use it to build and sell a competing game engine.

---

## Studio Prism

Lightstream is developed and maintained by [Studio Prism](https://github.com/studioprism).

**Titles in development:**

- [**Projekt Alpha(TBA)**](https://github.com/studioprism) — competitive cyberpunk FFA shooter

---

*This README reflects the current state of active development and will be updated as modules ship.*
