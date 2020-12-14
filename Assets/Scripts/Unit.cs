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
    public bool isSprite;

    // Start is called before the first frame update
    private void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
        selectorIcon = transform.GetChild(0).gameObject;
        selectorIcon.SetActive(false);
        startingPos = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        
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
}
