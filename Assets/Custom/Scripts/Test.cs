using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Euclid;

public class Test : MonoBehaviour {
	void Start () {
        Construction c = new Construction("Assets/Custom/Scripts/Constructions/test.euclid");
        List<Figure> render = c.Execute();
        foreach (Figure fig in render) {
            Debug.Log(fig.ToString());
        }
    }
}
