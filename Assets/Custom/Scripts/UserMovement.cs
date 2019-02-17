using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserMovement : MonoBehaviour {
    bool grabbed = false;
    
    public void Grab () {
        grabbed = true;
        //RenderMaster.main.transform.parent = transform;
    }
    public void Ungrab () {
        grabbed = false;
        //RenderMaster.main.transform.parent = null;
    }
    
}
