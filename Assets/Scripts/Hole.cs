using UnityEngine;

public class Hole : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance; // Obtenim la instància del GameManager
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball")) // Assegura't que la pilota té el tag "Ball"
        {
            gameManager.BallInHole(); // Notifiquem al GameManager que la pilota ha entrat al forat
        }
    }
}
