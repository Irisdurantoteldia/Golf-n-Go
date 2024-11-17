using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    private float force;
    public int chargeTime = 1; // Temps de càrrega per defecte
    public float launchSpeed = 1; // Velocitat de llançament de la pilota
    public Slider forceSlider;
    private LineRenderer lineRenderer; // Línia per mostrar la direcció del tir
    public float maxLineLength = 1.0f; // Longitud màxima de la línia d'apuntatge

    // Array per especificar noms d'escenes on el tir es reflectirà
    public string[] invertedLevelNames;

    private bool isMoving = false; // Detecta si la pilota ja està en moviment

    void Start()
    {
        forceSlider.gameObject.SetActive(false); 
        lineRenderer = gameObject.AddComponent<LineRenderer>(); 
        lineRenderer.positionCount = 2; 
        lineRenderer.startWidth = 0.05f; 
        lineRenderer.endWidth = 0.05f; 
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); 
        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = Color.red; 
        lineRenderer.sortingOrder = 1; 
    }

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector2(mousePosition.x, mousePosition.y));
        mousePosition.z = 0;

        Vector2 direction = (mousePosition - transform.position).normalized;
        Vector2 velocity = GetComponent<Rigidbody2D>().velocity;

        if (velocity.magnitude <= 0.02f)
        {
            velocity = Vector2.zero;
            lineRenderer.enabled = true;
        }
        else
        {
            lineRenderer.enabled = false;
            if (!isMoving) // Si la pilota comença a moure's, incrementem els hits
            {
                GameManager.Instance.IncrementHits(); // Incrementa els cops quan la pilota comença a moure
                isMoving = true;  // Evitem que el comptador s'incremeti més d'una vegada
            }
        }

        if (velocity.magnitude <= 0.02f)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                force = 0; 
                forceSlider.gameObject.SetActive(true); 
            }

            if (Input.GetKey(KeyCode.Space))
            {
                if (chargeTime == 0) { chargeTime = 1; } 
                force += Time.deltaTime / chargeTime;
                if (force >= 1) { force = 1; } 
                forceSlider.value = force;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                // Comprova si l'escena actual és una de les que ha d'invertir el tir
                Vector2 launchDirection;
                if (IsInvertedLevel(SceneManager.GetActiveScene().name))
                {
                    launchDirection = -direction; // Inverteix la direcció en nivells especificats
                }
                else
                {
                    launchDirection = direction; // Direcció normal en altres nivells
                }

                GetComponent<Rigidbody2D>().AddForce(launchDirection * (force * launchSpeed), ForceMode2D.Impulse);
                Invoke("ResetForce", 2);
            }
        }

        lineRenderer.SetPosition(0, transform.position);

        float distance = Vector2.Distance(transform.position, mousePosition);
        
        if (distance > maxLineLength)
        {
            Vector2 adjustedPosition = (Vector2)transform.position + direction * maxLineLength;
            lineRenderer.SetPosition(1, adjustedPosition);
        }
        else
        {
            lineRenderer.SetPosition(1, mousePosition);
        }
    }

    // Comprova si el nom de l'escena actual es troba a la llista de nivells invertits
    private bool IsInvertedLevel(string currentSceneName)
    {
        foreach (string levelName in invertedLevelNames)
        {
            if (levelName == currentSceneName)
            {
                return true;
            }
        }
        return false;
    }

    private void ResetForce()
    {
        force = 0;
        forceSlider.value = 0;
        forceSlider.gameObject.SetActive(false);
    }
}
