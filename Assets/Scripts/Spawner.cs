using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject[] Tetrominos;

    // Start is called before the first frame update
    void Start()
    {
        NewTetromino();
    }

    public void NewTetromino()
    {
        GameObject peice = Tetrominos[Random.Range(0, Tetrominos.Length)];
        peice.transform.position = transform.position;

        if (peice.GetComponent<TetrominoMovement>().ValidMove())
        {
            peice.GetComponent<TetrominoMovement>().SetActive();
            Instantiate(peice, transform.position, Quaternion.identity);
        }
        else
        {
            TetrominoMovement.Loss();
        }
    }
}
