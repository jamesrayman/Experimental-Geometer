using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
	void Start () {
        Euclid.Plane alpha = new Euclid.Plane(new Vector3(0, 0, 0), new Vector3(0, 0, 1));
        List<Euclid.Figure> list = Euclid.Figure.Intersection(new Euclid.Line(new Vector3(2, 2, 1), new Vector3(1, 0, 1)), alpha);

        Debug.Log(list.Count);
        foreach (Euclid.Figure f in list) {
            Debug.Log(f);
        }
    }
}
