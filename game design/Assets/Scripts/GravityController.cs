using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    public List<GameObject> controlled = new List<GameObject>();
    public Vector3 gravity = new Vector3(0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject i in controlled)
        {
            i.GetComponent<Rigidbody2D>().velocity = gravity;
            //i.transform.position += gravity;
        }
    }
}
