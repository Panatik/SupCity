using System.Collections.Generic;
using UnityEngine;

public class HousePlacer : MonoBehaviour
{
    private List<Node> previouslyBlockedNodes = new List<Node>();

    [Header("Références")]
    public GameObject housePrefab;
    public Transform houseParent;

    private Dictionary<GameObject, List<Node>> houseToBlockedNodes = new Dictionary<GameObject, List<Node>>();

    // Appelle cette méthode quand tu places une maison
    public void PlaceHouse(Vector3 position)
    {
        GameObject newHouse = Instantiate(housePrefab, position, Quaternion.identity, houseParent);
        HandleHouseBlocking(newHouse);
    }

    public void HandleHouseBlocking(GameObject house)
    {
        // Nettoyage des anciens blocs s’il y avait une maison au même emplacement
        if (houseToBlockedNodes.ContainsKey(house))
        {
            foreach (Node node in houseToBlockedNodes[house])
            {
                node.RemoveBlocker();
            }
            houseToBlockedNodes.Remove(house);
        }

        if (house != null)
        {
            Collider2D[] nodeHits = Physics2D.OverlapBoxAll(
                house.transform.position,
                house.GetComponentInChildren<BoxCollider2D>().size,
                0f,
                LayerMask.GetMask("Nodes")
            );

            List<Node> blockedNodes = new List<Node>();

            foreach (Collider2D hit in nodeHits)
            {
                Node node = hit.GetComponent<Node>();
                if (node != null)
                {
                    node.AddBlocker();
                    blockedNodes.Add(node);
                }
            }

            houseToBlockedNodes[house] = blockedNodes;
        }

        foreach (var npc in FindObjectsByType<NPC_Controller>(FindObjectsSortMode.None))
        {
            npc.RecalculatePath();
        }
    }

    public void UpdateNodeConnectionsAround(Vector3 center, float radius)
    {
        Node[] allNodes = FindObjectsByType<Node>(FindObjectsSortMode.None);

        foreach (Node node in allNodes)
        {
            if (Vector2.Distance(node.transform.position, center) > radius)
                continue;

            node.connections.Clear();

            if (node.IsBlocked)
                continue;

            foreach (Node other in allNodes)
            {
                if (other == node || other.IsBlocked)
                    continue;

                float distance = Vector2.Distance(node.transform.position, other.transform.position);
                if (distance <= 1.1f)
                {
                    node.connections.Add(other);
                }
            }
        }
    }

    private void UpdateAllNodeConnections()
    {
        Node[] allNodes = FindObjectsByType<Node>(FindObjectsSortMode.None);
        LayerMask nodeMask = LayerMask.GetMask("Nodes");

        Vector2[] directions = new Vector2[]
        {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
        };

        foreach (Node node in allNodes)
        {
            node.connections.Clear();

            if (node.IsBlocked)
                continue;

            foreach (Vector2 dir in directions)
            {
                RaycastHit2D hit = Physics2D.Raycast(node.transform.position, dir, 1.1f, nodeMask);
                if (hit.collider != null)
                {
                    Node neighbor = hit.collider.GetComponent<Node>();
                    if (neighbor != null && !neighbor.IsBlocked)
                    {
                        node.connections.Add(neighbor);
                    }
                }
            }
        }
    }

    public void UnblockHouseArea(GameObject house)
    {
        if (house != null && houseToBlockedNodes.ContainsKey(house))
        {
            foreach (Node node in houseToBlockedNodes[house]) { 
                node.RemoveBlocker();
            }

            houseToBlockedNodes.Remove(house);

            UpdateNodeConnectionsAround(house.transform.position, 2f);

            foreach (var npc in FindObjectsByType<PNJ>(FindObjectsSortMode.None)) { 
                npc.RecalculatePath();
            }
        }
    }

    public void RebuildAllConnections()
    {
        UpdateAllNodeConnections();
    }

    private void OnDrawGizmosSelected()
    {
        // Visualisation pour debug
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, housePrefab.GetComponentInChildren<BoxCollider2D>().size);
    }
}
