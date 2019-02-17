using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Euclid;

public class Test : MonoBehaviour {
	void Start () {
        Construction diagram = new Construction("Assets/Custom/Scripts/Constructions/test.euclid");
        diagram.Execute();
    }
}
