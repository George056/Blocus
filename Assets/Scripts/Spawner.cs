using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject[] Tetrominos;

    [HideInInspector] public GameObject HoldPiece;
    [HideInInspector] public GameObject[] NextTetrominosList;
    [HideInInspector] public bool Paused;

    private Vector3 NextTetPos1 = new Vector3(15.5f, 18f, -1);
    private Vector3 NextTetPos2 = new Vector3(15.5f, 14f, -1);
    private Vector3 NextTetPos3 = new Vector3(15.5f, 8f, -1);
    private Vector3 NextTetPos4 = new Vector3(15.5f, 2f, -1);
    private Vector3 HoldPos = new Vector3(-5f, 18f, -1);
    private bool started = false;

    private bool PlacedInitial = false;
    // Start is called before the first frame update
    void Start()
    {
        LoadTetList();
        SpawnFirstNextTets();
        Paused = true;
    }

    private void Update()
    {
        if (!Paused && !PlacedInitial)
        {
            PlacedInitial = true;
            started = true;

            NewTetromino();
        }
    }

    public void NewTetromino()
    {
        GameObject piece;
        if (started == false)
        {
            piece = Tetrominos[Random.Range(0, Tetrominos.Length)];
        }
        else
        {
            piece = NextTetrominosList[0];
            RotateList();

        }
        piece.transform.position = transform.position;
        piece.GetComponent<TetrominoMovement>().enabled = true;
        if (started)
            SpawnNextTet();

        if (piece.GetComponent<TetrominoMovement>().ValidMove())
        {
            if (started == false)
                GameManager.instance.ActivePiece = Instantiate(piece, transform.position, Quaternion.identity);
            else
                GameManager.instance.ActivePiece = piece;

            piece.GetComponent<TetrominoMovement>().SetPieceActive(true);
        }
        else
        {
            TetrominoMovement.Loss();
        }
    }

    public void SpawnFirstNextTets()
    {
        NextTetrominosList[0] = Instantiate(NextTetrominosList[0], NextTetPos1, Quaternion.identity);
        NextTetrominosList[0].GetComponent<TetrominoMovement>().enabled = false;
        NextTetrominosList[1] = Instantiate(NextTetrominosList[1], NextTetPos2, Quaternion.identity);
        NextTetrominosList[1].GetComponent<TetrominoMovement>().enabled = false;
        NextTetrominosList[2] = Instantiate(NextTetrominosList[2], NextTetPos3, Quaternion.identity);
        NextTetrominosList[2].GetComponent<TetrominoMovement>().enabled = false;
        NextTetrominosList[3] = Instantiate(NextTetrominosList[3], NextTetPos4, Quaternion.identity);
        NextTetrominosList[3].GetComponent<TetrominoMovement>().enabled = false;
    }

    public void SpawnNextTet() 
    {
        NextTetrominosList[0].transform.position = NextTetPos1;
        NextTetrominosList[1].transform.position = NextTetPos2;
        NextTetrominosList[2].transform.position = NextTetPos3;
        NextTetrominosList[3] = Instantiate(NextTetrominosList[3], NextTetPos4, Quaternion.identity);
        NextTetrominosList[3].GetComponent<TetrominoMovement>().enabled = false;
    }

    public void LoadTetList()
    {
        NextTetrominosList = new GameObject[4];
        for (int i = 0; i < 4; i++)
        {
            NextTetrominosList[i] = Tetrominos[Random.Range(0, Tetrominos.Length)];
        }
    }

    public void RotateList()
    {
        NextTetrominosList[0] = NextTetrominosList[1];
        NextTetrominosList[1] = NextTetrominosList[2];
        NextTetrominosList[2] = NextTetrominosList[3];
        NextTetrominosList[3] = Tetrominos[Random.Range(0, Tetrominos.Length)];
    }

    public void HoldIt(GameObject piece)
    {
        if (HoldPiece == null)
        {
            HoldPiece = piece;
            HoldPiece.transform.position = HoldPos;
            HoldPiece.GetComponent<TetrominoMovement>().enabled = false;
            NewTetromino();
        }
        else
        {
            GameObject temp = piece;                                //Place old piece on play area
            piece = HoldPiece;
            piece.transform.position = temp.transform.position;
            piece.GetComponent<TetrominoMovement>().enabled = true;
            GameManager.instance.ActivePiece = piece;

            HoldPiece = temp;                                       //Store new piece in hold
            HoldPiece.transform.position = HoldPos;
            HoldPiece.GetComponent<TetrominoMovement>().enabled = false;
        }

    }
}
