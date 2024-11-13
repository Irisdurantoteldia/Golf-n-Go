using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    private float force;
    public int chargeTime = 1; // Temps que es triga en carregar la força de llançament
    public float launchSpeed = 5; // Velocitat de llançament de la pilota
    public Slider forceSlider;
    private LineRenderer lineRenderer; // Dibuixa una línia entre la pilota i el ratolí
    public float maxLineLength = 2.0f; // Longitud màxima de la línia visible

    // Inicialitza valors al començament del joc
    void Start()
    {
        forceSlider.gameObject.SetActive(false); // Oculta el slider fins que es necessiti
        lineRenderer = gameObject.AddComponent<LineRenderer>(); // Crea un component LineRenderer per la pilota
        lineRenderer.positionCount = 2; // Estableix que la línia té dos extrems: la pilota i el cursor
        lineRenderer.startWidth = 0.05f; // Gruix de la línia a l'extrem inicial
        lineRenderer.endWidth = 0.05f; // Gruix de la línia a l'extrem final
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Assigna un material bàsic a la línia
        lineRenderer.startColor = Color.black; // Color de l'extrem de la línia més proper a la pilota
        lineRenderer.endColor = Color.black; // Color de l'extrem de la línia més proper al cursor
        lineRenderer.sortingOrder = 1; // Defineix l'ordre de renderització de la línia
    }

    // Executa codi cada fotograma per mantenir la lògica de moviment i interacció
    void Update()
    {
        // Obté la posició actual del ratolí a la pantalla
        Vector3 mousePosition = Input.mousePosition;
        // Converteix la posició del ratolí a coordenades del món
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector2(mousePosition.x, mousePosition.y));
        mousePosition.z = 0; // Estableix la profunditat en 0 per treballar en 2D

        // Calcula la direcció entre la pilota i el ratolí
        Vector2 direction = (mousePosition - transform.position).normalized;
        Vector2 velocity = GetComponent<Rigidbody2D>().velocity;

        // Comprova la velocitat de la pilota per activar o desactivar la línia de guia
        if (velocity.magnitude <= 0.02)
        {
            velocity = new Vector2(0, 0); // Si la velocitat és baixa, la estableix a 0 per evitar moviments residuals
            lineRenderer.enabled = true; // Mostra la línia si la pilota està quieta
        }
        else
        {
            lineRenderer.enabled = false; // Amaga la línia mentre la pilota es mou
        }

        // Si la pilota està quieta, permet carregar la força
        if (velocity.magnitude <= 0.02)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                force = 0; // Reinicia la força quan es comença a carregar
                forceSlider.gameObject.SetActive(true); // Mostra el slider per indicar la força
            }

            if (Input.GetKey(KeyCode.Space))
            {
                if (chargeTime == 0) { chargeTime = 1; } // Evita divisió per zero en el càlcul de la força
                force += Time.deltaTime / chargeTime; // Augmenta la força gradualment amb el temps de càrrega
                if (force >= 1) { force = 1; } // Limita la força a un màxim d'1
                forceSlider.value = force; // Actualitza el valor del slider
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                // Llança la pilota aplicant la força acumulada en la direcció desitjada
                GetComponent<Rigidbody2D>().AddForce(direction * (force * launchSpeed), ForceMode2D.Impulse);
                Invoke("ResetForce", 2); // Reinicia la força després de 2 segons
            }
        }

        // Actualitza els extrems de la línia de guia
        lineRenderer.SetPosition(0, transform.position); // Defineix el punt d'inici de la línia a la pilota

        // Calcula la distància entre la pilota i el cursor per limitar la longitud de la línia
        float distance = Vector2.Distance(transform.position, mousePosition);
        
        // Si la línia és massa llarga, ajusta el punt final de la línia
        if (distance > maxLineLength)
        {
            // Calcula un punt final limitat a maxLineLength des de la pilota
            Vector2 adjustedPosition = (Vector2)transform.position + direction * maxLineLength;
            lineRenderer.SetPosition(1, adjustedPosition); // Estableix el punt final de la línia ajustat
        }
        else
        {
            lineRenderer.SetPosition(1, mousePosition); // Estableix el punt final de la línia a la posició del ratolí
        }
    }

    // Reinicia la força després de cada llançament
    void ResetForce()
    {
        force = 0; // Reinicia la força acumulada després del llançament
        forceSlider.value = 0; // Reinicia el valor del slider
        forceSlider.gameObject.SetActive(false); // Amaga el slider fins a la propera càrrega
    }
}
