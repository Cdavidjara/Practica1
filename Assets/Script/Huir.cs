using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Huir : MonoBehaviour
{
    public float velocidad = 5;
    public Transform objetivo;
    public float cambioDireccionIntervalo = 2f; // Intervalo de tiempo para cambiar direcci�n
    public Vector3 limitesMinimos; // Coordenadas m�nimas del plano
    public Vector3 limitesMaximos; // Coordenadas m�ximas del plano
    private Vector3 direccion;

    private float x = 0;
    private float y = 0;
    private float z = 0;

    void Start()
    {
        y = transform.position.y;
        CambiarDireccion(); // Inicia con una direcci�n aleatoria
        StartCoroutine(CambiarDireccionPeriodicamente());
    }

    void Update()
    {
        x = transform.position.x;
        z = transform.position.z;

        // Mover en la direcci�n actual
        x += direccion.x * velocidad * Time.deltaTime;
        z += direccion.z * velocidad * Time.deltaTime;

        // Limitar el movimiento dentro de los l�mites del plano
        x = Mathf.Clamp(x, limitesMinimos.x, limitesMaximos.x);
        z = Mathf.Clamp(z, limitesMinimos.z, limitesMaximos.z);

        transform.position = new Vector3(x, y, z);
    }

    void CambiarDireccion()
    {
        // Cambia la direcci�n aleatoriamente
        direccion = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    }

    IEnumerator CambiarDireccionPeriodicamente()
    {
        while (true)
        {
            yield return new WaitForSeconds(cambioDireccionIntervalo);
            CambiarDireccion();
        }
    }

    public void ReiniciarPosicion()
    {
        // Reiniciar la posici�n en una posici�n aleatoria dentro de los l�mites
        float x = Random.Range(limitesMinimos.x, limitesMaximos.x);
        float z = Random.Range(limitesMinimos.z, limitesMaximos.z);
        transform.position = new Vector3(x, y, z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Persiguiendo"))
        {
            GameManager.Instance.DescuentoPuntos(); // Descontar puntos si un objeto que persigue toca al objeto que huye
        }
    }
}
