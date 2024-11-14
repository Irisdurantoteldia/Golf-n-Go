using UnityEngine;

public class Hole : MonoBehaviour
{
    private GameObject ball;

    public string targetScene;
    public bool isExitHole = false; // Indica si aquest forat és el d'exit

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Ball")
        {
             Debug.Log("La pilota ha entrat al forat!"); 
            ball = other.gameObject;
            ball.SetActive(false);
            Invoke("HandleGoal", 2);
        }
    }

    private void HandleGoal()
    {
        if (isExitHole)
        {
            #if UNITY_EDITOR
                Debug.Log("Fi del joc. Gràcies per jugar!"); // Missatge per l'Editor
            #else
                Application.Quit(); // Tanca el joc en una build
            #endif
        }
        else if (!string.IsNullOrEmpty(targetScene))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(targetScene); // Carrega l'escena especificada
        }
    }
}
