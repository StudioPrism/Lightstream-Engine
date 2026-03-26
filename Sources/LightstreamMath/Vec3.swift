// Copyright Studio Prism. Licensed under PolyForm Shield 1.0.0.
// https://polyformproject.org/licenses/shield/1.0.0
// Required Notice: Copyright Studio Prism (https://github.com/studioprism)
//
// Lightstream — LightstreamMath.Vec3
// Vec3.swift
//
// 3D vector type backed by SIMD3<Float>.
// Used for position, velocity, direction, normals, and forces.

import Foundation

// MARK: - Vec3

public struct Vec3: Equatable, CustomStringConvertible, Sendable {

    // ── Backing Storage ──────────────────────────────────────────────────────
    //
    // Same pattern as Vec2 — all data lives here.
    // x, y, z are computed properties that read/write into storage.
    // Callers never interact with storage directly.

    public var storage: SIMD3<Float>

    // ── Components ───────────────────────────────────────────────────────────

    public var x: Float {
        get { storage.x }
        set { storage.x = newValue }
    }

    public var y: Float {
        get { storage.y }
        set { storage.y = newValue }
    }

    public var z: Float {
        get { storage.z }
        set { storage.z = newValue }
    }

    // ── Initializers ─────────────────────────────────────────────────────────

    /// Primary init — provide all three components explicitly.
    public init(_ x: Float, _ y: Float, _ z: Float) {
        storage = SIMD3<Float>(x, y, z)
    }

    /// Splat init — fills all three lanes with the same value.
    /// Vec3(0) → (0, 0, 0)   Vec3(1) → (1, 1, 1)
    public init(_ scalar: Float) {
        storage = SIMD3<Float>(repeating: scalar)
    }

    /// SIMD init — direct construction from a SIMD3<Float>.
    /// Used internally and for interop with SIMD-based APIs.
    public init(_ simd: SIMD3<Float>) {
        storage = simd
    }

    // ── Static Constants ─────────────────────────────────────────────────────
    //
    // Lightstream uses a Y-up, right-handed coordinate system.
    // +X = right, +Y = up, +Z = toward the viewer (out of the screen).
    // This matches OpenGL, Vulkan (with Y flip), and most game engines.

    /// (0, 0, 0)
    public static let zero    = Vec3(0)

    /// (1, 1, 1)
    public static let one     = Vec3(1)

    /// (1, 0, 0)
    public static let right   = Vec3(1, 0, 0)

    /// (-1, 0, 0)
    public static let left    = Vec3(-1, 0, 0)

    /// (0, 1, 0)
    public static let up      = Vec3(0, 1, 0)

    /// (0, -1, 0)
    public static let down    = Vec3(0, -1, 0)

    /// (0, 0, 1) — toward the viewer in a right-handed Y-up system
    public static let forward = Vec3(0, 0, 1)

    /// (0, 0, -1) — away from the viewer
    public static let back    = Vec3(0, 0, -1)

    // ── CustomStringConvertible ──────────────────────────────────────────────

    public var description: String {
        "Vec3(\(x), \(y), \(z))"
    }

    // ── Equatable ────────────────────────────────────────────────────────────

    public static func == (lhs: Vec3, rhs: Vec3) -> Bool {
        IsApproxEqual(lhs.x, rhs.x) &&
        IsApproxEqual(lhs.y, rhs.y) &&
        IsApproxEqual(lhs.z, rhs.z)
    }

    // ── Geometry — Length ────────────────────────────────────────────────────

    /// Squared magnitude. Cheaper than length — use for comparisons.
    public var lengthSquared: Float {
        x * x + y * y + z * z
    }

    /// Euclidean length (magnitude) of the vector.
    public var length: Float {
       sqrt(x * x + y * y + z * z)
    }

    // ── Geometry — Normalize ─────────────────────────────────────────────────
    //
    // Identical pattern to Vec2 — guard against near-zero length,
    // divide storage by length, wrap in Vec3.

    /// Returns a unit vector (length == 1) in the same direction.
    /// Returns Vec3.zero if the vector is too short to normalize safely.
    public var normalized: Vec3 {
        guard !IsApproxZero(length) else{return . zero}
        return Vec3(storage / length)
    }

    // ── Geometry — Dot Product ───────────────────────────────────────────────
    //
    // Same formula as Vec2, now with three components:
    //   a.x * b.x + a.y * b.y + a.z * b.z
    //
    // Same meaning: > 0 same direction, 0 perpendicular, < 0 opposite.
    // When normalized, returns cosine of the angle between the vectors.

    /// Dot product of two vectors.
    public static func dot(_ a: Vec3, _ b: Vec3) -> Float {
        (a.storage * b.storage).sum()
    }

    // ── Geometry — Cross Product ─────────────────────────────────────────────
    //
    // The cross product is Vec3-exclusive — it has no Vec2 equivalent.
    // It takes two vectors and returns a THIRD vector that is perpendicular
    // to BOTH inputs. The result's direction follows the right-hand rule:
    // point fingers along a, curl toward b — thumb points in the result direction.
    //
    // Formula:
    //   result.x = a.y * b.z - a.z * b.y
    //   result.y = a.z * b.x - a.x * b.z
    //   result.z = a.x * b.y - a.y * b.x
    //
    // Key properties:
    //   cross(right, up) = forward       ← defines your coordinate system
    //   cross(a, b) = -cross(b, a)       ← order matters, anti-commutative
    //   cross(parallel vectors) = zero   ← no perpendicular exists
    //
    // Used for: surface normals, camera up vectors, torque, angular velocity.

    /// Cross product of two vectors. Result is perpendicular to both inputs.
    public static func cross(_ a: Vec3, _ b: Vec3) -> Vec3 {
        Vec3(
            a.y * b.z - a.z * b.y,
            a.z * b.x - a.x * b.z,
            a.x * b.y - a.y * b.x
        )
    }

    // ── Geometry — Distance ──────────────────────────────────────────────────

    /// Euclidean distance between two points.
    public static func distance(_ a: Vec3, _ b: Vec3) -> Float {
        (b - a).length
    }

    // ── Geometry — Lerp ──────────────────────────────────────────────────────

    /// Linear interpolation between two vectors by t.
    /// t is unclamped — extrapolates beyond [0, 1].
    public static func lerp(_ a: Vec3, _ b: Vec3, t: Float) -> Vec3 {
        Vec3(a.storage + (b.storage - a.storage) * t)
    }

    // ── Geometry — Reflect ───────────────────────────────────────────────────
    //
    // Reflection computes how a vector bounces off a surface defined by a normal.
    //
    // Formula: reflect(v, n) = v - 2 * dot(v, n) * n
    //
    // Think of a laser hitting a mirror:
    //   v = the incoming direction
    //   n = the surface normal (must be normalized)
    //   result = the outgoing reflected direction
    //
    // Used for: physically based rendering, projectile bouncing, AI sight.

    /// Reflects vector v off a surface with the given normal.
    /// Normal should be normalized for correct results.
    public static func reflect(_ v: Vec3, normal n: Vec3) -> Vec3 {
       v - n * (2 * dot(v, n))
    }

    // ── Approx Equality ──────────────────────────────────────────────────────

    /// Returns true if two vectors are component-wise within epsilon.
    public static func isApproxEqual(_ a: Vec3, _ b: Vec3, epsilon: Float = IsEpsilon) -> Bool {
        IsApproxEqual(a.x, b.x, epsilon: epsilon) &&
        IsApproxEqual(a.y, b.y, epsilon: epsilon) &&
        IsApproxEqual(a.z, b.z, epsilon: epsilon)
    }
}

// MARK: - Operators

extension Vec3 {

    /// Component-wise addition.
    public static func + (lhs: Vec3, rhs: Vec3) -> Vec3 {
        Vec3(lhs.storage + rhs.storage)
    }

    /// Component-wise subtraction.
    public static func - (lhs: Vec3, rhs: Vec3) -> Vec3 {
        Vec3(lhs.storage - rhs.storage)
    }

    /// Scalar multiplication — scale all components by rhs.
    public static func * (lhs: Vec3, rhs: Float) -> Vec3 {
        Vec3(lhs.storage * rhs)
    }

    /// Scalar multiplication — Float * Vec3.
    public static func * (lhs: Float, rhs: Vec3) -> Vec3 {
        Vec3(lhs * rhs.storage)
    }

    /// Scalar division — divide all components by rhs.
    /// Returns Vec3.zero safely if rhs is near zero.
    public static func / (lhs: Vec3, rhs: Float) -> Vec3 {
        guard !IsApproxZero(rhs) else {return .zero}
        return Vec3(lhs.storage / rhs)
    }

    /// Negation — flips the sign of all three components.
    public static prefix func - (v: Vec3) -> Vec3 {
        Vec3(-v.storage)
    }

    /// Component-wise addition assignment.
    public static func += (lhs: inout Vec3, rhs: Vec3) { lhs = lhs + rhs }

    /// Component-wise subtraction assignment.
    public static func -= (lhs: inout Vec3, rhs: Vec3) { lhs = lhs - rhs }

    /// Scalar multiplication assignment.
    public static func *= (lhs: inout Vec3, rhs: Float) { lhs = lhs * rhs }

    /// Scalar division assignment.
    public static func /= (lhs: inout Vec3, rhs: Float) { lhs = lhs / rhs }
}
