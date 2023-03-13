using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityField : MonoBehaviour
{
    [SerializeField] private Vector3 gravity = new Vector3(0, 0, 0);
    [SerializeField] private float gravityAcceleration = 1f;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Gravitizable") {
            collider.gameObject.GetComponent<ControllableObject>().ControlGravity(gravity, gravityAcceleration, gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.tag == "Gravitizable") {
            collider.gameObject.GetComponent<ControllableObject>().ControlGravity(new Vector3(0, 0, 0), gravityAcceleration, gameObject);
        }
    }
}
