using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool[] triggers;
    [SerializeField] private int inputs;
    // Start is called before the first frame update
    void Start()
    {
        triggers = new bool[inputs];
        for(int i = 0; i < inputs; i++)
        {
            triggers[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    void openDoor()
    { 
        for (int i = 0; i < inputs; i++) 
        {
            if (triggers[i] == false) 
            {
                gameObject.SetActive(true);
                return;
            }
        }
        gameObject.SetActive(false);
    }
    public void updateTrigger(bool state) {
    for (int i = 0; i < inputs; i++) {
        if (triggers[i] != state) { 
            triggers[i] = state;
            Debug.Log(triggers[i] + ", " + i + ", " + triggers);
            openDoor();
            return;
        }
    }
    }
    
}
