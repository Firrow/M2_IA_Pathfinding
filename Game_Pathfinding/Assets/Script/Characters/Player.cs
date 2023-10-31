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

    //Déplacement du joueur //A RETRAVAILLER POUR NE PAS QU'IL Y EST DE BUGS AVEC LES MURS
    void Update()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector3 nextPosition = this.transform.position + new Vector3(moveHorizontal / 2f, moveVertical / 2f, 0); //marche partiellement

        if (mapManager.GetTile(nextPosition).name != "wall")
        {
            float adjustedSpeed = speed / mapManager.GetTileMovementSpeed(this.transform.position + transform.up * Time.deltaTime);
            transform.position += new Vector3(moveHorizontal, moveVertical, 0) * adjustedSpeed * Time.deltaTime;
        }

        /*if (!isMoving)
            StartCoroutine(MovePlayer(new Vector3(moveHorizontal, moveVertical, 0f)));*/
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            Destroy(this.gameObject);
        }
    }

    /*IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;
        float nextMove = 0f;
        startPos = this.transform.position;
        endPos = startPos + direction;

        while (nextMove < MoveTime)
        {
            this.transform.position = Vector3.Lerp(startPos, endPos, nextMove / MoveTime);
            nextMove += Time.deltaTime;
            yield return null;
        }

        this.transform.position = endPos; //pour éviter les décalages avec les cases
        isMoving = false;
    }*/
}
