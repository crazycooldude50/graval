using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllableObject : MonoBehaviour
{

    [SerializeField] private float baseGravityForce = -2f;

    [SerializeField] private Vector2 force = new Vector2(0, 0);
    [SerializeField] private Vector2 deccelerationRate = new Vector2(5, 5);

    [SerializeField] private Vector2 maxSpeed = new Vector2(10f, -10f);

    public Vector2 controlledGravityAcceleration = new Vector2(0, 0);
    public Vector3 maxControlledGravity = new Vector3(0, 0, 0);

    public bool isControlledByGun = false;
    // Create dictionary or controllers: {gameObject, Vector4(Gravity.x, y, z, acceleration)}
    private Dictionary<GameObject, Vector4> controllers = new Dictionary<GameObject, Vector4>();
    private Rigidbody2D rb2d;

    [SerializeField] private bool isPlayer;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        isPlayer = gameObject.name == "Player";
    }

    // Update is called once per frame
    void Update()
    {
        float moveInput = maxControlledGravity.x;
        if (moveInput != 0)
        {
            force.x = controlledGravityAcceleration.x;
            if (Mathf.Abs(force.x) > maxSpeed.x)
            {
                force.x = Mathf.Sign(force.x) * maxSpeed.x;
            }
            
        }
        else if(!isPlayer)
        {
            force.x = -Mathf.Sign(rb2d.velocity.x) * deccelerationRate.x;
            if (Mathf.Abs(rb2d.velocity.x) < 1)
            {
                force.x = 0;
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            }
        }
        else
        {
            force.x = 0;
        }
        
        force.y = maxControlledGravity.y;
        
        /*
        if (isPlayer && force.y != 0)
        {
            if (gameObject.GetComponent<RidingMovement>().isGrounded)
            {
                force.y += gameObject.GetComponent<PlayerController>().gravityStrength + 0.5f;
            }
            else
            {
                //force.y += 2;
            }
        }
        */
        if (isControlledByGun)
        {
            if (maxControlledGravity.y == 0 && rb2d.velocity.y != 0)
            {
                force.y = -Mathf.Sign(rb2d.velocity.y) * deccelerationRate.y;
                if (Mathf.Abs(rb2d.velocity.x) < 1)
                {
                    force.y = 0;
                    rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
                }
            }
        }
        /*
        else if (!isPlayer)
        {
            // if not grounded
            if (!GetComponent<RidingMovement>().isGrounded)
            {
                force.y += baseGravityForce;
            }
        }
        */
        if (rb2d.velocity.y < maxSpeed.y && maxControlledGravity.y == 0 && !isControlledByGun && !isPlayer)
        {
            force.y = 0;
            rb2d.velocity = new Vector2(rb2d.velocity.x, maxSpeed.y);
        }

        // If being controlled, add 2 force to counteract gravity
        if (maxControlledGravity.y != 0 || isControlledByGun)
        {
            force.y += 2;
        }


        rb2d.AddForce(force * rb2d.mass);
    }

    public void ControlGravity(Vector3 maxForce, float baseAcceleration, GameObject changer)
    {
        // edit dictionary based off new force
        controllers[changer] = threeToFour(maxForce);
        controllers[changer] = new Vector4(controllers[changer].x, controllers[changer].y, controllers[changer].z, baseAcceleration);


        controlledGravityAcceleration = new Vector2(0, 0);
        maxControlledGravity = new Vector3(0, 0, 0);

        // Sum all of the gravity controllers and their acceleration vectors
        foreach (KeyValuePair<GameObject, Vector4> controller in controllers)
        {
            maxControlledGravity += FourToThree(controller.Value);
            controlledGravityAcceleration.x += baseAcceleration * Vector3.Dot(Vector3.right, FourToThree(controller.Value));
            controlledGravityAcceleration.y += baseAcceleration * Vector3.Dot(Vector3.up, FourToThree(controller.Value));
        }
    }

    private Vector4 threeToFour(Vector3 toChange)
    {
        return new Vector4(toChange.x, toChange.y, toChange.z, 0);
    }

    private Vector3 FourToThree(Vector4 toChange)
    {
        return new Vector3(toChange.x, toChange.y, toChange.z);
    }
}
