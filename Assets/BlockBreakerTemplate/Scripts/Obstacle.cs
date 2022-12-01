using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public GameManager manager;
    private int numberOfStrokes = 2;
    private List<Color> colors = new List<Color>();

    void Start()
    {
        manager.obstacles.Add(gameObject);
        colors.Add(Color.red);
        colors.Add(Color.yellow);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ball")
        {

            col.gameObject.GetComponent<Ball>().SetDirection(transform.position);

            if (numberOfStrokes != 0)
            {
                gameObject.GetComponent<Renderer>().material.color = colors[numberOfStrokes - 1];
                numberOfStrokes--;
            }
            else if (manager.bonusPrefabs.Count > 0)
            {
                int index = Random.Range(0, manager.bonusPrefabs.Count);

                GameObject bonus = Instantiate(manager.bonusPrefabs[index], gameObject.transform.position, Quaternion.identity) as GameObject;
                bonus.GetComponent<IBonus>().setGameManager(manager);

                manager.obstacles.Remove(gameObject);
                Destroy(gameObject);
            }
            else
            {
                manager.obstacles.Remove(gameObject);
                Destroy(gameObject);
            }
        }
    }
}
