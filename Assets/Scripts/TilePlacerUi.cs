using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class TilePlacerUi : MonoBehaviour
{
    public enum ToolMode { Place, Delete, Move }
    public ToolMode currentMode = ToolMode.Place;

    public Tilemap targetTilemap;
    public List<TileBase> placeableTiles;
    public List<TileBase> wallTiles;

    private TileBase selectedTile = null;
    private Vector3Int? lastPlacedCell = null;
    private GameObject selectedObject = null;

    public GameObject[] placeableObjects;
    public GameObject[] previewObjects;
    private GameObject currentPreview;

    private Vector3Int? draggedTileCell = null;
    private TileBase draggedTile = null;

    private GameObject draggedObject;
    private Vector3 originalPosition;
    private bool isDragging = false;

    private int selectedIndex = -1;

    private HousePlacer housePlacer;

    private Vector3Int? lastPlacedObjectCell = null;

    public enum ObjectSize
    {
        OneByOne,
        TwoByTwo
    }
    private ObjectSize currentObjectSize = ObjectSize.TwoByTwo;

    void Start()
    {
        housePlacer = FindFirstObjectByType<HousePlacer>();
    }

    public void SetToolMode(int mode)
    {
        currentMode = (ToolMode)mode;

        // Supprime la preview quand on passe en Delete ou Move
        if (currentMode == ToolMode.Delete || currentMode == ToolMode.Move)
        {
            if (currentPreview)
            {
                Destroy(currentPreview);
                currentPreview = null;
            }
        }
    }

    public void SelectObject(int index)
    {
        selectedTile = null;

        // Active le mode Place automatiquement
        currentMode = ToolMode.Place;

        if (currentPreview) Destroy(currentPreview);

        selectedIndex = index;

        if (index >= 0 && index < placeableObjects.Length)
        {
            selectedObject = placeableObjects[index];
            currentPreview = Instantiate(previewObjects[index]);
            MakePreview(currentPreview);

            // Définis la taille automatiquement (ajuste selon ta logique ou un tableau)
            currentObjectSize = (selectedObject.name.Contains("Road")) ? ObjectSize.OneByOne : ObjectSize.TwoByTwo;
        }
    }

    public void SelectTile(int index)
    {
        if (currentPreview)
            Destroy(currentPreview);
        selectedObject = null;
        if (index >= 0 && index < placeableTiles.Count)
            selectedTile = placeableTiles[index];
    }

    void MakePreview(GameObject obj)
    {
        foreach (var sr in obj.GetComponentsInChildren<SpriteRenderer>())
            sr.color = new Color(0f, 0.5f, 1f, 0.5f); // bleu transparent

        foreach (var col in obj.GetComponentsInChildren<Collider2D>())
            col.enabled = false;
    }

    void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        Vector3 intersectionPos = SnapToGrid(mouseWorldPos);
        float cellSize = targetTilemap.cellSize.x;

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButton(0))
        {
            switch (currentMode)
            {
                case ToolMode.Place:
                    if (selectedTile != null) PlaceTile(mouseWorldPos);
                    else if (selectedObject != null) PlaceObject(intersectionPos);
                    break;

                case ToolMode.Delete:
                    if (currentPreview)
                        Destroy(currentPreview);
                    HandleDelete();
                    break;

                case ToolMode.Move:
                    StartMove(intersectionPos);
                    break;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            lastPlacedCell = null;
            lastPlacedObjectCell = null;
        }

        if (isDragging)
        {
            currentPreview.transform.position = intersectionPos;

            if (Input.GetMouseButtonUp(0))
                EndMove(intersectionPos);
        }

        if (!isDragging && currentPreview)
        {
            Vector3Int cellPos = targetTilemap.WorldToCell(Input.mousePosition);
            currentPreview.transform.position = targetTilemap.GetCellCenterWorld(cellPos);
        }
        else if (isDragging && draggedTile != null && currentPreview)
        {
            Vector3Int cellPos = targetTilemap.WorldToCell(Input.mousePosition);
            currentPreview.transform.position = targetTilemap.GetCellCenterWorld(cellPos);
        }

        // La preview suit la souris si pas en déplacement
        if (!isDragging && currentPreview)
            currentPreview.transform.position = intersectionPos;
    }

    Vector3 SnapToGrid(Vector3 pos)
    {
        Vector3Int cell = targetTilemap.WorldToCell(pos);
        float cellSize = targetTilemap.cellSize.x;

        if (currentObjectSize == ObjectSize.OneByOne)
        {
            // Route : centré sur une case
            return targetTilemap.GetCellCenterWorld(cell);
        }
        else // TwoByTwo (Maison)
        {
            // On décale la sélection d'un demi vers le bas et la gauche
            Vector3 adjustedPos = pos - new Vector3(cellSize / 2f, cellSize / 2f, 0f);
            Vector3Int bottomLeftCell = targetTilemap.WorldToCell(adjustedPos);
            Vector3 bottomLeftWorld = targetTilemap.GetCellCenterWorld(bottomLeftCell);

            // Reviens au centre de 2x2 : ajout d'une demi-case en x et y
            return bottomLeftWorld + new Vector3(cellSize / 2f, cellSize / 2f, 0f);
        }
    }

    void PlaceTile(Vector3 worldPos)
    {
        Vector3Int cellPos = targetTilemap.WorldToCell(worldPos);

        if (lastPlacedCell.HasValue && lastPlacedCell.Value == cellPos)
            return; // on a déjà posé une tile ici

        TileBase currentTile = targetTilemap.GetTile(cellPos);

        if (!wallTiles.Contains(currentTile) && currentTile != null && !placeableTiles.Contains(currentTile))
        {
            targetTilemap.SetTile(cellPos, selectedTile);
            lastPlacedCell = cellPos; // mise à jour
        }
    }

    void PlaceObject(Vector3 worldPos)
    {
        float cellSize = targetTilemap.cellSize.x;

        // Prend la vraie position "snappée" pour comparaison
        Vector3 snappedWorldPos = SnapToGrid(worldPos);
        Vector3Int cell = targetTilemap.WorldToCell(snappedWorldPos);

        // Ne repose pas si même case que la dernière
        if (lastPlacedObjectCell.HasValue && lastPlacedObjectCell.Value == cell)
            return;

        if (currentObjectSize == ObjectSize.OneByOne)
        {
            cell.x -= 1;
            cell.y -= 1;

            // Vérifie s'il y a déjà un objet route à cet endroit
            Collider2D hit = Physics2D.OverlapPoint(snappedWorldPos, LayerMask.GetMask("Buildings"));
            if (hit != null && hit.CompareTag("Route"))
            {
                // On a déjà une route ici
                return;
            }
        }

        Vector2 size = new Vector2(2f * cellSize, 2f * cellSize);
        if (CanPlaceAt(cell) && IsAreaClear(snappedWorldPos, size))
        {
            GameObject placed = Instantiate(selectedObject, snappedWorldPos, Quaternion.identity);

            if (selectedObject.CompareTag("Buildings"))
            {
                selectedObject = null;
                Destroy(currentPreview);
            }
            else {
                if (currentPreview)
                    Destroy(currentPreview);
                currentPreview = Instantiate(previewObjects[selectedIndex]);
                MakePreview(currentPreview);
            }

            PlaceableObject id = placed.AddComponent<PlaceableObject>();
            id.objectIndex = selectedIndex;

            /*if (currentPreview)
                Destroy(currentPreview);
            currentPreview = Instantiate(previewObjects[selectedIndex]);
            MakePreview(currentPreview);*/

            // Marque la cellule comme dernière utilisée
            lastPlacedObjectCell = cell;

            if (housePlacer != null)
            {
                housePlacer.HandleHouseBlocking(placed);
            }
        }
        else
        {
            Debug.Log("Impossible de placer ici !");
        }
    }

    void HandleDelete()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(mousePos, LayerMask.GetMask("Buildings"));

        if (hit)
        {
            GameObject building = hit.transform.root.gameObject;
            Destroy(building);

            if (housePlacer != null)
                housePlacer.HandleHouseBlocking(null);
        }
        else
        {
            Vector3Int cellPos = targetTilemap.WorldToCell(mousePos);
            TileBase tile = targetTilemap.GetTile(cellPos);

            if (placeableTiles.Contains(tile))
            {
                // Remplace par une tile de fond plutôt que null (par exemple la première tile non placeable)
                TileBase groundTile = wallTiles.Count > 0 ? wallTiles[0] : null;

                targetTilemap.SetTile(cellPos, groundTile);
            }
        }

    }

    void StartMove(Vector3 worldPos)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(mousePos, LayerMask.GetMask("Buildings"));

        if (hit)
        {
            if (currentPreview) Destroy(currentPreview);

            draggedObject = hit.transform.root.gameObject;
            originalPosition = draggedObject.transform.position;

            PlaceableObject id = draggedObject.GetComponent<PlaceableObject>();
            if (id != null && id.objectIndex >= 0 && id.objectIndex < previewObjects.Length)
                currentPreview = Instantiate(previewObjects[id.objectIndex]);

            currentPreview.transform.position = SnapToGrid(originalPosition);
            draggedObject.SetActive(false);
            isDragging = true;
        }
        else
        {
            Vector3Int cell = targetTilemap.WorldToCell(mousePos);
            TileBase tile = targetTilemap.GetTile(cell);

            if (placeableTiles.Contains(tile))
            {
                draggedTileCell = cell;
                draggedTile = tile;

                targetTilemap.SetTile(cell, null); // on "prend" la tile

                // Crée un aperçu de la tile (optionnel)
                if (currentPreview) Destroy(currentPreview);
                currentPreview = new GameObject("TilePreview");
                SpriteRenderer sr = currentPreview.AddComponent<SpriteRenderer>();
                sr.sprite = ((Tile)tile).sprite;
                sr.color = new Color(0f, 0.5f, 1f, 0.5f);
            }
        }
    }

    void EndMove(Vector3 worldPos)
    {
        if (draggedTile != null && draggedTileCell.HasValue)
        {
            Vector3Int newCell = targetTilemap.WorldToCell(worldPos);
            TileBase current = targetTilemap.GetTile(newCell);

            if (current == null || placeableTiles.Contains(current))
            {
                targetTilemap.SetTile(newCell, draggedTile);
            }
            else
            {
                targetTilemap.SetTile(draggedTileCell.Value, draggedTile);
            }

            draggedTile = null;
            draggedTileCell = null;
        }

        if (draggedObject != null)
        {
            draggedObject.transform.position = worldPos;
            draggedObject.SetActive(true);
            draggedObject = null;
        }

        if (currentPreview)
            Destroy(currentPreview);

        isDragging = false;

        if (housePlacer != null)
            housePlacer.HandleHouseBlocking(draggedObject);
    }

    bool CanPlaceAt(Vector3Int cellPos)
    {
        BoundsInt bounds = new BoundsInt(cellPos.x, cellPos.y, 0, 2, 2, 1);
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            TileBase tile = targetTilemap.GetTile(pos);
            if (tile == null || wallTiles.Contains(tile))
                return false;
        }
        return true;
    }

    bool IsAreaClear(Vector3 center, Vector2 size)
    {
        if (currentPreview && currentPreview.GetComponent<Collider2D>())
            currentPreview.GetComponent<Collider2D>().enabled = false;

        Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, 0f, LayerMask.GetMask("Buildings"));

        //  Réduction légère des bounds pour éviter les bords qui se touchent
        Vector3 shrinkedSize = size * 0.98f; // 2% plus petit
        Bounds newBounds = new Bounds(center, shrinkedSize);

        foreach (var hit in hits)
        {
            if (hit == null) continue;

            Bounds otherBounds = hit.bounds;

            if (!newBounds.Intersects(otherBounds)) continue;

            Debug.Log($"Hit: {hit.name}, Tag: {hit.tag}");

            // Autorise les contacts entre routes
            if (currentObjectSize == ObjectSize.OneByOne && hit.CompareTag("Route"))
                continue;
            if (currentObjectSize == ObjectSize.TwoByTwo && hit.CompareTag("Route"))
                continue;

            return false;
        }

        return true;
    }
}
