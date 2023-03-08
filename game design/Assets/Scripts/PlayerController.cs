using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    // One Cell is 16px, Or 0.16 units of distance
    // 6.25 scale = 1 unit of distance


    // Start is called before the first frame update
    [SerializeField] public Vector2 velocity = new Vector2(0, 0);

    private float moveInput;
    [SerializeField] private float speed = 150;
    [SerializeField] private float walkAcceleration = 1;
    [SerializeField] private float groundDecceleration = 600;
    [SerializeField] public bool isGrounded = false;
    [SerializeField] public float terminalVelocity = -400f;
    [SerializeField] private float gravityStrength = -0.02f;
    [SerializeField] private bool cantGoUp = false;

    private Vector2 groundVelocity = new Vector2(0, 0);
    [SerializeField] private Vector3 controlledGravity = new Vector3(0, 0, 0);
    public float controlledGravityAcceleration = 0f;
    public Vector3 maxControlledGravity = new Vector3(0, 0, 0);

    private Rigidbody2D rb2d;
    private BoxCollider2D bc2d;

    private GameObject hardHat;


    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<BoxCollider2D>();
        hardHat = GameObject.Find("Hard Hat");
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = CheckGrounded(0);
        if (CheckGrounded(1))
        {
            velocity.y = 0;
        }
        if (CheckWalled())
        {
            velocity.x = 0;
        }
        CheckInput();
        Move();
    }

    private bool CheckGrounded(int type)
    {
        float extraHeight = 0.1f;

        Vector2 currentPos;
        RaycastHit2D raycastHitTest = Physics2D.Raycast(new Vector2(0, 0), Vector2.down, 0);
        for (float i = bc2d.bounds.center.x - bc2d.bounds.extents.x; i <= bc2d.bounds.center.x + bc2d.bounds.extents.x; i += bc2d.bounds.extents.x * 2)
        {
            currentPos = new Vector2(i, bc2d.bounds.center.y);

            if (type == 0)
            {
                currentPos.y -= extraHeight;
                raycastHitTest = Physics2D.Raycast(currentPos, Vector2.down, bc2d.bounds.extents.y);
            }

            else
            {
                currentPos.y += extraHeight;
                raycastHitTest = Physics2D.Raycast(currentPos, Vector2.up, bc2d.bounds.extents.y);
            }

            if (raycastHitTest.collider != null)
            {
                if (type == 0 && raycastHitTest.collider.tag != "Win")
                {
                    groundVelocity = raycastHitTest.collider.gameObject.GetComponent<Rigidbody2D>().velocity;
                }
                return true;
            }
        }
        return false;
    }

    private bool CheckWalled()
    {
        float extraWidth = 0.1f;

        Vector2 currentPos;
        RaycastHit2D raycastHitTest = Physics2D.Raycast(new Vector2(0, 0), Vector2.down, 0);
        for (float i = bc2d.bounds.center.y - bc2d.bounds.extents.y; i <= bc2d.bounds.center.y + bc2d.bounds.extents.y; i += bc2d.bounds.extents.y)
        {
            currentPos = new Vector2(bc2d.bounds.center.x, i);
            currentPos.x -= extraWidth;

            raycastHitTest = Physics2D.Raycast(currentPos, Vector2.left, bc2d.bounds.extents.x);

            if (raycastHitTest.collider != null)
            {
                return true;
            }

            currentPos.x += 2 * extraWidth;

            raycastHitTest = Physics2D.Raycast(currentPos, Vector2.right, bc2d.bounds.extents.x);

            if (raycastHitTest.collider != null)
            {
                return true;
            }

        }
        return false;
    }

    private void Move()
    {
        cantGoUp = false;
        // Falling
        if (!isGrounded)
        {
            if (velocity.y > terminalVelocity)
            {
                velocity.y += gravityStrength;
            }
            else
            {
                velocity.y = terminalVelocity;
            }
        }
        else if (velocity.y < 0 && maxControlledGravity.y == 0)
        {
            velocity.y = 0;
        }
        if (isGrounded) {
            if (-terminalVelocity > maxControlledGravity.y) {
                cantGoUp = true;
                controlledGravity.y = 0;
            }
            if (-terminalVelocity >= maxControlledGravity.y && maxControlledGravity.y > 0) 
            {
                velocity.y = -controlledGravity.y;
                Debug.Log("Nope");
            }
        }

        // Moving with ground
        rb2d.velocity = velocity + groundVelocity;
        // Move according to controlled gravity
        rb2d.velocity += new Vector2(controlledGravity.x, 0);
        if (!cantGoUp) {
            rb2d.velocity += new Vector2(0, controlledGravity.y);
        }
        hardHat.transform.position = transform.position + new Vector3(0, 1.4f, 0);
    }

    private int flipped = 1;

    private void CheckInput()
    {
        // Make controlled gravity approach the controller's force
        if (maxControlledGravity.x != 0) 
        {
            controlledGravity.x = Mathf.MoveTowards(controlledGravity.x, maxControlledGravity.x, controlledGravityAcceleration * Time.deltaTime);
        }
        else 
        {
            controlledGravity.x = Mathf.MoveTowards(controlledGravity.x, 0, groundDecceleration * Time.deltaTime);
        }
        if (maxControlledGravity.y != 0 && maxControlledGravity.y > terminalVelocity) 
        {
            controlledGravity.y = Mathf.MoveTowards(controlledGravity.y, maxControlledGravity.y, controlledGravityAcceleration * Time.deltaTime);
        }
        else if (controlledGravity.y > gravityStrength && controlledGravity.y < -gravityStrength) {
            controlledGravity.y = 0;
        }
        else if (controlledGravity.y > 0)
        {
            controlledGravity.y += gravityStrength;
        }
        else if (controlledGravity.y < 0)
        {
            controlledGravity.y -= gravityStrength;
        }
        


        moveInput = Input.GetAxisRaw("Horizontal");
        if (moveInput != 0)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, speed * moveInput, walkAcceleration);
        }
        else
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0, groundDecceleration * Time.deltaTime);
        }
        // Turn Around, only if no arrow keys are pressed, and no object is controlled
        if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.Space))
        {
            if (moveInput == 1 && moveInput != -1)
            {
                flipped = 1;
                Flip(1);
            }
            else if (moveInput == -1 && moveInput != 1)
            {
                flipped = -1;
                Flip(-1);
            }
        }
    }

    public void Flip(int dir)
    {
        if (dir > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (dir < 0)
        {
            transform.localRotation = Quaternion.Euler(0, -180, 0);
        }
        flipped = dir;
    }

    public int GetFlipped()
    {
        return flipped;
    }
}