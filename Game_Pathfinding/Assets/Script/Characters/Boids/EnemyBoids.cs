using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyBoids : MonoBehaviour
{
    private BoidsGameManager gameManager;
    private List<GameObject> AllSpiders = new List<GameObject>();
    private GameObject target;
    private float avoidanceRadius;
    private float speed;

    public float rotationSpeed = 20.0f;
    public Vector3 velocity;


    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<BoidsGameManager>();
        AllSpiders = gameManager.boids;
        target = GameObject.FindGameObjectWithTag("Player");
        velocity = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
        avoidanceRadius = gameManager.spiderAvoidanceRadius;
        speed = gameManager.speed;
    }

    void Update()
    {
        MoveSpider();
    }




    List<Transform> GetNeighbors()
    {
        List<Transform> neighbors = new List<Transform>();
        foreach (var spider in AllSpiders)
        {
            if (spider.transform != this.transform)
            {
                neighbors.Add(spider.transform);
            }
        }

        return neighbors;
    }


    //Rule1
    Vector3 MoveCloser(List<Transform> neighbors)
    {
        Vector2 cohesion = Vector2.zero;

        foreach (var neighbor in neighbors)
        {
            cohesion += (Vector2)neighbor.position;
        }

        cohesion /= neighbors.Count;

        return (cohesion - (Vector2)this.transform.position) / 100;
    }

    //Rule2
    Vector3 MoveAway(List<Transform> neighbors)
    {
        Vector3 avoidance = Vector3.zero;

        foreach (var neighbor in neighbors)
        {
            if (Vector3.Distance(this.transform.position, neighbor.position) < avoidanceRadius)
            {
                avoidance -= (neighbor.position - this.transform.position);
            }
        }

        return avoidance / 100;
    }

    //Rule3
    Vector3 MoveWith(List<Transform> neighbors)
    {
        Vector3 alignment = Vector3.zero;

        foreach (var neighbor in neighbors)
        {
            alignment += neighbor.GetComponent<EnemyBoids>().velocity;
        }

        alignment /= neighbors.Count;

        return (alignment - this.velocity) / 100;
    }



    private void MoveSpider()
    {
        // Obtient les voisins proches
        List<Transform> neighbors = GetNeighbors();

        Vector3 positionTarget = target.transform.position - this.transform.position;
        positionTarget.Normalize();

        // Calcule la direction moyenne des voisins
        Vector3 alignment = MoveWith(neighbors);
        // Calcule la position moyenne des voisins
        Vector3 cohesion = MoveCloser(neighbors);
        // Évite les collisions avec les voisins
        Vector3 avoidance = MoveAway(neighbors);


        // Applique les règles + Déplace le boid
        velocity = (positionTarget / 150f) + alignment + (cohesion / 1000f) + (avoidance / 10f);
        velocity *= speed;
        this.transform.position += velocity;
    }



    // Calculer la distance moyenne avec les autres boids
    // Adapter la vitesse (-=) de chaque boids en fonction de cette distance

    // FONCTION D'ALIGNEMENT moveWith(tableau boids) :
    // Calculer la vélocité moyenne du groupe de boids
    // Adapter la vitesse (+=) de chaque boids en fonction de cette vitesse

    // FONCTION DE REPULSION moveAway(tableau boids, distanceMinimale) :
    // SI LES BOIDS SONT DANS UNE ZONE DE REPULSION, ON DOIT CHANGER DE DIRECTION POUR S'ELOIGNER 
    //
    // Adapter la vitesse (+=) de chaque boids en fonction de cette vitesse

    // FONCTION DE MOUVEMENT move() : faire en sorte de ne pas aller sur les tuiles murs !

    // Update() :
    // Pour chaque boids :
    // Pour chaque boids :
    // Si le boid n'est pas le même :
    // On récupère la distance et si elle est pas trop grande, on ajoute le boids au groupe fermé (closeBoids)
    // Appel moveCloser()
    // Appel moveWith()
    // Appel moveAway()
    // Appel move()
}
