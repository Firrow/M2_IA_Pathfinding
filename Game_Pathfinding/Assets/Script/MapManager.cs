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
        //Permet d'afficher la position de la tuile sur laquelle on clique (pour debug)
        /*if (Input.GetMouseButtonDown(0))
        {
            DebugTilePositonWithClick();
        }*/
    }

    //Permet d'avoir le nom du type de tuile
    public TileBase GetTile(Vector3Int position)
    {
        Vector3Int gridPosition = map.WorldToCell(Vector3Int.FloorToInt(position));
        return map.GetTile(gridPosition);
    }

    public TileBase GetTile(Vector3 position)
    {
        Vector3Int gridPosition = map.WorldToCell(Vector3Int.FloorToInt(position));
        return map.GetTile(gridPosition);
    }


    //Retourne valeur de la tuile (permettant d'adapter la valeur au type de tuile)
    public float GetTileMovementSpeed(Vector3 worldPosition)
    {
        Vector3Int gridPosition = map.WorldToCell(worldPosition);
        TileBase tile = map.GetTile(gridPosition);

        if (tile == null)
            return 0f;

        return dataFromTile[tile].speedMovement;
    }


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

    //ASTAR : récupère uniquement les voisins de la tuile passée en paramètre
    public List<Vector3Int> GetNeighbors(Vector3Int cell)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();

        neighbors.Add(new Vector3Int(cell.x + 1, cell.y, cell.z)); // Droite
        neighbors.Add(new Vector3Int(cell.x - 1, cell.y, cell.z)); // Gauche
        neighbors.Add(new Vector3Int(cell.x, cell.y + 1, cell.z)); // Haut
        neighbors.Add(new Vector3Int(cell.x, cell.y - 1, cell.z)); // Bas


        /*
         NOTE : Lorsque l'ennemi peut aller en diagonal, il peut chevaucher certaines tuiles durant son déplacement
                Cependant, uniquement la valeur de la tuile d'arrivée sera prise en compte pour la vitesse de déplacement
        */

        return neighbors;
    }

    //DIJKSTRA : Récupérer les voisins du dernier point du chemin passé en paramètre ainsi que le cout global des chemins menant aux voisins
    public List<Path> GetPathNeighbors(Path path)
    {
        List<Path> neighbors = new List<Path>();

        neighbors.Add(new Path(new Vector3Int(path.destination.x + 1, path.destination.y, path.destination.z), (int)GetTileMovementSpeed(new Vector3(path.destination.x + 1, path.destination.y, path.destination.z)), path.GlobalPath)); // Droite
        neighbors.Add(new Path(new Vector3Int(path.destination.x - 1, path.destination.y, path.destination.z), (int)GetTileMovementSpeed(new Vector3(path.destination.x - 1, path.destination.y, path.destination.z)), path.GlobalPath)); // Gauche
        neighbors.Add(new Path(new Vector3Int(path.destination.x, path.destination.y + 1, path.destination.z), (int)GetTileMovementSpeed(new Vector3(path.destination.x, path.destination.y + 1, path.destination.z)), path.GlobalPath)); // Haut
        neighbors.Add(new Path(new Vector3Int(path.destination.x, path.destination.y - 1, path.destination.z), (int)GetTileMovementSpeed(new Vector3(path.destination.x, path.destination.y - 1, path.destination.z)), path.GlobalPath)); // Bas 

        return neighbors;
    }







    private void DebugTilePositonWithClick()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int gridPosition = map.WorldToCell(mousePosition);

        TileBase clickedTile = map.GetTile(gridPosition);

        float speedMovement = dataFromTile[clickedTile].speedMovement;

        Debug.Log(gridPosition);
    }
}
