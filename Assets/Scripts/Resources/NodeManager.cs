using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public enum ResourceTypes { material1 };
    public ResourceTypes resourceType;

    public float harvestTime;
    public float availableResource;

    public int numberGatherers;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ResourceTick());
    }

    // Update is called once per frame
    void Update()
    {
        if (availableResource <= 0)
        {
            Destroy(gameObject);
        }        
    }

    public void ResourceGather()
    {
        if(numberGatherers > 0)
        {
            availableResource -= (10 * numberGatherers);
            Debug.Log("Resources number have been decreased in " + (10 * numberGatherers) + "by seg");
        }
    }

    IEnumerator ResourceTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            ResourceGather();
        }
    }
}
