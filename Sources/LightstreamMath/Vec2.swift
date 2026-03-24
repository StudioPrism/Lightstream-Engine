// Copyright Studio Prism. Licensed under PolyForm Shield 1.0.0.
// https://polyformproject.org/licenses/shield/1.0.0
// Required Notice: Copyright Studio Prism (https://github.com/studioprism)
//
// Lightstream — LightstreamMath.Vec2
// Vec2.swift
//
// 2D vector type backed by SIMD2<Float>.
// Used for screen space, UV coordinates, 2D physics, and input.
import Foundation

// MARK: - Vec2

public struct Vec2: Equatable, CustomStringConvertible, Sendable {

    // ── Backing Storage ──────────────────────────────────────────────────────
    //
    // All data lives here. x and y are computed properties that read/write
    // into this. Callers never interact with storage directly.

    public var storage: SIMD2<Float>

    // ── Components ───────────────────────────────────────────────────────────

    public var x: Float {
        get { storage.x }
        set { storage.x = newValue }
    }

    public var y: Float {
        get { storage.y }
        set { storage.y = newValue }
    }

    // ── Initializers ─────────────────────────────────────────────────────────

    /// Primary init — provide both components explicitly.
    public init(_ x: Float, _ y: Float) {
        storage = SIMD2<Float>(x, y)
    }

    /// Splat init — fills both lanes with the same value.
    /// Vec2(0) → (0, 0)   Vec2(1) → (1, 1)
    public init(_ scalar: Float) {
        storage = SIMD2<Float>(repeating: scalar)
    }

    /// SIMD init — direct construction from a SIMD2<Float>.
    /// Used internally and for interop with SIMD-based APIs.
    public init(_ simd: SIMD2<Float>) {
        storage = simd
    }

    // ── Static Constants ─────────────────────────────────────────────────────

    /// (0, 0)
    public static let zero = Vec2(0)

    /// (1, 1)
    public static let one  = Vec2(1)

    /// (1, 0) — world right
    public static let right = Vec2(1, 0)

    /// (0, 1) — world up (Y-up convention)
    public static let up    = Vec2(0, 1)

    // ── CustomStringConvertible ──────────────────────────────────────────────

    public var description: String {
        "Vec2(\(x), \(y))"
    }

    // ── Equatable ────────────────────────────────────────────────────────────
    //
    // We use IsApproxEqual instead of exact == because floating point math
    // almost never produces bit-identical results. Two vectors that should
    // be equal after a series of operations will rarely pass an exact check.

    public static func == (lhs: Vec2, rhs: Vec2) -> Bool {
        IsApproxEqual(lhs.x, rhs.x) && IsApproxEqual(lhs.y, rhs.y)
    }

    // ── Geometry — Length ────────────────────────────────────────────────────
    //
    // lengthSquared is cheaper than length — it skips the sqrt.
    // Use it when you only need to compare magnitudes, not the actual length.
    // e.g. "is this vector longer than that one?" doesn't need sqrt.

    /// Squared magnitude. Cheaper than length — use for comparisons.
    public var lengthSquared: Float {
        x * x + y * y
    }

    /// Euclidean length (magnitude) of the vector.
    public var length: Float {
        sqrt(x * x + y * y)
        // using Sqrt due to Length not being a Memeber in SIMD2
    }

    // ── Geometry — Normalize ─────────────────────────────────────────────────
    //
    // Normalizing means scaling a vector so its length becomes exactly 1.0,
    // preserving only its direction. Formula: v / |v|
    //
    // The IsApproxZero guard is critical — dividing by a near-zero length
    // produces infinity or NaN which will silently corrupt everything downstream.
    // We return .zero as a safe fallback for degenerate input.

    /// Returns a unit vector (length == 1) in the same direction.
    /// Returns Vec2.zero if the vector is too short to normalize safely.
    public var normalized: Vec2 {
        guard !IsApproxZero(length) else { return .zero }
        return Vec2(storage / length)
    }

    // ── Geometry — Dot Product ───────────────────────────────────────────────
    //
    // The dot product of two vectors A and B is:
    //   A.x * B.x + A.y * B.y
    //
    // Its result tells you about the relationship between the two directions:
    //   > 0  → same general direction
    //   = 0  → perpendicular
    //   < 0  → opposite directions
    //
    // When both vectors are normalized, dot returns the cosine of the angle
    // between them. This is used constantly in lighting, AI, and physics.

    /// Dot product of two vectors.
    public static func dot(_ a: Vec2, _ b: Vec2) -> Float {
        (a.storage * b.storage).sum()
        // SIMD multiply gives (a.x*b.x, a.y*b.y), .sum() adds the lanes.
    }

    // ── Geometry — Distance ──────────────────────────────────────────────────
    //
    // Distance between two points is the length of the vector between them.
    // You get that vector by subtracting: b - a.
    // Then take its length.

    /// Euclidean distance between two points.
    public static func distance(_ a: Vec2, _ b: Vec2) -> Float {
        (b - a).length
    }

    // ── Geometry — Lerp ──────────────────────────────────────────────────────
    //
    // Vector lerp works component-wise — lerp x with x, y with y.
    // The scalar lerp from Scalar.swift does the work per component.
    // t is intentionally unclamped, matching Scalar.lerp behavior.

    /// Linear interpolation between two vectors by t.
    public static func lerp(_ a: Vec2, _ b: Vec2, t: Float) -> Vec2 {
        Vec2(a.storage + (b.storage - a.storage) * t)
    }

    // ── Geometry — Perpendicular ─────────────────────────────────────────────
    //
    // The perpendicular of (x, y) is (-y, x). This rotates the vector 90°
    // counter-clockwise. Useful for 2D normals, building tangent frames,
    // and orienting UI elements along a direction.

    /// Returns a vector perpendicular to this one (rotated 90° CCW).
    public var perpendicular: Vec2 {
        Vec2(-y, x)
    }

    // ── Geometry — Angle ─────────────────────────────────────────────────────
    //
    // atan2(y, x) returns the angle in radians of a 2D direction vector.
    // Range is [-π, π]. This is the standard way to get a direction angle
    // from a vector — atan alone can't disambiguate quadrants.

    /// Angle of this vector in radians. Range: [-π, π].
    public var angle: Float {
        atan2f(y, x)
    }

    // ── Approx Equality ──────────────────────────────────────────────────────

    /// Returns true if two vectors are component-wise within epsilon.
    public static func isApproxEqual(_ a: Vec2, _ b: Vec2, epsilon: Float = IsEpsilon) -> Bool {
        IsApproxEqual(a.x, b.x, epsilon: epsilon) &&
        IsApproxEqual(a.y, b.y, epsilon: epsilon)
    }
}

// MARK: - Operators

extension Vec2 {

    /// Component-wise addition.
    public static func + (lhs: Vec2, rhs: Vec2) -> Vec2 {
        Vec2(lhs.storage + rhs.storage)
    }

    /// Component-wise subtraction.
    public static func - (lhs: Vec2, rhs: Vec2) -> Vec2 {
        Vec2(lhs.storage - rhs.storage)
    }

    /// Scalar multiplication — scale all components by rhs.
    public static func * (lhs: Vec2, rhs: Float) -> Vec2 {
        Vec2(lhs.storage * rhs)
    }

    /// Scalar multiplication — scale all components by lhs (Float * Vec2).
    public static func * (lhs: Float, rhs: Vec2) -> Vec2 {
        Vec2(lhs * rhs.storage)
    }

    /// Scalar division — divide all components by rhs.
    public static func / (lhs: Vec2, rhs: Float) -> Vec2 {
        guard !IsApproxZero(rhs) else {
            return .zero
        }
        return Vec2(lhs.storage / rhs)
    }

    /// Negation — flips the sign of both components.
    public static prefix func - (v: Vec2) -> Vec2 {
        Vec2(-v.storage)
    }

    /// Component-wise addition assignment.
    public static func += (lhs: inout Vec2, rhs: Vec2) {
        lhs = lhs + rhs
    }

    /// Component-wise subtraction assignment.
    public static func -= (lhs: inout Vec2, rhs: Vec2) {
        lhs = lhs - rhs
    }

    /// Scalar multiplication assignment.
    public static func *= (lhs: inout Vec2, rhs: Float) {
        lhs = lhs * rhs
    }

    /// Scalar division assignment.
    public static func /= (lhs: inout Vec2, rhs: Float) {
        lhs = lhs / rhs
    }
}
