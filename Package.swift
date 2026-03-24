// Copyright Studio Prism. Licensed under PolyForm Shield 1.0.0.
// https://polyformproject.org/licenses/shield/1.0.0
// Required Notice: Copyright Studio Prism (https://github.com/studioprism)
//
// Lightstream Game Engine
// Developed by Studio Prism
//
// Cross-platform: macOS (Metal) | Windows (Vulkan) | Linux (Vulkan)
//
// swift build               — build all active targets
// swift test                — run all test suites
// swift build -c release    — optimized build

// swift-tools-version: 6.0
import PackageDescription

// ── Shared Swift settings applied to every target ───────────────────────────
//
// StrictConcurrency enforces Swift 6 data-race safety at compile time.
// An engine runs multiple threads (game loop, render, network, audio) —
// you want races caught by the compiler, not discovered at runtime.

let swiftSettings: [SwiftSetting] = [
    .enableExperimentalFeature("StrictConcurrency"),
]

let package = Package(
    name: "Lightstream",

    // ── No platform constraints ──────────────────────────────────────────────
    //
    // Omitting `platforms:` allows SPM to build on any Swift-supported
    // platform. Platform-specific code is gated with #if os(macOS) or
    // #if os(Windows) inside source files. LightstreamGfx adds Metal
    // linker flags conditionally so they never trigger on Windows/Linux.

    // ── Products ─────────────────────────────────────────────────────────────
    //
    // One product per module that external consumers may import.
    // Uncomment each product only when its Sources/ folder is ready.

    products: [
        .library(name: "LightstreamMath", targets: ["LightstreamMath"]),
        .library(name: "LightstreamCore", targets: ["LightstreamCore"]),

        // .library(name: "LightstreamGfx",  targets: ["LightstreamGfx"]),
        // .library(name: "LightstreamSim",  targets: ["LightstreamSim"]),
        // .library(name: "LightstreamNet",  targets: ["LightstreamNet"]),
        // .library(name: "LightstreamGame", targets: ["LightstreamGame"]),
    ],

    // ── External dependencies ────────────────────────────────────────────────
    //
    // None yet. Future entries:
    // .package(url: "https://github.com/example/Lib.git", from: "1.0.0"),

    targets: [

        // ════════════════════════════════════════════════════════════════════
        // FOUNDATION TIER
        // Zero platform imports. Compiles on macOS, Windows, Linux unchanged.
        // Every other module depends on at least one of these.
        // ════════════════════════════════════════════════════════════════════

        // LightstreamMath ─────────────────────────────────────────────────────
        // Vec2/3/4, Quat, Mat4, AABB, Ray, Plane, Frustum, interpolation.
        // No dependencies. The bedrock everything else is built on.
        .target(
            name: "LightstreamMath",
            dependencies: [],
            path: "Sources/LightstreamMath",
            swiftSettings: swiftSettings
        ),
        .testTarget(
            name: "LightstreamMathTests",
            dependencies: ["LightstreamMath"],
            path: "Tests/LightstreamMathTests"
        ),

        // ════════════════════════════════════════════════════════════════════
        // CORE TIER
        // Pure Swift engine runtime. No graphics, no sockets, no game rules.
        // Must compile and run on a headless Linux server with no GPU.
        // ════════════════════════════════════════════════════════════════════

        // LightstreamCore ─────────────────────────────────────────────────────
        // EngineLoop (fixed + variable tick, accumulator), ECS World,
        // typed event bus, handle-based asset registry.
        .target(
            name: "LightstreamCore",
            dependencies: ["LightstreamMath"],
            path: "Sources/LightstreamCore",
            swiftSettings: swiftSettings
        ),
        .testTarget(
            name: "LightstreamCoreTests",
            dependencies: ["LightstreamCore"],
            path: "Tests/LightstreamCoreTests"
        ),

        // ════════════════════════════════════════════════════════════════════
        // SHARED TIER  (server + client)
        // Deterministic simulation. Both the dedicated server and the client
        // run this code. No graphics dependency anywhere in this tier —
        // SPM enforces that boundary at compile time.
        // ════════════════════════════════════════════════════════════════════

        // LightstreamSim ──────────────────────────────────────────────────────
        // Physics integration, collision detection, character movement.
        // .target(
        //     name: "LightstreamSim",
        //     dependencies: ["LightstreamMath", "LightstreamCore"],
        //     path: "Sources/LightstreamSim",
        //     swiftSettings: swiftSettings
        // ),
        // .testTarget(
        //     name: "LightstreamSimTests",
        //     dependencies: ["LightstreamSim"],
        //     path: "Tests/LightstreamSimTests"
        // ),

        // LightstreamNet ──────────────────────────────────────────────────────
        // Packet definitions, serialization, delta compression.
        // Shared — both server and client targets import this.
        // .target(
        //     name: "LightstreamNet",
        //     dependencies: ["LightstreamMath", "LightstreamCore"],
        //     path: "Sources/LightstreamNet",
        //     swiftSettings: swiftSettings
        // ),
        // .testTarget(
        //     name: "LightstreamNetTests",
        //     dependencies: ["LightstreamNet"],
        //     path: "Tests/LightstreamNetTests"
        // ),

        // LightstreamGame ─────────────────────────────────────────────────────
        // ECS components, systems, and rules shared between server and client:
        // archetypes, weapons, economy, match state. No graphics dependency.
        // .target(
        //     name: "LightstreamGame",
        //     dependencies: [
        //         "LightstreamCore",
        //         "LightstreamSim",
        //         "LightstreamNet",
        //     ],
        //     path: "Sources/LightstreamGame",
        //     swiftSettings: swiftSettings
        // ),
        // .testTarget(
        //     name: "LightstreamGameTests",
        //     dependencies: ["LightstreamGame"],
        //     path: "Tests/LightstreamGameTests"
        // ),

        // ════════════════════════════════════════════════════════════════════
        // CLIENT TIER
        // Graphics and input. Never imported by server targets.
        // Metal frameworks are linked conditionally — they do not exist on
        // Windows or Linux and will not be linked there.
        // ════════════════════════════════════════════════════════════════════

        // LightstreamGfx ──────────────────────────────────────────────────────
        // Platform Abstraction Layer (RenderBackend + WindowBackend protocols)
        // + Metal implementation (macOS) + Vulkan implementation (Win/Linux).
        // .target(
        //     name: "LightstreamGfx",
        //     dependencies: ["LightstreamMath", "LightstreamCore"],
        //     path: "Sources/LightstreamGfx",
        //     swiftSettings: swiftSettings,
        //     linkerSettings: [
        //         .linkedFramework("Metal",       .when(platforms: [.macOS])),
        //         .linkedFramework("MetalKit",    .when(platforms: [.macOS])),
        //         .linkedFramework("QuartzCore",  .when(platforms: [.macOS])),
        //     ]
        // ),
        // .testTarget(
        //     name: "LightstreamGfxTests",
        //     dependencies: ["LightstreamGfx"],
        //     path: "Tests/LightstreamGfxTests"
        // ),

    ]
)
