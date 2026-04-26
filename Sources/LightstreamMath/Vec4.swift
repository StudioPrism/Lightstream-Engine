// Copyright Studio Prism. Licensed under PolyForm Shield 1.0.0.
// https://polyformproject.org/licenses/shield/1.0.0
// Required Notice: Copyright Studio Prism (https://github.com/studioprism)
//
// Lightstream — LightstreamMath.Vec4
// Vec4.swift
//
// 4D vector type backed by SIMD4<Float>.
// Used for homogeneous coordinates, RGBA color, and quaternion intermediates.
//
// w = 1.0 → point in space (affected by translation)
// w = 0.0 → direction     (not affected by translation)

import Foundation

// MARK: - Vec4

public struct Vec4: Equatable, CustomStringConvertible, Sendable {

    // ── Backing Storage ──────────────────────────────────────────────────────

    public var storage: SIMD4<Float>

    // ── Spatial Components ───────────────────────────────────────────────────

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

    /// The homogeneous coordinate.
    /// w = 1.0 → point (translation applies)
    /// w = 0.0 → direction (translation has no effect)
    public var w: Float {
        get { storage.w }
        set { storage.w = newValue }
    }

    // ── Color Aliases ────────────────────────────────────────────────────────
    //
    // Vec4 doubles as an RGBA color. r/g/b/a map directly onto x/y/z/w.
    // Same backing storage — just different semantic names.
    // This means a Vec4 color can be passed to a shader uniform directly.

    public var r: Float {
        get { storage.x }
        set { storage.x = newValue }
    }

    public var g: Float {
        get { storage.y }
        set { storage.y = newValue }
    }

    public var b: Float {
        get { storage.z }
        set { storage.z = newValue }
    }

    public var a: Float {
        get { storage.w }
        set { storage.w = newValue }
    }

    // ── xyz Extraction ───────────────────────────────────────────────────────
    //
    // Strips w and returns the spatial components as a Vec3.
    // Used constantly when pulling a position or direction out of a
    // transformed Vec4 and passing it back into Vec3 math.

    public var xyz: Vec3 {
        Vec3(x, y, z)
    }

    // ── Initializers ─────────────────────────────────────────────────────────

    /// Primary init — provide all four components explicitly.
    public init(_ x: Float, _ y: Float, _ z: Float, _ w: Float) {
        storage = SIMD4<Float>(x, y, z, w)
    }

    /// Splat init — fills all four lanes with the same value.
    public init(_ scalar: Float) {
        storage = SIMD4<Float>(repeating: scalar)
    }

    /// SIMD init — direct construction from a SIMD4<Float>.
    public init(_ simd: SIMD4<Float>) {
        storage = simd
    }

    /// Vec3 + w init — promote a Vec3 to Vec4 with an explicit w.
    public init(_ xyz: Vec3, w: Float) {
        storage = SIMD4<Float>(xyz.x, xyz.y, xyz.z, w)
    }

    // ── Semantic Constructors ─────────────────────────────────────────────────
    //
    // Use these instead of setting w manually.
    // Makes intent explicit at every call site.

    /// Creates a Vec4 representing a point in space. w = 1.0.
    /// Points are affected by translation in matrix transforms.
    public static func point(_ x: Float, _ y: Float, _ z: Float) -> Vec4 {
        Vec4(x, y, z, 1.0)
    }

    /// Creates a Vec4 representing a point from a Vec3. w = 1.0.
    public static func point(_ v: Vec3) -> Vec4 {
        Vec4(v, w: 1.0)
    }

    /// Creates a Vec4 representing a direction. w = 0.0.
    /// Directions are not affected by translation in matrix transforms.
    public static func direction(_ x: Float, _ y: Float, _ z: Float) -> Vec4 {
        Vec4(x, y, z, 0.0)
    }

    /// Creates a Vec4 representing a direction from a Vec3. w = 0.0.
    public static func direction(_ v: Vec3) -> Vec4 {
        Vec4(v, w: 0.0)
    }

    // ── Static Constants — Vectors ────────────────────────────────────────────

    /// (0, 0, 0, 0)
    public static let zero = Vec4(0)

    /// (1, 1, 1, 1)
    public static let one  = Vec4(1)

    // ── Static Constants — Colors ─────────────────────────────────────────────
    //
    // Colors use the r/g/b/a semantic. All values are in [0, 1] linear space.
    // a = 1.0 is fully opaque, a = 0.0 is fully transparent.

    /// (1, 1, 1, 1) — opaque white
    public static let white = Vec4(1, 1, 1, 1)

    /// (0, 0, 0, 1) — opaque black
    public static let black = Vec4(0, 0, 0, 1)

    /// (1, 0, 0, 1) — opaque red
    public static let red   = Vec4(1, 0, 0, 1)

    /// (0, 1, 0, 1) — opaque green
    public static let green = Vec4(0, 1, 0, 1)

    /// (0, 0, 1, 1) — opaque blue
    public static let blue  = Vec4(0, 0, 1, 1)

    /// (0, 0, 0, 0) — fully transparent
    public static let clear = Vec4(0, 0, 0, 0)

    // ── CustomStringConvertible ──────────────────────────────────────────────

    public var description: String {
        "Vec4(\(x), \(y), \(z), \(w))"
    }

    // ── Equatable ────────────────────────────────────────────────────────────

    public static func == (lhs: Vec4, rhs: Vec4) -> Bool {
        IsApproxEqual(lhs.x, rhs.x) &&
        IsApproxEqual(lhs.y, rhs.y) &&
        IsApproxEqual(lhs.z, rhs.z) &&
        IsApproxEqual(lhs.w, rhs.w)
    }

    // ── Geometry — Length ────────────────────────────────────────────────────

    /// Squared magnitude across all four components.
    public var lengthSquared: Float {
        x * x + y * y + z * z + w * w
    }

    /// Euclidean length across all four components.
    public var length: Float {
        sqrt(x * x + y * y + z * z + w * w)
    }

    // ── Geometry — Normalize ─────────────────────────────────────────────────
    //
    // Same guard pattern as Vec2 and Vec3.
    // Note: normalizing a Vec4 that represents a point (w=1) is unusual —
    // you almost always normalize directions (w=0) or raw 4D vectors.
    // The implementation is the same either way.

    /// Returns a unit Vec4 (length == 1). Returns .zero if too short to normalize.
    public var normalized: Vec4 {
        let len = length
        guard !IsApproxZero(len) else { return .zero}
        return Vec4(storage / len)
    }

    // ── Geometry — Dot Product ───────────────────────────────────────────────
    //
    // Four component dot product — same formula extended one more term:
    //   a.x*b.x + a.y*b.y + a.z*b.z + a.w*b.w
    //
    // Note: dot product across all four components including w is used in
    // shader math and quaternion operations. For spatial dot product between
    // two directions, use .xyz on both first.

    /// Dot product across all four components.
    public static func dot(_ a: Vec4, _ b: Vec4) -> Float {
        (a.storage * b.storage).sum()
    }

    // ── Geometry — Lerp ──────────────────────────────────────────────────────
    //
    // Component-wise lerp across all four components including w.
    // Used for color blending (lerp between two Vec4 colors)
    // and for interpolating homogeneous positions.

    /// Linear interpolation between two Vec4s by t. Unclamped.
    public static func lerp(_ a: Vec4, _ b: Vec4, t: Float) -> Vec4 {
        Vec4(a.storage + (b.storage - a.storage) * t)
    }

    // ── Approx Equality ──────────────────────────────────────────────────────

    /// Returns true if two Vec4s are component-wise within epsilon.
    public static func isApproxEqual(_ a: Vec4, _ b: Vec4, epsilon: Float = IsEpsilon) -> Bool {
        IsApproxEqual(a.x, b.x, epsilon: epsilon) &&
        IsApproxEqual(a.y, b.y, epsilon: epsilon) &&
        IsApproxEqual(a.z, b.z, epsilon: epsilon) &&
        IsApproxEqual(a.w, b.w, epsilon: epsilon)
    }
}

// MARK: - Operators

extension Vec4 {

    /// Component-wise addition.
    public static func + (lhs: Vec4, rhs: Vec4) -> Vec4 {
        Vec4(lhs.storage + rhs.storage)
    }

    /// Component-wise subtraction.
    public static func - (lhs: Vec4, rhs: Vec4) -> Vec4 {
        Vec4(lhs.storage - rhs.storage)
    }

    /// Scalar multiplication — Vec4 * Float.
    public static func * (lhs: Vec4, rhs: Float) -> Vec4 {
        Vec4(lhs.storage * rhs)
    }

    /// Scalar multiplication — Float * Vec4.
    public static func * (lhs: Float, rhs: Vec4) -> Vec4 {
        Vec4(lhs * rhs.storage)
    }

    /// Scalar division. Returns .zero safely if rhs is near zero.
    public static func / (lhs: Vec4, rhs: Float) -> Vec4 {
        guard !IsApproxZero(rhs) else {return .zero}
        return Vec4(lhs.storage / rhs)
    }

    /// Negation — flips the sign of all four components.
    public static prefix func - (v: Vec4) -> Vec4 {
        Vec4(-v.storage)
    }

    /// Component-wise addition assignment.
    public static func += (lhs: inout Vec4, rhs: Vec4) { lhs = lhs + rhs }

    /// Component-wise subtraction assignment.
    public static func -= (lhs: inout Vec4, rhs: Vec4) { lhs = lhs - rhs }

    /// Scalar multiplication assignment.
    public static func *= (lhs: inout Vec4, rhs: Float) { lhs = lhs * rhs }

    /// Scalar division assignment.
    public static func /= (lhs: inout Vec4, rhs: Float) { lhs = lhs / rhs }
}
