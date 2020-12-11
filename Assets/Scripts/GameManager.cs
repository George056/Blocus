using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI ScoreTxt;
    public GameObject Spawner;
    public TextMeshProUGUI CountdownText;
    public TextMeshProUGUI LevelTxt;

    public GameObject[] GridLinePlaneY;
    public GameObject[] GridLinePlaneZ;

    public GameObject ReturnHomeBtn;

    [HideInInspector] public static GameManager instance;

    [HideInInspector] public static int Score = 0;

    [HideInInspector] public GameObject ActivePiece = null;

    public bool Paused;
    private float _time;
    private int _countdown = 3;
    private int _level = 0;

    private int[] _levelScore = new int[] { 2000, 4000, 6000, 8000, 10000, 20000, 30000, 40000, 50000, 100000};

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

        _time = Time.time;
        CountdownText.text = (_countdown).ToString("f0");
        LevelTxt.text = "Level: " + (_level).ToString("f0");
        Score = 0;
        Paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        ScoreTxt.text = (Score).ToString("D6");

        if (Input.GetKeyDown(KeyCode.Escape))
            PauseBtn();

        if (((Time.time - _time)) > 1 && _countdown >= 0)
        {
            _time = Time.time;
            _countdown--;
            if (_countdown == 0)
            {
                CountdownText.text = "Start!!!";
            }
            else if (_countdown > 0)
                CountdownText.text = (_countdown).ToString("f0");
        }

        if (_countdown < 0)
        {
            CountdownText.gameObject.SetActive(false);
            Spawner.GetComponent<Spawner>().Paused = false;
        }

        if (ActivePiece != null)
        {
            foreach (GameObject plane in GridLinePlaneY)
            {
                plane.SetActive(false);
            }

            foreach (GameObject plane in GridLinePlaneZ)
            {
                plane.SetActive(false);
            }

            foreach (Transform child in ActivePiece.transform)
            {
                GridLinePlaneY[10 - Mathf.RoundToInt(child.transform.position.z)].SetActive(true);
                GridLinePlaneZ[9 - Mathf.RoundToInt(child.transform.position.z)].SetActive(true);
            }
        }

        if(Score >= _levelScore[_level] || Input.GetKeyDown(KeyCode.Alpha1))
        {
            _level++;
            TetrominoMovement.fallTime *= 0.5f;
            LevelTxt.text = "Level: " + (_level).ToString("f0");
            GetComponent<AudioSource>().pitch *= 1.05f;
        }
    }

    public static void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void SetActivePiece(GameObject piece)
    {
        ActivePiece = piece;
    }

    public void PauseBtn()
    {
        Paused = !Paused;
        ActivePiece.GetComponent<TetrominoMovement>().SetPieceActive(!Paused);
        ReturnHomeBtn.SetActive(Paused);
    }
}
