using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyDijkstra : MonoBehaviour
{
    private GameObject player;
    public Grid grid;
    public int speed;

    private MapManager mapManager;
    private Vector3Int start;
    private Vector3Int goal;
    private Vector3Int newGoal;
    List<Vector3Int> path = new List<Vector3Int>();
    int currentPathIndex = 0;

    private void Awake()
    {
        mapManager = FindAnyObjectByType<MapManager>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //récupérer tuile position ennemi
        start = mapManager.GetGridCellAtWorldPosition(this.transform.position, grid);

        //récupérer tuile position player
        goal = mapManager.GetGridCellAtWorldPosition(player.transform.position, grid);

        path = Dijkstra(goal);
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            newGoal = mapManager.GetGridCellAtWorldPosition(player.transform.position, grid);
            if (goal != newGoal)
            {
                goal = newGoal;
                Vector3Int nextTileMove = path[currentPathIndex];
                start = mapManager.GetGridCellAtWorldPosition(this.transform.position, grid);
                path = Dijkstra(goal);


                if (path.Count > 1 && nextTileMove == path[1]) //si la prochaine tuile de ton mouvement == tuile sur laquelle tu dois te rendre en premier
                                                               //--> ignore le mouvement qui te replace sur ta tuile actuelle
                    currentPathIndex = 1;
                else
                    currentPathIndex = 0;                      //sinon retourne au centre de ta tuile actuelle 
            }
            MoveEnemy();
        }
    }

    private List<Vector3Int> Dijkstra(Vector3Int goal)
    {
        List<Path> closeList = new List<Path>();
        List<Path> openList = new List<Path>();
        // On initialise la case actuelle
        openList.Add(new Path(start, 0, new List<Path>()));

        while (true)
        {
            // On récupère le point le plus proche, on le retire de la liste à traiter et on l'ajoute à la liste finale
            Path nextPoint = openList.Aggregate(openList[0], (min, p) => p < min ? p : min);
            closeList.Add(nextPoint);
            openList.Remove(nextPoint);

            // Quand on atteint la destination
            if (nextPoint.destination == goal)
            {
                return ReconstructPath(nextPoint);
            }

            // On récupère les sommets voisins
            List<Path> neighbors = mapManager.GetPathNeighbors(nextPoint);

            // On ajoute les sommets voisins à la liste en conservant les moins chers
            for (int i = 0; i < openList.Count; i++)
            {
                Path newPath = neighbors.Find((n) => n == openList[i]);

                if (newPath is not null && newPath.GlobalCost < openList[i].GlobalCost)
                {
                    openList[i] = newPath;
                }
            }

            // On ajoute les sommets voisins s'ils n'existent pas dans la liste ouverte ni si les chemins sont déjà verrouilés
            foreach (var neighbor in neighbors)
            {
                if (!openList.Contains(neighbor) && !closeList.Contains(neighbor) && mapManager.GetTile(neighbor.destination).name != "wall")
                {
                    openList.Add(neighbor);
                }
            }
        }
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
                transform.position += moveDirection * adjustedSpeed * Time.fixedDeltaTime; // Ajoutez la vitesse de déplacement de l'ennemi en fonction de la tuile sur laquelle il se trouve
            }
        }
    }

    private List<Vector3Int> ReconstructPath(Path path)
    {
        List<Vector3Int> finalPath = new List<Vector3Int>();
        foreach (var p in path.GlobalPath)
        {
            finalPath.Add(p.destination);
        }

        return finalPath;
    }
}





public class Path
{
    public Vector3Int destination;
    public int cost;
    public List<Path> paths;

    public Path(Vector3Int destination, int cost, List<Path> paths)
    {
        this.destination = destination;
        this.cost = cost; //cout du dernier déplacement uniquement
        this.paths = paths; //liste de tous les points par lesquels on a besoin de passer
    }


    //Redéfinition des opérateurs pour réaliser des calculs sur les chemins
    public static bool operator ==(Path path1, Path path2)
    {
        return path1.destination == path2.destination;
    }

    public static bool operator !=(Path path1, Path path2)
    {
        return path1.destination != path2.destination;
    }

    public static bool operator >(Path path1, Path path2)
    {
        return path1.GlobalCost > path2.GlobalCost;
    }

    public static bool operator <(Path path1, Path path2)
    {
        return path1.GlobalCost < path2.GlobalCost;
    }

    public static bool operator >=(Path path1, Path path2)
    {
        return path1.GlobalCost >= path2.GlobalCost;
    }

    public static bool operator <=(Path path1, Path path2)
    {
        return path1.GlobalCost <= path2.GlobalCost;
    }

    public override bool Equals(object path)
    {
        Path p = (Path)path;
        return destination == p.destination;
    }

    public int GlobalCost
    {
        get { return cost + paths.Aggregate(0, (sum, p) => sum + p.cost); }
    }


    public List<Path> GlobalPath
    {
        get
        {
            List<Path> copyPaths = new List<Path>(paths);
            copyPaths.Add(this);
            return copyPaths;
        }
    }

}
