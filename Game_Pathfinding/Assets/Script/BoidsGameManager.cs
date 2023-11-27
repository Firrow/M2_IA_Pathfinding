using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsGameManager : MonoBehaviour
{
    [Range(1, 35)]
    public int spiderQuantity;
    [Range(1, 5)]
    public int spiderAvoidanceRadius;

    public GameObject enemy;
    public List<GameObject> boids = new List<GameObject>();


    void Start()
    {
        InstantiateBoids();
        GetAllSpiders();
    }


    void Update()
    {

    }

    //FONCTION CREATION BOIDS RANDOM
    private void InstantiateBoids()
    {
        for (int i = 0; i < spiderQuantity; i++)
        {
            Instantiate(enemy, new Vector2 (this.transform.position.x + Random.Range(0, 5), this.transform.position.y + Random.Range(0, 5)), Quaternion.Euler(0, 0, 0));
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
