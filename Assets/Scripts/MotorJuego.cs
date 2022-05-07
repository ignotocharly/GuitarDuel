using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MotorJuego : MonoBehaviour
{
    public Nota[] notas; //Almacenará las notas disponibles.
    public List<int> ListaAleatoria = new List<int>(); //En esta lista se almacenan las notas aleatorias a emitir por el PC.

    public bool listaLlena; //Bandera de lista llena.   
    public bool turnoPc; //Bandera que indica cuando le toca al PC lanzar notas.
    public bool notaOk; // Bandera que indica si la nota es correcta.
    public bool ExisteMagia; //Bandera que indica si existen magias aún en la escena.


    public int contador; //Contador que indica la nota a comparar de la lista del PC.
    public int contadorUsuario; // Contador qeu indica que nota del jugador toca comparar.
    public int nivelActual; //Indicará el nivel de partida.

    public Text GameOverText;
    public Text Nivel;

    // Se traen los paneles que activar o desactivar.
    public GameObject panelGameOver;
    public GameObject PanelAciertaGO;

    // Conexión con los scripts/clases necesarias del juego.
    EntradaAudio entradaAudioScript;
    Metronomo metronomoScript;
    Magia Magiascript;
    MagiaGen magiaGenScript;
    Jugador jugadorScript;

    // Variables para el metronomo.
    float tiempoCompas;
    float tiempoIntervalo;    
    public bool beat1Check;

    public GameObject[] jugadorPcGO;

    Animator PcAnima;


    // Metodo que llena la lista de notas aleatorias.
    void LlenaLista()
    {
        for(int i =0; i<=20; i++)
        {
            ListaAleatoria.Add(Random.Range(0,8));
            listaLlena = true;
        }
    }

    // Metodo que gestiona el turno del PC.
    public void TurnoPc()
    {
        if (listaLlena && turnoPc)
        {
            // Lanza la nota actual de la lista.
            if (contador > 0) {
                notas[ListaAleatoria[contador - 1]].DesactivaSonidoNota();
            }
            
            notas[ListaAleatoria[contador]].ActivaNota();
            
            StartCoroutine("TocaGuitarra");

            // Si se han lanzado todas las notas correspondientes al array de la lista actual se cambia el turno al jugador.
            if (contador >= nivelActual)
            {
                CambiarTurno();
            }
            else
            {
                // Se apunta a la siguiente nota del array de notas almacenado en la lista.
                contador++;
            }
            // Se utiliza un valor aleatorio para cambiar la cadencia entre notas y sea más divertrido jugando con la ritmica.
            Invoke("TurnoPc", tiempoIntervalo * IntervaloTiempoAleatorio());
        }
    }

    // Gestinoa el turno del Jugador.
    public void TurnoJugador(int notaId)
    {
        // Si la nota tocada por el jugador es igual a la nota actual de la lista se activa la bandera notaOK.
        // Si no se sale de la funcion.
        if (notaId != ListaAleatoria[contadorUsuario])
        {            
            return;
        }
        if (contadorUsuario == contador)
        {
            notaOk = true;
            //Se deja de analizar la entrada de audio.
            jugadorScript.AnalizaNota(false);
            // Se indica que las notas se han acertado.
            PanelAciertaGO.SetActive(true);
        }
        else
        {
            notaOk = true;
            //Se deja de analizar la entrada de audio.
            jugadorScript.AnalizaNota(false);
            // Se aumenta el contador para comparar la siguiente nota.
            contadorUsuario++;
        }
    }

    // Funcion para cambiar el turno entre jugador y PC.
    public void CambiarTurno()
    {   
        
        // Si es el turno del PC se cambia al jugador y viceversa.
        if (turnoPc)
        {
            nivelActual++; // Se aumenta al nivel siguiente para lanzar más notas;
            turnoPc = false;
            notaOk = false; // Resetea la bandera.
        }
        else
        {
            // Si aún quedan "magias" en la escena es que el turno del Jugador todavía no ha finalizado.
            if (!ExisteMagia)
            {
                ResetTurnoPc();
            }
            else
            {
                return;
            }
            
        }
    }

    // Espera al primer beat del siguiente compás para empezar el turno del PC. Así se sincronizan las notas.
    IEnumerator EsperaBeat1()
    {        
        yield return new WaitForSeconds(3);
        PanelAciertaGO.SetActive(false);
        yield return new WaitUntil(() => metronomoScript.IsBeat1());
        TurnoPc();
    }

    // Funcion para cambiar la cadencia de notas entre negras, blancas y corcheas.
    public float IntervaloTiempoAleatorio()
    {
        float num = Random.Range(1f, 2f);
        float[] listado;
        listado = new float[3];
        listado[0] = 0.5f; // Corcheas (De momento no las activo)
        listado[1] = 1; // Negras
        listado[2] = 2; // Blancas

        print("Radnom=" + Mathf.RoundToInt(num));
        return listado[Mathf.RoundToInt(num)];
    }

    // Funcion para controlar la finalización de la partida.
    // Si el jugador gana lanza un cartel de ganador, de lo contrario de perdedor.
    // Ademas permite volver a empezar o volver al menú principal.
    public void GameOver(bool ganaJugador)
    {

        entradaAudioScript.audioSource.Stop(); // Para la entrada de audio.
        jugadorScript.analizaNota = false;
        metronomoScript.MetronomoStop(); // Para el metronomo.
        

        
        // Se destruyen todas las magias aun existentes ene escena.
        GameObject[] magias = GameObject.FindGameObjectsWithTag("Magia");
        print(magias);
        foreach(GameObject magia in magias)
        {
            Destroy(magia);
            print(magias);
        }

        
        // Cartel ganador.
        if (ganaJugador)
        {
            GameOverText.text = "Has ganado";
            GameOverText.color = Color.green;
        }
        // Cartel perdedor.
        else
        {
            GameOverText.text = "Has perdido amigo";
            GameOverText.color = Color.red;
        }

        // Activa el cartel
        panelGameOver.gameObject.SetActive(true);
        turnoPc = false; // Apaga la bandera del turno PC.
        listaLlena = false; 
        
    }

    // Funcion de reinicio del juego.
    public void Reiniciar()
    {
        panelGameOver.gameObject.SetActive(false);
        contador = 0;
        contadorUsuario = 0;
    }

    // Fucnion para empezar el juego.
    public void StartJuego()
    {
        tiempoIntervalo = metronomoScript.tiempoIntervalo;
        tiempoCompas = metronomoScript.tiempoCompas;

        panelGameOver.gameObject.SetActive(false); // Desactiva el panel de Game Over.

        // Reseteo de vidas.
        jugadorScript.vidaJugador = 100;
        jugadorScript.vidaPc = 100;

        //Nivel.text = "Nivel:" + nivelActual;

        
        metronomoScript.MetronomoStart(); // Inicia el metronomo.
                        
        LlenaLista(); // Llena la lista de notas.
        turnoPc = true; // Empieza el PC.
        
        // Se sincroniza con el primer beat del siguiente compas. 
        Invoke("TurnoPc", (tiempoCompas + tiempoIntervalo));
    }

    // Devuelve la bandera de notaok.
    public bool IsNotaOk()
    {
        return notaOk;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Se traen las clases necesarias.
        entradaAudioScript = FindObjectOfType<EntradaAudio>();        
        metronomoScript = FindObjectOfType<Metronomo>();
        jugadorScript = FindObjectOfType<Jugador>();
        magiaGenScript = FindObjectOfType<MagiaGen>();

        jugadorPcGO = GameObject.FindGameObjectsWithTag("JugadorPc");
        PcAnima = jugadorPcGO[0].GetComponent<Animator>();

    }
    
    // Update is called once per frame
    void Update()
    {
        // Se chequea el primer beat del compas.
        beat1Check = metronomoScript.IsBeat1();
        
    }

    // Se resetean los valores para el Turno del PC.
    public void ResetTurnoPc()
    {      
        //Sincroniza con el primer beat del compas.
        StartCoroutine(EsperaBeat1());
        turnoPc = true;
        contador = 0;
        contadorUsuario = 0;
    }


    // Es necesario chequear las magias de escena cuando acabe el frame 
    // y se hayan destruido los objetos correspondientes.
    public IEnumerator EncuentraMagiaEnumerator()
    {
        print("enumerator");
        yield return new WaitForEndOfFrame();

        // Si aún quedan magias en la escena se activa la bandera.
        // De lo contrario se desactiva y cambia el turno al PC.
        if (GameObject.FindGameObjectsWithTag("Magia").Length >= 1)
        {
            print("Esto es = " + GameObject.FindGameObjectsWithTag("Magia").Length);
            ExisteMagia = true;
        }
        else
        {
            print("Esto es = " + GameObject.FindGameObjectsWithTag("Magia").Length);
            ExisteMagia = false;
            magiaGenScript.DestroyClones();
            CambiarTurno();
        }

    }
    

    IEnumerator TocaGuitarra()
    {
        PcAnima.SetFloat("PLAY", 1f);
        yield return new WaitForSeconds(0.2f);
        PcAnima.SetFloat("PLAY", 0f);
    }
}
