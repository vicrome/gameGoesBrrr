using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildManager : MonoBehaviour
{
    public GameObject foundation;
    public buildSystem myBuildSystem;

    // Update is called once per frame
    public void BeginBuild()
    {
        if (!myBuildSystem.isBuilding)
        {
            Debug.Log("Has empezado a  construir");
            myBuildSystem.NewBuild(foundation);
        }
    }
}
