using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MovimientoJugador : MonoBehaviour
{
    public float velocidad = 15;
    public float velAngular = 60;
    public TextMeshProUGUI textoPuntaje;  // Referencia al objeto TextMeshPro para mostrar el puntaje

    private int puntaje = 0;
    private bool juegoTerminado = false;  // Variable para controlar si el juego ha terminado

    void Start()
    {
        ActualizarTextoPuntaje();
    }

    void Update()
    {
        if (juegoTerminado) return;  // No permitir más movimiento si el juego ha terminado

        // Rotación hacia la izquierda con la flecha izquierda
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(new Vector3(0, -velAngular * Time.deltaTime, 0));
        }

        // Rotación hacia la derecha con la flecha derecha
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(new Vector3(0, velAngular * Time.deltaTime, 0));
        }

        // Movimiento hacia adelante con la flecha arriba
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(new Vector3(0, 0, velocidad * Time.deltaTime));
        }

        // Movimiento hacia atrás con la flecha abajo
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(new Vector3(0, 0, -velocidad * Time.deltaTime));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ObjetoQueHuye"))
        {
            puntaje++;
            ActualizarTextoPuntaje();

            if (puntaje >= 3)
            {
                Ganar();
                return;
            }

            // Reiniciar todos los objetos que huyen y persiguen
            GameManager.Instance.ReiniciarObjetos();
        }
        else if (other.CompareTag("Persiguiendo"))
        {
            GameManager.Instance.DescuentoPuntos();
        }
    }

    private void ActualizarTextoPuntaje()
    {
        if (textoPuntaje != null)
        {
            textoPuntaje.text = "Puntaje: " + puntaje;
        }
    }

    private void Ganar()
    {
        juegoTerminado = true;
        textoPuntaje.text = "¡Ganaste!";
        GameManager.Instance.FinalizarJuego(true);  // Detener el juego y mostrar mensaje de victoria
    }

    public void GameOver()
    {
        juegoTerminado = true;
        textoPuntaje.text = "Game Over";
        GameManager.Instance.FinalizarJuego(false);  // Detener el juego y mostrar mensaje de derrota
    }

    public void RestarPuntos()
    {
        puntaje--; // Restar un punto
        ActualizarTextoPuntaje();
        if (puntaje <= 0)
        {
            GameOver(); // Si el puntaje llega a 0, finalizar el juego
        }
    }
}
