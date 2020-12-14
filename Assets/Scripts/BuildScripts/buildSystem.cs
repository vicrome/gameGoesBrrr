using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildSystem : MonoBehaviour
{
    public Camera cam;
    public LayerMask layer;
    private GameObject previewGameObject = null;
    private preview previewScript = null;

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

        if (Physics.Raycast(ray, out hit, 100f, layer))
        {
            float y = hit.point.y + (previewGameObject.transform.localScale.y / 2f);
            Vector3 pos = new Vector3(hit.point.x, y, hit.point.z);
            previewGameObject.transform.position = pos;
        }
    }
}
