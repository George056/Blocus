using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject[] Tetrominos;

    [HideInInspector] public GameObject HoldPiece;
    [HideInInspector] public GameObject[] NextTetrominosList;

    private Vector3 NextTetPos = new Vector3(18f, 15f, -6);
    private bool started = false;
    // Start is called before the first frame update
    void Start()
    {
        LoadTetList();
        NewTetromino();
        started = true;
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
        }
        piece.transform.position = transform.position;
        piece.GetComponent<TetrominoMovement>().enabled = true;
        RotateList();
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

    public void SpawnNextTet()
    {
        NextTetrominosList[0] = Instantiate(NextTetrominosList[0], NextTetPos, Quaternion.identity);
        NextTetrominosList[0].GetComponent<TetrominoMovement>().enabled = false;
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
        }
        else
        {
            GameObject temp = piece;
            piece = HoldPiece;
            HoldPiece = temp;
        }
    }
}
