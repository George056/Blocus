using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoMovement : MonoBehaviour
{
    public Vector3 RotationPoint;
    public static int Xmax = 10;
    public static int Ymax = 20;
    public static int Zmax = 10;

    public static int LineClearScore = 100;
    public static int PlaneClearScore = LineClearScore * 20;
    public static int HardDropLineScore = 10;
    public static int SoftDropLineScore = 5;

    [HideInInspector] public bool ActivePiece = false;
    [HideInInspector] public static bool NotLost = true;

    private float previousTime;
    [HideInInspector] public static float fallTime = 0.8f;
    private static Transform[,,] grid = new Transform[Xmax, Ymax, Zmax];

    private int frameCount = 0;
    private int waitFrames = 10;
    private int newPieceDropBuffer = 5;

    private GameManager instance;

    // Start is called before the first frame update
    void Start()
    {
        previousTime = Time.time;
        instance = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (ActivePiece && NotLost)//if not in a paused state
        {

            if(newPieceDropBuffer > 0)
            {
                newPieceDropBuffer--;
            }

            if (frameCount == waitFrames)
            {
                frameCount = 0;
                if (Input.GetKey(KeyCode.A))//moveNegX
                {
                    transform.position += new Vector3(-1, 0, 0);
                    if (!ValidMove())
                        transform.position -= new Vector3(-1, 0, 0);
                }
                else if (Input.GetKey(KeyCode.D))//movePosX
                {
                    transform.position += new Vector3(1, 0, 0);
                    if (!ValidMove())
                        transform.position -= new Vector3(1, 0, 0);
                }
                else if (Input.GetKey(KeyCode.W))
                {
                    transform.position += new Vector3(0, 0, 1);
                    if (!ValidMove())
                        transform.position -= new Vector3(0, 0, 1);
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    transform.position += new Vector3(0, 0, -1);
                    if (!ValidMove())
                        transform.position -= new Vector3(0, 0, -1);
                }
                else if (Input.GetKey(KeyCode.LeftArrow))//rotateClockwise
                {
                    transform.RotateAround(transform.TransformPoint(RotationPoint), new Vector3(0, 0, 1), 90);
                    if (!ValidMove())
                        transform.RotateAround(transform.TransformPoint(RotationPoint), new Vector3(0, 0, 1), -90);
                }
                else if (Input.GetKey(KeyCode.RightArrow))//rotateCounterclockwise
                {
                    transform.RotateAround(transform.TransformPoint(RotationPoint), new Vector3(0, 0, 1), -90);
                    if (!ValidMove())
                        transform.RotateAround(transform.TransformPoint(RotationPoint), new Vector3(0, 0, 1), 90);
                }
                else if (Input.GetKey(KeyCode.UpArrow))
                {
                    transform.RotateAround(transform.TransformPoint(RotationPoint), new Vector3(1, 0, 0), 90);
                    if (!ValidMove())
                        transform.RotateAround(transform.TransformPoint(RotationPoint), new Vector3(1, 0, 0), -90);
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    transform.RotateAround(transform.TransformPoint(RotationPoint), new Vector3(1, 0, 0), -90);
                    if (!ValidMove())
                        transform.RotateAround(transform.TransformPoint(RotationPoint), new Vector3(1, 0, 0), 90);
                }
            }

            if (Input.GetKey(KeyCode.E) && newPieceDropBuffer == 0)
            {
                int linesDroped = 0;
                do
                {
                    transform.position += new Vector3(0, -1, 0);
                    linesDroped++;
                } while (ValidMove());
                if (!ValidMove())
                {
                    transform.position -= new Vector3(0, -1, 0);
                    linesDroped--;
                }
                GameManager.Score += HardDropLineScore * linesDroped;
            }

            if (Input.GetKey(KeyCode.Q))
            {
                FindObjectOfType<Spawner>().HoldIt(gameObject);
            }

            FixX();
            FixY();
            FixZ();

            bool softDrop = Input.GetKey(KeyCode.X);

            if (Time.time - previousTime > ((softDrop) ? (fallTime / 10) : fallTime))
            {
                transform.position += new Vector3(0, -1, 0);
                if (!ValidMove())
                {
                    transform.position -= new Vector3(0, -1, 0);
                    AddToGrid();
                    CheckForPlanes();
                    CheckForLines();
                    this.enabled = false;
                    FindObjectOfType<Spawner>().NewTetromino();
                }
                else if(softDrop)
                {
                    GameManager.Score += SoftDropLineScore;
                }
                previousTime = Time.time;
            }
            frameCount++;
        }
    }

    void CheckForPlanes()
    {
        int totalPlanesCleared = 0;
        for (int y = 0; y < Ymax; y++)
        {
            if (HasPlane(y))
            {
                DeletePlane(y);
                PlaneDown(y);
                totalPlanesCleared++;
            }
        }
        GameManager.Score += PlaneClearScore * (totalPlanesCleared + 1);
    }

    bool HasPlane(int y)
    {
        for (int z = 0; z < Zmax; z++)
        {
            if (!HasLine(y, z))
                return false;
        }
        return true;
    }

    bool HasLine(int y, int z)
    {
        for (int x = 0; x < Xmax; x++)
        {
            if (grid[x, y, z] == null)
                return false;
        }
        return true;
    }

    void DeletePlane(int y)
    {
        for (int x = 0, z = 0; x < Xmax && z < Zmax; x++, z++)
        {
            Destroy(grid[x, y, z].gameObject);
            grid[x, y, z] = null;
        }
    }

    void PlaneDown(int y)
    {
        for (int i = y; i < Ymax; i++)
        {
            for (int x = 0; x < Xmax; x++)
            {
                for (int z = 0; z < Zmax; z++)
                {
                    if (grid[x, i, z] != null)
                    {
                        grid[x, i - 1, z] = grid[x, i, 0];
                        grid[x, i, z] = null;
                        grid[x, i - 1, z].transform.position -= new Vector3(0, 1, 0);
                    }
                }
            }
        }
    }

    void CheckForLines()
    {
        int totalLinesCleared = 0;
        for (int y = Ymax - 1; y >= 0; y--)
        {
            for (int z = 0; z < Zmax; z++)
            {
                if (HasLine(y, z))
                {
                    DeleteLine(y, z);
                    RowDown(y, z);
                    totalLinesCleared++;
                }
            }
        }
        GameManager.Score += LineClearScore * (totalLinesCleared + 1);
    }

    void DeleteLine(int y, int z)
    {
        for (int x = 0; x < Xmax; x++)
        {
            Destroy(grid[x, y, z].gameObject);
            grid[x, y, z] = null;
        }
    }

    void RowDown(int y, int z)
    {
        for (int i = y; i < Ymax; i++)
        {
            for (int x = 0; x < Xmax; x++)
            {
                if (grid[x, i, z] != null)
                {
                    grid[x, i - 1, z] = grid[x, i, z];
                    grid[x, i, z] = null;
                    grid[x, i - 1, z].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    void AddToGrid()
    {
        foreach (Transform child in transform)
        {
            int roundedX = Mathf.RoundToInt(child.transform.position.x);
            int roundedY = Mathf.RoundToInt(child.transform.position.y);
            int roundedZ = Mathf.RoundToInt(child.transform.position.z);

            grid[roundedX, roundedY, roundedZ] = child;
        }
    }

    public bool ValidMove()
    {
        foreach (Transform child in transform)
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

    void FixY()
    {
        int roundedY = Mathf.RoundToInt(transform.position.y);
        float currentY = transform.position.y;

        if (currentY - roundedY != 0)
        {
            transform.position += new Vector3(0, currentY - roundedY, 0);
        }
    }

    void FixX()
    {
        int roundedX = Mathf.RoundToInt(transform.position.x);
        float currentX = transform.position.x;

        if (currentX - roundedX != 0)
        {
            transform.position += new Vector3(currentX - roundedX, 0, 0);
        }
    }

    void FixZ()
    {
        int roundedZ = Mathf.RoundToInt(transform.position.z);
        float currentZ = transform.position.z;

        if (currentZ - roundedZ != 0)
        {
            transform.position += new Vector3(0, 0, currentZ - roundedZ);
        }
    }

    public static void Loss()
    {
        NotLost = false;
    }

    public void SetPieceActive(bool active)
    {
        ActivePiece = active;
    }
}
