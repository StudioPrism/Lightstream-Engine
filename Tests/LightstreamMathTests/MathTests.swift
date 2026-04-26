// Copyright Studio Prism. Licensed under PolyForm Shield 1.0.0.
// https://polyformproject.org/licenses/shield/1.0.0
// Required Notice: Copyright Studio Prism (https://github.com/studioprism)
//
// Lightstream — LightstreamMathTests
// MathTests.swift
//
// Unified test suite for LightstreamMath.
// Covers: Scalar, Vec2, Vec3, Vec4.

import Testing
@testable import LightstreamMath

// MARK: ═══════════════════════════════════════════════════════════════════════
// SCALAR
// MARK: ═══════════════════════════════════════════════════════════════════════

@Suite("Scalar — Constants")
struct ScalarConstantTests {

    @Test("IsEpsilon is 1e-6")
    func epsilonValue() {
        #expect(IsEpsilon == 1e-6)
    }
}

@Suite("Scalar — IsApproxEqual")
struct ScalarApproxEqualTests {

    @Test("Equal values return true")
    func equalValues() {
        #expect(IsApproxEqual(1.0, 1.0))
    }

    @Test("Values within default epsilon return true")
    func withinEpsilon() {
        #expect(IsApproxEqual(1.0, 1.0 + IsEpsilon * 0.5))
    }

    @Test("Values outside default epsilon return false")
    func outsideEpsilon() {
        #expect(!IsApproxEqual(1.0, 1.0 + IsEpsilon * 2))
    }

    @Test("Comparison is symmetric")
    func symmetric() {
        #expect(IsApproxEqual(1.0, 1.0 + IsEpsilon * 0.5))
        #expect(IsApproxEqual(1.0 + IsEpsilon * 0.5, 1.0))
    }

    @Test("Works correctly around zero")
    func aroundZero() {
        #expect(IsApproxEqual(0.0, 0.0))
        #expect(IsApproxEqual(0.0, IsEpsilon * 0.5))
        #expect(!IsApproxEqual(0.0, IsEpsilon * 2))
    }

    @Test("Custom epsilon is respected")
    func customEpsilon() {
        #expect(IsApproxEqual(1.0, 1.05, epsilon: 0.1))
        #expect(!IsApproxEqual(1.0, 1.2, epsilon: 0.1))
    }
}

@Suite("Scalar — IsApproxZero")
struct ScalarApproxZeroTests {

    @Test("Exactly zero returns true")
    func exactlyZero() {
        #expect(IsApproxZero(0.0))
    }

    @Test("Value within epsilon of zero returns true")
    func withinEpsilon() {
        #expect(IsApproxZero(IsEpsilon * 0.5))
    }

    @Test("Value outside epsilon of zero returns false")
    func outsideEpsilon() {
        #expect(!IsApproxZero(IsEpsilon * 2))
    }

    @Test("Negative near-zero returns true")
    func negativeNearZero() {
        #expect(IsApproxZero(-IsEpsilon * 0.5))
    }
}

@Suite("Scalar — IsClamp")
struct ScalarClampTests {

    @Test("Value below min returns min")
    func belowMin() {
        #expect(IsClamp(-5.0, min: 0.0, max: 1.0) == 0.0)
    }

    @Test("Value above max returns max")
    func aboveMax() {
        #expect(IsClamp(5.0, min: 0.0, max: 1.0) == 1.0)
    }

    @Test("Value within range is unchanged")
    func withinRange() {
        #expect(IsClamp(0.5, min: 0.0, max: 1.0) == 0.5)
    }

    @Test("Value at exact min boundary is unchanged")
    func atMin() {
        #expect(IsClamp(0.0, min: 0.0, max: 1.0) == 0.0)
    }

    @Test("Value at exact max boundary is unchanged")
    func atMax() {
        #expect(IsClamp(1.0, min: 0.0, max: 1.0) == 1.0)
    }

    @Test("Works with negative ranges")
    func negativeRange() {
        #expect(IsClamp(-3.0,  min: -5.0, max: -1.0) == -3.0)
        #expect(IsClamp(0.0,   min: -5.0, max: -1.0) == -1.0)
        #expect(IsClamp(-10.0, min: -5.0, max: -1.0) == -5.0)
    }
}

@Suite("Scalar — saturate")
struct ScalarSaturateTests {

    @Test("Values in [0,1] are unchanged", arguments: [
        Float(0.0), Float(0.25), Float(0.5), Float(0.75), Float(1.0)
    ])
    func withinRange(value: Float) {
        #expect(saturate(value) == value)
    }

    @Test("Value below 0 clamps to 0")
    func belowZero() {
        #expect(saturate(-1.0) == 0.0)
    }

    @Test("Value above 1 clamps to 1")
    func aboveOne() {
        #expect(saturate(2.0) == 1.0)
    }
}

@Suite("Scalar — IsSign")
struct ScalarSignTests {

    @Test("Positive value returns 1.0")
    func positive() {
        #expect(IsSign(5.0) == 1.0)
        #expect(IsSign(0.001) == 1.0)
    }

    @Test("Negative value returns -1.0")
    func negative() {
        #expect(IsSign(-5.0) == -1.0)
        #expect(IsSign(-0.001) == -1.0)
    }

    @Test("Zero returns 0.0")
    func zero() {
        #expect(IsSign(0.0) == 0.0)
    }
}

@Suite("Scalar — lerp")
struct ScalarLerpTests {

    @Test("t=0 returns a")
    func tZero() {
        #expect(lerp(0.0, 10.0, t: 0.0) == 0.0)
    }

    @Test("t=1 returns b")
    func tOne() {
        #expect(lerp(0.0, 10.0, t: 1.0) == 10.0)
    }

    @Test("t=0.5 returns midpoint")
    func tHalf() {
        #expect(lerp(0.0, 10.0, t: 0.5) == 5.0)
    }

    @Test("Extrapolates beyond range when t > 1")
    func extrapolateAbove() {
        #expect(lerp(0.0, 10.0, t: 2.0) == 20.0)
    }

    @Test("Extrapolates beyond range when t < 0")
    func extrapolateBelow() {
        #expect(lerp(0.0, 10.0, t: -1.0) == -10.0)
    }
}

@Suite("Scalar — inverseLerp")
struct ScalarInverseLerpTests {

    @Test("Value at a returns 0")
    func atA() {
        #expect(inverseLerp(0.0, 10.0, value: 0.0) == 0.0)
    }

    @Test("Value at b returns 1")
    func atB() {
        #expect(inverseLerp(0.0, 10.0, value: 10.0) == 1.0)
    }

    @Test("Midpoint value returns 0.5")
    func midpoint() {
        #expect(inverseLerp(0.0, 10.0, value: 5.0) == 0.5)
    }

    @Test("Round-trips with lerp")
    func roundTrip() {
        let t: Float = 0.37
        let value = lerp(4.0, 16.0, t: t)
        let recovered = inverseLerp(4.0, 16.0, value: value)
        #expect(IsApproxEqual(recovered, t))
    }

    @Test("Value outside range returns t outside [0,1]")
    func outsideRange() {
        #expect(inverseLerp(0.0, 10.0, value: 20.0) == 2.0)
        #expect(inverseLerp(0.0, 10.0, value: -10.0) == -1.0)
    }
}

@Suite("Scalar — remap")
struct ScalarRemapTests {

    @Test("Maps min of input range to min of output range")
    func inputMin() {
        #expect(remap(0.0, inMin: 0.0, inMax: 10.0, outMin: 0.0, outMax: 100.0) == 0.0)
    }

    @Test("Maps max of input range to max of output range")
    func inputMax() {
        #expect(remap(10.0, inMin: 0.0, inMax: 10.0, outMin: 0.0, outMax: 100.0) == 100.0)
    }

    @Test("Maps midpoint correctly")
    func midpoint() {
        #expect(remap(5.0, inMin: 0.0, inMax: 10.0, outMin: 0.0, outMax: 100.0) == 50.0)
    }

    @Test("Works with non-zero based input range")
    func offsetInputRange() {
        #expect(IsApproxEqual(remap(15.0, inMin: 10.0, inMax: 20.0, outMin: 0.0, outMax: 1.0), 0.5))
    }

    @Test("Works with reversed output range")
    func reversedOutput() {
        #expect(remap(0.0, inMin: 0.0, inMax: 10.0, outMin: 1.0, outMax: 0.0) == 1.0)
    }
}

@Suite("Scalar — smoothstep")
struct ScalarSmoothstepTests {

    @Test("Value below edge0 returns 0")
    func belowEdge0() {
        #expect(smoothstep(0.0, 1.0, value: -1.0) == 0.0)
    }

    @Test("Value above edge1 returns 1")
    func aboveEdge1() {
        #expect(smoothstep(0.0, 1.0, value: 2.0) == 1.0)
    }

    @Test("Value at edge0 returns 0")
    func atEdge0() {
        #expect(smoothstep(0.0, 1.0, value: 0.0) == 0.0)
    }

    @Test("Value at edge1 returns 1")
    func atEdge1() {
        #expect(smoothstep(0.0, 1.0, value: 1.0) == 1.0)
    }

    @Test("Midpoint returns 0.5")
    func midpoint() {
        #expect(IsApproxEqual(smoothstep(0.0, 1.0, value: 0.5), 0.5))
    }

    @Test("Output is always in [0, 1]", arguments: [
        Float(0.0), Float(0.1), Float(0.25), Float(0.5), Float(0.75), Float(0.9), Float(1.0)
    ])
    func outputInRange(value: Float) {
        let result = smoothstep(0.0, 1.0, value: value)
        #expect(result >= 0.0 && result <= 1.0)
    }
}

@Suite("Scalar — IsPowerOfTwo")
struct ScalarPowerOfTwoTests {

    @Test("Known powers of two return true", arguments: [1, 2, 4, 8, 16, 32, 64, 128, 256, 1024])
    func knownPowers(value: Int) {
        #expect(IsPowerOfTwo(value))
    }

    @Test("Known non-powers return false", arguments: [3, 5, 6, 7, 9, 10, 12, 100])
    func knownNonPowers(value: Int) {
        #expect(!IsPowerOfTwo(value))
    }

    @Test("Zero returns false")
    func zero() {
        #expect(!IsPowerOfTwo(0))
    }

    @Test("Negative values return false")
    func negative() {
        #expect(!IsPowerOfTwo(-4))
    }
}

@Suite("Scalar — nextPowerOfTwo")
struct ScalarNextPowerOfTwoTests {

    @Test("Already a power of two returns itself", arguments: [1, 2, 4, 8, 16, 32, 64])
    func alreadyPower(value: Int) {
        #expect(nextPowerOfTwo(value) == value)
    }

    @Test("Non-powers round up correctly", arguments: [
        (3, 4), (5, 8), (6, 8), (7, 8), (9, 16), (100, 128)
    ])
    func roundsUp(input: Int, expected: Int) {
        #expect(nextPowerOfTwo(input) == expected)
    }

    @Test("Zero or negative returns 1")
    func zeroOrNegative() {
        #expect(nextPowerOfTwo(0) == 1)
    }

    @Test("Result is always a power of two")
    func resultIsPowerOfTwo() {
        for value in 1...200 {
            #expect(IsPowerOfTwo(nextPowerOfTwo(value)))
        }
    }
}

// MARK: ═══════════════════════════════════════════════════════════════════════
// VEC2
// MARK: ═══════════════════════════════════════════════════════════════════════

@Suite("Vec2 — Init & Storage")
struct Vec2InitTests {

    @Test("Primary init stores correct components")
    func primaryInit() {
        let v = Vec2(3.0, 4.0)
        #expect(v.x == 3.0)
        #expect(v.y == 4.0)
    }

    @Test("Splat init fills both components with scalar")
    func splatInit() {
        let v = Vec2(5.0)
        #expect(v.x == 5.0)
        #expect(v.y == 5.0)
    }

    @Test("SIMD init stores correct components")
    func simdInit() {
        let simd = SIMD2<Float>(7.0, 8.0)
        let v = Vec2(simd)
        #expect(v.x == 7.0)
        #expect(v.y == 8.0)
    }

    @Test("Component assignment mutates correctly")
    func componentAssignment() {
        var v = Vec2(1.0, 2.0)
        v.x = 9.0
        v.y = 10.0
        #expect(v.x == 9.0)
        #expect(v.y == 10.0)
    }
}

@Suite("Vec2 — Constants")
struct Vec2ConstantTests {

    @Test("zero is (0, 0)")
    func zero() {
        #expect(Vec2.zero.x == 0.0)
        #expect(Vec2.zero.y == 0.0)
    }

    @Test("one is (1, 1)")
    func one() {
        #expect(Vec2.one.x == 1.0)
        #expect(Vec2.one.y == 1.0)
    }

    @Test("right is (1, 0)")
    func right() {
        #expect(Vec2.right.x == 1.0)
        #expect(Vec2.right.y == 0.0)
    }

    @Test("up is (0, 1)")
    func up() {
        #expect(Vec2.up.x == 0.0)
        #expect(Vec2.up.y == 1.0)
    }
}

@Suite("Vec2 — Equatable")
struct Vec2EquatableTests {

    @Test("Identical vectors are equal")
    func identical() {
        #expect(Vec2(1.0, 2.0) == Vec2(1.0, 2.0))
    }

    @Test("Vectors within epsilon are equal")
    func withinEpsilon() {
        let a = Vec2(1.0, 2.0)
        let b = Vec2(1.0 + IsEpsilon * 0.5, 2.0 + IsEpsilon * 0.5)
        #expect(a == b)
    }

    @Test("Vectors outside epsilon are not equal")
    func outsideEpsilon() {
        #expect(Vec2(1.0, 2.0) != Vec2(1.1, 2.1))
    }

    @Test("Vectors differing only in x are not equal")
    func differInX() {
        #expect(Vec2(1.0, 2.0) != Vec2(9.0, 2.0))
    }

    @Test("Vectors differing only in y are not equal")
    func differInY() {
        #expect(Vec2(1.0, 2.0) != Vec2(1.0, 9.0))
    }
}

@Suite("Vec2 — Description")
struct Vec2DescriptionTests {

    @Test("Description format is correct")
    func descriptionFormat() {
        #expect(Vec2(1.0, 2.0).description == "Vec2(1.0, 2.0)")
    }
}

@Suite("Vec2 — Length")
struct Vec2LengthTests {

    @Test("Length of (3, 4) is 5")
    func knownLength() {
        #expect(IsApproxEqual(Vec2(3.0, 4.0).length, 5.0))
    }

    @Test("Length of zero vector is 0")
    func zeroLength() {
        #expect(Vec2.zero.length == 0.0)
    }

    @Test("Length of unit vectors is 1")
    func unitLength() {
        #expect(IsApproxEqual(Vec2.right.length, 1.0))
        #expect(IsApproxEqual(Vec2.up.length, 1.0))
    }

    @Test("lengthSquared of (3, 4) is 25")
    func lengthSquared() {
        #expect(IsApproxEqual(Vec2(3.0, 4.0).lengthSquared, 25.0))
    }

    @Test("lengthSquared equals length squared within tolerance")
    func lengthSquaredConsistency() {
        let v = Vec2(3.0, 7.0)
        #expect(IsApproxEqual(v.lengthSquared, v.length * v.length, epsilon: 1e-4))
    }
}

@Suite("Vec2 — Normalize")
struct Vec2NormalizeTests {

    @Test("Normalized vector has length 1")
    func normalizedLength() {
        #expect(IsApproxEqual(Vec2(3.0, 4.0).normalized.length, 1.0))
    }

    @Test("Normalizing a unit vector returns itself")
    func normalizeUnitVector() {
        #expect(Vec2.isApproxEqual(Vec2.right.normalized, Vec2.right))
        #expect(Vec2.isApproxEqual(Vec2.up.normalized, Vec2.up))
    }

    @Test("Normalizing zero vector returns zero safely")
    func normalizeZeroVector() {
        #expect(Vec2.zero.normalized == Vec2.zero)
    }

    @Test("Direction is preserved after normalization")
    func directionPreserved() {
        let v = Vec2(3.0, 4.0)
        #expect(IsApproxEqual(Vec2.dot(v.normalized, v.normalized), 1.0))
    }
}

@Suite("Vec2 — Dot Product")
struct Vec2DotTests {

    @Test("Parallel vectors have positive dot product")
    func parallel() {
        #expect(Vec2.dot(Vec2.right, Vec2.right) > 0)
    }

    @Test("Perpendicular vectors have dot product of 0")
    func perpendicular() {
        #expect(IsApproxEqual(Vec2.dot(Vec2.right, Vec2.up), 0.0))
    }

    @Test("Opposite vectors have negative dot product")
    func opposite() {
        #expect(Vec2.dot(Vec2.right, -Vec2.right) < 0)
    }

    @Test("Dot product is commutative")
    func commutative() {
        let a = Vec2(2.0, 3.0)
        let b = Vec2(4.0, 5.0)
        #expect(IsApproxEqual(Vec2.dot(a, b), Vec2.dot(b, a)))
    }

    @Test("Known dot product value — (1,2)·(3,4) = 11")
    func knownValue() {
        #expect(IsApproxEqual(Vec2.dot(Vec2(1.0, 2.0), Vec2(3.0, 4.0)), 11.0))
    }
}

@Suite("Vec2 — Distance")
struct Vec2DistanceTests {

    @Test("Distance between identical points is 0")
    func samePoint() {
        #expect(Vec2.distance(Vec2(1.0, 1.0), Vec2(1.0, 1.0)) == 0.0)
    }

    @Test("Distance from origin to (3, 4) is 5")
    func knownDistance() {
        #expect(IsApproxEqual(Vec2.distance(.zero, Vec2(3.0, 4.0)), 5.0))
    }

    @Test("Distance is symmetric")
    func symmetric() {
        let a = Vec2(1.0, 2.0)
        let b = Vec2(4.0, 6.0)
        #expect(IsApproxEqual(Vec2.distance(a, b), Vec2.distance(b, a)))
    }

    @Test("Distance is always non-negative")
    func nonNegative() {
        #expect(Vec2.distance(Vec2(5.0, 5.0), Vec2(1.0, 1.0)) >= 0)
    }
}

@Suite("Vec2 — Lerp")
struct Vec2LerpTests {

    @Test("t=0 returns a")
    func tZero() {
        #expect(Vec2.isApproxEqual(Vec2.lerp(.zero, Vec2(10.0, 20.0), t: 0.0), .zero))
    }

    @Test("t=1 returns b")
    func tOne() {
        #expect(Vec2.isApproxEqual(Vec2.lerp(.zero, Vec2(10.0, 20.0), t: 1.0), Vec2(10.0, 20.0)))
    }

    @Test("t=0.5 returns midpoint")
    func tHalf() {
        #expect(Vec2.isApproxEqual(Vec2.lerp(.zero, Vec2(10.0, 20.0), t: 0.5), Vec2(5.0, 10.0)))
    }

    @Test("Extrapolates beyond range when t > 1")
    func extrapolate() {
        #expect(Vec2.isApproxEqual(Vec2.lerp(.zero, Vec2(10.0, 10.0), t: 2.0), Vec2(20.0, 20.0)))
    }
}

@Suite("Vec2 — Perpendicular")
struct Vec2PerpendicularTests {

    @Test("Perpendicular of right is up")
    func rightIsUp() {
        #expect(Vec2.isApproxEqual(Vec2.right.perpendicular, Vec2.up))
    }

    @Test("Perpendicular dot original is zero")
    func dotIsZero() {
        let v = Vec2(3.0, 4.0)
        #expect(IsApproxEqual(Vec2.dot(v, v.perpendicular), 0.0))
    }

    @Test("Perpendicular preserves length")
    func preservesLength() {
        let v = Vec2(3.0, 4.0)
        #expect(IsApproxEqual(v.perpendicular.length, v.length))
    }

    @Test("Double perpendicular returns negated original")
    func doublePerpendicular() {
        let v = Vec2(3.0, 4.0)
        #expect(Vec2.isApproxEqual(v.perpendicular.perpendicular, -v))
    }
}

@Suite("Vec2 — Angle")
struct Vec2AngleTests {

    @Test("Right vector has angle 0")
    func rightAngle() {
        #expect(IsApproxEqual(Vec2.right.angle, 0.0))
    }

    @Test("Up vector has angle π/2")
    func upAngle() {
        #expect(IsApproxEqual(Vec2.up.angle, Float.pi / 2))
    }

    @Test("Negative x vector has angle π")
    func leftAngle() {
        #expect(IsApproxEqual(abs(Vec2(-1.0, 0.0).angle), Float.pi))
    }

    @Test("Down vector has angle -π/2")
    func downAngle() {
        #expect(IsApproxEqual(Vec2(0.0, -1.0).angle, -Float.pi / 2))
    }
}

@Suite("Vec2 — Operators")
struct Vec2OperatorTests {

    @Test("Addition is component-wise")
    func addition() {
        #expect(Vec2.isApproxEqual(Vec2(1, 2) + Vec2(3, 4), Vec2(4, 6)))
    }

    @Test("Subtraction is component-wise")
    func subtraction() {
        #expect(Vec2.isApproxEqual(Vec2(5, 6) - Vec2(1, 2), Vec2(4, 4)))
    }

    @Test("Scalar multiplication — vec * float")
    func scalarMultiplyRight() {
        #expect(Vec2.isApproxEqual(Vec2(2, 3) * 4.0, Vec2(8, 12)))
    }

    @Test("Scalar multiplication — float * vec")
    func scalarMultiplyLeft() {
        #expect(Vec2.isApproxEqual(4.0 * Vec2(2, 3), Vec2(8, 12)))
    }

    @Test("Scalar division divides all components")
    func scalarDivision() {
        #expect(Vec2.isApproxEqual(Vec2(8, 12) / 4.0, Vec2(2, 3)))
    }

    @Test("Division by zero returns zero safely")
    func divisionByZero() {
        #expect(Vec2(8, 12) / 0.0 == .zero)
    }

    @Test("Negation flips both components")
    func negation() {
        #expect(Vec2.isApproxEqual(-Vec2(3.0, -4.0), Vec2(-3.0, 4.0)))
    }

    @Test("Negation of zero is zero")
    func negateZero() {
        #expect(-Vec2.zero == Vec2.zero)
    }

    @Test("+= mutates in place")
    func addAssign() {
        var v = Vec2(1, 2); v += Vec2(3, 4)
        #expect(Vec2.isApproxEqual(v, Vec2(4, 6)))
    }

    @Test("-= mutates in place")
    func subtractAssign() {
        var v = Vec2(5, 6); v -= Vec2(1, 2)
        #expect(Vec2.isApproxEqual(v, Vec2(4, 4)))
    }

    @Test("*= mutates in place")
    func multiplyAssign() {
        var v = Vec2(2, 3); v *= 3.0
        #expect(Vec2.isApproxEqual(v, Vec2(6, 9)))
    }

    @Test("/= mutates in place")
    func divideAssign() {
        var v = Vec2(6, 9); v /= 3.0
        #expect(Vec2.isApproxEqual(v, Vec2(2, 3)))
    }
}

@Suite("Vec2 — isApproxEqual")
struct Vec2ApproxEqualTests {

    @Test("Identical vectors are approx equal")
    func identical() {
        #expect(Vec2.isApproxEqual(Vec2(1, 2), Vec2(1, 2)))
    }

    @Test("Vectors within epsilon are approx equal")
    func withinEpsilon() {
        let a = Vec2(1.0, 2.0)
        let b = Vec2(1.0 + IsEpsilon * 0.5, 2.0 + IsEpsilon * 0.5)
        #expect(Vec2.isApproxEqual(a, b))
    }

    @Test("Vectors outside epsilon are not approx equal")
    func outsideEpsilon() {
        #expect(!Vec2.isApproxEqual(Vec2(1, 2), Vec2(1.1, 2.1)))
    }

    @Test("Custom epsilon is respected")
    func customEpsilon() {
        #expect(Vec2.isApproxEqual(Vec2(1, 2), Vec2(1.05, 2.05), epsilon: 0.1))
        #expect(!Vec2.isApproxEqual(Vec2(1, 2), Vec2(1.2, 2.2), epsilon: 0.1))
    }
}

// MARK: ═══════════════════════════════════════════════════════════════════════
// VEC3
// MARK: ═══════════════════════════════════════════════════════════════════════

@Suite("Vec3 — Init & Storage")
struct Vec3InitTests {

    @Test("Primary init stores correct components")
    func primaryInit() {
        let v = Vec3(1.0, 2.0, 3.0)
        #expect(v.x == 1.0)
        #expect(v.y == 2.0)
        #expect(v.z == 3.0)
    }

    @Test("Splat init fills all three components")
    func splatInit() {
        let v = Vec3(5.0)
        #expect(v.x == 5.0)
        #expect(v.y == 5.0)
        #expect(v.z == 5.0)
    }

    @Test("SIMD init stores correct components")
    func simdInit() {
        let simd = SIMD3<Float>(7.0, 8.0, 9.0)
        let v = Vec3(simd)
        #expect(v.x == 7.0)
        #expect(v.y == 8.0)
        #expect(v.z == 9.0)
    }

    @Test("Component assignment mutates correctly")
    func componentAssignment() {
        var v = Vec3(1.0, 2.0, 3.0)
        v.x = 10.0; v.y = 20.0; v.z = 30.0
        #expect(v.x == 10.0)
        #expect(v.y == 20.0)
        #expect(v.z == 30.0)
    }
}

@Suite("Vec3 — Constants")
struct Vec3ConstantTests {

    @Test("zero is (0, 0, 0)")
    func zero() {
        #expect(Vec3.zero == Vec3(0, 0, 0))
    }

    @Test("one is (1, 1, 1)")
    func one() {
        #expect(Vec3.one == Vec3(1, 1, 1))
    }

    @Test("Axis constants have correct values")
    func axisValues() {
        #expect(Vec3.right   == Vec3( 1,  0,  0))
        #expect(Vec3.left    == Vec3(-1,  0,  0))
        #expect(Vec3.up      == Vec3( 0,  1,  0))
        #expect(Vec3.down    == Vec3( 0, -1,  0))
        #expect(Vec3.forward == Vec3( 0,  0,  1))
        #expect(Vec3.back    == Vec3( 0,  0, -1))
    }

    @Test("Axis constants are unit length")
    func axisLengths() {
        for axis in [Vec3.right, Vec3.left, Vec3.up, Vec3.down, Vec3.forward, Vec3.back] {
            #expect(IsApproxEqual(axis.length, 1.0))
        }
    }

    @Test("Opposite axis constants are negations of each other")
    func oppositeAxes() {
        #expect(Vec3.isApproxEqual(-Vec3.right,   Vec3.left))
        #expect(Vec3.isApproxEqual(-Vec3.up,      Vec3.down))
        #expect(Vec3.isApproxEqual(-Vec3.forward, Vec3.back))
    }
}

@Suite("Vec3 — Equatable")
struct Vec3EquatableTests {

    @Test("Identical vectors are equal")
    func identical() {
        #expect(Vec3(1, 2, 3) == Vec3(1, 2, 3))
    }

    @Test("Vectors within epsilon are equal")
    func withinEpsilon() {
        let a = Vec3(1.0, 2.0, 3.0)
        let b = Vec3(1.0 + IsEpsilon * 0.5, 2.0 + IsEpsilon * 0.5, 3.0 + IsEpsilon * 0.5)
        #expect(a == b)
    }

    @Test("Vectors outside epsilon are not equal")
    func outsideEpsilon() {
        #expect(Vec3(1, 2, 3) != Vec3(1.1, 2, 3))
    }

    @Test("Vectors differing only in z are not equal")
    func differInZ() {
        #expect(Vec3(1, 2, 3) != Vec3(1, 2, 9))
    }
}

@Suite("Vec3 — Description")
struct Vec3DescriptionTests {

    @Test("Description format is correct")
    func descriptionFormat() {
        #expect(Vec3(1.0, 2.0, 3.0).description == "Vec3(1.0, 2.0, 3.0)")
    }
}

@Suite("Vec3 — Length")
struct Vec3LengthTests {

    @Test("Length of (2, 3, 6) is 7")
    func knownLength() {
        #expect(IsApproxEqual(Vec3(2, 3, 6).length, 7.0))
    }

    @Test("Length of zero vector is 0")
    func zeroLength() {
        #expect(Vec3.zero.length == 0.0)
    }

    @Test("lengthSquared of (2, 3, 6) is 49")
    func lengthSquared() {
        #expect(IsApproxEqual(Vec3(2, 3, 6).lengthSquared, 49.0))
    }

    @Test("lengthSquared equals length squared within tolerance")
    func lengthSquaredConsistency() {
        let v = Vec3(3, 5, 7)
        #expect(IsApproxEqual(v.lengthSquared, v.length * v.length, epsilon: 1e-4))
    }
}

@Suite("Vec3 — Normalize")
struct Vec3NormalizeTests {

    @Test("Normalized vector has length 1")
    func normalizedLength() {
        #expect(IsApproxEqual(Vec3(2, 3, 6).normalized.length, 1.0))
    }

    @Test("Normalizing axis constants returns themselves")
    func normalizeAxisConstants() {
        #expect(Vec3.isApproxEqual(Vec3.right.normalized,   Vec3.right))
        #expect(Vec3.isApproxEqual(Vec3.up.normalized,      Vec3.up))
        #expect(Vec3.isApproxEqual(Vec3.forward.normalized, Vec3.forward))
    }

    @Test("Normalizing zero vector returns zero safely")
    func normalizeZeroVector() {
        #expect(Vec3.zero.normalized == Vec3.zero)
    }
}

@Suite("Vec3 — Dot Product")
struct Vec3DotTests {

    @Test("Perpendicular axis vectors have dot product of 0")
    func perpendicularAxes() {
        #expect(IsApproxEqual(Vec3.dot(Vec3.right,   Vec3.up),      0.0))
        #expect(IsApproxEqual(Vec3.dot(Vec3.right,   Vec3.forward), 0.0))
        #expect(IsApproxEqual(Vec3.dot(Vec3.up,      Vec3.forward), 0.0))
    }

    @Test("Dot product is commutative")
    func commutative() {
        let a = Vec3(1, 2, 3)
        let b = Vec3(4, 5, 6)
        #expect(IsApproxEqual(Vec3.dot(a, b), Vec3.dot(b, a)))
    }

    @Test("Known dot product value — (1,2,3)·(4,5,6) = 32")
    func knownValue() {
        #expect(IsApproxEqual(Vec3.dot(Vec3(1, 2, 3), Vec3(4, 5, 6)), 32.0))
    }
}

@Suite("Vec3 — Cross Product")
struct Vec3CrossTests {

    @Test("cross(right, up) = forward — defines coordinate system")
    func rightCrossUpIsForward() {
        #expect(Vec3.isApproxEqual(Vec3.cross(Vec3.right, Vec3.up), Vec3.forward))
    }

    @Test("cross(up, right) = back — anti-commutative")
    func upCrossRightIsBack() {
        #expect(Vec3.isApproxEqual(Vec3.cross(Vec3.up, Vec3.right), Vec3.back))
    }

    @Test("cross(a, b) = -cross(b, a)")
    func antiCommutative() {
        let a = Vec3(1, 2, 3)
        let b = Vec3(4, 5, 6)
        #expect(Vec3.isApproxEqual(Vec3.cross(a, b), -Vec3.cross(b, a)))
    }

    @Test("Result is perpendicular to both inputs")
    func resultIsPerpendicular() {
        let a = Vec3(1, 2, 3)
        let b = Vec3(4, 5, 6)
        let c = Vec3.cross(a, b)
        #expect(IsApproxEqual(Vec3.dot(c, a), 0.0))
        #expect(IsApproxEqual(Vec3.dot(c, b), 0.0))
    }

    @Test("Cross product of parallel vectors is zero")
    func parallelVectorsGiveZero() {
        #expect(Vec3.isApproxEqual(Vec3.cross(Vec3.right, Vec3.right), Vec3.zero))
    }

    @Test("cross(up, forward) = right")
    func upCrossForwardIsRight() {
        #expect(Vec3.isApproxEqual(Vec3.cross(Vec3.up, Vec3.forward), Vec3.right))
    }
}

@Suite("Vec3 — Distance")
struct Vec3DistanceTests {

    @Test("Distance between identical points is 0")
    func samePoint() {
        #expect(Vec3.distance(Vec3(1, 2, 3), Vec3(1, 2, 3)) == 0.0)
    }

    @Test("Distance from origin to (2, 3, 6) is 7")
    func knownDistance() {
        #expect(IsApproxEqual(Vec3.distance(.zero, Vec3(2, 3, 6)), 7.0))
    }

    @Test("Distance is symmetric")
    func symmetric() {
        let a = Vec3(1, 2, 3)
        let b = Vec3(4, 6, 3)
        #expect(IsApproxEqual(Vec3.distance(a, b), Vec3.distance(b, a)))
    }
}

@Suite("Vec3 — Lerp")
struct Vec3LerpTests {

    @Test("t=0 returns a")
    func tZero() {
        #expect(Vec3.isApproxEqual(Vec3.lerp(Vec3(1, 2, 3), Vec3(10, 20, 30), t: 0.0), Vec3(1, 2, 3)))
    }

    @Test("t=1 returns b")
    func tOne() {
        #expect(Vec3.isApproxEqual(Vec3.lerp(Vec3(1, 2, 3), Vec3(10, 20, 30), t: 1.0), Vec3(10, 20, 30)))
    }

    @Test("t=0.5 returns midpoint")
    func tHalf() {
        #expect(Vec3.isApproxEqual(Vec3.lerp(.zero, Vec3(10, 20, 30), t: 0.5), Vec3(5, 10, 15)))
    }

    @Test("Extrapolates beyond range when t > 1")
    func extrapolate() {
        #expect(Vec3.isApproxEqual(Vec3.lerp(.zero, Vec3(10, 10, 10), t: 2.0), Vec3(20, 20, 20)))
    }
}

@Suite("Vec3 — Reflect")
struct Vec3ReflectTests {

    @Test("Reflecting off a floor flips the y component")
    func reflectOffFloor() {
        let result = Vec3.reflect(Vec3(1, -1, 0), normal: Vec3.up)
        #expect(Vec3.isApproxEqual(result, Vec3(1, 1, 0)))
    }

    @Test("Reflecting off a wall flips the x component")
    func reflectOffWall() {
        let result = Vec3.reflect(Vec3(1, 0, 0), normal: Vec3.right)
        #expect(Vec3.isApproxEqual(result, Vec3(-1, 0, 0)))
    }

    @Test("Reflected vector has same length as input")
    func preservesLength() {
        let v = Vec3(1, -2, 3)
        let result = Vec3.reflect(v, normal: Vec3.up)
        #expect(IsApproxEqual(result.length, v.length))
    }

    @Test("Reflecting a vector parallel to surface leaves it unchanged")
    func reflectParallelToSurface() {
        let v = Vec3(1, 0, 0)
        #expect(Vec3.isApproxEqual(Vec3.reflect(v, normal: Vec3.up), v))
    }
}

@Suite("Vec3 — Operators")
struct Vec3OperatorTests {

    @Test("Addition is component-wise")
    func addition() {
        #expect(Vec3.isApproxEqual(Vec3(1, 2, 3) + Vec3(4, 5, 6), Vec3(5, 7, 9)))
    }

    @Test("Subtraction is component-wise")
    func subtraction() {
        #expect(Vec3.isApproxEqual(Vec3(5, 7, 9) - Vec3(1, 2, 3), Vec3(4, 5, 6)))
    }

    @Test("Scalar multiplication — vec * float")
    func scalarMultiplyRight() {
        #expect(Vec3.isApproxEqual(Vec3(1, 2, 3) * 2.0, Vec3(2, 4, 6)))
    }

    @Test("Scalar multiplication — float * vec")
    func scalarMultiplyLeft() {
        #expect(Vec3.isApproxEqual(2.0 * Vec3(1, 2, 3), Vec3(2, 4, 6)))
    }

    @Test("Scalar division divides all components")
    func scalarDivision() {
        #expect(Vec3.isApproxEqual(Vec3(2, 4, 6) / 2.0, Vec3(1, 2, 3)))
    }

    @Test("Division by zero returns zero safely")
    func divisionByZero() {
        #expect(Vec3(1, 2, 3) / 0.0 == .zero)
    }

    @Test("Negation flips all three components")
    func negation() {
        #expect(Vec3.isApproxEqual(-Vec3(1, -2, 3), Vec3(-1, 2, -3)))
    }

    @Test("+= mutates in place")
    func addAssign() {
        var v = Vec3(1, 2, 3); v += Vec3(4, 5, 6)
        #expect(Vec3.isApproxEqual(v, Vec3(5, 7, 9)))
    }

    @Test("-= mutates in place")
    func subtractAssign() {
        var v = Vec3(5, 7, 9); v -= Vec3(1, 2, 3)
        #expect(Vec3.isApproxEqual(v, Vec3(4, 5, 6)))
    }

    @Test("*= mutates in place")
    func multiplyAssign() {
        var v = Vec3(1, 2, 3); v *= 3.0
        #expect(Vec3.isApproxEqual(v, Vec3(3, 6, 9)))
    }

    @Test("/= mutates in place")
    func divideAssign() {
        var v = Vec3(3, 6, 9); v /= 3.0
        #expect(Vec3.isApproxEqual(v, Vec3(1, 2, 3)))
    }
}

@Suite("Vec3 — isApproxEqual")
struct Vec3ApproxEqualTests {

    @Test("Identical vectors are approx equal")
    func identical() {
        #expect(Vec3.isApproxEqual(Vec3(1, 2, 3), Vec3(1, 2, 3)))
    }

    @Test("Vectors within epsilon are approx equal")
    func withinEpsilon() {
        let a = Vec3(1.0, 2.0, 3.0)
        let b = Vec3(1.0 + IsEpsilon * 0.5, 2.0 + IsEpsilon * 0.5, 3.0 + IsEpsilon * 0.5)
        #expect(Vec3.isApproxEqual(a, b))
    }

    @Test("Vectors outside epsilon are not approx equal")
    func outsideEpsilon() {
        #expect(!Vec3.isApproxEqual(Vec3(1, 2, 3), Vec3(1.1, 2, 3)))
    }

    @Test("Custom epsilon is respected")
    func customEpsilon() {
        #expect(Vec3.isApproxEqual(Vec3(1, 2, 3), Vec3(1.05, 2.05, 3.05), epsilon: 0.1))
        #expect(!Vec3.isApproxEqual(Vec3(1, 2, 3), Vec3(1.2, 2, 3), epsilon: 0.1))
    }
}

// MARK: ═══════════════════════════════════════════════════════════════════════
// VEC4
// MARK: ═══════════════════════════════════════════════════════════════════════

@Suite("Vec4 — Init & Storage")
struct Vec4InitTests {

    @Test("Primary init stores correct components")
    func primaryInit() {
        let v = Vec4(1.0, 2.0, 3.0, 4.0)
        #expect(v.x == 1.0)
        #expect(v.y == 2.0)
        #expect(v.z == 3.0)
        #expect(v.w == 4.0)
    }

    @Test("Splat init fills all four components")
    func splatInit() {
        let v = Vec4(5.0)
        #expect(v.x == 5.0)
        #expect(v.y == 5.0)
        #expect(v.z == 5.0)
        #expect(v.w == 5.0)
    }

    @Test("SIMD init stores correct components")
    func simdInit() {
        let simd = SIMD4<Float>(1.0, 2.0, 3.0, 4.0)
        let v = Vec4(simd)
        #expect(v.x == 1.0)
        #expect(v.y == 2.0)
        #expect(v.z == 3.0)
        #expect(v.w == 4.0)
    }

    @Test("Vec3 + w init stores correct components")
    func vec3WInit() {
        let v = Vec4(Vec3(1.0, 2.0, 3.0), w: 4.0)
        #expect(v.x == 1.0)
        #expect(v.y == 2.0)
        #expect(v.z == 3.0)
        #expect(v.w == 4.0)
    }

    @Test("Component assignment mutates correctly")
    func componentAssignment() {
        var v = Vec4(1.0, 2.0, 3.0, 4.0)
        v.x = 10.0; v.y = 20.0; v.z = 30.0; v.w = 40.0
        #expect(v.x == 10.0)
        #expect(v.y == 20.0)
        #expect(v.z == 30.0)
        #expect(v.w == 40.0)
    }
}

@Suite("Vec4 — Color Aliases")
struct Vec4ColorAliasTests {

    @Test("r/g/b/a aliases map to x/y/z/w")
    func aliasesMapCorrectly() {
        let v = Vec4(0.1, 0.2, 0.3, 0.4)
        #expect(v.r == v.x)
        #expect(v.g == v.y)
        #expect(v.b == v.z)
        #expect(v.a == v.w)
    }

    @Test("Writing via r/g/b/a mutates x/y/z/w")
    func aliasMutation() {
        var v = Vec4(0.0, 0.0, 0.0, 0.0)
        v.r = 1.0; v.g = 0.5; v.b = 0.25; v.a = 0.75
        #expect(v.x == 1.0)
        #expect(v.y == 0.5)
        #expect(v.z == 0.25)
        #expect(v.w == 0.75)
    }
}

@Suite("Vec4 — xyz Extraction")
struct Vec4XYZTests {

    @Test("xyz returns correct Vec3")
    func xyzCorrect() {
        let v = Vec4(3.0, 4.0, 5.0, 1.0)
        #expect(v.xyz == Vec3(3.0, 4.0, 5.0))
    }

    @Test("xyz extraction ignores w")
    func ignoresW() {
        let a = Vec4(1.0, 2.0, 3.0, 0.0)
        let b = Vec4(1.0, 2.0, 3.0, 99.0)
        #expect(a.xyz == b.xyz)
    }

    @Test("xyz on zero vector returns Vec3.zero")
    func xyzOfZero() {
        #expect(Vec4.zero.xyz == Vec3.zero)
    }
}

@Suite("Vec4 — Semantic Constructors")
struct Vec4SemanticConstructorTests {

    @Test("point(_:_:_:) sets w to 1.0")
    func pointScalar() {
        let p = Vec4.point(1.0, 2.0, 3.0)
        #expect(p.x == 1.0)
        #expect(p.y == 2.0)
        #expect(p.z == 3.0)
        #expect(p.w == 1.0)
    }

    @Test("point(_ v:) sets w to 1.0")
    func pointVec3() {
        let p = Vec4.point(Vec3(4.0, 5.0, 6.0))
        #expect(p.xyz == Vec3(4.0, 5.0, 6.0))
        #expect(p.w == 1.0)
    }

    @Test("direction(_:_:_:) sets w to 0.0")
    func directionScalar() {
        let d = Vec4.direction(1.0, 0.0, 0.0)
        #expect(d.x == 1.0)
        #expect(d.w == 0.0)
    }

    @Test("direction(_ v:) sets w to 0.0")
    func directionVec3() {
        let d = Vec4.direction(Vec3(0.0, 1.0, 0.0))
        #expect(d.xyz == Vec3(0.0, 1.0, 0.0))
        #expect(d.w == 0.0)
    }

    @Test("point and direction differ only in w")
    func pointVsDirection() {
        let p = Vec4.point(Vec3(1.0, 2.0, 3.0))
        let d = Vec4.direction(Vec3(1.0, 2.0, 3.0))
        #expect(p.xyz == d.xyz)
        #expect(p.w != d.w)
    }
}

@Suite("Vec4 — Constants")
struct Vec4ConstantTests {

    @Test("zero is (0, 0, 0, 0)")
    func zero() {
        #expect(Vec4.zero == Vec4(0, 0, 0, 0))
    }

    @Test("one is (1, 1, 1, 1)")
    func one() {
        #expect(Vec4.one == Vec4(1, 1, 1, 1))
    }

    @Test("white is (1, 1, 1, 1)")
    func white() {
        #expect(Vec4.white == Vec4(1, 1, 1, 1))
    }

    @Test("black is (0, 0, 0, 1)")
    func black() {
        #expect(Vec4.black == Vec4(0, 0, 0, 1))
    }

    @Test("red is (1, 0, 0, 1)")
    func red() {
        #expect(Vec4.red == Vec4(1, 0, 0, 1))
    }

    @Test("green is (0, 1, 0, 1)")
    func green() {
        #expect(Vec4.green == Vec4(0, 1, 0, 1))
    }

    @Test("blue is (0, 0, 1, 1)")
    func blue() {
        #expect(Vec4.blue == Vec4(0, 0, 1, 1))
    }

    @Test("clear is (0, 0, 0, 0)")
    func clear() {
        #expect(Vec4.clear == Vec4(0, 0, 0, 0))
    }

    @Test("Named color constants are fully opaque except clear")
    func opaqueColors() {
        #expect(Vec4.white.a  == 1.0)
        #expect(Vec4.black.a  == 1.0)
        #expect(Vec4.red.a    == 1.0)
        #expect(Vec4.green.a  == 1.0)
        #expect(Vec4.blue.a   == 1.0)
        #expect(Vec4.clear.a  == 0.0)
    }
}

@Suite("Vec4 — Equatable")
struct Vec4EquatableTests {

    @Test("Identical vectors are equal")
    func identical() {
        #expect(Vec4(1, 2, 3, 4) == Vec4(1, 2, 3, 4))
    }

    @Test("Vectors within epsilon are equal")
    func withinEpsilon() {
        let a = Vec4(1.0, 2.0, 3.0, 4.0)
        let b = Vec4(
            1.0 + IsEpsilon * 0.5,
            2.0 + IsEpsilon * 0.5,
            3.0 + IsEpsilon * 0.5,
            4.0 + IsEpsilon * 0.5
        )
        #expect(a == b)
    }

    @Test("Vectors outside epsilon are not equal")
    func outsideEpsilon() {
        #expect(Vec4(1, 2, 3, 4) != Vec4(1.1, 2, 3, 4))
    }

    @Test("Vectors differing only in w are not equal")
    func differInW() {
        #expect(Vec4(1, 2, 3, 0) != Vec4(1, 2, 3, 1))
    }
}

@Suite("Vec4 — Description")
struct Vec4DescriptionTests {

    @Test("Description format is correct")
    func descriptionFormat() {
        #expect(Vec4(1.0, 2.0, 3.0, 4.0).description == "Vec4(1.0, 2.0, 3.0, 4.0)")
    }
}

@Suite("Vec4 — Length")
struct Vec4LengthTests {

    @Test("Length of (0, 0, 0, 1) is 1")
    func pureWLength() {
        #expect(IsApproxEqual(Vec4(0, 0, 0, 1).length, 1.0))
    }

    @Test("Length of (1, 0, 0, 0) is 1")
    func pureXLength() {
        #expect(IsApproxEqual(Vec4(1, 0, 0, 0).length, 1.0))
    }

    @Test("Length of (0, 0, 0, 0) is 0")
    func zeroLength() {
        #expect(Vec4.zero.length == 0.0)
    }

    @Test("w participates in length — (0,0,0,2) has length 2")
    func wParticipates() {
        #expect(IsApproxEqual(Vec4(0, 0, 0, 2).length, 2.0))
    }

    @Test("lengthSquared of (1, 2, 3, 4) is 30")
    func knownLengthSquared() {
        #expect(IsApproxEqual(Vec4(1, 2, 3, 4).lengthSquared, 30.0))
    }

    @Test("lengthSquared equals length squared within tolerance")
    func lengthSquaredConsistency() {
        let v = Vec4(1, 2, 3, 4)
        #expect(IsApproxEqual(v.lengthSquared, v.length * v.length, epsilon: 1e-4))
    }
}

@Suite("Vec4 — Normalize")
struct Vec4NormalizeTests {

    @Test("Normalized vector has length 1")
    func normalizedLength() {
        #expect(IsApproxEqual(Vec4(1, 2, 3, 4).normalized.length, 1.0))
    }

    @Test("Normalizing a unit x-axis vector returns itself")
    func normalizeUnitX() {
        #expect(Vec4.isApproxEqual(Vec4(1, 0, 0, 0).normalized, Vec4(1, 0, 0, 0)))
    }

    @Test("Normalizing a unit w-axis vector returns itself")
    func normalizeUnitW() {
        #expect(Vec4.isApproxEqual(Vec4(0, 0, 0, 1).normalized, Vec4(0, 0, 0, 1)))
    }

    @Test("Normalizing zero vector returns zero safely")
    func normalizeZeroVector() {
        #expect(Vec4.zero.normalized == Vec4.zero)
    }

    @Test("Direction is preserved after normalization")
    func directionPreserved() {
        let v = Vec4(1, 2, 3, 4)
        let n = v.normalized
        #expect(IsApproxEqual(Vec4.dot(n, n), 1.0))
    }
}

@Suite("Vec4 — Dot Product")
struct Vec4DotTests {

    @Test("Parallel vectors have dot product equal to 1 when normalized")
    func parallelNormalized() {
        let v = Vec4(1, 0, 0, 0)
        #expect(IsApproxEqual(Vec4.dot(v, v), 1.0))
    }

    @Test("Perpendicular vectors have dot product of 0")
    func perpendicular() {
        #expect(IsApproxEqual(Vec4.dot(Vec4(1, 0, 0, 0), Vec4(0, 1, 0, 0)), 0.0))
        #expect(IsApproxEqual(Vec4.dot(Vec4(1, 0, 0, 0), Vec4(0, 0, 0, 1)), 0.0))
    }

    @Test("w component participates in dot product")
    func wParticipates() {
        let a = Vec4(0, 0, 0, 2)
        let b = Vec4(0, 0, 0, 3)
        #expect(IsApproxEqual(Vec4.dot(a, b), 6.0))
    }

    @Test("Dot product is commutative")
    func commutative() {
        let a = Vec4(1, 2, 3, 4)
        let b = Vec4(5, 6, 7, 8)
        #expect(IsApproxEqual(Vec4.dot(a, b), Vec4.dot(b, a)))
    }

    @Test("Known value — (1,2,3,4)·(5,6,7,8) = 70")
    func knownValue() {
        #expect(IsApproxEqual(Vec4.dot(Vec4(1, 2, 3, 4), Vec4(5, 6, 7, 8)), 70.0))
    }
}

@Suite("Vec4 — Lerp")
struct Vec4LerpTests {

    @Test("t=0 returns a")
    func tZero() {
        #expect(Vec4.isApproxEqual(Vec4.lerp(.zero, Vec4.one, t: 0.0), .zero))
    }

    @Test("t=1 returns b")
    func tOne() {
        #expect(Vec4.isApproxEqual(Vec4.lerp(.zero, Vec4.one, t: 1.0), .one))
    }

    @Test("t=0.5 returns midpoint")
    func tHalf() {
        let mid = Vec4.lerp(Vec4.black, Vec4.white, t: 0.5)
        // black=(0,0,0,1) white=(1,1,1,1) → mid=(0.5,0.5,0.5,1.0)
        #expect(IsApproxEqual(mid.r, 0.5))
        #expect(IsApproxEqual(mid.g, 0.5))
        #expect(IsApproxEqual(mid.b, 0.5))
        #expect(IsApproxEqual(mid.a, 1.0))
    }

    @Test("Color lerp — red to blue at t=0.5 gives equal r and b")
    func colorLerp() {
        let mid = Vec4.lerp(Vec4.red, Vec4.blue, t: 0.5)
        #expect(IsApproxEqual(mid.r, 0.5))
        #expect(IsApproxEqual(mid.b, 0.5))
        #expect(IsApproxEqual(mid.g, 0.0))
    }

    @Test("Extrapolates beyond range when t > 1")
    func extrapolate() {
        let result = Vec4.lerp(.zero, Vec4.one, t: 2.0)
        #expect(IsApproxEqual(result.x, 2.0))
    }

    @Test("w component lerps correctly")
    func wLerps() {
        let a = Vec4(0, 0, 0, 0)
        let b = Vec4(0, 0, 0, 1)
        let mid = Vec4.lerp(a, b, t: 0.5)
        #expect(IsApproxEqual(mid.w, 0.5))
    }
}

@Suite("Vec4 — Operators")
struct Vec4OperatorTests {

    @Test("Addition is component-wise")
    func addition() {
        #expect(Vec4.isApproxEqual(
            Vec4(1, 2, 3, 4) + Vec4(4, 3, 2, 1),
            Vec4(5, 5, 5, 5)
        ))
    }

    @Test("Subtraction is component-wise")
    func subtraction() {
        #expect(Vec4.isApproxEqual(
            Vec4(5, 5, 5, 5) - Vec4(1, 2, 3, 4),
            Vec4(4, 3, 2, 1)
        ))
    }

    @Test("Scalar multiplication — vec * float")
    func scalarMultiplyRight() {
        #expect(Vec4.isApproxEqual(Vec4(1, 2, 3, 4) * 2.0, Vec4(2, 4, 6, 8)))
    }

    @Test("Scalar multiplication — float * vec")
    func scalarMultiplyLeft() {
        #expect(Vec4.isApproxEqual(2.0 * Vec4(1, 2, 3, 4), Vec4(2, 4, 6, 8)))
    }

    @Test("Scalar division divides all components")
    func scalarDivision() {
        #expect(Vec4.isApproxEqual(Vec4(2, 4, 6, 8) / 2.0, Vec4(1, 2, 3, 4)))
    }

    @Test("Division by zero returns zero safely")
    func divisionByZero() {
        #expect(Vec4(1, 2, 3, 4) / 0.0 == .zero)
    }

    @Test("Negation flips all four components")
    func negation() {
        #expect(Vec4.isApproxEqual(-Vec4(1, -2, 3, -4), Vec4(-1, 2, -3, 4)))
    }

    @Test("Negation of zero is zero")
    func negateZero() {
        #expect(-Vec4.zero == Vec4.zero)
    }

    @Test("+= mutates in place")
    func addAssign() {
        var v = Vec4(1, 2, 3, 4); v += Vec4(4, 3, 2, 1)
        #expect(Vec4.isApproxEqual(v, Vec4(5, 5, 5, 5)))
    }

    @Test("-= mutates in place")
    func subtractAssign() {
        var v = Vec4(5, 5, 5, 5); v -= Vec4(1, 2, 3, 4)
        #expect(Vec4.isApproxEqual(v, Vec4(4, 3, 2, 1)))
    }

    @Test("*= mutates in place")
    func multiplyAssign() {
        var v = Vec4(1, 2, 3, 4); v *= 2.0
        #expect(Vec4.isApproxEqual(v, Vec4(2, 4, 6, 8)))
    }

    @Test("/= mutates in place")
    func divideAssign() {
        var v = Vec4(2, 4, 6, 8); v /= 2.0
        #expect(Vec4.isApproxEqual(v, Vec4(1, 2, 3, 4)))
    }
}

@Suite("Vec4 — isApproxEqual")
struct Vec4ApproxEqualTests {

    @Test("Identical vectors are approx equal")
    func identical() {
        #expect(Vec4.isApproxEqual(Vec4(1, 2, 3, 4), Vec4(1, 2, 3, 4)))
    }

    @Test("Vectors within epsilon are approx equal")
    func withinEpsilon() {
        let a = Vec4(1.0, 2.0, 3.0, 4.0)
        let b = Vec4(
            1.0 + IsEpsilon * 0.5,
            2.0 + IsEpsilon * 0.5,
            3.0 + IsEpsilon * 0.5,
            4.0 + IsEpsilon * 0.5
        )
        #expect(Vec4.isApproxEqual(a, b))
    }

    @Test("Vectors outside epsilon are not approx equal")
    func outsideEpsilon() {
        #expect(!Vec4.isApproxEqual(Vec4(1, 2, 3, 4), Vec4(1.1, 2, 3, 4)))
    }

    @Test("w difference outside epsilon makes vectors not equal")
    func wDifference() {
        #expect(!Vec4.isApproxEqual(Vec4(1, 2, 3, 0), Vec4(1, 2, 3, 1)))
    }

    @Test("Custom epsilon is respected")
    func customEpsilon() {
        #expect(Vec4.isApproxEqual(Vec4(1, 2, 3, 4), Vec4(1.05, 2.05, 3.05, 4.05), epsilon: 0.1))
        #expect(!Vec4.isApproxEqual(Vec4(1, 2, 3, 4), Vec4(1.2, 2, 3, 4), epsilon: 0.1))
    }
}
