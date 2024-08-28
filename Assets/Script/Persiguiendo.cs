using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persiguiendo : MonoBehaviour
{
    public float velocidad = 5;
    public Transform objetivo;
    public float distanciaMinimaEntrePersonajes = 1.5f; // Distancia mínima entre personajes

    private float x = 0;
    private float y = 0;
    private float z = 0;

    void Start()
    {
        y = transform.position.y;
    }

    void Update()
    {
        x = transform.position.x;
        z = transform.position.z;

        // Lógica de persecución
        if (objetivo.position.x > x)
        {
            x += velocidad * Time.deltaTime;
        }
        if (objetivo.position.x < x)
        {
            x -= velocidad * Time.deltaTime;
        }
        if (objetivo.position.z > z)
        {
            z += velocidad * Time.deltaTime;
        }
        if (objetivo.position.z < z)
        {
            z -= velocidad * Time.deltaTime;
        }

        // Obtener todos los personajes con el mismo script para calcular la distancia
        Persiguiendo[] personajes = FindObjectsOfType<Persiguiendo>();

        foreach (Persiguiendo personaje in personajes)
        {
            if (personaje != this) // Evitar calcular la distancia con uno mismo
            {
                float distancia = Vector3.Distance(transform.position, personaje.transform.position);

                if (distancia < distanciaMinimaEntrePersonajes)
                {
                    // Repulsión para evitar que se superpongan
                    Vector3 direccionDeSeparacion = (transform.position - personaje.transform.position).normalized;
                    x += direccionDeSeparacion.x * velocidad * Time.deltaTime;
                    z += direccionDeSeparacion.z * velocidad * Time.deltaTime;
                }
            }
        }

        // Actualizar la posición del personaje
        transform.position = new Vector3(x, y, z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Jugador"))
        {
            // Notificar al juego que se ha acabado
            GameManager.Instance.FinalizarJuego(false); // Cambiado a FinalizarJuego
        }
    }

    public void ReiniciarPosicion()
    {
        // Reiniciar la posición en una posición aleatoria dentro de los límites
        float x = Random.Range(-5f, 5f);  // Ajusta los límites según tu plano
        float z = Random.Range(-5f, 5f);  // Ajusta los límites según tu plano
        transform.position = new Vector3(x, y, z);
    }
}
