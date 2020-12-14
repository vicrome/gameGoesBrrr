using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class preview : MonoBehaviour
{
    public GameObject prefab;

    private MeshRenderer myRend;
    public Material goodMat;
    public Material badMat;
    public string tipoBuilding;

    private buildSystem myBuildSystem;

    private bool canBuild = true;
    public bool isFoundation = false;

    public List<string> tagISnapTo = new List<string>();


    private void Start()
    {
        myBuildSystem = GameObject.FindObjectOfType<buildSystem>();
        myRend = GetComponent<MeshRenderer>();
        ChangeColor();
    }

    public void Place()
    {
        Instantiate(prefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void ChangeColor()
    {
        if (canBuild)
        {
            myRend.material = goodMat;
        }
        else
        {
            myRend.material = badMat;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < tagISnapTo.Count; i++)
        {
            string currentTag = tagISnapTo[i];

            if (other.tag == currentTag)
            {
                transform.position = other.transform.position;
                canBuild = false;
                ChangeColor();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < tagISnapTo.Count; i++)
        {
            string currentTag = tagISnapTo[i];

            if (other.tag == currentTag)
            {
                canBuild = true;
                ChangeColor();
            }
        }
    }

    public bool GetCanBuild() 
    {
        return canBuild;
    }
}
