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
    [SerializeField] public Vector2 force = new Vector2(0, 0);

    private float moveInput;
    [SerializeField] private float speed = 150;
    [SerializeField] private float groundDecceleration = 600;
    public bool isGrounded = false;
    [SerializeField] private float terminalVelocity = -400f;

    [SerializeField] public float maxSpeedX = 5f;

    // Basically determines the gravity of the player
    public float gravityStrength = -2f;

    public Vector2 rbVel = new Vector2(0, 0);

    private Vector2 groundVelocity = new Vector2(0, 0);

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
        rbVel = rb2d.velocity;
        isGrounded = CheckGrounded(0);
        if (CheckGrounded(1))
        {
            force.y = 0;
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        }
        if (CheckWalled())
        {
            force.x = 0;
            rb2d.velocity = new Vector2(0, rb2d.velocity.y); ;
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

        rb2d.gravityScale = 0f;

        rb2d.AddForce(force);
        // Move with floor
        rb2d.velocity = new Vector2(rb2d.velocity.x + groundVelocity.x, rb2d.velocity.y + groundVelocity.y);

        hardHat.transform.position = transform.position + new Vector3(0, 1.4f, 0);
    }

    private int flipped = 1;

    private void CheckInput()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        if (moveInput != 0)
        {
            force.x = speed * moveInput;
            if (Mathf.Abs(force.x) > maxSpeedX)
            {
                force.x = Mathf.Sign(force.x) * maxSpeedX;
            }
        }
        else
        {
            force.x = -Mathf.Sign(rb2d.velocity.x) * groundDecceleration;
            if (Mathf.Abs(rb2d.velocity.x) < 1)
            {
                force.x = 0;
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            }
        }

        // Turn Around, only if no arrow keys are pressed, and no object is controlled
        if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.Space))
        {
            if (moveInput != 0)
            {
                flipped = (int) moveInput;
                Flip((int) moveInput);
            }
        }

        if (!isGrounded)
        {
            if (rb2d.velocity.y < terminalVelocity)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, terminalVelocity);
                force.y = 0;
            }
            else
            {
                force.y = gravityStrength;
            }
        }
        else
        {
            force.y = 0;
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