using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAdjust : MonoBehaviour
{
    float margin = 1.5f; // space between screen border and nearest fighter
    float marginY = 1.5f; // space between screen border and nearest fighter

    private float z0; // coord z of the fighters plane
    private float zCam; // camera distance to the fighters plane
    private float wScene; // scene width
    private float hScene; // scene height
    private Transform f1; // fighter1 transform
    private Transform f2; // fighter2 transform
    private float xL; // left screen X coordinate
    private float xR; // right screen X coordinate
    private float yU; // up screen Y coordinate
    private float yD; // down screen Y coordinate
    Camera cam;
    float camheight;
    float camwidth;

    //Relativos
    float minortsize = 5f, maxortsize = 7.5f;
    float maxX = -4.5f, minX = -18f;

    public void calcScreen(Transform p1, Transform p2)
    {
        // Calculates the xL and xR screen coordinates 
        if (p1.position.x < p2.position.x)
        {
            xL = p1.position.x - margin;
            xR = p2.position.x + margin;
        }
        else
        {
            xL = p2.position.x - margin;
            xR = p1.position.x + margin;
        }

        if (p1.position.y < p2.position.y)
        {
            yD = p1.position.y - marginY;
            yU = p2.position.y + marginY;
        }
        else
        {
            yD = p2.position.y - marginY;
            yU = p1.position.y + marginY;
        }
    }
    
    public void Start()
    {
        cam = Camera.main;
        float camheight = 2f * cam.orthographicSize;
        float camwidth = camheight * cam.aspect;

    }

    public void Update()
    {
        if(f1 != null)
        {
            Vector3 pos = transform.position;
            calcScreen(f1, f2);
            float width = xR - xL;
            float height = yU - yD;

            if (width > wScene)
            { // if fighters too far adjust camera distance
                pos.z = zCam * width / wScene + z0;

            }
            if (height > hScene)
            { // if fighters too far adjust camera distance
                pos.z = zCam * height / hScene + z0;
            }

            // centers the camera
            pos.x = (xR + xL) / 2;
            //pos.y = (yU + yD) / 2;

            float new_ortSize = (width / cam.aspect) / 2f;
            if (new_ortSize < minortsize) new_ortSize = minortsize;
            else if (new_ortSize > maxortsize) new_ortSize = maxortsize;

            cam.orthographicSize = new_ortSize;

            //if (pos.y > maxy) pos.y = maxy;

            if (pos.x < minX) pos.x = minX;
            else if (pos.x > maxX) pos.x = maxX;

            transform.position = pos;
        }

    }

    public void SetupCamera()
    {
        // find references to the fighters
        f1 = GameObject.FindGameObjectWithTag("Player1").transform;
        f2 = GameObject.FindGameObjectWithTag("Player2").transform;
        // initializes scene size and camera distance
        calcScreen(f1, f2);
        wScene = xR - xL;
        hScene = yU - yD;
        zCam = transform.position.z - z0;
    }

}
