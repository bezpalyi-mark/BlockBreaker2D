using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public GameManager manager;		//The GameManager

    public bool rotating;

    public Rigidbody2D rig;	

	void Start () 
	{
		manager.walls.Add(gameObject);
	}

    void Update ()
    {
        if (rotating) 
        {
            transform.Rotate(new Vector3(0, 0, (float) 0.1));
        }
    }

    void OnTriggerEnter2D (Collider2D col)
	{
		if(col.gameObject.tag == "Ball"){											//Is the colliding object got the tag "Ball"?
			col.gameObject.GetComponent<Ball>().SetDirection(transform.position);	//Get the 'Ball' component of the colliding object and call the 'SetDirection()' function to bounce the ball of the paddle
		}
	}
}
