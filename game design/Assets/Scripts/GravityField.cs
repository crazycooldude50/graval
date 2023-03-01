using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityField : MonoBehaviour
{

    private GravityController gravityController;

    // Start is called before the first frame update
    void Start()
    {
        gravityController = GetComponent<GravityController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Gravitizable") {
            gravityController.controlled.Add(collider.gameObject);
            collider.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f; 
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.tag == "Gravitizable") {
            gravityController.controlled.Remove(collider.gameObject);
            collider.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1f;
            if (collider.gameObject.name == "Player")
            {
                collider.gameObject.GetComponent<PlayerController>().maxControlledGravity = new Vector3(0, 0, 0);
            }
        }
    }
}
