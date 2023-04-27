using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityGun : MonoBehaviour
{
    [SerializeField] private Vector2 aimDir = new Vector2(0, 0);
    public bool controlling = false;
    private GameObject barrel;
    private GameObject hand;
    private GameObject beamEndPoint;
    private GameObject hitObject;
    [SerializeField] private float gravityStrength = 5f;
    [SerializeField] private float gravityAcceleration = 1f;
    [SerializeField] private float maxBeamLength = 20f;

    [SerializeField] float beamDir;

    LineRenderer line;

    [SerializeField] private LayerMask gravityGunLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        barrel = transform.GetChild(0).gameObject;
        hand = transform.parent.gameObject;
        beamEndPoint = GameObject.Find("BeamEndPoint");
        

        line = barrel.GetComponent<LineRenderer>();
        line.positionCount = 2;
        
    }

    // Update is called once per frame
    void Update()
    {
        // Aim in 1 of 8 directions

        Aim();
        Detect();
    }

    void Aim()
    {

        // Get aim direction through arrow
        aimDir.x = Input.GetAxisRaw("HorizontalArrow");
        aimDir.y = Input.GetAxisRaw("VerticalArrow");


        GameObject player = GameObject.Find("Player");
        int playerFlipped = player.GetComponent<PlayerController>().GetFlipped();

        // Flip player if aiming in opposite direction
        beamDir = transform.parent.transform.right.x;

        //Debug.Log(transform.parent.transform.right);
        if (beamDir > 0)
        {
            beamDir = 1;
        }
        else if (beamDir < 0)
        {
            beamDir = -1;
        }

        // If not aiming, default to player direction, FOR SOME REASON IT ALSO TURNS THE GUN WHEN THE PLAYER TURNS IDK WHY
        if (aimDir == new Vector2(0, 0) && !controlling)
        {
            aimDir.x = playerFlipped;
            
        }

        // Turn player if aiming in opposite direction && not controlling anything
        if (aimDir.x != playerFlipped && aimDir.x != 0 && !controlling)
        {
            player.GetComponent<PlayerController>().Flip((int)aimDir.x);
        }

    }

    private void Detect()
    {
        if (!controlling)
        {
            // Raycast in one of 8 directions
            RaycastHit2D aimBeam = Physics2D.Raycast(hand.transform.position, aimDir, maxBeamLength, gravityGunLayerMask);

            // Rotate gravity gun to one of 8 directions
            transform.parent.transform.right = new Vector3(aimDir.x, aimDir.y, 0);

            // If it doens't hit anything, draw ray to aimDir, return out of method
            if (aimBeam.collider == null)
            {
                DrawGravityRay(hand.transform.position + new Vector3(aimDir.x, aimDir.y, 0) * maxBeamLength);
                return;
            }

            // Draw ray until first collider hit, add a little extra distance for non-flat surfaces
            DrawGravityRay(aimBeam.point + aimDir * 0.1f);

            if (aimBeam.collider.gameObject.tag == "Gravitizable")
            {
                // Control hit object if Shift is pressed
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    float extraDistance = 0.1f;

                    controlling = true;
                    hitObject = aimBeam.collider.gameObject;
                    // Move beamEndPoint to hit object, set it that as parent
                    beamEndPoint.transform.parent = aimBeam.collider.gameObject.transform;
                    beamEndPoint.transform.position = aimBeam.point;
                    beamEndPoint.transform.position += extraDistance * new Vector3(aimDir.x, aimDir.y, 0);
                    hitObject.GetComponent<ControllableObject>().isControlledByGun = true;
                    aimDir = new Vector2(0, 0);
                }
            }
        }

        else // if controlling
        {
            // Apply Gravity Force to object
            hitObject.GetComponent<ControllableObject>().ControlGravity(aimDir * gravityStrength, gravityAcceleration, gameObject);

            // Use line cast to beam End point
            RaycastHit2D aimBeam = Physics2D.Linecast(hand.transform.position, beamEndPoint.transform.position, gravityGunLayerMask);

            // Rotate gravity gun to face beamEndPoint
            hand.transform.right = beamEndPoint.transform.position - hand.transform.position;

            // If space is released, or the line cast hits a different object, or the object goes out of range,
            // remove the object from the controlled list and reparent to self.
            if (Input.GetKeyUp(KeyCode.LeftShift) || aimBeam.collider.gameObject != hitObject || Vector3.Distance(hand.transform.position, beamEndPoint.transform.position) > maxBeamLength)
            {
                beamEndPoint.transform.parent = transform.parent.transform;
                beamEndPoint.transform.position = transform.parent.transform.position;
                hitObject.GetComponent<ControllableObject>().ControlGravity(new Vector3(0, 0, 0), gravityAcceleration, gameObject);
                hitObject.GetComponent<ControllableObject>().isControlledByGun = false;
                controlling = false;
                hitObject = null;
                return;
            }

            // This code only occurs the gravity connection is still valid
            DrawGravityRay(beamEndPoint.transform.position);        
        }
    }

    private void DrawGravityRay(Vector2 endPos)
    {
        // If not contolling, draw ray in a direction. Otherwise draw ray to the beamEndPoint
        Vector2 startPos = hand.transform.position;
        line.SetPosition(0, startPos);
        line.SetPosition(1, endPos);
    }
}
