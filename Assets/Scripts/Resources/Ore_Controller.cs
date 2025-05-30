using System.Collections.Generic;
using UnityEngine;

public class Ore_Controller : Harvestable_Controller {

    [ContextMenu("Harvest ore !")]
    public override Dictionary<string, int> harvest() {
        return base.harvest();
    }
}
