using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneBonus : MonoBehaviour, IBonus
{

    public GameManager gameManager;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Paddle")
        {
            gameObject.transform.localScale = new Vector3(0, 0, 0);

            List<GameObject> balls = new List<GameObject>(gameManager.balls);

            foreach (GameObject ball in balls)
            {
                GameObject newBall = Instantiate(gameManager.ballPrefab, ball.transform.position, Quaternion.identity) as GameObject;
                newBall.GetComponent<Ball>().manager = gameManager;
                newBall.GetComponent<Ball>().direction = ball.GetComponent<Ball>().direction;
                gameManager.balls.Add(newBall);
            }

        }

    }

    public void setGameManager(GameManager gManager)
    {
        gameManager = gManager;
    }
}
