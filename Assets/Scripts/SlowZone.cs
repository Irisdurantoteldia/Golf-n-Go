using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowZone : MonoBehaviour
{
    public float slowDownMultiplier = 0.5f; // Multiplicador per reduir la velocitat (més baix = frena més ràpid)
    public float stopThreshold = 0.2f; // Velocitat mínima perquè l'objecte es pari completament
    public float slowDownStep = 0.05f; // Pas de reducció de velocitat en cada frame

    private void OnTriggerStay2D(Collider2D other)
    {
        // Comprova si l'objecte que entra té un Rigidbody2D (per exemple, la pilota)
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Mostra missatge de depuració per assegurar-nos que detectem la pilota
            Debug.Log($"Objecte detectat: {other.gameObject.name} amb velocitat {rb.velocity.magnitude}");

            // Redueix la velocitat progressivament
            Vector2 currentVelocity = rb.velocity;
            currentVelocity = currentVelocity * slowDownMultiplier - currentVelocity.normalized * slowDownStep;

            // Assegura que la velocitat no baixa més enllà del llindar
            if (currentVelocity.magnitude <= stopThreshold)
            {
                Debug.Log("Parant l'objecte completament.");
                rb.velocity = Vector2.zero;
            }
            else
            {
                rb.velocity = currentVelocity;
            }
        }
    }
}
