using static UnityEditor.Progress;
using System.Collections.Generic;
using UnityEngine;

public class Batiment : MonoBehaviour
{
    public string nomBatiment;

    public List<PNJ> GetPNJ() { /* TO DO */ }
    public List<Item> GetTools() { /* TO DO */ }
    public List<Item> GetRessourcesInput() { /* TO DO */ }
    public List<Item> GetRessourcesOutput() { /* TO DO */ }

    private void OnMouseDown()
    {
        BuildingUI.Instance.AfficherInfosBatiment(this);
    }
}
