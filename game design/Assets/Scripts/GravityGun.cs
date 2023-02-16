using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityGun : MonoBehaviour
{
    private Vector2 aimDir = new Vector2(1, 0);
    private bool controlling = false;
    private GameObject barrel;

    // Start is called before the first frame update
    void Start()
    {
        barrel = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!controlling)
        {
            // Aim in 1 of 8 directions
            Aim();
        }
        DrawGravityRay();
        Detect();
    }

    private void Aim()
    {
        // Uses arrow keys to detect aim direction for gravity beam
        // If no direction is specified, default to player facing direction
        aimDir = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            aimDir.x = -1;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            aimDir.x += 1;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            aimDir.y = -1;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            aimDir.y += 1;
        }
        if (aimDir == new Vector2(0, 0))
        {
            GameObject player = GameObject.Find("Player");
            aimDir.x = player.GetComponent<PlayerController>().GetFlipped();
        }
    }

    private void Detect()
    {
        if (!controlling)
        {
            RaycastHit2D aimBeam = Physics2D.Raycast(barrel.transform.position, aimDir);
            //Debug.Log(aimBeam.collider.gameObject.tag);
        }
    }

    private void DrawGravityRay()
    {
        Vector2 startPos = barrel.transform.position;
        Vector2 endPos = startPos + aimDir * 100;
        // CHANGE GETCHILD TO FIND GAMEOBJECT
        LineRenderer line = barrel.GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.SetPosition(0, startPos);
        line.SetPosition(1, endPos);
    }
}
