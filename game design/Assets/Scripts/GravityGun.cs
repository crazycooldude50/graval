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
        barrel = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        DrawGravityRay();
    }

    private void Aim()
    {
        if (!controlling) {
            RaycastHit2D aimBeam = Physics2D.Raycast(barrel.position, aimDir * 100);
        }
    }

    private void DrawGravityRay()
    {
        Vector2 startPos = transform.position;
        Vector2 endPos = startPos + new Vector2(100, 0);
        // CHANGE GETCHILD TO FIND GAMEOBJECT
        LineRenderer line = barrel.GetComponent<LineRenderer>();
        line.SetVertexCount(2);
        line.SetPosition(0, startPos);
        line.SetPosition(1, endPos);
    }
}
