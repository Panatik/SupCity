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

    private Node TrouverRouteProche(Vector3 position)
    {
        float minDistance = Mathf.Infinity;
        Node routeLaPlusProche = null;

        Node[] tousLesNodes = FindObjectsByType<Node>(FindObjectsSortMode.None);

        foreach (Node node in tousLesNodes)
        {
            if (!node.isRoute) continue;

            float dist = Vector3.Distance(position, node.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                routeLaPlusProche = node;
            }
        }

        return routeLaPlusProche;
    }

    private List<Batiment> TrouverBatimentsAdjacents(Node node)
    {
        List<Batiment> batimentsTrouvés = new List<Batiment>();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(node.transform.position, 1.0f);
        foreach (var col in colliders)
        {
            Batiment bat = col.GetComponentInParent<Batiment>();
            if (bat != null && !batimentsTrouvés.Contains(bat))
            {
                batimentsTrouvés.Add(bat);
            }
        }

        return batimentsTrouvés;
    }

    public Batiment TrouverBatimentProche(float range)
    {
        Collider2D[] resultats = Physics2D.OverlapCircleAll(transform.position, range);
        foreach (var hit in resultats)
        {
            var placeable = hit.GetComponentInParent<PlaceableObject>();
            var bat = hit.GetComponentInParent<Batiment>();
            if (placeable != null && bat != null && bat.PeutAccepterOuvrier())
            {
                return bat;
            }
        }
        return null;
    }

    public Batiment TrouverBatimentViaRoute()
    {
        Queue<Node> toVisit = new Queue<Node>();
        HashSet<Node> visited = new HashSet<Node>();

        // Cherche la route la plus proche de la maison
        Node routeDépart = TrouverRouteProche(transform.position);
        if (routeDépart == null) return null;

        toVisit.Enqueue(routeDépart);
        visited.Add(routeDépart);

        while (toVisit.Count > 0)
        {
            Node current = toVisit.Dequeue();

            // Vérifie les bâtiments autour de ce node
            foreach (var bat in TrouverBatimentsAdjacents(current))
            {
                if (bat.PeutAccepterOuvrier())
                    return bat;
            }

            // Explore les voisins (routes connectées)
            foreach (var voisin in current.GetVoisins())
            {
                if (!visited.Contains(voisin) && voisin.isRoute)
                {
                    toVisit.Enqueue(voisin);
                    visited.Add(voisin);
                }
            }
        }

        return null;
    }

    public void AssignerTravailAuxOccupants(bool viaRoute, float range = 100f)
    {
        foreach (var pnj in occupants)
        {
            if (pnj.assignedWork != null) continue;

            Batiment batimentTrouvé = viaRoute
                ? TrouverBatimentViaRoute()
                : TrouverBatimentProche(range);

            if (batimentTrouvé != null)
            {
                pnj.AssignerTravail(batimentTrouvé);
            }
        }
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
