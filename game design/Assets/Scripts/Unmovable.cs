using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unmovable : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Rigidbody2D playerRb2D;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        playerRb2D = GameObject.Find("Player").GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Vector3 D = collision.gameObject.transform.position - transform.position;
            Vector3 temp = D.normalized * Vector3.Dot(rb2d.velocity, D);
            rb2d.velocity -= new Vector2(temp.x, temp.y);
        }
    }
}
