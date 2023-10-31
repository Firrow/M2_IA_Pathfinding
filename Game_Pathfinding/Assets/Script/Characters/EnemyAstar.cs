using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyAstar : MonoBehaviour
{
    //map
    private MapManager mapManager;
    [SerializeField] private Tilemap map;
    public Grid grid;

    //déplacement Ennemis
    [SerializeField] private float moveTime;
    private float moveCounter;
    [SerializeField] private float baseSpeed;

    //AStar
    List<Vector3Int> openList = new List<Vector3Int>();
    List<Vector3Int> closeList = new List<Vector3Int>();
    Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();
    private Vector3Int goal;
    private Vector3Int start;
    private Vector3Int current;
    List<Vector3Int> neighbors = new List<Vector3Int>();
    Dictionary<Vector3Int, int> gScore = new Dictionary<Vector3Int, int>();
    Dictionary<Vector3Int, int> hScore = new Dictionary<Vector3Int, int>();
    Dictionary<Vector3Int, int> fScore = new Dictionary<Vector3Int, int>();
    int index;
    int tempFS;
    List<float> cost = new List<float>();
    List<Vector3Int> possibleTile = new List<Vector3Int>();

    //Autre éléments
    private GameObject player;


    private void Awake()
    {
        mapManager = FindAnyObjectByType<MapManager>();
        //moveCounter = moveTime;
    }


    // Algo A*
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //récupérer tuile position player
        /*goal = mapManager.GetGridCellAtWorldPosition(player.transform.position, grid);
        //récupérer tuile position ennemi
        start = mapManager.GetGridCellAtWorldPosition(this.transform.position, grid);
        current = start;*/

        current = mapManager.GetGridCellAtWorldPosition(this.transform.position, grid);

    }

    [Obsolete]
    void Update()
    {
        //récupérer tuile position player
        goal = mapManager.GetGridCellAtWorldPosition(player.transform.position, grid);
        List<Vector3Int> path = new List<Vector3Int>();


        //--------------------------------------------------------------------------------------------
        openList.Add(current);
        gScore.Add(current, 0);
        hScore.Add(current, Heuristique(current, goal)); //attention : est ce qu'il faut utiliser start ou current ?
        fScore.Add(current, gScore[current] + hScore[current]);

        while (openList.Count > 0)
        {
            Debug.Log("current : " + current);

            tempFS = 10000000;
            foreach (var fs in fScore)
            {
                if (fs.Value < tempFS)
                {
                    current = fs.Key;
                    tempFS = fs.Value;
                }
            }

            if (current == goal)
            {
                Debug.Log("FIN !");
                path = ReconstructPath(cameFrom, current);
            }

            openList.Remove(current);
            closeList.Add(current);

            neighbors = mapManager.GetNeighbors(current);

            index = 0;
            foreach (var neighbor in neighbors)
            {
                index++;

                if (closeList.Contains(neighbor))
                    continue;

                if (!closeList.Contains(neighbor) && !openList.Contains(neighbor)/* || tentativeGScore < gScore[neighbor]*/)
                {
                    //Ajout des cases voisines à la liste ouverte
                    //gScore.Remove(neighbor);
                    int tentativeGScore = DistanceBetween(current, neighbor);
                    gScore.Add(neighbor, tentativeGScore);
                    hScore.Add(neighbor, Heuristique(neighbor, goal));
                    fScore.Add(neighbor, gScore[neighbor] + hScore[neighbor]);
                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                        cameFrom.Add(neighbor, current);
                        cost.Add(mapManager.GetTileMovementSpeed(neighbor));
                    }
                }
            }

            //On regarde quelles tuiles ont le coup minimum et on en choisit une au hasard
            int min = (int)cost.Min(); //PROBLEME AVEC COST MIN : la tuile n'est pas forcément accessible (si elle est dans la listClose)
            
            Debug.Log("cout min : " + min);
            //cost.Sort();
            foreach (var tile in openList)
            {
                if (mapManager.GetTileMovementSpeed(tile) == min)
                {
                    possibleTile.Add(tile);
                }
            }
            /*
             PARCOURIR TOUTES LES TUILES VOISINES :
                SI LA TUILE N'EST PAS DANS LA LISTE CLOSE
                    SI LA TUILE A LE COUT LE PLUS BAS :
                        AJOUTER A LA LISTE DES TUILES POSSIBLES
                SI TOUTES LES TUILES AVEC LE COUT LE + BAS SONT DANS LA LISTE CLOSE, LE MIN PASSE À Cost[1], etc.
             */

            int RandomIndex = UnityEngine.Random.Range(0, possibleTile.Count());
            current = possibleTile[RandomIndex];
            this.transform.position = current; //JUSTE POUR INFORMATION
            gScore.Clear();
            hScore.Clear();
            fScore.Clear();
            possibleTile.Clear();
            cost.Clear();
            neighbors.Clear();
            Debug.Log("------------------------------------------------------------------------------------------------------");
        }

        //throw new Exception("Erreur chemin");

        /*foreach (var c in path)
        {
            Debug.Log(c);
        }*/
        //Debug.Log("----------------------------");
    }

    /*private void Move()
    {
        //Rotation (aléatoire)
        moveCounter -= Time.deltaTime;
        if (moveCounter <= 0)
        {
            moveCounter = moveTime;
            //transform.rotation = Quaternion.Euler(0f, 0f, Random.RandomRange(0f, 360f));
        }

        //Déplacement
        float adjustedSpeed = baseSpeed / mapManager.GetTileMovementSpeed(transform.position + transform.up * Time.deltaTime); //on regarde la case plus loin pour ne pas être bloquée sur la case mur
        transform.position += transform.up * Time.deltaTime * adjustedSpeed;
    }*/

    /*public List<Vector3Int> AStar(Vector3Int start, Vector3Int goal) //pas un void
    {
        Debug.Log(start);
        Debug.Log(goal);

        openList.Add(start);
        //gScore.Add(start, 0);
        hScore.Add(start, Heuristique(current, goal)); //attention : est ce qu'il faut utiliser start ou current ?
        fScore.Add(start, gScore[start] + hScore[start]);

        while (openList.Count > 0)
        {
            int tempFS = 10000000;
            foreach (var fs in fScore)
            {
                if (fs.Value < tempFS)
                {
                    current = fs.Key;
                    tempFS = fs.Value;
                }
            }
            Debug.Log("current : " + current);
            if (current == goal)
            {
                Debug.Log("FIN !");
                return ReconstructPath(cameFrom, current);
            }

            openList.Remove(current);
            closeList.Add(current);

            neighbors = mapManager.GetNeighbors(current);
            gScore.Add(current, (int)mapManager.GetTileMovementSpeed(current)); //mise à jour du gScore

            Debug.Log(" AVANT FOREACH ");
            foreach (var neighbor in neighbors)
            {
                Debug.Log("voisin : " + neighbor);
                if (closeList.Contains(neighbor))
                    continue;

                gScore.Add(neighbor, (int)mapManager.GetTileMovementSpeed(neighbor)); //on ajoute la valeur de la tuile voisin qu'on observe
                int tentativeGScore = gScore[neighbor] + DistanceBetween(current, neighbor);

                if (!openList.Contains(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    //Passage de la case "current" à la case voisine
                    cameFrom.Add(neighbor, current);
                    gScore.Add(neighbor, tentativeGScore);
                    hScore.Add(neighbor, Heuristique(neighbor, goal));
                    fScore.Add(neighbor, gScore[neighbor] + hScore[neighbor]);
                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                        this.transform.position = neighbor;
                    }
                }
            }
        }

        throw new Exception("Erreur chemin");
    }*/

    private int Heuristique(Vector3Int current, Vector3Int goal)
    {
        //diapo 45 : prendre pour coup la distance entre les deux tuiles + le cout du terrain
        int dx = Math.Abs(current.x - goal.x);
        int dy = Math.Abs(current.y - goal.y);
        int currentCost = (int)mapManager.GetTileMovementSpeed(current);

        return dx + dy + currentCost;
    }

    //Distance en fonction du cout de la tuile
    private int DistanceBetween(Vector3Int current, Vector3Int neighbor)
    {
        int tileValueA = (int)mapManager.GetTileMovementSpeed(current);
        int tileValueB = (int)mapManager.GetTileMovementSpeed(neighbor);
        return Math.Abs(tileValueA - tileValueB);
    }

    private List<Vector3Int> ReconstructPath(Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int current)
    {
        List<Vector3Int> totalPath = new List<Vector3Int>();
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Add(current);
        }
        return totalPath;
    }
}

