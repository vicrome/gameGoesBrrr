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

    public TaskList task;
    public ResourceManager resourceManager;

    public GameObject targetNode;

    GameObject[] drops;

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
            Debug.Log("Vamos a dejar los materiales");
            drops = GameObject.FindGameObjectsWithTag("Drops");
            MoveToSpot(GetClosestDropOff(drops).transform.position);
            drops = null;
            isGathering = false;
            targetNode.GetComponent<NodeManager>().miDiccionario.Remove(myAgent.GetComponent<Unit>().GetInstanceID());
            task = TaskList.Delivering;
        }

        if (targetNode == null)
        {
            if (heldResource != 0)
            {
                drops = GameObject.FindGameObjectsWithTag("Drops");
                MoveToSpot(GetClosestDropOff(drops).transform.position);
                drops = null;
                task = TaskList.Delivering;
            }
            else
            {
                task = TaskList.Idle;
            }
        }
    }

    GameObject GetClosestDropOff(GameObject[] dropOffs)
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

        if (hitObject.tag == "Resource" && task == TaskList.Gathering)
        {
            isGathering = true;
            hitObject.GetComponent<NodeManager>().miDiccionario.Add(myAgent.GetComponent<Unit>().GetInstanceID(), isGathering);
            heldResourceType = hitObject.GetComponent<NodeManager>().resourceType;
            targetNode = hitObject;
        }
        else if(hitObject.tag == "Drops" && task == TaskList.Delivering)
        {
            if (resourceManager.material1 < resourceManager.maxMaterial1)
            {
                resourceManager.material1 += heldResource;
                heldResource = 0;
                task = TaskList.Gathering;
                MoveToSpot(targetNode.transform.position);
            }
            else
            {
                task = TaskList.Idle;
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        GameObject hitObject = other.gameObject;

        if (hitObject.tag == "Resource")
        {
            isGathering = false;
        }
    }

    IEnumerator GatherTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (isGathering)
            {
                heldResource += 10;
                Debug.Log("Unit materials have increase in 10");
            }
        }
    }
}
