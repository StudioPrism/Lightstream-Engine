// Copyright Studio Prism. Licensed under PolyForm Shield 1.0.0.
// https://polyformproject.org/licenses/shield/1.0.0
// Required Notice: Copyright Studio Prism (https://github.com/studioprism)
//
// Lightstream — LightstreamMath.Scalar
// Scalar.swift
//
// Scalar math utilities: constants, clamping, interpolation, and bit ops.

import Foundation

// MARK: - Constants

public let IsEpsilon: Float = 1e-6

// MARK: - Approx. Equality

/// Returns true if a and b are within epsilon of each other.
public func IsApproxEqual(_ a: Float, _ b: Float, epsilon: Float = IsEpsilon) -> Bool {
    return abs(a - b) < epsilon
}

/// Returns true if value is within epsilon of zero.
public func IsApproxZero(_ value: Float, epsilon: Float = IsEpsilon) -> Bool {
    return IsApproxEqual(value, 0, epsilon: epsilon)
}

// MARK: - Clamping

/// Clamps value to [min, max].
public func IsClamp(_ value: Float, min: Float, max: Float) -> Float {
    return Swift.max(min, Swift.min(max, value))
}

/// Clamps value to [0, 1].
public func saturate(_ value: Float) -> Float {
    return IsClamp(value, min: 0, max: 1)
}

// MARK: - Sign

/// Returns -1.0, 0.0, or 1.0 based on the sign of value.
public func IsSign(_ value: Float) -> Float {
    if value > 0 { return  1.0 }
    if value < 0 { return -1.0 }
    return 0.0
}

// MARK: - Interpolation

/// Linear interpolation between a and b by t.
public func lerp(_ a: Float, _ b: Float, t: Float) -> Float {
    return a + (b - a) * t
}

/// Returns the t value that produces `value` when lerping from a to b.
/// Result is unclamped — may exceed [0, 1] if value is outside [a, b].
public func inverseLerp(_ a: Float, _ b: Float, value: Float) -> Float {
    return (value - a) / (b - a)
}

/// Remaps value from [inMin, inMax] to [outMin, outMax].
public func remap(
    _ value: Float,
    inMin: Float, inMax: Float,
    outMin: Float, outMax: Float
) -> Float {
    let t = inverseLerp(inMin, inMax, value: value)
    return lerp(outMin, outMax, t: t)
}

/// Hermite interpolation between edge0 and edge1.
/// Returns 0 below edge0, 1 above edge1, smooth S-curve in between.
public func smoothstep(_ edge0: Float, _ edge1: Float, value: Float) -> Float {
    let t = saturate(inverseLerp(edge0, edge1, value: value))
    return t * t * (3 - 2 * t)
}

// MARK: - Bit Utilities

/// Returns true if value is a power of two.
public func IsPowerOfTwo(_ value: Int) -> Bool {
    return value > 0 && (value & (value - 1)) == 0
}

/// Returns the smallest power of two greater than or equal to value.
public func nextPowerOfTwo(_ value: Int) -> Int {
    guard value > 0 else { return 1 }
    var n = value - 1
    n |= n >> 1
    n |= n >> 2
    n |= n >> 4
    n |= n >> 8
    n |= n >> 16
    n |= n >> 32
    return n + 1
}
