using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TextMeshProUGUI textoGameOver; // Para mostrar "Game Over"
    public TextMeshProUGUI textoTemporizador;

    public float tiempoLimite = 30f;
    private float tiempoRestante;
    private bool juegoTerminado = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        tiempoRestante = tiempoLimite;
        textoGameOver.gameObject.SetActive(false); // Oculta el texto Game Over al inicio
        StartCoroutine(Temporizador());
    }

    private IEnumerator Temporizador()
    {
        while (!juegoTerminado)
        {
            // Temporizador descendente
            while (tiempoRestante > 0)
            {
                tiempoRestante -= Time.deltaTime;
                ActualizarTextoTemporizador();
                yield return null; // Espera un frame antes de continuar
            }

            // Temporizador llegó a 0, reinicia objetos y reinicia el temporizador
            Debug.Log("Temporizador terminó. Reiniciando objetos.");
            ReiniciarObjetos();
            tiempoRestante = tiempoLimite; // Restablecer tiempo
            ActualizarTextoTemporizador();
        }
    }

    private void ActualizarTextoTemporizador()
    {
        if (textoTemporizador != null)
        {
            textoTemporizador.text = "Tiempo: " + Mathf.Ceil(tiempoRestante).ToString();
        }
    }

    public void FinalizarJuego(bool haGanado)
    {
        juegoTerminado = true;
        if (!haGanado && textoGameOver != null)
        {
            textoGameOver.gameObject.SetActive(true);
            textoGameOver.text = "Game Over";
        }
        Time.timeScale = 0; // Detiene el juego
    }

    public void DescuentoPuntos()
    {
        MovimientoJugador jugador = FindObjectOfType<MovimientoJugador>();
        if (jugador != null)
        {
            jugador.RestarPuntos();
        }
        else
        {
            Debug.LogWarning("MovimientoJugador no encontrado en la escena.");
        }
    }

    public void ReiniciarObjetos()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("ObjetoQueHuye"))
        {
            obj.GetComponent<Huir>().ReiniciarPosicion();
        }

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Persiguiendo"))
        {
            obj.GetComponent<Persiguiendo>().ReiniciarPosicion();
        }
    }
}
