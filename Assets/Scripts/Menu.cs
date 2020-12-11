using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    

    public static void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public static void Quit()
    {
        Application.Quit();
    }

    public void ShowControls(GameObject InfoBox)
    {
        InfoBox.SetActive(true);
    }

    public void HideControls(GameObject InfoBox)
    {
        InfoBox.SetActive(false);
    }

}
