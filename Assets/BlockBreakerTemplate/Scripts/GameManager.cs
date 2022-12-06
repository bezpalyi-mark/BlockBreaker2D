using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public int score;               //The player's current score
    public int lives;               //The amount of lives the player has remaining
    public int ballSpeedIncrement;  //The amount of speed the ball will increase by everytime it hits a brick
    public bool gameOver;           //Set true when the game is over
    public bool wonGame;            //Set true when the game has been won

    public GameObject paddle;       //The paddle game object
    public GameObject ballPrefab;
    public List<GameObject> balls = new List<GameObject>();

    public GameUI gameUI;           //The GameUI class

    //Prefabs
    public GameObject brickPrefab;  //The prefab of the Brick game object which will be spawned

    public List<GameObject> bricks = new List<GameObject>();    //List of all the bricks currently on the screen

    public GameObject wallPrefab;

    public List<GameObject> walls = new List<GameObject>();

    public Color[] colors;          //The color array of the bricks. This can be modified to create different brick color patterns

    public List<GameObject> bonusPrefabs = new List<GameObject>();

    public GameObject obstaclePrefab;

    public List<GameObject> obstacles = new List<GameObject>();

    public int leftSpawnBorder = -8;

    public int rightSpawnBorder = 8;

    public int topSpawnBorder = 4;

    public int bottomSpawnBorder = 1;

    public bool generateEntitiesRandomly;
    public int wallsCountToGenerate = 5;

    void Start()
    {
        StartGame(); //Starts the game by setting values and spawning bricks
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Time.timeScale = 2;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    //Called when the game starts
    public void StartGame()
    {
        score = 0;
        lives = 3;
        gameOver = false;
        wonGame = false;
        paddle.active = true;
        balls.ForEach(delegate (GameObject w) { w.active = true; });
        paddle.GetComponent<Paddle>().ResetPaddle();
        CreateBrickArray();
    }

    //Spawns the bricks and sets their colours
    public void CreateBrickArray()
    {
        /*         int colorId = 0;                    //'colorId' is used to tell which color is currently being used on the bricks. Increased by 1 every row of bricks

                for (int y = 0; y < brickCountY; y++)
                {
                    for (int x = -(brickCountX / 2); x < (brickCountX / 2); x++)
                    {
                        Vector3 pos = new Vector3(0.8f + (x * 1.6f), 1 + (y * 0.4f), 0);                        //The 'pos' variable is where the brick will spawn at
                        GameObject brick = Instantiate(brickPrefab, pos, Quaternion.identity) as GameObject;    //Creates a new brick game object at the 'pos' value
                        brick.GetComponent<Brick>().manager = this;                                             //Gets the 'Brick' component of the game object and sets its 'manager' variable to this the GameManager
                        brick.GetComponent<SpriteRenderer>().color = colors[colorId];                           //Gets the 'SpriteRenderer' component of the brick object and sets the color
                                                                                                                //bricks.Add(brick);																		//Adds the new brick object to the 'bricks' list
                    }

                    colorId++;                      //Increases the 'colorId' by 1 as a new row is about to be made

                    if (colorId == colors.Length)   //If the 'colorId' is equal to the 'colors' array length. This means there is no more colors left
                        colorId = 0;
                } */

        if (generateEntitiesRandomly)
        {
            bool canAddMoreEntities = true;
            int leftBorder = leftSpawnBorder;
            int topBorder = topSpawnBorder;
            int wallsCount = 0;
            int colorId = 0;

            while (canAddMoreEntities)
            {
                int randomChoice = Random.Range(1, 4);

                Vector3 sizesOfNewObject = new Vector3(0, 0, 0);

                switch (randomChoice)
                {
                    case 1:
                        Vector3 brickPos = new Vector3(leftBorder, topBorder, 0);
                        GameObject brick = Instantiate(brickPrefab, brickPos, Quaternion.identity) as GameObject;    //Creates a new brick game object at the 'pos' value
                        brick.GetComponent<Brick>().manager = this;
                        brick.GetComponent<SpriteRenderer>().color = colors[colorId];
                        sizesOfNewObject = new Vector3(brick.transform.localScale.x / 100, brick.transform.localScale.y / 100, 0);
                        break;
                    case 2:
                        Vector3 obstaclePos = new Vector3(leftBorder, topBorder, 0);
                        GameObject obstacle = Instantiate(obstaclePrefab, obstaclePos, Quaternion.identity) as GameObject;
                        obstacle.GetComponent<Obstacle>().manager = this;
                        sizesOfNewObject = new Vector3(obstacle.transform.localScale.x, obstacle.transform.localScale.y, 0);
                        break;
                    case 3:
                        if (wallsCount < wallsCountToGenerate)
                        {
                            Vector3 wallPos = new Vector3(leftBorder, topBorder, 0);
                            GameObject wall = Instantiate(wallPrefab, wallPos, Quaternion.identity) as GameObject;
                            wall.GetComponent<Wall>().manager = this;
                            wall.GetComponent<Wall>().rotating = Random.Range(0, 2) == 1;
                            sizesOfNewObject = new Vector3(wall.transform.localScale.x / 100, wall.transform.localScale.y / 100, 0);
                            wallsCount++;
                        }
                        break;
                }

                leftBorder += (int)sizesOfNewObject.x + 1;

                if (leftBorder >= rightSpawnBorder)
                {
                    leftBorder = leftSpawnBorder;
                    topBorder -= (int)wallPrefab.transform.localScale.y / 100; // because of sprite render instead of mesh render
                    colorId++;                      //Increases the 'colorId' by 1 as a new row is about to be made

                    if (colorId == colors.Length)   //If the 'colorId' is equal to the 'colors' array length. This means there is no more colors left
                        colorId = 0;
                }

                if (topBorder <= bottomSpawnBorder)
                {
                    canAddMoreEntities = false;
                }
            }
        }

        GameObject newBall = Instantiate(ballPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        newBall.GetComponent<Ball>().manager = this;
        newBall.GetComponent<Ball>().ResetBall();
        balls.Add(newBall);
    }

    //Called when there is no bricks left and the player has won
    public void WinGame()
    {
        wonGame = true;
        paddle.active = false;          //Disables the paddle so it's invisible
        balls.ForEach(delegate (GameObject w) { w.active = false; });
        walls.ForEach(delegate (GameObject w) { w.active = false; });
        obstacles.ForEach(delegate (GameObject w) { w.active = false; });
        gameUI.SetWin();                //Set the game over UI screen
    }

    public void DestroyBall()
    {
        if (balls.Count == 0)
        {
            LiveLost();
        }
    }

    //Called when the ball goes under the paddle and "dies"
    public void LiveLost()
    {
        lives--;                                        //Removes a life

        if (lives < 0)
        {                                   //Are the lives less than 0? Are there no lives left?
            gameOver = true;
            paddle.active = false;                      //Disables the paddle so it's invisible
            balls.ForEach(delegate (GameObject w) { w.active = false; });
            walls.ForEach(delegate (GameObject w) { w.active = false; });
            obstacles.ForEach(delegate (GameObject w) { w.active = false; });
            gameUI.SetGameOver();                       //Set the game over UI screen

            for (int x = 0; x < bricks.Count; x++)
            {       //Loops through the 'bricks' list
                Destroy(bricks[x]);                     //Destory the brick
            }

            bricks = new List<GameObject>();            //Resets the 'bricks' list variable
        }
        else
        {
            GameObject newBall = Instantiate(ballPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            newBall.GetComponent<Ball>().manager = this;
            newBall.GetComponent<Ball>().ResetBall();
            balls.Add(newBall);
        }
    }
}
