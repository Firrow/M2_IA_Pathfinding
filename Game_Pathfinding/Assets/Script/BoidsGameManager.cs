using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsGameManager : MonoBehaviour
{
    [Range(1.0f, 25.0f)]
    public int SpiderQuantity;
    public GameObject Enemy;
    public List<GameObject> Boids = new List<GameObject>();


    void Start()
    {
        
    }


    void Update()
    {
        
    }

    //FONCTION CREATION BOIDS RANDOM
    private void InstantiateBoids()
    {
        for (int i = 0; i < SpiderQuantity; i++)
        {

        }
    }
}
