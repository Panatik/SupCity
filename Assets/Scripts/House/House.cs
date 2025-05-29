using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    [Header("Configuration")]
    public int houseLevel = 2;
    public int maxOccupants = 2;

    private List<PNJ> occupants = new List<PNJ>();
    private List<Node> occupiedNodes = new List<Node>();

    public Sprite house;
    public string houseName;
    public string description;

    private void Start()
    {
        UpdateOccupiedNodes(); // détecte automatiquement les nodes que la maison occupe
    }

    private void UpdateOccupiedNodes()
    {
        occupiedNodes.Clear();
        Node[] allNodes = FindObjectsByType<Node>(FindObjectsSortMode.None);

        foreach (Node node in allNodes)
        {
            // Teste si la position du node est "sous" la maison
            Collider2D houseCollider = GetComponentInChildren<Collider2D>();
            if (houseCollider == null) return;

            Bounds houseBounds = houseCollider.bounds;
            if (houseBounds.Contains(node.transform.position))
            {
                occupiedNodes.Add(node);
            }
        }
    }

    public void RegisterOccupiedNodes()
    {
        occupiedNodes.Clear();

        Node[] allNodes = FindObjectsByType<Node>(FindObjectsSortMode.None);
        Bounds houseBounds = GetComponentInChildren<Collider2D>().bounds;

        foreach (Node node in allNodes)
        {
            if (houseBounds.Contains(node.transform.position))
            {
                occupiedNodes.Add(node);
            }
        }

        Debug.Log($"[House] {occupiedNodes.Count} nodes occupés enregistrés pour la maison {name}.");
    }

    public bool HasSpace()
    {
        return occupants.Count < maxOccupants;
    }

    public void AddOccupant(PNJ pnj)
    {
        if (!occupants.Contains(pnj) && HasSpace())
        {
            occupants.Add(pnj);
        }
    }

    public void RemoveOccupant(PNJ pnj)
    {
        if (occupants.Contains(pnj))
        {
            occupants.Remove(pnj);
        }
    }

    public List<PNJ> GetOccupants()
    {
        return occupants;
    }

    public List<Node> GetOccupiedNodes()
    {
        return occupiedNodes;
    }

    public void AfficherInfos()
    {
        // Appelle le UIManager pour afficher les infos
        HouseUi.Instance.AfficherMenuMaison(this);
    }

    private void OnMouseDown()
    {
        AfficherInfos();
    }
}
