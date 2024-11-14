using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 pointA;  // Punt d'inici de la plataforma
    public Vector3 pointB;  // Punt final de la plataforma
    public float speed = 2f;  // Velocitat de moviment de la plataforma
    private bool movingToB = true;  // Controla si la plataforma es mou cap al punt B

    void Start()
    {
        transform.position = pointA;  // Inicia la plataforma al punt A
    }

    void Update()
    {
        // Moviment de la plataforma entre els punts A i B
        if (movingToB)
        {
            transform.position = Vector3.MoveTowards(transform.position, pointB, speed * Time.deltaTime);
            if (transform.position == pointB)
            {
                movingToB = false;  // Canvia la direcció quan arriba al punt B
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, pointA, speed * Time.deltaTime);
            if (transform.position == pointA)
            {
                movingToB = true;  // Torna al punt A quan arriba al punt A
            }
        }
    }

    // Detecta quan la pilota col·lideix amb la plataforma
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ball")  // Si la col·lisió és amb la pilota
        {
            Rigidbody2D ballRb = collision.gameObject.GetComponent<Rigidbody2D>();

            // Modificar la velocitat de la pilota segons la direcció i la velocitat de la plataforma
            Vector2 platformVelocity = (pointB - pointA).normalized * speed;
            ballRb.velocity += platformVelocity;  // Afegeix la velocitat de la plataforma a la pilota
        }
    }
}
