﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Unit : MonoBehaviour
{
    public NavMeshAgent myAgent;
    public GameObject selectorIcon;
    private Vector3 startingPos;

    public PlayerController PC;
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
                myAgent.SetDestination(targetNode.transform.position);
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
            if (isGathering && targetNode.GetComponent<NodeManager>().availableResource>0)
            {
                heldResource += 10;
                Debug.Log("Unit materials have increase in 10");
            }
            if (heldResource >= maxHeldResource)
            {
                Debug.Log("Vamos a dejar los materiales");
                drops = GameObject.FindGameObjectsWithTag("Drops");
                myAgent.SetDestination(PC.GetClosestDropOff(drops).transform.position);
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
                    myAgent.SetDestination(PC.GetClosestDropOff(drops).transform.position);
                    drops = null;
                    task = TaskList.Delivering;
                }
                else
                {
                    task = TaskList.Idle;
                }
            }
        }
    }
}
