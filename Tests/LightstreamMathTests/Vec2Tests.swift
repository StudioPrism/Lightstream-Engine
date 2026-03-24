// Copyright Studio Prism. Licensed under PolyForm Shield 1.0.0.
// https://polyformproject.org/licenses/shield/1.0.0
// Required Notice: Copyright Studio Prism (https://github.com/studioprism)
//
// Lightstream — LightstreamMathTests
// Vec2Tests.swift
//
// Tests for all public Vec2 functionality.

import Testing
@testable import LightstreamMath

// MARK: - Init & Storage

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

// MARK: - Static Constants

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

// MARK: - Equatable

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
        let a = Vec2(1.0, 2.0)
        let b = Vec2(1.1, 2.1)
        #expect(a != b)
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

// MARK: - Description

@Suite("Vec2 — Description")
struct Vec2DescriptionTests {

    @Test("Description format is correct")
    func descriptionFormat() {
        let v = Vec2(1.0, 2.0)
        #expect(v.description == "Vec2(1.0, 2.0)")
    }
}

// MARK: - Length

@Suite("Vec2 — Length")
struct Vec2LengthTests {

    @Test("Length of (3, 4) is 5 — classic 3-4-5 triangle")
    func knownLength() {
        let v = Vec2(3.0, 4.0)
        #expect(IsApproxEqual(v.length, 5.0))
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

    @Test("lengthSquared equals length squared")
    func lengthSquaredConsistency() {
        let v = Vec2(3.0, 7.0)
        #expect(IsApproxEqual(v.lengthSquared, v.length * v.length))
    }
}

// MARK: - Normalize

@Suite("Vec2 — Normalize")
struct Vec2NormalizeTests {

    @Test("Normalized vector has length 1")
    func normalizedLength() {
        let v = Vec2(3.0, 4.0).normalized
        #expect(IsApproxEqual(v.length, 1.0))
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

    @Test("Normalized direction is preserved")
    func directionPreserved() {
        let v = Vec2(3.0, 4.0)
        let n = v.normalized
        // Normalized and original should point the same way — dot == 1 when both normalized
        let dot = Vec2.dot(n, v.normalized)
        #expect(IsApproxEqual(dot, 1.0))
    }
}

// MARK: - Dot Product

@Suite("Vec2 — Dot Product")
struct Vec2DotTests {

    @Test("Parallel vectors have positive dot product")
    func parallel() {
        #expect(Vec2.dot(Vec2.right, Vec2.right) > 0)
    }

    @Test("Perpendicular vectors have dot product of 0")
    func perpendicular() {
        let dot = Vec2.dot(Vec2.right, Vec2.up)
        #expect(IsApproxEqual(dot, 0.0))
    }

    @Test("Opposite vectors have negative dot product")
    func opposite() {
        #expect(Vec2.dot(Vec2.right, -Vec2.right) < 0)
    }

    @Test("Dot of normalized vectors equals cosine of angle between them")
    func dotEqualsCosine() {
        // right and up are 90° apart — cos(90°) = 0
        let dot = Vec2.dot(Vec2.right.normalized, Vec2.up.normalized)
        #expect(IsApproxEqual(dot, 0.0))
    }

    @Test("Dot product is commutative")
    func commutative() {
        let a = Vec2(2.0, 3.0)
        let b = Vec2(4.0, 5.0)
        #expect(IsApproxEqual(Vec2.dot(a, b), Vec2.dot(b, a)))
    }

    @Test("Known dot product value")
    func knownValue() {
        // (1,2)·(3,4) = 1*3 + 2*4 = 3 + 8 = 11
        let dot = Vec2.dot(Vec2(1.0, 2.0), Vec2(3.0, 4.0))
        #expect(IsApproxEqual(dot, 11.0))
    }
}

// MARK: - Distance

@Suite("Vec2 — Distance")
struct Vec2DistanceTests {

    @Test("Distance between identical points is 0")
    func samePoint() {
        #expect(Vec2.distance(Vec2(1.0, 1.0), Vec2(1.0, 1.0)) == 0.0)
    }

    @Test("Distance from origin to (3, 4) is 5")
    func knownDistance() {
        let d = Vec2.distance(.zero, Vec2(3.0, 4.0))
        #expect(IsApproxEqual(d, 5.0))
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

// MARK: - Lerp

@Suite("Vec2 — Lerp")
struct Vec2LerpTests {

    @Test("t=0 returns a")
    func tZero() {
        let result = Vec2.lerp(Vec2(0.0, 0.0), Vec2(10.0, 20.0), t: 0.0)
        #expect(Vec2.isApproxEqual(result, Vec2(0.0, 0.0)))
    }

    @Test("t=1 returns b")
    func tOne() {
        let result = Vec2.lerp(Vec2(0.0, 0.0), Vec2(10.0, 20.0), t: 1.0)
        #expect(Vec2.isApproxEqual(result, Vec2(10.0, 20.0)))
    }

    @Test("t=0.5 returns midpoint")
    func tHalf() {
        let result = Vec2.lerp(Vec2(0.0, 0.0), Vec2(10.0, 20.0), t: 0.5)
        #expect(Vec2.isApproxEqual(result, Vec2(5.0, 10.0)))
    }

    @Test("Extrapolates beyond range when t > 1")
    func extrapolate() {
        let result = Vec2.lerp(Vec2(0.0, 0.0), Vec2(10.0, 10.0), t: 2.0)
        #expect(Vec2.isApproxEqual(result, Vec2(20.0, 20.0)))
    }
}

// MARK: - Perpendicular

@Suite("Vec2 — Perpendicular")
struct Vec2PerpendicularTests {

    @Test("Perpendicular of right is up")
    func rightIsUp() {
        #expect(Vec2.isApproxEqual(Vec2.right.perpendicular, Vec2.up))
    }

    @Test("Perpendicular dot original is zero")
    func dotIsZero() {
        let v = Vec2(3.0, 4.0)
        let dot = Vec2.dot(v, v.perpendicular)
        #expect(IsApproxEqual(dot, 0.0))
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

// MARK: - Angle

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

// MARK: - Operators

@Suite("Vec2 — Operators")
struct Vec2OperatorTests {

    @Test("Addition is component-wise")
    func addition() {
        let result = Vec2(1.0, 2.0) + Vec2(3.0, 4.0)
        #expect(Vec2.isApproxEqual(result, Vec2(4.0, 6.0)))
    }

    @Test("Subtraction is component-wise")
    func subtraction() {
        let result = Vec2(5.0, 6.0) - Vec2(1.0, 2.0)
        #expect(Vec2.isApproxEqual(result, Vec2(4.0, 4.0)))
    }

    @Test("Scalar multiplication scales all components — vec * float")
    func scalarMultiplyRight() {
        let result = Vec2(2.0, 3.0) * 4.0
        #expect(Vec2.isApproxEqual(result, Vec2(8.0, 12.0)))
    }

    @Test("Scalar multiplication scales all components — float * vec")
    func scalarMultiplyLeft() {
        let result = 4.0 * Vec2(2.0, 3.0)
        #expect(Vec2.isApproxEqual(result, Vec2(8.0, 12.0)))
    }

    @Test("Scalar division divides all components")
    func scalarDivision() {
        let result = Vec2(8.0, 12.0) / 4.0
        #expect(Vec2.isApproxEqual(result, Vec2(2.0, 3.0)))
    }

    @Test("Division by zero returns zero safely")
    func divisionByZero() {
        let result = Vec2(8.0, 12.0) / 0.0
        #expect(result == .zero)
    }

    @Test("Negation flips both components")
    func negation() {
        let result = -Vec2(3.0, -4.0)
        #expect(Vec2.isApproxEqual(result, Vec2(-3.0, 4.0)))
    }

    @Test("Negation of zero is zero")
    func negateZero() {
        #expect(-Vec2.zero == Vec2.zero)
    }

    @Test("+= mutates in place")
    func addAssign() {
        var v = Vec2(1.0, 2.0)
        v += Vec2(3.0, 4.0)
        #expect(Vec2.isApproxEqual(v, Vec2(4.0, 6.0)))
    }

    @Test("-= mutates in place")
    func subtractAssign() {
        var v = Vec2(5.0, 6.0)
        v -= Vec2(1.0, 2.0)
        #expect(Vec2.isApproxEqual(v, Vec2(4.0, 4.0)))
    }

    @Test("*= mutates in place")
    func multiplyAssign() {
        var v = Vec2(2.0, 3.0)
        v *= 3.0
        #expect(Vec2.isApproxEqual(v, Vec2(6.0, 9.0)))
    }

    @Test("/= mutates in place")
    func divideAssign() {
        var v = Vec2(6.0, 9.0)
        v /= 3.0
        #expect(Vec2.isApproxEqual(v, Vec2(2.0, 3.0)))
    }
}

// MARK: - isApproxEqual

@Suite("Vec2 — isApproxEqual")
struct Vec2ApproxEqualTests {

    @Test("Identical vectors are approx equal")
    func identical() {
        #expect(Vec2.isApproxEqual(Vec2(1.0, 2.0), Vec2(1.0, 2.0)))
    }

    @Test("Vectors within epsilon are approx equal")
    func withinEpsilon() {
        let a = Vec2(1.0, 2.0)
        let b = Vec2(1.0 + IsEpsilon * 0.5, 2.0 + IsEpsilon * 0.5)
        #expect(Vec2.isApproxEqual(a, b))
    }

    @Test("Vectors outside epsilon are not approx equal")
    func outsideEpsilon() {
        #expect(!Vec2.isApproxEqual(Vec2(1.0, 2.0), Vec2(1.1, 2.1)))
    }

    @Test("Custom epsilon is respected")
    func customEpsilon() {
        #expect(Vec2.isApproxEqual(Vec2(1.0, 2.0), Vec2(1.05, 2.05), epsilon: 0.1))
        #expect(!Vec2.isApproxEqual(Vec2(1.0, 2.0), Vec2(1.2, 2.2), epsilon: 0.1))
    }
}
