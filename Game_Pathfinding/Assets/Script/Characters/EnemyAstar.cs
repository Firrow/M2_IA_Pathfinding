using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyAstar : MonoBehaviour
{
    //map
    private MapManager mapManager;
    [SerializeField] private Tilemap map;
    public Grid grid;

    //déplacement Ennemis
    [SerializeField] private float moveTime;
    private float moveCounter;
    [SerializeField] private float baseSpeed;

    //AStar
    List<TileBase> openList = new List<TileBase>();
    List<TileBase> closeList = new List<TileBase>();

    //Autre éléments
    private GameObject player;


    private void Awake()
    {
        mapManager = FindAnyObjectByType<MapManager>();
        moveCounter = moveTime;
    }


    // Algo A*
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    void Update()
    {

    }

    private void Move()
    {
        //Rotation (aléatoire)
        moveCounter -= Time.deltaTime;
        if (moveCounter <= 0)
        {
            moveCounter = moveTime;
            //transform.rotation = Quaternion.Euler(0f, 0f, Random.RandomRange(0f, 360f));
        }

        //Déplacement
        float adjustedSpeed = baseSpeed / mapManager.GetTileMovementSpeed(transform.position + transform.up * Time.deltaTime); //on regarde la case plus loin pour ne pas être bloquée sur la case mur
        transform.position += transform.up * Time.deltaTime * adjustedSpeed;
    }

}

