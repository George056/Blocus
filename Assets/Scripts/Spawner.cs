using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject[] Tetrominos;

    [HideInInspector] public GameObject HoldPiece;
    [HideInInspector] public GameObject[] NextTetrominosList = new GameObject [4];

    // Start is called before the first frame update
    void Start()
    {
        LoadTetList();
        NewTetromino();
    }

    public void NewTetromino()
    {
        GameObject piece = NextTetrominosList[0];
        piece.transform.position = transform.position;

        if (piece.GetComponent<TetrominoMovement>().ValidMove())
        {
            piece.GetComponent<TetrominoMovement>().SetActive();
            Instantiate(piece, transform.position, Quaternion.identity);
        }
        else
        {
            TetrominoMovement.Loss();
        }
        RotateList();
    }

    public void LoadTetList()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject piece = Tetrominos[Random.Range(0, Tetrominos.Length)];
            NextTetrominosList[i] = piece;
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
