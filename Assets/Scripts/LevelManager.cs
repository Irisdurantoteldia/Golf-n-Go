using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int levelScore; // Nombre de cops d'aquest nivell

    private void OnLevelComplete()
    {
        GameManager.Instance.AddScore(levelScore); // Afegeix els cops d'aquest nivell al total
        GameManager.Instance.EndGame(); // Finalitza el joc i torna a la pantalla principal
    }
}
