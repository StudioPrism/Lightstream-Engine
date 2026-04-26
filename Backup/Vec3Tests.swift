// Copyright Studio Prism. Licensed under PolyForm Shield 1.0.0.
// https://polyformproject.org/licenses/shield/1.0.0
// Required Notice: Copyright Studio Prism (https://github.com/studioprism)
//
// Lightstream — LightstreamMathTests
// Vec3Tests.swift
//
// Tests for all public Vec3 functionality.

import Testing
@testable import LightstreamMath

// MARK: - Init & Storage

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
        v.x = 10.0
        v.y = 20.0
        v.z = 30.0
        #expect(v.x == 10.0)
        #expect(v.y == 20.0)
        #expect(v.z == 30.0)
    }
}

// MARK: - Constants

@Suite("Vec3 — Constants")
struct Vec3ConstantTests {

    @Test("zero is (0, 0, 0)")
    func zero() {
        #expect(Vec3.zero.x == 0.0)
        #expect(Vec3.zero.y == 0.0)
        #expect(Vec3.zero.z == 0.0)
    }

    @Test("one is (1, 1, 1)")
    func one() {
        #expect(Vec3.one.x == 1.0)
        #expect(Vec3.one.y == 1.0)
        #expect(Vec3.one.z == 1.0)
    }

    @Test("right is (1, 0, 0)")
    func right() {
        #expect(Vec3.right == Vec3(1, 0, 0))
    }

    @Test("left is (-1, 0, 0)")
    func left() {
        #expect(Vec3.left == Vec3(-1, 0, 0))
    }

    @Test("up is (0, 1, 0)")
    func up() {
        #expect(Vec3.up == Vec3(0, 1, 0))
    }

    @Test("down is (0, -1, 0)")
    func down() {
        #expect(Vec3.down == Vec3(0, -1, 0))
    }

    @Test("forward is (0, 0, 1)")
    func forward() {
        #expect(Vec3.forward == Vec3(0, 0, 1))
    }

    @Test("back is (0, 0, -1)")
    func back() {
        #expect(Vec3.back == Vec3(0, 0, -1))
    }

    @Test("Axis constants are unit length")
    func axisConstantsAreUnit() {
        #expect(IsApproxEqual(Vec3.right.length,   1.0))
        #expect(IsApproxEqual(Vec3.left.length,    1.0))
        #expect(IsApproxEqual(Vec3.up.length,      1.0))
        #expect(IsApproxEqual(Vec3.down.length,    1.0))
        #expect(IsApproxEqual(Vec3.forward.length, 1.0))
        #expect(IsApproxEqual(Vec3.back.length,    1.0))
    }

    @Test("Opposite axis constants are negations of each other")
    func oppositeAxes() {
        #expect(Vec3.isApproxEqual(-Vec3.right,   Vec3.left))
        #expect(Vec3.isApproxEqual(-Vec3.up,      Vec3.down))
        #expect(Vec3.isApproxEqual(-Vec3.forward, Vec3.back))
    }
}

// MARK: - Equatable

@Suite("Vec3 — Equatable")
struct Vec3EquatableTests {

    @Test("Identical vectors are equal")
    func identical() {
        #expect(Vec3(1.0, 2.0, 3.0) == Vec3(1.0, 2.0, 3.0))
    }

    @Test("Vectors within epsilon are equal")
    func withinEpsilon() {
        let a = Vec3(1.0, 2.0, 3.0)
        let b = Vec3(1.0 + IsEpsilon * 0.5, 2.0 + IsEpsilon * 0.5, 3.0 + IsEpsilon * 0.5)
        #expect(a == b)
    }

    @Test("Vectors outside epsilon are not equal")
    func outsideEpsilon() {
        #expect(Vec3(1.0, 2.0, 3.0) != Vec3(1.1, 2.0, 3.0))
    }

    @Test("Vectors differing only in z are not equal")
    func differInZ() {
        #expect(Vec3(1.0, 2.0, 3.0) != Vec3(1.0, 2.0, 9.0))
    }
}

// MARK: - Description

@Suite("Vec3 — Description")
struct Vec3DescriptionTests {

    @Test("Description format is correct")
    func descriptionFormat() {
        let v = Vec3(1.0, 2.0, 3.0)
        #expect(v.description == "Vec3(1.0, 2.0, 3.0)")
    }
}

// MARK: - Length

@Suite("Vec3 — Length")
struct Vec3LengthTests {

    @Test("Length of (2, 3, 6) is 7")
    func knownLength() {
        // 2² + 3² + 6² = 4 + 9 + 36 = 49 → sqrt(49) = 7
        #expect(IsApproxEqual(Vec3(2.0, 3.0, 6.0).length, 7.0))
    }

    @Test("Length of zero vector is 0")
    func zeroLength() {
        #expect(Vec3.zero.length == 0.0)
    }

    @Test("All axis constants have length 1")
    func axisLengths() {
        #expect(IsApproxEqual(Vec3.right.length, 1.0))
        #expect(IsApproxEqual(Vec3.up.length,    1.0))
        #expect(IsApproxEqual(Vec3.forward.length, 1.0))
    }

    @Test("lengthSquared of (2, 3, 6) is 49")
    func lengthSquared() {
        #expect(IsApproxEqual(Vec3(2.0, 3.0, 6.0).lengthSquared, 49.0))
    }

    @Test("lengthSquared equals length squared")
    func lengthSquaredConsistency() {
        let v = Vec3(3.0, 5.0, 7.0)
        #expect(IsApproxEqual(v.lengthSquared, v.length * v.length))
    }
}

// MARK: - Normalize

@Suite("Vec3 — Normalize")
struct Vec3NormalizeTests {

    @Test("Normalized vector has length 1")
    func normalizedLength() {
        let v = Vec3(2.0, 3.0, 6.0).normalized
        #expect(IsApproxEqual(v.length, 1.0))
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

    @Test("Direction is preserved after normalization")
    func directionPreserved() {
        let v = Vec3(2.0, 3.0, 6.0)
        let n = v.normalized
        let dot = Vec3.dot(n, v.normalized)
        #expect(IsApproxEqual(dot, 1.0))
    }
}

// MARK: - Dot Product

@Suite("Vec3 — Dot Product")
struct Vec3DotTests {

    @Test("Parallel vectors have positive dot product")
    func parallel() {
        #expect(Vec3.dot(Vec3.right, Vec3.right) > 0)
    }

    @Test("Perpendicular axis vectors have dot product of 0")
    func perpendicularAxes() {
        #expect(IsApproxEqual(Vec3.dot(Vec3.right,   Vec3.up),      0.0))
        #expect(IsApproxEqual(Vec3.dot(Vec3.right,   Vec3.forward), 0.0))
        #expect(IsApproxEqual(Vec3.dot(Vec3.up,      Vec3.forward), 0.0))
    }

    @Test("Opposite vectors have negative dot product")
    func opposite() {
        #expect(Vec3.dot(Vec3.right, Vec3.left) < 0)
    }

    @Test("Dot product is commutative")
    func commutative() {
        let a = Vec3(1.0, 2.0, 3.0)
        let b = Vec3(4.0, 5.0, 6.0)
        #expect(IsApproxEqual(Vec3.dot(a, b), Vec3.dot(b, a)))
    }

    @Test("Known dot product value")
    func knownValue() {
        // (1,2,3)·(4,5,6) = 4 + 10 + 18 = 32
        let dot = Vec3.dot(Vec3(1.0, 2.0, 3.0), Vec3(4.0, 5.0, 6.0))
        #expect(IsApproxEqual(dot, 32.0))
    }
}

// MARK: - Cross Product

@Suite("Vec3 — Cross Product")
struct Vec3CrossTests {

    @Test("cross(right, up) = forward — defines coordinate system")
    func rightCrossUpIsForward() {
        let result = Vec3.cross(Vec3.right, Vec3.up)
        #expect(Vec3.isApproxEqual(result, Vec3.forward))
    }

    @Test("cross(up, right) = back — anti-commutative")
    func upCrossRightIsBack() {
        let result = Vec3.cross(Vec3.up, Vec3.right)
        #expect(Vec3.isApproxEqual(result, Vec3.back))
    }

    @Test("cross(a, b) = -cross(b, a)")
    func antiCommutative() {
        let a = Vec3(1.0, 2.0, 3.0)
        let b = Vec3(4.0, 5.0, 6.0)
        let ab = Vec3.cross(a, b)
        let ba = Vec3.cross(b, a)
        #expect(Vec3.isApproxEqual(ab, -ba))
    }

    @Test("Result is perpendicular to both inputs")
    func resultIsPerpendicular() {
        let a = Vec3(1.0, 2.0, 3.0)
        let b = Vec3(4.0, 5.0, 6.0)
        let c = Vec3.cross(a, b)
        #expect(IsApproxEqual(Vec3.dot(c, a), 0.0))
        #expect(IsApproxEqual(Vec3.dot(c, b), 0.0))
    }

    @Test("Cross product of parallel vectors is zero")
    func parallelVectorsGiveZero() {
        let result = Vec3.cross(Vec3.right, Vec3.right)
        #expect(Vec3.isApproxEqual(result, Vec3.zero))
    }

    @Test("cross(up, forward) = right")
    func upCrossForwardIsRight() {
        let result = Vec3.cross(Vec3.up, Vec3.forward)
        #expect(Vec3.isApproxEqual(result, Vec3.right))
    }

    @Test("Known cross product value")
    func knownValue() {
        // (1,0,0) × (0,1,0) = (0*0-0*1, 0*0-1*0, 1*1-0*0) = (0,0,1)
        let result = Vec3.cross(Vec3(1, 0, 0), Vec3(0, 1, 0))
        #expect(Vec3.isApproxEqual(result, Vec3(0, 0, 1)))
    }
}

// MARK: - Distance

@Suite("Vec3 — Distance")
struct Vec3DistanceTests {

    @Test("Distance between identical points is 0")
    func samePoint() {
        #expect(Vec3.distance(Vec3(1, 2, 3), Vec3(1, 2, 3)) == 0.0)
    }

    @Test("Distance from origin to (2, 3, 6) is 7")
    func knownDistance() {
        let d = Vec3.distance(.zero, Vec3(2.0, 3.0, 6.0))
        #expect(IsApproxEqual(d, 7.0))
    }

    @Test("Distance is symmetric")
    func symmetric() {
        let a = Vec3(1.0, 2.0, 3.0)
        let b = Vec3(4.0, 6.0, 3.0)
        #expect(IsApproxEqual(Vec3.distance(a, b), Vec3.distance(b, a)))
    }

    @Test("Distance is always non-negative")
    func nonNegative() {
        #expect(Vec3.distance(Vec3(5, 5, 5), Vec3(1, 1, 1)) >= 0)
    }
}

// MARK: - Lerp

@Suite("Vec3 — Lerp")
struct Vec3LerpTests {

    @Test("t=0 returns a")
    func tZero() {
        let result = Vec3.lerp(Vec3(1, 2, 3), Vec3(10, 20, 30), t: 0.0)
        #expect(Vec3.isApproxEqual(result, Vec3(1, 2, 3)))
    }

    @Test("t=1 returns b")
    func tOne() {
        let result = Vec3.lerp(Vec3(1, 2, 3), Vec3(10, 20, 30), t: 1.0)
        #expect(Vec3.isApproxEqual(result, Vec3(10, 20, 30)))
    }

    @Test("t=0.5 returns midpoint")
    func tHalf() {
        let result = Vec3.lerp(Vec3(0, 0, 0), Vec3(10, 20, 30), t: 0.5)
        #expect(Vec3.isApproxEqual(result, Vec3(5, 10, 15)))
    }

    @Test("Extrapolates beyond range when t > 1")
    func extrapolate() {
        let result = Vec3.lerp(Vec3(0, 0, 0), Vec3(10, 10, 10), t: 2.0)
        #expect(Vec3.isApproxEqual(result, Vec3(20, 20, 20)))
    }
}

// MARK: - Reflect

@Suite("Vec3 — Reflect")
struct Vec3ReflectTests {

    @Test("Reflecting off a floor flips the y component")
    func reflectOffFloor() {
        // Ball moving right and down, floor normal is up
        let v = Vec3(1.0, -1.0, 0.0)
        let n = Vec3.up
        let result = Vec3.reflect(v, normal: n)
        #expect(Vec3.isApproxEqual(result, Vec3(1.0, 1.0, 0.0)))
    }

    @Test("Reflecting off a wall flips the x component")
    func reflectOffWall() {
        // Ball moving into a right-facing wall
        let v = Vec3(1.0, 0.0, 0.0)
        let n = Vec3.right
        let result = Vec3.reflect(v, normal: n)
        #expect(Vec3.isApproxEqual(result, Vec3(-1.0, 0.0, 0.0)))
    }

    @Test("Reflected vector has same length as input")
    func preservesLength() {
        let v = Vec3(1.0, -2.0, 3.0)
        let n = Vec3.up
        let result = Vec3.reflect(v, normal: n)
        #expect(IsApproxEqual(result.length, v.length))
    }

    @Test("Reflecting a vector along its own normal negates it")
    func reflectAlongNormal() {
        // A vector pointing straight into a surface reflects straight back
        let v = Vec3(0.0, -1.0, 0.0)
        let n = Vec3.up
        let result = Vec3.reflect(v, normal: n)
        #expect(Vec3.isApproxEqual(result, Vec3(0.0, 1.0, 0.0)))
    }

    @Test("Reflecting a vector parallel to surface leaves it unchanged")
    func reflectParallelToSurface() {
        // Vector moving purely sideways along a floor — no bounce
        let v = Vec3(1.0, 0.0, 0.0)
        let n = Vec3.up
        let result = Vec3.reflect(v, normal: n)
        #expect(Vec3.isApproxEqual(result, v))
    }
}

// MARK: - Operators

@Suite("Vec3 — Operators")
struct Vec3OperatorTests {

    @Test("Addition is component-wise")
    func addition() {
        let result = Vec3(1, 2, 3) + Vec3(4, 5, 6)
        #expect(Vec3.isApproxEqual(result, Vec3(5, 7, 9)))
    }

    @Test("Subtraction is component-wise")
    func subtraction() {
        let result = Vec3(5, 7, 9) - Vec3(1, 2, 3)
        #expect(Vec3.isApproxEqual(result, Vec3(4, 5, 6)))
    }

    @Test("Scalar multiplication — vec * float")
    func scalarMultiplyRight() {
        let result = Vec3(1, 2, 3) * 2.0
        #expect(Vec3.isApproxEqual(result, Vec3(2, 4, 6)))
    }

    @Test("Scalar multiplication — float * vec")
    func scalarMultiplyLeft() {
        let result = 2.0 * Vec3(1, 2, 3)
        #expect(Vec3.isApproxEqual(result, Vec3(2, 4, 6)))
    }

    @Test("Scalar division divides all components")
    func scalarDivision() {
        let result = Vec3(2, 4, 6) / 2.0
        #expect(Vec3.isApproxEqual(result, Vec3(1, 2, 3)))
    }

    @Test("Division by zero returns zero safely")
    func divisionByZero() {
        let result = Vec3(1, 2, 3) / 0.0
        #expect(result == .zero)
    }

    @Test("Negation flips all three components")
    func negation() {
        let result = -Vec3(1.0, -2.0, 3.0)
        #expect(Vec3.isApproxEqual(result, Vec3(-1.0, 2.0, -3.0)))
    }

    @Test("Negation of zero is zero")
    func negateZero() {
        #expect(-Vec3.zero == Vec3.zero)
    }

    @Test("+= mutates in place")
    func addAssign() {
        var v = Vec3(1, 2, 3)
        v += Vec3(4, 5, 6)
        #expect(Vec3.isApproxEqual(v, Vec3(5, 7, 9)))
    }

    @Test("-= mutates in place")
    func subtractAssign() {
        var v = Vec3(5, 7, 9)
        v -= Vec3(1, 2, 3)
        #expect(Vec3.isApproxEqual(v, Vec3(4, 5, 6)))
    }

    @Test("*= mutates in place")
    func multiplyAssign() {
        var v = Vec3(1, 2, 3)
        v *= 3.0
        #expect(Vec3.isApproxEqual(v, Vec3(3, 6, 9)))
    }

    @Test("/= mutates in place")
    func divideAssign() {
        var v = Vec3(3, 6, 9)
        v /= 3.0
        #expect(Vec3.isApproxEqual(v, Vec3(1, 2, 3)))
    }
}

// MARK: - isApproxEqual

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
