using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject player;
    private GameObject triggers;
    private PlayerController playerScript;
    [SerializeField] private float xBoundLeft;
    [SerializeField] private float xBoundRight;
    [SerializeField] private float width;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        triggers = GameObject.Find("Triggers");
        playerScript = player.GetComponent<PlayerController>();
        Camera cam = Camera.main;
        float height = cam.orthographicSize;
        width = height * cam.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        // If in first room, get custom camera starting pos
        if (playerScript.roomNumber == 1)
        {
            xBoundLeft = GameObject.Find("Camera Start").transform.position.x;
        }
        else
        {
            xBoundLeft = triggers.transform.GetChild(playerScript.roomNumber - 1).position.x;
        }

        // If in last room, get custom ending pos
        if (playerScript.roomNumber == triggers.transform.childCount)
        {
            xBoundRight = GameObject.Find("Camera End").transform.position.x;
        }
        else
        {
            xBoundRight = triggers.transform.GetChild(playerScript.roomNumber).position.x;
        }

        // Account for width of camera view in bounds
        xBoundLeft += width;
        xBoundRight -= width;

        // Find center point if left bound is to the right of the right bound
        if (xBoundLeft > xBoundRight)
        {
            xBoundLeft += xBoundRight;
            xBoundLeft /= 2;
            xBoundRight = xBoundLeft;
        }

        // Convert world coordinate to camera pos
        xBoundLeft -= 29;
        xBoundRight -= 27;

        // If in bounds follow player
        if (transform.position.x > xBoundLeft && transform.position.x < xBoundRight)
        {
            transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(xBoundRight + 28, transform.position.y, transform.position.z);
        }
        
    }
}
