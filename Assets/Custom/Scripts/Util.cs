using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util {
    public const float EPSILON = 1e-5f;
    public static bool Approximately (float x, float y) {
        return Mathf.Abs(x-y) <= EPSILON;
    }
    public static bool Approximately (Vector3 v, Vector3 u) {
        return Approximately(v.x, u.x) && Approximately(v.y, u.y) && Approximately(v.z, u.z);
    }

    public static float RandomValue () {
        return Random.Range(-10f, 10f);
    }
    public static Vector3 RandomVector () {
        return new Vector3(RandomValue(), RandomValue(), RandomValue());
    }
    
}
