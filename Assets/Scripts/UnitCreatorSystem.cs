using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCreatorSystem : MonoBehaviour
{
   
    public void createUnit(GameObject myObject)
    {
        Instantiate(myObject, Vector3.zero, Quaternion.identity);
    }

}
