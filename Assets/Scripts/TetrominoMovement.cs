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
    private static Transform[,,] grid = new Transform[Xmax, Ymax, Zmax];

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
            if (!ValidMove())
            {
                transform.position -= new Vector3(0, -1, 0);
                AddToGrid();
                CheckForLines();
                this.enabled = false;
                FindObjectOfType<Spawner>().NewTetromino();
            }
            previousTime = Time.time;
        }
    }

    void CheckForLines()
    {
        for(int y = Ymax - 1; y > 0 ; y--)
        {
            if (HasPlane(y))
            {
                DeleteLine(y);
                RowDown(y);
            }
        }
    }

    bool HasPlane(int y)
    {
        for(int z = 0; z < Zmax; z++)
        {
            if (!HasLine(y, z))
                return false;
        }
        return true;
    }

    bool HasLine(int y, int z)
    {
        for(int x = 0; x < Xmax; x++)
        {
            if (grid[x, y, z] == null)
                return false;
        }
        return true;
    }

    void DeleteLine(int y)
    {
        for(int x = 0; x < Xmax; x++)
        {
            Destroy(grid[x, y, 0].gameObject);
            grid[x, y, 0] = null;
        }
    }

    void RowDown(int y)
    {
        for(int i = y; i < Ymax; i++)
        {
            for(int x = 0; x < Xmax; x++)
            {
                if(grid[x, i, 0] != null)
                {
                    grid[x, i - 1, 0] = grid[x, i, 0];
                    grid[x, i, 0] = null;
                    grid[x, i - 1, 0].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    void AddToGrid()
    {
        foreach(Transform child in transform)
        {
            int roundedX = Mathf.RoundToInt(child.transform.position.x);
            int roundedY = Mathf.RoundToInt(child.transform.position.y);
            int roundedZ = Mathf.RoundToInt(child.transform.position.z);

            grid[roundedX, roundedY, roundedZ] = child;
        }
    }

    bool ValidMove()
    {
        foreach(Transform child in transform)
        {
            int roundedX = Mathf.RoundToInt(child.transform.position.x);
            int roundedY = Mathf.RoundToInt(child.transform.position.y);
            int roundedZ = Mathf.RoundToInt(child.transform.position.z);

            if (roundedX < 0 || roundedX >= Xmax || roundedY < 0 || roundedY >= Ymax || roundedZ < 0 || roundedZ >= Zmax)
            {
                return false;
            }
            else if (grid[roundedX, roundedY, roundedZ] != null)
                return false;
        }
        return true;
    }
}
