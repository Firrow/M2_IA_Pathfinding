using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int speed;

    private MapManager mapManager;
    private Vector3 startPos, endPos;
    private bool isMoving = false;
    private float MoveTime = 0.2f; //à modifier en fonction du type de tuile

    public void Awake()
    {
        mapManager = FindAnyObjectByType<MapManager>();
    }

    //Déplacement du joueur
    void Update()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        /*Vector3 nextPosition = this.transform.position + new Vector3(moveHorizontal / 2f, moveVertical / 2f, 0); //marche partiellement

        if (mapManager.GetTile(nextPosition).name != "wall")
        {
            float adjustedSpeed = speed / mapManager.GetTileMovementSpeed(this.transform.position);
            transform.position += new Vector3(moveHorizontal, moveVertical, 0) * adjustedSpeed * Time.deltaTime;
        }*/


        Vector3 nextPosition = transform.position + new Vector3(moveHorizontal, moveVertical, 0);
        float tileSpeed = mapManager.GetTileMovementSpeed(nextPosition);

        if (tileSpeed < 1e+22)
        {
            // Vérifiez si la tuile n'est pas un "mur"
            float adjustedSpeed = speed / tileSpeed;
            transform.position += new Vector3(moveHorizontal, moveVertical, 0) * adjustedSpeed * Time.deltaTime;
        }



        /*if (!isMoving)
            StartCoroutine(MovePlayer(new Vector3(moveHorizontal, moveVertical, 0f)));*/
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

    /*private void basicMove()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        transform.position += new Vector3(moveHorizontal, moveVertical, 0) * speed * Time.deltaTime;

        float adjustedSpeed = speed / mapManager.GetTileMovementSpeed(transform.position + transform.up * Time.deltaTime); //on regarde plus loin
        transform.position += transform.up * Time.deltaTime * adjustedSpeed;
    }*/
}
