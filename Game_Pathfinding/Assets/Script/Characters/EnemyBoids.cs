using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyBoids : MonoBehaviour
{
    private BoidsGameManager gameManager;
    private List<GameObject> AllSpiders = new List<GameObject>();
    private Rigidbody rb;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<BoidsGameManager>();
        AllSpiders = gameManager.boids;
        rb = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        moveCloser(AllSpiders);
    }

    // Calculer la distance avec un autre Boid
    private float DistanceBetweenTwoSpiders(GameObject spider)
    {
        return Vector3.Distance(this.transform.position, spider.transform.position);
    }

    // FONCTION D'ATTRACTION moveCloser(tableau boids) : 
    private void moveCloser(List<GameObject> spiders)
    {
        float avg = 0;
        foreach (var spider in spiders)
        {
            if (spider != this.gameObject)
            {
                avg += Vector3.Distance(spider.transform.position, this.transform.position);
            }
        }

        avg /= spiders.Count;

        rb.velocity -= new Vector3((avg / 100), (avg / 100), 0); // DONT WORK
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
}
