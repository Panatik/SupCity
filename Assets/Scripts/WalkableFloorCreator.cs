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

    public List<NPC_Controller> npcPrefabs;

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
                    Vector3Int cellPos = new Vector3Int(x + tilemapOrigin.x, y + tilemapOrigin.y, 0);

                    Vector3 worldPos = floorMap.GetCellCenterWorld(cellPos);

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

        for (int i = 0; i < 20; i++) 
        {
            SpawnAI();
        }

        canDrawGizmos = true;
        StartCoroutine(SpawnAIWithDelay());
    }

    void CreateConnections()
    {
        float maxDistance = 1.1f;

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

        nodeList.RemoveAll(node => node.connections.Count == 0);

        for(int i=0; i<20; i++)
            SpawnAI();

        canDrawGizmos = true;
        StartCoroutine(SpawnAIWithDelay());
    }

    IEnumerator SpawnAIWithDelay()
    {
        for (int i = 0; i < 5; i++)
        {
            SpawnAI();
            yield return new WaitForSeconds(5 * 60f);
        }
    }


    bool IsWallBetween(Vector2 start, Vector2 end)
    {
        Vector2 direction = (end - start).normalized;
        float distance = Vector2.Distance(start, end);

        RaycastHit2D hit = Physics2D.Raycast(start, direction, distance);

        if (hit.collider != null && hit.collider.gameObject.CompareTag("Wall"))
        {
            return true;
        }

        return false;
    }

    void ConnectNodes(Node from, Node to)
    {
        if (from == to) { return; }
        from.connections.Add(to);
    }

    IEnumerator SpawnAIWithDelay() 
    {
        for (int i = 0; i < 5; i++) 
        {
            SpawnAI();
            yield return new WaitForSeconds(5 * 60f);
        }
    }

    void SpawnAI()
    {
        List<Node> validSpawnNodes = nodeList.FindAll(n => n.connections.Count > 0);

        if (validSpawnNodes.Count == 0)
        {
            Debug.LogWarning("Aucun Node connecté disponible pour le spawn !");
            return;
        }

        //Node spawnNode = validSpawnNodes[154];
        Node spawnNode = validSpawnNodes[Random.Range(0, validSpawnNodes.Count)];

        int index = Random.Range(0, npcPrefabs.Count);
        NPC_Controller chosenNPC = npcPrefabs[index];

        NPC_Controller newNPC = Instantiate(chosenNPC, spawnNode.transform.position, Quaternion.identity);
        newNPC.currentNode = spawnNode;

        Debug.Log("NPC spawné à : " + spawnNode.transform.position + " avec prefab : " + chosenNPC.name);
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
