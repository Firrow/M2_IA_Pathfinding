using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyBoids : MonoBehaviour
{
    private BoidsGameManager gameManager;
    private List<GameObject> AllSpiders = new List<GameObject>();
    private GameObject target;

    public float speed = 4.0f;
    public float rotationSpeed = 20.0f; // vitesse aléatoire
    public float avoidanceRadius = 20.0f;
    public Vector2 velocity;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<BoidsGameManager>();
        AllSpiders = gameManager.boids;
        target = GameObject.FindGameObjectWithTag("Player");
        velocity = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
    }

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
        this.transform.position = Vector2.MoveTowards(this.transform.position, ((Vector3)directionTarget - this.transform.position) + (Vector3)boidsDirection, speed * Time.deltaTime);
        //this.transform.position += (Vector3)boidsDirection + ((Vector3)directionTarget - this.transform.position);
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
    Vector2 MoveCloser(List<Transform> neighbors)
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
    Vector2 MoveAway(List<Transform> neighbors)
    {
        Vector2 avoidance = Vector2.zero;

        foreach (var neighbor in neighbors)
        {
            if (Vector2.Distance(this.transform.position, neighbor.position) < avoidanceRadius)
            {
                avoidance -= ((Vector2)neighbor.position - (Vector2)this.transform.position);
            }
        }

        return avoidance / 100;
    }

    //Rule3
    Vector2 MoveWith(List<Transform> neighbors)
    {
        Vector2 alignment = Vector2.zero;

        foreach (var neighbor in neighbors)
        {
            //alignment += neighbor.GetComponent<Rigidbody2D>().velocity;
            alignment += neighbor.GetComponent<EnemyBoids>().velocity;
        }

        alignment /= neighbors.Count;

        //return (alignment - this.GetComponent<Rigidbody2D>().velocity) / 8;
        return (alignment - this.velocity) / 100;
    }

    /*private void MoveSpider()
    {
        
    }*/








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
}
