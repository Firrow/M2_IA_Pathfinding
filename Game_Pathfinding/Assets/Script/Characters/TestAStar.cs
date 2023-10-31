using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestAStar : MonoBehaviour
{
    //Autre éléments
    private GameObject player;
    public Grid grid;

    private int cost;
    private MapManager mapManager;
    private Vector3Int goal;
    private Vector3Int start;
    private Vector3Int current;
    Dictionary<Vector3Int, int> openList = new Dictionary<Vector3Int, int>();
    Dictionary<Vector3Int, int> closeList = new Dictionary<Vector3Int, int>();
    Dictionary<Vector3Int, int> possibleTiles = new Dictionary<Vector3Int, int>();
    Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();
    List<Vector3Int> neighbors = new List<Vector3Int>();
    List<Vector3Int> path = new List<Vector3Int>();


    private void Awake()
    {
        mapManager = FindAnyObjectByType<MapManager>();
        //moveCounter = moveTime;
    }


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        start = mapManager.GetGridCellAtWorldPosition(this.transform.position, grid);
        //récupérer tuile position player
        goal = mapManager.GetGridCellAtWorldPosition(player.transform.position, grid);

        path = AStar(goal);

        foreach (var tile in path)
        {
            Debug.Log(tile);
        }

    }

    void Update()
    {

    }

    private List<Vector3Int> AStar(Vector3Int goal)
    {
        //--------------------------------------------------------------------------------------------
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
                //Debug.Log("FIN !!!");
                openList.Clear();
                return ReconstructPath(cameFrom, current);
            }
            else
            {
                //b) déplacer la tuile actuelle de la liste ouverte à la liste fermée //ATTENTION : A VOIR SI PAS D'ERREUR TOUR 1 AVEC CURRENT
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
                this.transform.position = current;

                possibleTiles.Clear();
            }
        }

        throw new Exception("ERREUR CHEMIN IMPOSSIBLE ! ");
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
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Add(current);
        }

        totalPath.Reverse();
        return totalPath;
    }
}
