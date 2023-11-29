using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public int speed;

    private MapManager mapManager;
    private StateBoidsGameManager gameManager;

    public void Awake()
    {
        mapManager = FindAnyObjectByType<MapManager>();
    }

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StateBoidsGameManager>();
    }

    //Déplacement du joueur
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        float adjustedSpeed = speed / mapManager.GetTileMovementSpeed(this.transform.position + transform.up * Time.deltaTime);
        transform.position += new Vector3(moveHorizontal, moveVertical, 0) * adjustedSpeed * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            /*Destroy(this.gameObject);
            Debug.Log(" Ennemi vainqueur : " + collision.transform.name);*/
        }
        else if (collision.transform.tag == "Item")
        {
            Debug.Log("collision");
            Item item = collision.gameObject.GetComponent<Item>();

            gameManager.CurrentState = item.type;
        }
    }
}
