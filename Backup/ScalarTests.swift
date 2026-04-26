// Copyright Studio Prism. Licensed under PolyForm Shield 1.0.0.
// https://polyformproject.org/licenses/shield/1.0.0
// Required Notice: Copyright Studio Prism (https://github.com/studioprism)
//
// Lightstream — LightstreamMathTests
// ScalarTests.swift
//
// Tests for all public Scalar utilities.

import Testing
@testable import LightstreamMath

// MARK: - Constants

@Suite("Scalar — Constants")
struct ScalarConstantTests {

    @Test("IsEpsilon is 1e-6")
    func epsilonValue() {
        #expect(IsEpsilon == 1e-6)
    }
}

// MARK: - Approx Equality

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

// MARK: - Approx Zero

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

// MARK: - Clamping

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
        #expect(IsClamp(-3.0, min: -5.0, max: -1.0) == -3.0)
        #expect(IsClamp(0.0,  min: -5.0, max: -1.0) == -1.0)
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

// MARK: - Sign

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

// MARK: - Interpolation

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
        // [10, 20] → [0, 1]: value 15 should produce 0.5
        #expect(IsApproxEqual(remap(15.0, inMin: 10.0, inMax: 20.0, outMin: 0.0, outMax: 1.0), 0.5))
    }

    @Test("Works with reversed output range")
    func reversedOutput() {
        // Remapping 0→10 into 1→0: value 0 should produce 1.0
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

    @Test("Output is always in [0, 1] for inputs in [edge0, edge1]", arguments: [
        Float(0.0), Float(0.1), Float(0.25), Float(0.5), Float(0.75), Float(0.9), Float(1.0)
    ])
    func outputInRange(value: Float) {
        let result = smoothstep(0.0, 1.0, value: value)
        #expect(result >= 0.0 && result <= 1.0)
    }
}

// MARK: - Bit Utilities

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
