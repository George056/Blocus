using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI ScoreTxt;

    [HideInInspector] public static GameManager instance;

    [HideInInspector] public static int Score;

    [HideInInspector] public GameObject ActivePiece;

    private bool Paused;

    // Start is called before the first frame update
    void Start()
    {

        if(instance == null)
        {
            instance = this;
        }
        else 
        {
            Destroy(gameObject);
        }

        Score = 0;
        Paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        ScoreTxt.text = (Score).ToString("D6");

        if (Input.GetKeyDown(KeyCode.Escape))
            PauseBtn();
    }

    public void SetActivePiece(GameObject piece)
    {
        ActivePiece = piece;
    }

    public void PauseBtn()
    {
        Paused = !Paused;
        ActivePiece.GetComponent<TetrominoMovement>().SetPeiceActive(!Paused);
    }
}
