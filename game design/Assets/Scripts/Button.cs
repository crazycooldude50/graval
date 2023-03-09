using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public GameObject Door;
    private Door doorScript;
    private SpriteRenderer buttonGlow;

    // Start is called before the first frame update
    void Start()
    {
        doorScript = GameObject.Find("Door").GetComponent<Door>();
        buttonGlow = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        buttonGlow.color = new Color(1, 0.5f, 0, 1);
        doorScript.updateTrigger(true);
    }
    
    public void OnTriggerExit2D(Collider2D other)
        {
            buttonGlow.color = Color.blue;
            doorScript.updateTrigger(false);
        }

}
