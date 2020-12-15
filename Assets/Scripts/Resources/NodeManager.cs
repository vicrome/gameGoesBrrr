using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public enum ResourceTypes { material1 };
    public ResourceTypes resourceType;

    public float harvestTime;
    public float availableResource;

    public Dictionary<int, bool> miDiccionario = new Dictionary<int, bool>();
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
        if(miDiccionario.Count > 0)
        {
            availableResource -= (10 * miDiccionario.Count);
            Debug.Log("Resources number have been decreased in " + (10 * miDiccionario.Count) + "by seg");
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
