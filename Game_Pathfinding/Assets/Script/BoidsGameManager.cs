using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsGameManager : MonoBehaviour
{
    [Range(1.0f, 25.0f)]
    public int spiderQuantity;
    public GameObject enemy;
    public List<GameObject> boids = new List<GameObject>();


    void Start()
    {
        InstantiateBoids();
        GetAllSpiders();
    }


    void Update()
    {
        Debug.Log(boids.Count);
    }

    //FONCTION CREATION BOIDS RANDOM
    private void InstantiateBoids()
    {
        for (int i = 0; i < spiderQuantity; i++)
        {
            Instantiate(enemy, this.transform.position, Quaternion.Euler(0, 0, 0));
        }
    }

    private void GetAllSpiders()
    {
        foreach (var spider in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            boids.Add(spider);
        }
    }

}
