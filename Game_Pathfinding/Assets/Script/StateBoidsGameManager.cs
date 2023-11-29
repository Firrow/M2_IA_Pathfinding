using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBoidsGameManager : MonoBehaviour
{
    [Range(1, 35)]
    public int spiderQuantity;
    [Range(1, 3)]
    public int speed;
    [Range(1, 5)]
    public int spiderAvoidanceRadius;
    public GameObject enemy;
    public List<GameObject> boids = new List<GameObject>();

    private string currentState;


    void Start()
    {
        currentState = "attack";
        InstantiateBoids();
        GetAllSpiders();
        GetState();
    }


    void Update()
    {
        GetState();
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

    private void GetState()
    {
        foreach (var boid in boids)
        {
            boid.GetComponent<StateEnemy>().NewState = currentState;
        }
    }

    public string CurrentState
    {
        get { return currentState; }
        set { currentState = value; }
    }


}
