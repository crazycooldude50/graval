using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool[] triggers;
    [SerializeField]
    private int inputs;
    public GameObject door;
    // Start is called before the first frame update
    void Start()
    {
        triggers = new bool[inputs];
        door.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }
    /*void openDoor()
    { 
        for (int i = 0; i < inputs; i++) 
        {
            if (triggers[i] == false) 
            { 
                door.gameObject.SetActive(false);
                return;
            }
        }
        door.gameObject.SetActive(true);
    }
    void updateTrigger(bool state) {
    for (int i = 0; i < inputs; i++) { 
        if (triggers[i] != state) { 
            triggers[i] = state;
            openDoor();
            return;
        }
    }
    }
    */
}
