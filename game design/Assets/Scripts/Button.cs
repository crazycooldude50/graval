using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public GameObject Door;
    Door doorScript;

    // Start is called before the first frame update
    void Start()
    {
        doorScript = Door.GetComponent<Door>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;
        doorScript.updateTrigger(true);
    }
   
}
