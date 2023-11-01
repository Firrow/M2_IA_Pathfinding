using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int speed;

    private MapManager mapManager;

    public void Awake()
    {
        mapManager = FindAnyObjectByType<MapManager>();
    }

    //Déplacement du joueur
    void Update()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        float adjustedSpeed = speed / mapManager.GetTileMovementSpeed(this.transform.position + transform.up * Time.deltaTime);
        transform.position += new Vector3(moveHorizontal, moveVertical, 0) * adjustedSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            Destroy(this.gameObject);
        }
    }
}
