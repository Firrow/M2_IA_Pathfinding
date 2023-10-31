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

            //Debug.Log("Tile : " + clickedTile + " speed movement : " + speedMovement);
            Debug.Log(gridPosition);
        }
    }

    //Permet d'avoir le type de tuile
    public TileBase GetTile(Vector3Int position)
    {
        Vector3Int gridPosition = map.WorldToCell(Vector3Int.FloorToInt(position));
        return map.GetTile(gridPosition);
    }


    //Retourne valeur de la tuile
    public float GetTileMovementSpeed(Vector3 worldPosition)
    {
        Vector3Int gridPosition = map.WorldToCell(worldPosition);
        TileBase tile = map.GetTile(gridPosition);

        if (tile == null)
            return 0f;

        return dataFromTile[tile].speedMovement;
    }
    //Retourne valeur de la tuile
    /*public float GetTileMovementSpeed(TileBase tile)
    {
        if (tile == null)
            return 0f;

        return dataFromTile[tile].speedMovement;
    }*/


    //Retourne la position de la tuile
    public Vector3Int GetGridCellAtWorldPosition(Vector3 worldPosition, Grid grid)
    {
        Vector3Int gridPosition = grid.WorldToCell(worldPosition);
        return gridPosition;
    }

    public Vector3 GetWorldPositionFromGridCell(Vector3Int cell)
    {
        return map.CellToWorld(cell);
    }



    public List<Vector3Int> GetNeighbors(Vector3Int cell)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();

        // Ajoutez ici les cellules voisines en fonction de votre structure de grille.
        // Par exemple, pour une grille 2D, vous pouvez ajouter les cellules voisines en haut, en bas, à gauche et à droite.

        /*if (map.GetTile(cell) == null)
        {

        }*/
        neighbors.Add(new Vector3Int(cell.x + 1, cell.y, cell.z)); // Droite
        neighbors.Add(new Vector3Int(cell.x - 1, cell.y, cell.z)); // Gauche
        neighbors.Add(new Vector3Int(cell.x, cell.y + 1, cell.z)); // Haut
        neighbors.Add(new Vector3Int(cell.x, cell.y - 1, cell.z)); // Bas

        neighbors.Add(new Vector3Int(cell.x + 1, cell.y + 1, cell.z)); // Diagonale Haut Droit
        neighbors.Add(new Vector3Int(cell.x + 1, cell.y - 1, cell.z)); // Diagonale Haut Gauche
        neighbors.Add(new Vector3Int(cell.x - 1, cell.y + 1, cell.z)); // Diagonale Bas Droit
        neighbors.Add(new Vector3Int(cell.x - 1, cell.y - 1, cell.z)); // Diagonale Bas Gauche

        return neighbors;
    }
}
