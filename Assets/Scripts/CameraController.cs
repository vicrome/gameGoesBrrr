using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 LimitUpLeft;
    public Vector3 LimitDownRight;
    public float MinZoom, MaxZoom;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.x += Input.GetAxis("Horizontal") * .3f;
        pos.y += -scroll * 5f;
        pos.z += Input.GetAxis("Vertical") * .3f;

        pos.y = Mathf.Clamp(pos.y, MinZoom, MaxZoom);

        /*Camera.main.fieldOfView -= scroll*20;
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView,MinZoom,MaxZoom);*/

        // TODO Funcion para que cambie la z y x max y min segun el zoom que tengamos 

        pos.x = Mathf.Clamp(pos.x, -LimitUpLeft.x, LimitDownRight.x);
        pos.z = Mathf.Clamp(pos.z, -LimitDownRight.z, LimitUpLeft.z);
        transform.position = pos;
    }
}
