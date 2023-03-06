using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    public List<GameObject> controlled = new List<GameObject>();
    public Vector3 gravity = new Vector3(0, 0, 0);
    public float acceleration = 5f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        foreach (GameObject i in controlled)
        {
            Vector3 velocity = i.GetComponent<Rigidbody2D>().velocity;
            if (i.name == "Player")
            {
                i.GetComponent<PlayerController>().maxControlledGravity = gravity;
                i.GetComponent<PlayerController>().controlledGravityAcceleration = acceleration;
            }
            else {
                velocity.x = Mathf.MoveTowards(velocity.x, gravity.x, acceleration);
                velocity.y = Mathf.MoveTowards(velocity.y, gravity.y, acceleration);
                i.GetComponent<Rigidbody2D>().velocity = velocity;
            }
        }
    }
}
