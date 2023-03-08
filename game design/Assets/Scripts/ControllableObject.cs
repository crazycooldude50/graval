using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllableObject : MonoBehaviour
{

    [SerializeField] private Vector2 velocity = new Vector2(0, 0);
    [SerializeField] private Vector2 deccelerationRate = new Vector2(5, 5);

    public Vector2 controlledGravityAcceleration = new Vector2(0, 0);
    public Vector3 maxControlledGravity = new Vector3(0, 0, 0);

    public bool isControlledByGun = false;
    // Create dictionary or controllers: {gameObject, Vector4(Gravity.x, y, z, acceleration)}
    public Dictionary<GameObject, Vector4> controllers = new Dictionary<GameObject, Vector4>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        velocity = GetComponent<Rigidbody2D>().velocity;

        // Disable physics gravity if controlled by player 
        isControlledByGun = GameObject.Find("Gravity Gun").GetComponent<GravityGun>().controlling; // Change this condition, this activates if the player controls anything
        if (isControlledByGun)
        {
            GetComponent<Rigidbody2D>().gravityScale = 0f; 
        }
        else 
        {
            GetComponent<Rigidbody2D>().gravityScale = 1f; 
        }

        if (maxControlledGravity.x != 0)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, maxControlledGravity.x, Mathf.Sign(maxControlledGravity.x) * controlledGravityAcceleration.x);
        }
        else if (velocity.x != 0 && isControlledByGun)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0, deccelerationRate.x);
        }
        if (maxControlledGravity.y != 0)
        {
            velocity.y = Mathf.MoveTowards(velocity.y, maxControlledGravity.y, Mathf.Sign(maxControlledGravity.y) * controlledGravityAcceleration.y);
        }
        else if (velocity.y != 0 && isControlledByGun)
        {
           velocity.y = Mathf.MoveTowards(velocity.y, 0, deccelerationRate.y);
        }
        
        
        GetComponent<Rigidbody2D>().velocity = velocity;
    }

    public void ControlGravity(Vector3 maxForce, float baseAcceleration, GameObject changer)
    {
        // edit dictionary based off new force
        controllers[changer] = threeToFour(maxForce);
        controllers[changer] = new Vector4(controllers[changer].x, controllers[changer].y, controllers[changer].z, baseAcceleration);

        

        

        controlledGravityAcceleration = new Vector2(0, 0);
        maxControlledGravity = new Vector3(0, 0, 0);

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
