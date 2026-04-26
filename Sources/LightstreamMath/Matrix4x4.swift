// Copyright Studio Prism. Licensed under PolyForm Shield 1.0.0.
// https://polyformproject.org/licenses/shield/1.0.0
// Required Notice: Copyright Studio Prism (https://github.com/studioprism)
//
// Lightstream — LightstreamMath.Matrix4x4
// Matrix4x4.swift
//
// Column-major 4×4 matrix backed by four Vec4 columns.
// Matches Metal and Vulkan memory layout — upload directly to GPU uniforms.
//
// Column-major means m[c][r]: m[0] is column 0, m[0].x is row 0 of column 0.
// Transforms chain right-to-left: FinalMatrix = Projection × View × Model.
//
// w = 1.0 points are affected by translation.
// w = 0.0 directions are not affected by translation.

import Foundation

// MARK: - Matrix4x4

public struct Matrix4x4: Equatable, CustomStringConvertible, Sendable {

    // ── Backing Storage ──────────────────────────────────────────────────────
    //
    // Four columns, each a Vec4.
    // columns[0] = first column  (x-axis basis vector + translation.x)
    // columns[1] = second column (y-axis basis vector + translation.y)
    // columns[2] = third column  (z-axis basis vector + translation.z)
    // columns[3] = fourth column (translation vector  + 1.0 in w)
    //
    // Memory layout matches simd_float4x4 — safe to pass to Metal as-is.

    public var columns: (Vec4, Vec4, Vec4, Vec4)

    // ── Subscript ─────────────────────────────────────────────────────────────
    //
    // Access by [column][row] — matches the column-major convention.
    // m[0][1] = column 0, row 1 = the y component of the first column.
    //
    // This is intentionally [column][row] not [row][column].
    // Reading order on paper is row-major, but storage is column-major.
    // The subscript reflects storage, not paper notation.

    public subscript(column: Int, row: Int) -> Float {
        get {
            switch column {
            case 0: return columnAsArray(columns.0)[row]
            case 1: return columnAsArray(columns.1)[row]
            case 2: return columnAsArray(columns.2)[row]
            case 3: return columnAsArray(columns.3)[row]
            default: fatalError("Matrix4x4 column index \(column) out of range")
            }
        }
        set {
            switch column {
            case 0: setComponent(&columns.0, row: row, value: newValue)
            case 1: setComponent(&columns.1, row: row, value: newValue)
            case 2: setComponent(&columns.2, row: row, value: newValue)
            case 3: setComponent(&columns.3, row: row, value: newValue)
            default: fatalError("Matrix4x4 column index \(column) out of range")
            }
        }
    }

    // ── Subscript Helpers ────────────────────────────────────────────────────
    //
    // Swift tuples aren't subscriptable, so we use these small helpers
    // to read and write individual components by row index.

    private func columnAsArray(_ col: Vec4) -> [Float] {
        [col.x, col.y, col.z, col.w]
    }

    private func setComponent(_ col: inout Vec4, row: Int, value: Float) {
        switch row {
        case 0: col.x = value
        case 1: col.y = value
        case 2: col.z = value
        case 3: col.w = value
        default: fatalError("Matrix4x4 row index \(row) out of range")
        }
    }

    // ── Initializers ─────────────────────────────────────────────────────────

    /// Primary init — provide all four columns explicitly.
    public init(_ col0: Vec4, _ col1: Vec4, _ col2: Vec4, _ col3: Vec4) {
        columns = (col0, col1, col2, col3)
    }

    /// Diagonal init — sets the four diagonal elements, all others zero.
    /// Used internally to build identity and scale matrices cleanly.
    ///
    /// diagonal(1) → identity matrix
    /// diagonal(2) → uniform scale by 2 matrix
    public init(diagonal: Float) {
        columns = (
            Vec4(diagonal, 0, 0, 0),
            Vec4(0, diagonal, 0, 0),
            Vec4(0, 0, diagonal, 0),
            Vec4(0, 0, 0, diagonal)
        )
    }

    // ── Static Constants ──────────────────────────────────────────────────────
    //
    // The identity matrix — multiplying any vector or matrix by identity
    // returns the original unchanged. Starting point for all transforms.
    //
    // [1  0  0  0]
    // [0  1  0  0]
    // [0  0  1  0]
    // [0  0  0  1]

    /// The identity matrix. Applying this transform changes nothing.
    public static let identity = Matrix4x4(diagonal: 1.0)

    // ── CustomStringConvertible ──────────────────────────────────────────────

    public var description: String {
        """
        Matrix4x4(
          [\(self[0,0]), \(self[1,0]), \(self[2,0]), \(self[3,0])]
          [\(self[0,1]), \(self[1,1]), \(self[2,1]), \(self[3,1])]
          [\(self[0,2]), \(self[1,2]), \(self[2,2]), \(self[3,2])]
          [\(self[0,3]), \(self[1,3]), \(self[2,3]), \(self[3,3])]
        )
        """
    }

    // ── Equatable ─────────────────────────────────────────────────────────────

    public static func == (lhs: Matrix4x4, rhs: Matrix4x4) -> Bool {
        lhs.columns.0 == rhs.columns.0 &&
        lhs.columns.1 == rhs.columns.1 &&
        lhs.columns.2 == rhs.columns.2 &&
        lhs.columns.3 == rhs.columns.3
    }

    // ── Transpose ─────────────────────────────────────────────────────────────
    //
    // Transpose swaps rows and columns.
    // The x component of each column becomes the new column built from those x's.
    //
    // Original columns (stored):    Transposed columns:
    // col0 = (a, b, c, d)           new col0 = (a, e, i, m)   ← x of each original col
    // col1 = (e, f, g, h)           new col1 = (b, f, j, n)   ← y of each original col
    // col2 = (i, j, k, l)           new col2 = (c, g, k, o)   ← z of each original col
    // col3 = (m, n, o, p)           new col3 = (d, h, l, p)   ← w of each original col
    //
    // Used for: normal matrix computation, converting between row/column major,
    // certain shader math operations.

    /// Returns a new matrix with rows and columns swapped.
    public var transposed: Matrix4x4 {
        // TODO: Implement transposed.
        //
        // Each new column is built from the same-index component across all
        // four original columns.
        //
        // new column 0 = Vec4(columns.0.x, columns.1.x, columns.2.x, columns.3.x)
        // new column 1 = Vec4(columns.0.y, columns.1.y, columns.2.y, columns.3.y)
        // new column 2 = Vec4(columns.0.z, columns.1.z, columns.2.z, columns.3.z)
        // new column 3 = Vec4(columns.0.w, columns.1.w, columns.2.w, columns.3.w)
        fatalError("transposed not implemented")
    }

    // ── Factory — Translation ─────────────────────────────────────────────────
    //
    // A translation matrix moves a point by (tx, ty, tz).
    // It is the identity matrix with the translation in the last column.
    //
    // [1  0  0  tx]
    // [0  1  0  ty]
    // [0  0  1  tz]
    // [0  0  0   1]
    //
    // In column-major storage:
    //   col0 = (1, 0, 0, 0)
    //   col1 = (0, 1, 0, 0)
    //   col2 = (0, 0, 1, 0)
    //   col3 = (tx, ty, tz, 1)   ← translation lives in column 3
    //
    // Why column 3? Because when you multiply Matrix × Vec4, the last column
    // dotted with (x, y, z, 1) adds tx*1, ty*1, tz*1 to the result.
    // With w=0 (direction), the translation has no effect: tx*0 = 0.

    /// Returns a matrix that translates by the given vector.
    public static func translation(_ v: Vec3) -> Matrix4x4 {
        // TODO: Implement translation matrix.
        //
        // Start from identity. Only column 3 differs from identity:
        //   col3 = Vec4(v.x, v.y, v.z, 1.0)
        fatalError("translation(_:) not implemented")
    }

    // ── Factory — Scale ───────────────────────────────────────────────────────
    //
    // A scale matrix stretches or shrinks along each axis independently.
    // Scale values live on the diagonal.
    //
    // [sx  0   0   0]
    // [0   sy  0   0]
    // [0   0   sz  0]
    // [0   0   0   1]
    //
    // In column-major storage:
    //   col0 = (sx, 0,  0,  0)
    //   col1 = (0,  sy, 0,  0)
    //   col2 = (0,  0,  sz, 0)
    //   col3 = (0,  0,  0,  1)
    //
    // Uniform scale: all three components the same → Vec3(s, s, s)
    // Non-uniform scale: stretch/squash per axis → e.g. Vec3(2, 1, 0.5)

    /// Returns a matrix that scales by the given per-axis factors.
    public static func scale(_ v: Vec3) -> Matrix4x4 {
        // TODO: Implement scale matrix.
        //
        // Each diagonal element is the corresponding scale component.
        // col0.x = v.x, col1.y = v.y, col2.z = v.z, col3.w = 1.0
        // Everything else is zero.
        fatalError("scale(_:) not implemented")
    }

    // ── Factory — lookAt ──────────────────────────────────────────────────────
    //
    // Builds a VIEW matrix from camera position and target.
    // Transforms world space into camera space — moves the world so the
    // camera sits at the origin looking down -Z.
    //
    // Algorithm:
    //   1. forward = normalize(eye - center)
    //      The direction FROM center TO eye (camera looks in -forward)
    //
    //   2. right = normalize(cross(up, forward))
    //      Camera's right axis — perpendicular to both up hint and forward
    //
    //   3. cameraUp = cross(forward, right)
    //      Recomputed up — guaranteed orthogonal to both forward and right
    //      (The input `up` is just a hint, not always perfectly orthogonal)
    //
    //   4. Build matrix:
    //      Rows are the three basis vectors (right, cameraUp, forward)
    //      Last column holds the eye translation, negated and dotted:
    //        tx = -dot(right,    eye)
    //        ty = -dot(cameraUp, eye)
    //        tz = -dot(forward,  eye)
    //
    // Final matrix in column-major:
    //   col0 = (right.x,    cameraUp.x, forward.x, 0)
    //   col1 = (right.y,    cameraUp.y, forward.y, 0)
    //   col2 = (right.z,    cameraUp.z, forward.z, 0)
    //   col3 = (tx,         ty,         tz,         1)

    /// Builds a view matrix from camera position, target, and up hint.
    /// eye    = camera world position
    /// center = point the camera looks at
    /// up     = world up hint (usually Vec3.up)
    public static func lookAt(eye: Vec3, center: Vec3, up: Vec3) -> Matrix4x4 {
        // TODO: Implement lookAt.
        //
        // Step 1: compute the three basis vectors
        //   let forward  = (eye - center).normalized
        //   let right    = Vec3.cross(up, forward).normalized
        //   let cameraUp = Vec3.cross(forward, right)
        //
        // Step 2: compute translation components
        //   let tx = -Vec3.dot(right,    eye)
        //   let ty = -Vec3.dot(cameraUp, eye)
        //   let tz = -Vec3.dot(forward,  eye)
        //
        // Step 3: assemble columns
        //   col0 = Vec4(right.x,    cameraUp.x, forward.x, 0)
        //   col1 = Vec4(right.y,    cameraUp.y, forward.y, 0)
        //   col2 = Vec4(right.z,    cameraUp.z, forward.z, 0)
        //   col3 = Vec4(tx,         ty,         tz,         1)
        fatalError("lookAt(eye:center:up:) not implemented")
    }

    // ── Factory — Orthographic Projection ────────────────────────────────────
    //
    // Builds a projection matrix that flattens a 3D box to NDC.
    // Uses Metal's depth convention: z maps to [0, 1], not OpenGL's [-1, 1].
    //
    // Parameters define the visible box in camera space:
    //   left/right  = horizontal clip planes
    //   bottom/top  = vertical clip planes
    //   near/far    = depth clip planes
    //
    // Derivation — each axis maps its range to NDC:
    //
    //   x: [left, right] → [-1, 1]
    //      scale  = 2 / (right - left)
    //      offset = -(right + left) / (right - left)
    //
    //   y: [bottom, top] → [-1, 1]
    //      scale  = 2 / (top - bottom)
    //      offset = -(top + bottom) / (top - bottom)
    //
    //   z: [near, far] → [0, 1]   ← Metal convention
    //      scale  = 1 / (far - near)
    //      offset = -near / (far - near)
    //
    // Assembled matrix (scale on diagonal, offsets in last column):
    //   col0 = (sx,  0,   0,   0)
    //   col1 = (0,   sy,  0,   0)
    //   col2 = (0,   0,   sz,  0)
    //   col3 = (ox,  oy,  oz,  1)
    //
    // where:
    //   sx = 2 / (right - left)
    //   sy = 2 / (top - bottom)
    //   sz = 1 / (far - near)
    //   ox = -(right + left) / (right - left)
    //   oy = -(top + bottom) / (top - bottom)
    //   oz = -near / (far - near)

    /// Orthographic projection matrix. Maps the given box to NDC.
    /// Uses Metal depth convention: z → [0, 1].
    public static func orthographic(
        left:   Float, right: Float,
        bottom: Float, top:   Float,
        near:   Float, far:   Float
    ) -> Matrix4x4 {
        // TODO: Implement orthographic projection.
        //
        // Step 1: compute scale and offset for each axis
        //   let sx = 2 / (right - left)
        //   let sy = 2 / (top - bottom)
        //   let sz = 1 / (far - near)
        //   let ox = -(right + left) / (right - left)
        //   let oy = -(top + bottom) / (top - bottom)
        //   let oz = -near / (far - near)
        //
        // Step 2: assemble columns
        //   col0 = Vec4(sx, 0,  0,  0)
        //   col1 = Vec4(0,  sy, 0,  0)
        //   col2 = Vec4(0,  0,  sz, 0)
        //   col3 = Vec4(ox, oy, oz, 1)
        fatalError("orthographic(left:right:bottom:top:near:far:) not implemented")
    }
}

// MARK: - Operators

extension Matrix4x4 {

    // ── Matrix × Vec4 ────────────────────────────────────────────────────────
    //
    // Transforms a Vec4 by the matrix. The core operation of the pipeline.
    //
    // Each output component is a dot product of one matrix ROW with the vector.
    // Since we store columns, we read across columns to get row values:
    //
    //   result.x = m[0][0]*v.x + m[1][0]*v.y + m[2][0]*v.z + m[3][0]*v.w
    //   result.y = m[0][1]*v.x + m[1][1]*v.y + m[2][1]*v.z + m[3][1]*v.w
    //   result.z = m[0][2]*v.x + m[1][2]*v.y + m[2][2]*v.z + m[3][2]*v.w
    //   result.w = m[0][3]*v.x + m[1][3]*v.y + m[2][3]*v.z + m[3][3]*v.w
    //
    // Equivalent to: sum of (each column × its corresponding vector component)
    //   = col0*v.x + col1*v.y + col2*v.z + col3*v.w
    //
    // The second form is cleaner in code and equally correct — it's how
    // column-major matrix-vector multiply is typically implemented.

    /// Transforms a Vec4 by this matrix.
    public static func * (lhs: Matrix4x4, rhs: Vec4) -> Vec4 {
        // TODO: Implement Matrix4x4 × Vec4.
        //
        // Use the column-sum form — multiply each column by its vector component
        // and sum the results. SIMD Vec4 * Float and Vec4 + Vec4 make this clean:
        //
        //   lhs.columns.0 * rhs.x
        // + lhs.columns.1 * rhs.y
        // + lhs.columns.2 * rhs.z
        // + lhs.columns.3 * rhs.w
        fatalError("Matrix4x4 * Vec4 not implemented")
    }

    // ── Matrix × Matrix ───────────────────────────────────────────────────────
    //
    // Combines two transforms into one.
    // Each column of the result = lhs × that column of rhs (as a Vec4).
    //
    // result.col0 = lhs * rhs.col0
    // result.col1 = lhs * rhs.col1
    // result.col2 = lhs * rhs.col2
    // result.col3 = lhs * rhs.col3
    //
    // Order matters: lhs is applied AFTER rhs to any subsequent vector.
    // FinalMatrix = Projection × View means View is applied first.

    /// Combines two matrices. lhs is applied after rhs.
    public static func * (lhs: Matrix4x4, rhs: Matrix4x4) -> Matrix4x4 {
        // TODO: Implement Matrix4x4 × Matrix4x4.
        //
        // Four matrix-vector multiplies, one per column of rhs.
        // You already have the * operator for Matrix4x4 × Vec4 —
        // call it four times:
        //
        //   let col0 = lhs * rhs.columns.0
        //   let col1 = lhs * rhs.columns.1
        //   let col2 = lhs * rhs.columns.2
        //   let col3 = lhs * rhs.columns.3
        //   return Matrix4x4(col0, col1, col2, col3)
        fatalError("Matrix4x4 * Matrix4x4 not implemented")
    }
}
