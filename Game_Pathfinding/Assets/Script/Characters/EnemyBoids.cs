using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyBoids : MonoBehaviour
{
    private BoidsGameManager gameManager;
    private List<GameObject> AllBoids = new List<GameObject>();

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<BoidsGameManager>();
        AllBoids = gameManager.Boids;
    }

    void Update()
    {
        
    }

    // Calculer la distance avec un autre Boid

    // FONCTION D'ATTRACTION moveCloser(tableau boids) : 
    // Calculer la distance moyenne avec les autres boids
    // Adapter la vitesse (-=) de chaque boids en fonction de cette distance

    // FONCTION D'ALIGNEMENT moveWith(tableau boids) :
    // Calculer la v�locit� moyenne du groupe de boids
    // Adapter la vitesse (+=) de chaque boids en fonction de cette vitesse

    // FONCTION DE REPULSION moveAway(tableau boids, distanceMinimale) :
    // SI LES BOIDS SONT DANS UNE ZONE DE REPULSION, ON DOIT CHANGER DE DIRECTION POUR S'ELOIGNER 
    //
    // Adapter la vitesse (+=) de chaque boids en fonction de cette vitesse

    // FONCTION DE MOUVEMENT move() : faire en sorte de ne pas aller sur les tuiles murs !

    // Update() :
    // Pour chaque boids :
        // Pour chaque boids :
            // Si le boid n'est pas le m�me :
                // On r�cup�re la distance et si elle est pas trop grande, on ajoute le boids au groupe ferm� (closeBoids)
        // Appel moveCloser()
        // Appel moveWith()
        // Appel moveAway()
        // Appel move()





    // Qu'est ce que closeBoids ? les boids qui sont dans une zone qu'on consid�re
    // Comment marche la fonction de r�pulsion ?
}
