using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Unit : MonoBehaviour
{
 
    private NavMeshAgent myAgent;
    private GameObject selectorIcon;
    private Vector3 startingPos;
    private Vector3 offset;

    public enum heldResources { material1 };
    public NodeManager.ResourceTypes heldResourceType;
    public int heldResource;
    public int maxHeldResource;
    public bool isGathering = false;

    // Start is called before the first frame update
    private void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
        selectorIcon = transform.GetChild(0).gameObject;
        selectorIcon.SetActive(false);
        startingPos = transform.position;
        StartCoroutine(GatherTick());
    }

    // Update is called once per frame
    private void Update()
    {
        if (heldResource >= maxHeldResource)
        {

        }
    }


    public void SelectUnit()
    {
        selectorIcon.SetActive(true);
    }

    public void DeselectUnit()
    {
        selectorIcon.SetActive(false);
        offset = Vector3.zero;
    }

    public void CalculateOffset(Vector3 _center)
    {
        Vector3 center = new Vector3(_center.x, transform.position.y, _center.z);
        offset = center - transform.position;
    }

    public void MoveToSpot(Vector3 _pos)
    {
        Vector3 pos = new Vector3(_pos.x, transform.position.y, _pos.z);
        Vector3 moveToPos = pos + offset;
        myAgent.SetDestination(moveToPos);
    }

    public void MoveToSpot(Vector3 _pos, Vector3 _center)
    {
        Vector3 center = new Vector3(_center.x, transform.position.y, _center.z);
        Vector3 pos = new Vector3(_pos.x, transform.position.y, _pos.z);

        offset = center - transform.position;
        Vector3 moveToPos = pos + offset;
        myAgent.SetDestination(moveToPos);
    }


    public void OnTriggerEnter(Collider other)
    {
        GameObject hitObject = other.gameObject;

        if (hitObject.tag == "Resource")
        {
            isGathering = true;
            hitObject.GetComponent<NodeManager>().numberGatherers++;
            heldResourceType = hitObject.GetComponent<NodeManager>().resourceType;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        GameObject hitObject = other.gameObject;

        if (hitObject.tag == "Resource")
        {
            isGathering = false;
            hitObject.GetComponent<NodeManager>().numberGatherers--;
        }
    }

    IEnumerator GatherTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (isGathering)
            {
                heldResource = heldResource + 10;
                Debug.Log("Unit materials have increase in 10");
            }
        }
    }
}
