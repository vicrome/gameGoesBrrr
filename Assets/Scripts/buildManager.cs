using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildManager : MonoBehaviour
{
    public GameObject foundation;
    public GameObject foundation2L;
    public GameObject foundation2X2;
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
    public void BeginBuild2L()
    {
        if (!myBuildSystem.isBuilding)
        {
            Debug.Log("Has empezado a  construir2L");
            myBuildSystem.NewBuild(foundation2L);
        }
    }
    public void BeginBuild2X2()
    {
        if (!myBuildSystem.isBuilding)
        {
            Debug.Log("Has empezado a  construir2X2");
            myBuildSystem.NewBuild(foundation2X2);
        }
    }
}
