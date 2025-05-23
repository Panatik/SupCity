using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WalkableFloorCreator : MonoBehaviour
{
    public enum Grid
    {
        Floor,
        Wall,
        Empty
    }

    public Grid[,] grid;

    public Tilemap floorMap;
    //public Tilemap wallMap;

    //public List<TileBase> walltiles;
    public Tilemap wallTilemap;

    public int mapWidth;
    public int mapHeight;

    public Node nodePrefab;
    public List<Node> nodeList;

    public NPC_Controller npc;

    private bool canDrawGizmos;

    private Vector3Int tilemapOrigin;

    private void Awake()
    {
        tilemapOrigin = floorMap.origin;
        InitializeGridFromTilemap();
    }

    private void InitializeGridFromTilemap()
    {
        BoundsInt bounds = floorMap.cellBounds;
        grid = new Grid[bounds.size.x, bounds.size.y];
        nodeList = new List<Node>();

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);
                TileBase wallTile = wallTilemap.GetTile(cellPosition);
                TileBase floorTile = floorMap.GetTile(cellPosition);

                if (wallTile != null)
                {
                    grid[x - bounds.xMin, y - bounds.yMin] = Grid.Wall;
                }
                else if (floorTile != null)
                {
                    grid[x - bounds.xMin, y - bounds.yMin] = Grid.Floor;
                }
                else
                {
                    grid[x - bounds.xMin, y - bounds.yMin] = Grid.Empty;
                }
            }
        }

        CreateNodes();
    }

    void CreateNodes()
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (grid[x, y] == Grid.Floor)
                {
                    // Convertir les coordonnées internes (grid) en coordonnées tilemap
                    Vector3Int cellPos = new Vector3Int(x + tilemapOrigin.x, y + tilemapOrigin.y, 0);

                    // Obtenir la position exacte dans le monde
                    Vector3 worldPos = floorMap.GetCellCenterWorld(cellPos);

                    // Créer un node à cette position
                    Node node = Instantiate(nodePrefab, worldPos, Quaternion.identity);
                    nodeList.Add(node);
                }
            }
        }

        //CreateConnections();

        HousePlacer housePlacer = FindFirstObjectByType<HousePlacer>();
        if (housePlacer != null)
        {
            housePlacer.RebuildAllConnections();
        }

        canDrawGizmos = true;
        SpawnAI();
    }

    void CreateConnections()
    {
        float maxDistance = 1.1f; // tolérance pour les diagonales si tu veux (ou mets 1.01f pour 4 directions)

        for (int i = 0; i < nodeList.Count; i++)
        {
            for (int j = i + 1; j < nodeList.Count; j++)
            {
                float dist = Vector2.Distance(nodeList[i].transform.position, nodeList[j].transform.position);

                if (dist <= maxDistance)
                {
                    ConnectNodes(nodeList[i], nodeList[j]);
                    ConnectNodes(nodeList[j], nodeList[i]);
                }
            }
        }

        // Supprimer les Nodes isolés
        nodeList.RemoveAll(node => node.connections.Count == 0);

        canDrawGizmos = true;
        SpawnAI();
    }

    void ConnectNodes(Node from, Node to)
    {
        if (from == to) { return; }
        from.connections.Add(to);
    }

    void SpawnAI()
    {
        List<Node> validSpawnNodes = nodeList.FindAll(n => n.connections.Count > 0);

        if (validSpawnNodes.Count == 0)
        {
            Debug.LogWarning("Aucun Node connecté disponible pour le spawn !");
            return;
        }

        Node spawnNode = validSpawnNodes[154];

        NPC_Controller newNPC = Instantiate(npc, spawnNode.transform.position, Quaternion.identity);
        newNPC.currentNode = spawnNode;

        Debug.Log("NPC spawn à : " + spawnNode.transform.position);
    }

    private void OnDrawGizmos()
    {
        if (canDrawGizmos && nodeList != null)
        {
            Gizmos.color = Color.blue;
            foreach (Node node in nodeList)
            {
                foreach (Node connection in node.connections)
                {
                    Gizmos.DrawLine(node.transform.position, connection.transform.position);
                }
            }
        }
    }
}
