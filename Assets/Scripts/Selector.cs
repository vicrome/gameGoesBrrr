using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Selector : MonoBehaviour
{
    [Header("For Raycast")]
    public Camera cam;
    public LayerMask mask;

    [Header("Gameobject for selection box")]
    public GameObject selectorBox;
    //[Header("Selection Box start/end points")]
    private Vector3 startPoint;
    private Vector3 endPoint;
    public Vector3 offset;


    //[Header("Selection Box Info")]
    private Vector3 rectCenter;
    private Vector3 rectSize;
    private Vector3 halfExtents;

    //[Header("All of the currently Selected Units")]
    public List<GameObject> selectedObjects = new List<GameObject>();

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPoint = DoRay();
            selectorBox.SetActive(true);
        }

        if (Input.GetMouseButton(0))
        {
            endPoint = DoRay();
            HandleRectangle();
            selectorBox.transform.position = rectCenter;
            selectorBox.transform.localScale = rectSize + new Vector3(0f, 1f, 0f);
        }

        if (Input.GetMouseButtonUp(0))
        {
            selectorBox.SetActive(false);
            endPoint = DoRay();
            HandleRectangle();
            SelectAllUnits();
        }
    }

    private void HandleRectangle()
    {
        rectSize = startPoint - endPoint;
        rectSize.x = Mathf.Abs(rectSize.x);
        rectSize.y = Mathf.Abs(rectSize.y);
        rectSize.z = Mathf.Abs(rectSize.z);

        rectCenter = (startPoint + endPoint) * 0.5f;

        halfExtents = rectSize * 0.5f;
    }

    private void ClearAllUnits()
    {
        for (int i = 0; i < selectedObjects.Count; i++)
        {
            Unit unit = selectedObjects[i].GetComponent<Unit>();
            DeselectUnit(unit);
        
        }

        selectedObjects.Clear();
    }

    private void SelectAllUnits()
    {
        ClearAllUnits();

        RaycastHit[] check = Physics.BoxCastAll(rectCenter, halfExtents, Vector3.up);

        for (int i = 0; i < check.Length; i++)
        {
            if (check[i].collider.CompareTag("Unit"))
            {
                Unit unit = check[i].collider.GetComponent<Unit>();
                SelectUnit(unit);
                CalculateOffset(rectCenter);
                selectedObjects.Add(unit.gameObject);
            }
        }
    }


    private Vector3 DoRay()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f, mask))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                return hit.point;
            }
        }

        return Vector3.zero;
    }

    public void SelectUnit(Unit unit)
    {
        unit.selectorIcon.SetActive(true);
    }

    public void DeselectUnit(Unit unit)
    {
        unit.selectorIcon.SetActive(false);
        offset = Vector3.zero;
    }

    public void CalculateOffset(Vector3 _center)
    {
        Vector3 center = new Vector3(_center.x, transform.position.y, _center.z);
        offset = center - transform.position;
    }
}
