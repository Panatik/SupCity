using System.Collections.Generic;
using UnityEngine;

public class Storage_Controller : MonoBehaviour {
    private Dictionary<string, Resource> resources = new Dictionary<string,Resource>();
    [SerializeField] string[] storable_resources;
    [SerializeField] int storage_capacity;
    [SerializeField] int storage_priority;
    void Start()
    {
        foreach (var resource_name in storable_resources) {
            resources.Add(resource_name, new Resource(resource_name, storage_capacity));
        }
    }

    void Update()
    {
        
    }
}
