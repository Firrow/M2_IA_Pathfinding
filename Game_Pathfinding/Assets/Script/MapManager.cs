using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{

    [SerializeField] private Tilemap map;
    [SerializeField] private List<TileData> tileDatas;
    private Dictionary<TileBase, TileData> dataFromTile;

    private void Awake()
    {
        //récupération des données
        dataFromTile = new Dictionary<TileBase, TileData>();
        foreach (var tileData in tileDatas)
        {
            foreach (var tile in tileData.tiles)
            {
                dataFromTile.Add(tile, tileData);
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = map.WorldToCell(mousePosition);

            TileBase clickedTile = map.GetTile(gridPosition);

            float speedMovement = dataFromTile[clickedTile].speedMovement;

            Debug.Log("Tile : " + clickedTile + " speed movement : " + speedMovement);
        }
    }


    public TileBase GetTile(Vector2 position)
    {
        Vector3Int gridPosition = map.WorldToCell(Vector3Int.FloorToInt(position));
        return map.GetTile(gridPosition);
    }

    public Vector3Int GetGridCellAtWorldPosition(Vector3 worldPosition, Grid grid)
    {
        Vector3Int gridPosition = grid.WorldToCell(worldPosition);
        return gridPosition;
    }

    public float GetTileMovementSpeed(Vector3 worldPosition)
    {
        Vector3Int gridPosition = map.WorldToCell(worldPosition);
        TileBase tile = map.GetTile(gridPosition);

        if (tile == null)
            return 0f;

        return dataFromTile[tile].speedMovement;
    }

    public float GetTileMovementSpeed(TileBase tile)
    {
        if (tile == null)
            return 0f;

        return dataFromTile[tile].speedMovement;
    }

    public List<Vector3Int> GetNeighbors(Vector3Int cell)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();

        // Ajoutez ici les cellules voisines en fonction de votre structure de grille.
        // Par exemple, pour une grille 2D, vous pouvez ajouter les cellules voisines en haut, en bas, à gauche et à droite.

        neighbors.Add(new Vector3Int(cell.x + 1, cell.y, cell.z)); // Droite
        neighbors.Add(new Vector3Int(cell.x - 1, cell.y, cell.z)); // Gauche
        neighbors.Add(new Vector3Int(cell.x, cell.y + 1, cell.z)); // Haut
        neighbors.Add(new Vector3Int(cell.x, cell.y - 1, cell.z)); // Bas

        return neighbors;
    }

    public Vector3 GetWorldPositionFromGridCell(Vector3Int cell)
    {
        return map.CellToWorld(cell);
    }
}
