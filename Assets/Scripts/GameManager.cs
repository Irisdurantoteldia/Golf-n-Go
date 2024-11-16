using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TMP_InputField playerInpt; // Nom del jugador
    private string playerName;
    private TextMeshProUGUI scoreText; // Puntuació acumulada
    public TextMeshProUGUI rankingText;
    public List<int> topScores = new List<int>();

    private const string ScoresKey = "TopScores";
    private const int MaxScores = 5;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadScores();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Afegeix puntuació al rànquing
    public void AddScore(int score)
    {
        totalScore += score;
    }

    // Finalitza el joc i torna a la pantalla principal
    public void EndGame()
    {
        AddScoreToRanking(); // Desa el total de cops al rànquing
        SceneManager.LoadScene("MainMenu"); // Carrega la pantalla principal
    }

    // Desa la puntuació actual al rànquing
    private void AddScoreToRanking()
    {
        topScores.Add(totalScore);
        topScores.Sort();
        topScores.Reverse();

        if (topScores.Count > MaxScores)
        {
            topScores.RemoveAt(topScores.Count - 1);
        }

        SaveScores();
    }

    private void SaveScores()
    {
        for (int i = 0; i < topScores.Count; i++)
        {
            PlayerPrefs.SetInt($"{ScoresKey}_{i}", topScores[i]);
        }
        PlayerPrefs.SetInt($"{ScoresKey}_Count", topScores.Count);
        PlayerPrefs.Save();
    }

    private void LoadScores()
    {
        topScores.Clear();
        int count = PlayerPrefs.GetInt($"{ScoresKey}_Count", 0);
        for (int i = 0; i < count; i++)
        {
            topScores.Add(PlayerPrefs.GetInt($"{ScoresKey}_{i}", 0));
        }
    }
}
