using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public TextMeshProUGUI daysSurviveText;

    public void EndGame(int daysSurvived)
    {
        GetComponentInChildren<Canvas>().enabled = true;
        daysSurviveText.text = "Nights Survived: " + daysSurvived;
    }

    public void OnStartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnExitGame()
    {
        Application.Quit();
    }
}
