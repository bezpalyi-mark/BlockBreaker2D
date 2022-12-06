using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public GameManager manager;
    public bool spawnDelay;
    private int numberOfStrokes = 2;
    private List<Color> colors = new List<Color>();

    void Awake()
    {
        if (spawnDelay)
        {
            MeshRenderer renderer = GetComponent<MeshRenderer>();
            renderer.enabled = false;

            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            collider.enabled = false;

            int random = GetRandomValue(
                new RandomSelection(0, 30, .05f),
                new RandomSelection(30, 60, .45f),
                new RandomSelection(60, 90, .30f),
                new RandomSelection(90, 120, .15f),
                new RandomSelection(120, 150, .05f)
            );

            // this will wait 2 seconds before turning on
            StartCoroutine(WaitToDisplay((float)random));
        }
    }

    IEnumerator WaitToDisplay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.enabled = true;

        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.enabled = true;
    }

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


    int GetRandomValue(params RandomSelection[] selections)
    {
        float rand = Random.value;
        float currentProb = 0;
        foreach (var selection in selections)
        {
            currentProb += selection.probability;
            if (rand <= currentProb)
                return selection.GetValue();
        }

        //will happen if the input's probabilities sums to less than 1
        //throw error here if that's appropriate
        return -1;
    }

    private class RandomSelection
    {
        public float probability;

        public int min;

        public int max;


        public RandomSelection(int minP, int maxP, float probabilityP)
        {
            this.min = minP;
            this.max = maxP;
            this.probability = probabilityP;
        }

        public int GetValue()
        {
            return Random.Range(min, max);
        }
    }
}
