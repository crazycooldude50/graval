using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    private GameObject rooms;
    private PlayerController playerScript;
    private Dictionary<GameObject, Dictionary<GameObject, Vector3>> roomStates = new Dictionary<GameObject, Dictionary<GameObject, Vector3>>();


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        playerScript = player.GetComponent<PlayerController>();
        rooms = GameObject.Find("Rooms");
        //LoadScene("Level 0");
        SaveObjectPositions();
    }



    // Update is called once per frame
    void Update()
    {
        int roomNumber = playerScript.roomNumber;
        if (Input.GetKeyDown(KeyCode.R))
        {
            playerScript.force = new Vector2(0, 0);
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            GameObject room = rooms.transform.GetChild(roomNumber - 1).gameObject;
            for (int i = 0; i < room.transform.childCount; i++)
            {
                GameObject child = room.transform.GetChild(i).gameObject;




                if (child.name.Contains("Controllable Object"))
                {
                    child.transform.position = roomStates[room][child];
                }
                if(roomNumber == 1)
                {
                    player.transform.position = GameObject.Find("Level Start").transform.position;
                }
                else
                {
                    
                    player.transform.position = GameObject.Find("Room Trigger " + roomNumber).transform.position;
                }
                
            }

        }
    }
    void ResetLevel()
    {
        
    }
    void LoadScene(string sceneToLoad)
    {
        for (int i = 0; i< SceneManager.sceneCount; i++)     
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if(scene.name != "GameManager")
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
        SaveObjectPositions();
    }
    void SaveObjectPositions()
    {

        // Gets number of rooms
        for (int j = 0; j < rooms.transform.childCount; j++)
        {
            GameObject room = rooms.transform.GetChild(j).gameObject;
            for (int i = 0; i < room.transform.childCount; i++)
            {

                GameObject child = room.transform.GetChild(i).gameObject;
                if (child.name.Contains("Controllable Object"))
                {
                    if (!roomStates.ContainsKey(room))
                    {
                        roomStates.Add(room, new Dictionary<GameObject, Vector3>() { { child, child.transform.position } });
                    }
                    else
                    {

                        roomStates[room][child] = child.transform.position;
                    }
                }
            }
        }
    }
}
