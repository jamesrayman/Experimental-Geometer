using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
	void Start () {
        Euclid.Plane alpha = new Euclid.Plane(new Vector3(0, 0, 0), new Vector3(0, 0, 1));
        List<Euclid.Figure> list = Euclid.Figure.Intersection(new Euclid.Point(2, 3, 0.1f), alpha);

        Debug.Log(list.Count);
        foreach (Euclid.Figure f in list) {
            Debug.Log(f);
        }
    }
}
