using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Jugador")]
    public string playerName;  // Nom del jugador
    public int totalHits = 0;  // Comptador de cops totals
    public int maxScores = 5;  // Nombre màxim de resultats a guardar
    private List<int> topScores = new List<int>();  // Llista dels millors resultats

    [Header("Nivells")]
    public List<string> levels;  // Llista de nivells
    private int currentLevelIndex = 0;

    [Header("UI")]
    public TMP_Text playerScoreText;  // Text per mostrar la puntuació del jugador
    public TMP_Text rankingText;      // Text per mostrar el rànquing

    [Header("Pantalla Inicial")]
    public TMP_InputField playerNameInput;  // Input per al nom del jugador
    public UnityEngine.UI.Button startButton;  // Botó per iniciar el joc

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Evitar que es destrueixi entre escenes
        }
        else
        {
            Destroy(gameObject);
        }

        LoadScores();  // Carregar el rànquing en iniciar
    }

    void Start()
    {
        // Si estem a la MainScene, actualitzem el rànquing i la puntuació
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            ShowEndScreen();  // Mostrar el rànquing i la puntuació final
        }

        // Afegir l'escoltador del botó de start si estem a la pantalla inicial
        if (startButton != null && SceneManager.GetActiveScene().name == "MainScene")
        {
            startButton.onClick.AddListener(OnStartGame);
        }
    }

    // Mètode per iniciar el joc des de la pantalla inicial
    public void OnStartGame()
    {
        if (!string.IsNullOrEmpty(playerNameInput.text))
        {
            playerName = playerNameInput.text;  // Assignar el nom del jugador
            totalHits = 0;  // Reiniciar el comptador de cops
            currentLevelIndex = 0;  // Reiniciar el nivell
            SceneManager.LoadScene(levels[currentLevelIndex]);  // Carregar el primer nivell
        }
        else
        {
            Debug.Log("Si us plau, escriu el teu nom abans d'iniciar.");
        }
    }

    // Mètode per gestionar l'entrada al forat
    public void BallInHole()
    {
        IncrementHits();  // Incrementar el comptador de cops quan la pilota entra al forat
        AddScoreToRanking();  // Afegir la puntuació al rànquing immediatament
        SaveScores();  // Desa el rànquing després d'afegir la nova puntuació

        if (currentLevelIndex < levels.Count - 1)
        {
            currentLevelIndex++; // Incrementem el nivell
            SceneManager.LoadScene(levels[currentLevelIndex]); // Carreguem el següent nivell
        }
        else
        {
            Debug.Log("Últim nivell completat!");
            StartCoroutine(EndGameCoroutine());  // Cridem una corutina per afegir una pausa abans de carregar la MainScene
        }
    }

    // Corutina per afegir un petit retard abans de finalitzar el joc
    private IEnumerator EndGameCoroutine()
    {
        yield return new WaitForSeconds(1f);  // Esperem 1 segon
        EndGame();  // Finalitzar el joc
    }

    // Finalitza el joc i mostra la pantalla final
    public void EndGame()
    {
        AddScoreToRanking();  // Afegeix la puntuació al rànquing

        // Desa el rànquing
        SaveScores();
        ShowEndScreen();

        // Carregar la pantalla principal (MainScene)
        SceneManager.LoadScene("MainScene");
    }

    // Incrementa els cops del jugador
    public void IncrementHits()
    {
        totalHits++;  // Comptabilitzar un cop més
    }

    // Afegeix la puntuació al rànquing
    private void AddScoreToRanking()
    {
        topScores.Add(totalHits);  // Afegir la puntuació actual
        topScores.Sort();  // Ordenar per puntuació
        topScores.Reverse();  // Posa les més altes primer

        // Limitar el nombre de resultats guardats
        if (topScores.Count > maxScores)
        {
            topScores.RemoveAt(topScores.Count - 1);  // Elimina l'últim si supera el límit
        }
    }

    // Desa el rànquing
    private void SaveScores()
    {
        for (int i = 0; i < topScores.Count; i++)
        {
            PlayerPrefs.SetInt($"TopScore_{i}", topScores[i]);  // Desa cada puntuació
        }
        PlayerPrefs.SetInt("TopScoresCount", topScores.Count);  // Desa la quantitat de resultats guardats
        PlayerPrefs.Save();  // Guarda les preferències (perquè es conserven entre escenes)
    }

    // Carrega el rànquing des de PlayerPrefs
    private void LoadScores()
    {
        topScores.Clear();  // Esborrar les puntuacions existents
        int count = PlayerPrefs.GetInt("TopScoresCount", 0);  // Carregar la quantitat de resultats guardats

        for (int i = 0; i < count; i++)
        {
            topScores.Add(PlayerPrefs.GetInt($"TopScore_{i}", 0));  // Carregar cada puntuació
        }
    }

    // Mostra la pantalla final amb la puntuació i el rànquing
    public void ShowEndScreen()
    {
        string ranking = "Rànquing dels millors resultats:\n";
        for (int i = 0; i < topScores.Count; i++)
        {
            ranking += $"{i + 1}. {topScores[i]} cops\n";
        }

        // Mostrar la puntuació final
        if (playerScoreText != null)
        {
            playerScoreText.text = "Puntuació final: " + totalHits + " cops";
        }

        // Mostrar el rànquing a la UI
        if (rankingText != null)
        {
            rankingText.text = ranking;
        }

        Debug.Log("Puntuació final: " + totalHits);
        Debug.Log(ranking);  // Aquí mostra el rànquing a la consola
    }
}
