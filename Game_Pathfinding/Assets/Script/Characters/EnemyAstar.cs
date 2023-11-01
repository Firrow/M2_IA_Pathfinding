using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class EnemyAstar : MonoBehaviour
{
    private GameObject player;
    public Grid grid;
    public int speed;

    private MapManager mapManager;
    private Vector3Int goal;
    private Vector3Int newGoal;
    private Vector3Int start;
    private Vector3Int current;

    List<Vector3Int> path = new List<Vector3Int>();
    int currentPathIndex = 0;


    private void Awake()
    {
        mapManager = FindAnyObjectByType<MapManager>();
    }


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        start = mapManager.GetGridCellAtWorldPosition(this.transform.position, grid);

        //récupérer tuile position player
        goal = mapManager.GetGridCellAtWorldPosition(player.transform.position, grid);
        

        path = AStar(goal);
    }

    void Update()
    {
        newGoal = mapManager.GetGridCellAtWorldPosition(player.transform.position, grid);
        if (goal != newGoal)
        {
            goal = newGoal;
            start = mapManager.GetGridCellAtWorldPosition(this.transform.position, grid);
            path = AStar(goal);
        }
        MoveEnemy();
    }

    private List<Vector3Int> AStar(Vector3Int goal)
    {
        Dictionary<Vector3Int, int> openList = new Dictionary<Vector3Int, int>();
        Dictionary<Vector3Int, int> closeList = new Dictionary<Vector3Int, int>();
        Dictionary<Vector3Int, int> possibleTiles = new Dictionary<Vector3Int, int>();
        Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();
        List<Vector3Int> neighbors = new List<Vector3Int>();
        int cost;


        //1) On ajoute la tuile de départ à la liste ouverte
        //2) Ajouter son cout (= 0)
        cost = 0;
        openList.Add(start, cost);
        current = start;

        //3) TANT QUE LA LISTE OUVERTE N'EST PAS VIDE
        while (openList.Count > 0)
        {
            //Debug.Log("tuile actuelle : " + current);
            //a) si tuile actuelle est la tuile de fin --> FIN sinon
            if (current == goal)
            {
                openList.Clear();
                return ReconstructPath(cameFrom, current);
            }
            else
            {
                //b) déplacer la tuile actuelle de la liste ouverte à la liste fermée
                closeList.Add(current, cost);
                openList.Remove(current);

                //c) déterminer toutes les tuiles voisines
                neighbors = mapManager.GetNeighbors(current);
                //d) pour toutes les tuiles voisines v :
                foreach (var neighbor in neighbors)
                {
                    //si v n'est ni dans liste ouverte, ni dans liste fermée, ni de type "mur" -->
                    if (!openList.ContainsKey(neighbor) && !closeList.ContainsKey(neighbor) && mapManager.GetTile(neighbor).name != "wall")
                    {
                        //calculer son cout
                        cost = DistanceBetween(neighbor, start) + DistanceBetween(neighbor, goal) + (int)mapManager.GetTileMovementSpeed(neighbor);

                        //ajouter dans liste ouverte (dictionnaire (tuile, cout))
                        openList.Add(neighbor, cost);
                        cameFrom.Add(neighbor, current);
                    }
                }
                neighbors.Clear();

                int minCost = 1000000;
                //e) déterminer le cout le + bas
                foreach (var tile in openList)
                {
                    if (tile.Value < minCost)
                    {
                        minCost = tile.Value;
                    }
                }

                //f) choisir un des voisins ayant le cout le + bas pour devenir la nouvelle "current"
                foreach (var tile in openList)
                {
                    if (tile.Value == minCost)
                    {
                        possibleTiles.Add(tile.Key, tile.Value);
                    }
                }

                //g) choisir la nouvelle tuile actuelle
                System.Random r = new System.Random();
                int RandomIndex = r.Next(possibleTiles.Count);
                current = possibleTiles.ElementAt(RandomIndex).Key;

                possibleTiles.Clear();
            }
        }

        throw new Exception("ERREUR CHEMIN IMPOSSIBLE ! ");
    }

    private void MoveEnemy()
    {
        // Vérifiez si l'ennemi a atteint la fin du chemin
        if (currentPathIndex < path.Count)
        {
            Vector3Int nextTile = path[currentPathIndex];
            Vector3 nextTilePosition = grid.GetCellCenterWorld(nextTile);

            // Vérifiez la distance entre l'ennemi et la tuile suivante
            float distanceToNextTile = Vector3.Distance(transform.position, nextTilePosition);
            if (distanceToNextTile < 0.1f)
            {
                currentPathIndex++; // Passez à la tuile suivante
            }
            else
            {
                // Déplacez l'ennemi vers la tuile suivante
                float adjustedSpeed = speed / mapManager.GetTileMovementSpeed(transform.position);
                Vector3 moveDirection = (nextTilePosition - transform.position).normalized;
                transform.position += moveDirection * adjustedSpeed * Time.deltaTime; // Ajoutez la vitesse de déplacement de l'ennemi en fonction de la tuile sur laquelle il se trouve
            }
        }
    }



    private int DistanceBetween(Vector3Int current, Vector3Int otherTile)
    {
        //diapo 45 : prendre pour cout la distance entre les deux tuiles + le cout du terrain
        int dx = Math.Abs(current.x - otherTile.x);
        int dy = Math.Abs(current.y - otherTile.y);

        return dx + dy;
    }

    private List<Vector3Int> ReconstructPath(Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int current)
    {
        List<Vector3Int> totalPath = new List<Vector3Int>();
        totalPath.Add(goal);
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Add(current);
        }

        totalPath.Reverse();
        return totalPath;
    }
}

