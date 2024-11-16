using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    public Text totalScoreText; // Text per mostrar la puntuació total

    private void Start()
    {
        ShowFinalScore();
    }

    private void ShowFinalScore()
    {
        if (GameManager.Instance != null)
        {
            totalScoreText.text = $"Total de cops: {GameManager.Instance.totalScore}";
        }
        else
        {
            totalScoreText.text = "Benvingut al joc!";
        }
    }
}