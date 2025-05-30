using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Harvestable_Controller : MonoBehaviour
{
    protected bool is_harvestable = false;
    [SerializeField] protected int harvest_amount = 1;
    [SerializeField] protected string resource_name;

    public string get_res_name() {
        return resource_name;
    }

    public virtual Dictionary<string,int> harvest() {
        Dictionary<string,int> resources = new Dictionary<string,int>();
        resources.Add(resource_name, harvest_amount);
        return resources;
    }
}
