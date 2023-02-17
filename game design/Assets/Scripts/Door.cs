using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject door;
    // Start is called before the first frame update
    void Start()
    {
        door.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }
    /*void openDoor()
    {
        Door.SetActive(true);
    }
    */
}
