using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyBoids : MonoBehaviour
{
    private BoidsGameManager gameManager;
    private List<GameObject> AllSpiders = new List<GameObject>();
    private Rigidbody rb;
    private GameObject target;

    public float speed = 3.0f;
    public float rotationSpeed = 20.0f;
    public float neighborRadius = 2.0f;
    public float avoidanceRadius = 2.0f;
    private Vector2 direction;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<BoidsGameManager>();
        AllSpiders = gameManager.boids;
        rb = this.GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player");
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





    // Qu'est ce que closeBoids ? les boids qui sont dans une zone qu'on considère
    // Comment marche la fonction de répulsion ?

    void Update()
    {
        // Obtient les voisins proches
        List<Transform> neighbors = GetNeighbors();

        Vector2 directionToTarget = target.transform.position - this.transform.position;
        directionToTarget.Normalize();

        // Calcule la direction moyenne des voisins
        Vector2 alignment = ComputeAlignment(neighbors, directionToTarget);
        // Calcule la position moyenne des voisins
        Vector2 cohesion = ComputeCohesion(neighbors);
        // Évite les collisions avec les voisins
        Vector2 avoidance = ComputeAvoidance(neighbors);

        // Applique les règles pour mettre à jour la direction
        Vector2 boidsDirection = alignment + cohesion + avoidance;
        boidsDirection.Normalize();



        // Déplace le boid
        direction = directionToTarget + boidsDirection;
        transform.position = Vector2.MoveTowards(this.transform.position, directionToTarget + boidsDirection, speed * Time.deltaTime);
        
    }

    List<Transform> GetNeighbors()
    {
        //Collider[] colliders = Physics.OverlapSphere(transform.position, neighborRadius);
        List<CircleCollider2D> colliders = new List<CircleCollider2D>();

        foreach (var spider in AllSpiders)
        {
            colliders.Add(spider.GetComponent<CircleCollider2D>());
        }
        List<Transform> neighbors = new List<Transform>();

        foreach (var collider in colliders)
        {
            if (collider.transform != transform)
            {
                neighbors.Add(collider.transform);
            }
        }

        return neighbors;
    }

    Vector2 ComputeAlignment(List<Transform> neighbors, Vector2 directionToTarget)
    {
        Vector2 alignment = Vector2.zero;

        foreach (var neighbor in neighbors)
        {
            alignment += direction + directionToTarget;
            Debug.Log(neighbor.up);
        }

        alignment /= neighbors.Count;

        return alignment;
    }

    Vector2 ComputeCohesion(List<Transform> neighbors)
    {
        Vector2 cohesion = Vector2.zero;

        foreach (var neighbor in neighbors)
        {
            cohesion += (Vector2)neighbor.position;
        }

        cohesion /= neighbors.Count;

        return cohesion - (Vector2)transform.position;
    }

    Vector2 ComputeAvoidance(List<Transform> neighbors)
    {
        Vector2 avoidance = Vector2.zero;

        foreach (var neighbor in neighbors)
        {
            if (Vector2.Distance(transform.position, neighbor.position) < avoidanceRadius)
            {
                avoidance += (Vector2)transform.position - (Vector2)neighbor.position;
            }
        }

        return avoidance;
    }

    /*private void MoveSpider()
    {
        
    }*/
}
