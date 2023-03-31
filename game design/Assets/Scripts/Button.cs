using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public GameObject door;
    private Door doorScript;
    private SpriteRenderer buttonGlow;

    // Start is called before the first frame update
    void Start()
    {
        doorScript = door.GetComponent<Door>();
        buttonGlow = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnTriggerEnter2D()
    {
        buttonGlow.color = new Color(1, 0.5f, 0, 1);
        doorScript.updateTrigger(true);
    }
    
    public void OnTriggerExit2D()
        {
            buttonGlow.color = Color.blue;
            doorScript.updateTrigger(false);
        }

}
