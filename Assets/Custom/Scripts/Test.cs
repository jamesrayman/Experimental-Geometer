using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
	void Start () {
        Debug.Log(Euclid.Figure.Binormal(new Euclid.Line(new Vector3(0, 0, 0), new Vector3(1, 1, 0)), new Euclid.Line(new Vector3(0, 5, 2), new Vector3(1, -1, 0))));
    }
}
