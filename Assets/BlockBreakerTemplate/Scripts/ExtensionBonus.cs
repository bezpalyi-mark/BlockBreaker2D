using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtensionBonus : MonoBehaviour, IBonus
{
    public GameManager gameManager;

    public float seconds;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Paddle")
        {
            gameObject.transform.localScale = new Vector3(0, 0, 0);

            col.gameObject.transform.localScale += new Vector3(200, 0, 0);

            StartCoroutine("ReturnScaleBack", col.gameObject);
        }

    }

    IEnumerator ReturnScaleBack(GameObject paddleObject)
    {
        yield return new WaitForSeconds(seconds);

        paddleObject.transform.localScale -= new Vector3(200, 0, 0);

        Destroy(gameObject);
    }

    public void setGameManager(GameManager gManager)
    {
        gameManager = gManager;
    }
}
