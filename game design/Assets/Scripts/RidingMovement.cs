using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RidingMovement : MonoBehaviour
{
    private BoxCollider2D bc2d;
    [SerializeField] private LayerMask groundDetectionMask;
    private GameObject lastGround = null;
    [SerializeField] private Vector3 lastParentVel = new Vector2(0, 0);

    [SerializeField] private Vector3 floorVelocity;

    public bool isGrounded = false;
    [SerializeField] private Vector2 force;
    [SerializeField] private float terminalVelocity;
    [SerializeField] private float gravityStrength;


    //[SerializeField] private GameObject ground;

    // Start is called before the first frame update
    void Start()
    {
        bc2d = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Box Cast to see what object this is on top of
        float downOffset = 0.01f;
        RaycastHit2D boxCast = Physics2D.BoxCast(new Vector2(transform.position.x, bc2d.bounds.center.y - bc2d.bounds.extents.y - 2 * downOffset), new Vector2(2 * bc2d.bounds.extents.x, downOffset), 0, Vector2.down, 0f, groundDetectionMask);
        if (boxCast.collider != null)
        {
            // If something is hit, set parent to that
            transform.parent = boxCast.collider.gameObject.transform;
            isGrounded = true;
            //ground = transform.parent.gameObject;
        }
        else
        {
            // If nothing is hit, set parent to ground
            transform.parent = GameObject.Find("LevelTileMap").transform;
            isGrounded = false;
        }

        // Check if parent is movable
        
        if (transform.parent.GetComponent<Rigidbody2D>() != null)
        {
            Rigidbody2D parentrb2d = transform.parent.GetComponent<Rigidbody2D>();
            if (parentrb2d.isKinematic == false && parentrb2d.bodyType != RigidbodyType2D.Static)
            {
                // push downwards because the riding hops for some reason
                //GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -1));
                floorVelocity = transform.parent.position;

                
                // If it a new floor object is hit
                if (lastGround != transform.parent.gameObject)
                {
                    lastParentVel = floorVelocity;
                    lastGround = transform.parent.gameObject;
                }
                else
                {
                    // Change velocity based on parent velocity changes
                    if (floorVelocity.x != lastParentVel.x)
                    {
                        transform.position += new Vector3(floorVelocity.x - lastParentVel.x, 0, 0);
                        
                    }
                }
                lastParentVel = floorVelocity;
                if (gameObject.name == "Player")
                {
                    Debug.Log("WHY");
                }
                
            }
        }
        else
        {
            floorVelocity = new Vector2(0, 0);
        }

        force = new Vector2(0, 0);
        if (!isGrounded)
        {
            if (GetComponent<Rigidbody2D>().velocity.y < terminalVelocity)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, terminalVelocity);
                force.y = 0;
            }
            else
            {
                force.y = gravityStrength;
            }
        }
        else
        {
            //force.y = -0.5f;
        }
        GetComponent<Rigidbody2D>().AddForce(force * GetComponent<Rigidbody2D>().mass);
    }

}
