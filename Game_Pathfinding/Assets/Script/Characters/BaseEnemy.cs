using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BaseEnemy : MonoBehaviour
{
    //map
    private MapManager mapManager;

    //d�placement Ennemis
    [SerializeField] private float moveTime;
    private float moveCounter;
    [SerializeField] private float baseSpeed;


    private void Awake()
    {
        mapManager = FindAnyObjectByType<MapManager>();
        moveCounter = moveTime;
    }


    // Algo A*
    void Start()
    {

    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        //Rotation (al�atoire)
        moveCounter -= Time.deltaTime;
        if (moveCounter <= 0)
        {
            moveCounter = moveTime;
            transform.rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.RandomRange(0f, 360f));
        }

        //D�placement
        float adjustedSpeed = baseSpeed / mapManager.GetTileMovementSpeed(transform.position + transform.up * Time.deltaTime); //on regarde la case plus loin pour ne pas �tre bloqu�e sur la case mur
        transform.position += transform.up * Time.deltaTime * adjustedSpeed;
    }
}
