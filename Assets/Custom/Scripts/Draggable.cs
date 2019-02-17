using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour {
    bool grabbed = false;
    public string id;

    void Update() {
        if (grabbed && name.Length > 0) {
            RenderMaster.main.diagram.UpdatePoint(id, transform.position);
            RenderMaster.main.Render();
        }
    }

    public void Grab () {
        grabbed = true;
        RenderMaster.main.dragged = transform;
        RenderMaster.main.draggedName = id;
    }
    public void Ungrab () {
        grabbed = false;
        RenderMaster.main.dragged = null;
        RenderMaster.main.draggedName = null;
    }
}
