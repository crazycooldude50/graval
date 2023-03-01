using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool[] triggers;
    [SerializeField] private int inputs;
    public GameObject door;
    // Start is called before the first frame update
    void Start()
    {
        triggers = new bool[inputs];
        for(int i = 0; i < inputs; i++)
        {
            triggers[i] = false;
            Debug.Log(triggers[i]);
        }
        Debug.Log(triggers.Length);
       
    }

    // Update is called once per frame
    void Update()
    {

    }
    void openDoor()
    { 
        for (int i = inputs - 1; i >= 0; i--) 
        {
            if (triggers[i] == false) 
            {
                door.gameObject.SetActive(true);
                return;
            }
        }
        door.gameObject.SetActive(false);
    }
    public void updateTrigger(bool state) {
    for (int i = 0; i < inputs; i++) {
        if (triggers[i] != state) { 
            triggers[i] = state;
            openDoor();
            return;
        }
    }
    }
    
}
