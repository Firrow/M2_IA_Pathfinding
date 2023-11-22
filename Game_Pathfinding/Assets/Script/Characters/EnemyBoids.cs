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
    public float neighborRadius = 5.0f;
    public float avoidanceRadius = 20.0f;

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

        Vector2 directionTarget = target.transform.position;
        //directionTarget.Normalize();

        // Calcule la direction moyenne des voisins
        Vector2 alignment = MoveWith(neighbors);
        // Calcule la position moyenne des voisins
        Vector2 cohesion = MoveCloser(neighbors);
        // Évite les collisions avec les voisins
        Vector2 avoidance = MoveAway(neighbors);

        // Applique les règles pour mettre à jour la direction
        Vector2 boidsDirection = alignment + cohesion + avoidance;



        // Déplace le boid
        this.transform.position = Vector2.MoveTowards(this.transform.position, directionTarget + boidsDirection, speed * Time.deltaTime);
    }

    List<Transform> GetNeighbors()
    {
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

    Vector2 MoveWith(List<Transform> neighbors)
    {
        Vector2 alignment = Vector2.zero;

        foreach (var neighbor in neighbors)
        {
            alignment += (Vector2)neighbor.position;
        }

        alignment /= neighbors.Count;

        return alignment / 100;
    }

    Vector2 MoveCloser(List<Transform> neighbors)
    {
        Vector2 cohesion = Vector2.zero;

        foreach (var neighbor in neighbors)
        {
            cohesion += (Vector2)neighbor.position;
        }

        cohesion /= neighbors.Count;

        return (cohesion - (Vector2)this.transform.position) / 2;
    }

    Vector2 MoveAway(List<Transform> neighbors)
    {
        Vector2 avoidance = Vector2.zero;

        foreach (var neighbor in neighbors)
        {
            if (Vector2.Distance(transform.position, neighbor.position) < avoidanceRadius)
            {
                avoidance -= (Vector2)neighbor.position - (Vector2)this.transform.position;
            }
        }


        /*float distanceX = 0;
        float distanceY = 0;

        foreach (var neighbor in neighbors)
        {
            float distance = Vector2.Distance(transform.position, neighbor.position);
            
            if (distance < avoidanceRadius)
            {
                float xDiff = this.transform.position.x - neighbor.position.x;
                float yDiff = this.transform.position.y - neighbor.position.y;

                if (xDiff >= 0)
                    distanceX = Mathf.Sqrt(avoidanceRadius) - xDiff;
                else if (xDiff < 0)
                    distanceX = -Mathf.Sqrt(avoidanceRadius) - xDiff;

                if (yDiff >= 0)
                    distanceY = Mathf.Sqrt(avoidanceRadius) - yDiff;
                else if (yDiff < 0)
                    distanceY = -Mathf.Sqrt(avoidanceRadius) - yDiff;

                distanceX += xDiff;
                distanceY += yDiff;
            }
        }
        
        avoidance = new Vector2(distanceX/5, distanceY/5);*/
        return avoidance;
    }

    /*private void MoveSpider()
    {
        
    }*/
}
