using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoMovement : MonoBehaviour
{
    public Vector3 RotationPoint;
    public static int Xmax = 10;
    public static int Ymax = 20;
    public static int Zmax = 10;

    private float previousTime;
    private float fallTime = 0.8f;

    // Start is called before the first frame update
    void Start()
    {
        previousTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))//moveNegX
        {
            transform.position += new Vector3(-1, 0, 0);
            if (!ValidMove())
                transform.position -= new Vector3(-1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.D))//movePosX
        {
            transform.position += new Vector3(1, 0, 0);
            if (!ValidMove())
                transform.position -= new Vector3(1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))//rotateClockwise
        {
            transform.RotateAround(transform.TransformPoint(RotationPoint), new Vector3(0, 0, 1), 90);
            if(!ValidMove())
                transform.RotateAround(transform.TransformPoint(RotationPoint), new Vector3(0, 0, 1), -90);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))//rotateCounterclockwise
        {
            transform.RotateAround(transform.TransformPoint(RotationPoint), new Vector3(0, 0, 1), -90);
            if (!ValidMove())
                transform.RotateAround(transform.TransformPoint(RotationPoint), new Vector3(0, 0, 1), 90);
        }

        if (Time.time - previousTime > ((Input.GetKeyDown(KeyCode.S)) ? (fallTime/10) : fallTime))
        {
            transform.position += new Vector3(0, -1, 0);
            if(!ValidMove())
                transform.position -= new Vector3(0, -1, 0);
            previousTime = Time.time;
        }
    }

    bool ValidMove()
    {
        foreach(Transform child in transform)
        {
            int roundedX = Mathf.RoundToInt(child.transform.position.x);
            int roundedY = Mathf.RoundToInt(child.transform.position.y);
            int roundedZ = Mathf.RoundToInt(child.transform.position.z);

            if(roundedX < 0 || roundedX >= Xmax || roundedY < 0 || roundedY >= Ymax || roundedZ < 0 || roundedZ >= Zmax)
            {
                return false;
            }
        }
        return true;
    }
}
