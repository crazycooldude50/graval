using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityGun : MonoBehaviour
{
    [SerializeField] private Vector2 aimDir = new Vector2(0, 0);
    public bool controlling = false;
    private GameObject barrel;
    private GameObject beamEndPoint;
    private GameObject hitObject;
    [SerializeField] private float gravityStrength = 5f;
    [SerializeField] private float gravityAcceleration = 1f;
    [SerializeField] private float maxBeamLength = 20f;

    // Start is called before the first frame update
    void Start()
    {
        barrel = transform.GetChild(0).gameObject;
        beamEndPoint = GameObject.Find("BeamEndPoint");
    }

    // Update is called once per frame
    void Update()
    {
        // Aim in 1 of 8 directions
        Aim(aimDir);
        Detect();
    }

    void Aim(Vector2 lastAimDir)
    {
        // Uses arrow keys to detect aim direction for gravity beam

        /*
        aimDir = new Vector2(Input.GetAxisRaw("HorizontalArrow"), Input.GetAxisRaw("VerticalArrow"));
        bool edited = false;

        if (aimDir != lastAimDir)
        {
            edited = true;
        }
        */
        
        aimDir = new Vector2(0, 0);
        bool edited = false;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            aimDir.x = -1;
            edited = true;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            aimDir.x += 1;
            edited = true;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            aimDir.y = -1;
            edited = true;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            aimDir.y += 1;
            edited = true;
        }

        // When a key is released, also set edited to true
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            edited = true;
        }
        
        GameObject player = GameObject.Find("Player");
        int playerFlipped = player.GetComponent<PlayerController>().GetFlipped();

        // Only correct aim when no input when not controlling an object
        if (!edited)
        {
            aimDir = lastAimDir;
            //return;
        }

        // Flip player if aiming in opposite direction
        float beamDir = transform.parent.transform.right.x;
        //Debug.Log(transform.parent.transform.right);
        if (beamDir > 0)
        {
            beamDir = 1;
        }
        else if (beamDir < 0)
        {
            beamDir = -1;
        }
        if (beamDir != playerFlipped && aimDir.x != 0 && edited)
        {
            player.GetComponent<PlayerController>().Flip((int)aimDir.x);
        }

        // Turn around beam when player turns around
        if (aimDir.x != playerFlipped && aimDir.y == 0 && !controlling)
        {
            aimDir.x = playerFlipped;
        }
    }

    private void Detect()
    {
        if (!controlling)
        {
            // Raycast in one of 8 directions
            RaycastHit2D aimBeam = Physics2D.Raycast(barrel.transform.position, aimDir, maxBeamLength);

            // Rotate gravity gun to one of 8 directions
            transform.parent.transform.right = new Vector3(aimDir.x, aimDir.y, 0);

            // If it doens't hit anything, draw ray to aimDir, return out of method
            if (aimBeam.collider == null)
            {
                DrawGravityRay(barrel.transform.position + new Vector3(aimDir.x, aimDir.y, 0) * maxBeamLength);
                return;
            }

            // Draw ray until first collider hit, add a little extra distance for non-flat surfaces
            DrawGravityRay(aimBeam.point + aimDir * 0.5f);

            if (aimBeam.collider.gameObject.tag == "Gravitizable")
            {
                // Control hit object if Space is pressed
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    float extraDistance = 0.1f;

                    controlling = true;
                    hitObject = aimBeam.collider.gameObject;
                    // Move beamEndPoint to hit object, set it that as parent
                    beamEndPoint.transform.parent = aimBeam.collider.gameObject.transform;
                    beamEndPoint.transform.position = aimBeam.point;
                    beamEndPoint.transform.position += extraDistance * new Vector3(aimDir.x, aimDir.y, 0);
                    aimDir = new Vector2(0, 0);
                }
            }
        }

        else // if controlling
        {
            // Apply Gravity Force to object
            hitObject.GetComponent<ControllableObject>().ControlGravity(aimDir * gravityStrength, gravityAcceleration, gameObject);

            // Use line cast to beam End point
            RaycastHit2D aimBeam = Physics2D.Linecast(barrel.transform.position, beamEndPoint.transform.position);

            // Rotate gravity gun to face beamEndPoint
            transform.parent.transform.right = beamEndPoint.transform.position - transform.position;

            // If space is released, or the line cast hits a different object, or the object goes out of range,
            // remove the object from the controlled list and reparent to self.
            if (Input.GetKeyUp(KeyCode.Space) || aimBeam.collider.gameObject != hitObject || Vector3.Distance(barrel.transform.position, beamEndPoint.transform.position) > maxBeamLength)
            {
                beamEndPoint.transform.parent = transform.parent.transform;
                beamEndPoint.transform.position = transform.parent.transform.position;
                hitObject.GetComponent<ControllableObject>().ControlGravity(new Vector3(0, 0, 0), gravityAcceleration, gameObject);
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
        Vector2 startPos = barrel.transform.position;

        LineRenderer line = barrel.GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.SetPosition(0, startPos);
        line.SetPosition(1, endPos);
    }
}
