using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixCameraTransformation : MatrixTransformation {

    public float x = 1f;
    public float y = 1f;
    public float z = 1f;

    public override Matrix4x4 Matrix
    {
        get
        {
            Matrix4x4 matrix = new Matrix4x4();
            matrix.SetRow(0, new Vector4(1f, 0f, 0f, 0f));
            matrix.SetRow(1, new Vector4(0f, 1f, 0f, 0f));
            matrix.SetRow(2, new Vector4(0f, 0f, 1f, 0f));
            matrix.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
            return matrix;
        }
    }
}
