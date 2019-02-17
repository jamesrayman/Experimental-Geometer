using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserMovement : MonoBehaviour {
    public GameObject renderMaster;
    bool grabbed = false;
    
    public void Grab () {
        grabbed = true;
        renderMaster.transform.parent = transform;
        Debug.Log("Grabed");
        renderMaster.GetComponent<RenderMaster>().diagram.UpdatePoint();
    }
    public void Ungrab () {
        grabbed = false;
        renderMaster.transform.parent = null;
        Debug.Log("Ungrabed");
    }
    
}
