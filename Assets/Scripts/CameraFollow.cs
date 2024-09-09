using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // Referencja do obiektu gracza
    public Vector3 offset;    // Wektor przesunięcia kamery względem gracza
    public float smoothSpeed = 0.125f; // Prędkość płynnego podążania kamery

    void LateUpdate()
    {
        // Oblicz pożądaną pozycję kamery z uwzględnieniem przesunięcia
        Vector3 desiredPosition = player.position + offset;

        // Płynne przejście z aktualnej pozycji kamery do nowej pozycji
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Aktualizacja pozycji kamery
        transform.position = smoothedPosition;
    }
}
