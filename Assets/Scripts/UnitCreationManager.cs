using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCreationManager : MonoBehaviour
{
    public GameObject foundation;
    public UnitCreatorSystem myUnitCreatorSystem;


    // Update is called once per frame
    public void CreateUnit()
    {        
            Debug.Log("Has creado un fulanito");
            myUnitCreatorSystem.createUnit(foundation);
    }
}
