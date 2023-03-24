using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int roomNumber;
    public GameObject player;
    public GameObject rooms;
    private PlayerController playerScript;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerController>();
        rooms = GameObject.Find("Rooms");

        List<Dictionary<GameObject, Vector3>> roomStates = new List<Dictionary<GameObject, Vector3>>();
        for(int j = 0; j < rooms.transform.childCount; j++)
        {
            //roomStates.Add(new Dictionary<GameObject, Vector3>);
            for (int i = 0; i < rooms.transform.GetChild(j).transform.childCount; i++)
            {
                
                GameObject child = rooms.transform.GetChild(j).GetChild(i).gameObject;
                Debug.Log(roomStates);
                Debug.Log(child.name.Substring(0, 21));
                if (child.name.Substring(0, 19).Equals("Controllable Object") || child.name.Substring(0, 21).Equals("UnControllable Object"))
                {
                   // roomStates.Set(i[child] = child.transform.position);

                }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        roomNumber = playerScript.roomNumber;
        if(Input.GetKeyDown("R"))
        {
            //roomTrigger = GameObject.Find("Room Trigger " + roomNumber);
            //player.position.transform();
        }
    }
    void ResetLevel()
    {
        
    }
    void LoadScene()
    {

    }
}
