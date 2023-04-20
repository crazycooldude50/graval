using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private float xLeft;
    [SerializeField] private float xRight;
    private GameObject seperators;
    private float cameraWidth;
    [SerializeField] private float velocity;



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        seperators = GameObject.Find("Room Seperators");
        cameraWidth = Camera.main.orthographicSize * Camera.main.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        
        UpdateBounds(player.GetComponent<PlayerController>().roomNumber);

        // Default track player, change target if out of bounds
        float targetPos = player.transform.position.x;
        
        // If the camera is too far to the left, snap to left bound
        if (transform.position.x < xLeft) {
            targetPos = xLeft;
        }
        // If the camera is too far to the right, snap to right bound
        else if (transform.position.x > xRight)
        {
            targetPos = xRight;
        }
        // Ease camera movement
        Vector3 lerpPosition = Vector3.Lerp(transform.position, new Vector3(targetPos, transform.position.y, transform.position.z), 0.01f);
        transform.position = lerpPosition;
    }

    public void UpdateBounds(int roomNum)
    {
        roomNum--;
        // If in first room, set camera left bound to custom object
        if (roomNum == 0)
        {
            xLeft = GameObject.Find("Camera Left").transform.position.x;
        }
        else
        {
            xLeft = seperators.transform.GetChild(roomNum - 1).Find("Camera Trigger").position.x;
        }

        // If in last room, set camera right bound to custom object
       if (roomNum == seperators.transform.childCount)
        {
            xRight = GameObject.Find("Camera Right").transform.position.x;
        }
        else
        {
            xRight = seperators.transform.GetChild(roomNum).Find("Camera Trigger").position.x;
        }
       
        // Account for camera width is bound size
        xLeft += cameraWidth;
        xRight -= cameraWidth;

        // If the left bound is in front of the right one after addition, set the bounds to the average value
        if (xLeft > xRight)
        {
            xLeft = (xLeft + xRight) / 2;
            xRight = xLeft;
        }

    }
}
