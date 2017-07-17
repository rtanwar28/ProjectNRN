using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MatrixTransformation : MonoBehaviour
{
    public abstract Matrix4x4 Matrix
    {
        get;
    }

    public Vector3 Apply(Vector3 point)
    {
        return Matrix.MultiplyPoint(point);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
