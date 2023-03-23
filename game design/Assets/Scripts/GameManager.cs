using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int roomNumber;
    public GameObject player;
    public GameObject roomTrigger;
    private PlayerController playerScript;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        playerScript = player.GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        roomNumber = playerScript.roomNumber;
        if(Input.GetKeyDown("R"))
        {
            roomTrigger = GameObject.Find("Room Trigger " + roomNumber);
            player.position.transform()
        }
    }
    void ResetLevel()
    {
        
    }
}
