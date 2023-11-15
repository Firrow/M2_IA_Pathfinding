using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoids : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // FONCTION CREATION RANDOM BOIDS

    // Calculer la distance avec un autre Boid

    // FONCTION D'ATTRACTION moveCloser(tableau boids) : 
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
