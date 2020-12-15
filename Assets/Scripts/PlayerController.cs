using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Selector mySelector;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000f))
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    MoveAllUnits(hit.point);
                    for (int i = 0; i < mySelector.selectedObjects.Count; i++)
                    {
                        Unit unit = mySelector.selectedObjects[i].GetComponent<Unit>();
                        unit.task = TaskList.Moving;
                    }
                    Debug.Log("Moving at hitted ground location");

                }
                else if (hit.collider.tag == "Resource")
                {
                    MoveAllUnits(hit.collider.gameObject.transform.position);
                    for (int i = 0; i < mySelector.selectedObjects.Count; i++)
                    {
                        Unit unit = mySelector.selectedObjects[i].GetComponent<Unit>();
                        unit.task = TaskList.Gathering;
                        unit.targetNode = hit.collider.gameObject;
                    }
                    Debug.Log("Moving to harvest");
                }
            }
        }
    }

    public void MoveAllUnits(Vector3 _pos)
    {
        for (int i = 0; i < mySelector.selectedObjects.Count; i++)
        {
            Unit unit = mySelector.selectedObjects[i].GetComponent<Unit>();
            MoveToSpot(_pos, unit);
        }
    }

    public GameObject GetClosestDropOff(GameObject[] dropOffs)
    {
        GameObject closestDrop = null;
        float closestDistance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject targetDrop in dropOffs)
        {
            Vector3 direction = targetDrop.transform.position - position;
            float distance = direction.sqrMagnitude;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestDrop = targetDrop;
            }
        }
        return closestDrop;
    }

    public void MoveToSpot(Vector3 _pos, Unit unit)
    {
        Vector3 pos = new Vector3(_pos.x, transform.position.y, _pos.z);
        Vector3 moveToPos = _pos;
        Debug.Log(unit + " se mueve a " + moveToPos);
        Debug.Log(unit.myAgent);
        unit.myAgent.SetDestination(moveToPos);
    }
}


