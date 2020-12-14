using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildSystem : MonoBehaviour
{
    public Camera cam;
    public LayerMask layer;
    private GameObject previewGameObject = null;
    private preview previewScript = null;
    int LastPosX, LastPosY, LastPosZ;
    public string tipoBuilding;
    public float OffsetX,OffsetY, OffsetZ;

    public float stickTolerance = 1.5f;

    public bool isBuilding = false;
    private bool pauseBuilding = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            previewGameObject.transform.Rotate(0, 5f, 0);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            CancelBuild();
        }
        if (Input.GetMouseButtonDown(0) && isBuilding)
        {
            if (previewScript.GetCanBuild())
            {
                StopBuild();
            }
            else
            {
                Debug.Log("You can not build here");
            }
        }

        if (isBuilding)
        {
            if (pauseBuilding)
            {
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");

                if (Mathf.Abs(mouseX) >= stickTolerance || Mathf.Abs(mouseY) >= stickTolerance)
                {
                    pauseBuilding = false;
                }

            }
            else
            {
                DoBuildRay();
            }
        }
    }

    public void NewBuild(GameObject myObject)
    {
        previewGameObject = Instantiate(myObject, Vector3.zero, Quaternion.identity);
        previewScript = previewGameObject.GetComponent<preview>();
        isBuilding = true;
        tipoBuilding = previewScript.tipoBuilding;
        if (tipoBuilding == "1L")
        {
            OffsetX = 0;
            OffsetY = 0.5f;
            OffsetZ = 0;
        }
        if (tipoBuilding == "2L")
        {
            OffsetX = 0.5f;
            OffsetY = 0.5f;
            OffsetZ = 0;
        }
        if (tipoBuilding == "2X2")
        {
            OffsetX = 0.5f;
            OffsetY = 0.5f;
            OffsetZ = 0.5f;
        }
    }

    private void CancelBuild()
    {
        Destroy(previewGameObject);
        previewGameObject = null;
        previewScript = null;
        isBuilding = false;
    }

    private void StopBuild()
    {
        previewScript.Place();
        previewGameObject = null;
        previewScript = null;
        isBuilding = false;
    }

    public void PauseBuild(bool value) 
    {
        pauseBuilding = value;
    }

    private void DoBuildRay()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
        {
            int PosX = (int)Mathf.Round(hit.point.x);
            int PosY = (int)Mathf.Round(hit.point.y);
            int PosZ = (int)Mathf.Round(hit.point.z);
            Vector3 pos = new Vector3(PosX + OffsetX, PosY + OffsetY, PosZ+ OffsetZ);
            previewGameObject.transform.position = pos;

            if(PosX != LastPosX || PosY != LastPosY || PosZ != LastPosZ)
            {
                LastPosX = PosX;
                LastPosY = PosY;
                LastPosZ = PosZ;
                previewGameObject.transform.position = new Vector3(PosX+ OffsetX, PosY+ OffsetY, PosZ+ OffsetZ);
            }

            Debug.Log(pos);
        }
    }
}
