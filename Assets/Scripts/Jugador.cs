using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jugador : MonoBehaviour
{

    EntradaAudio entradaAudioScript;
    MotorJuego motorJuegoScript;
    public Nota[] notaScript;

    public bool analizaNota;
   
    public int vidaJugador;
    public int vidaPc;

    public Text txtVidaJugador;
    public Text txtVidaPc;

    // Start is called before the first frame update
    void Start()
    {
        //Se traen las clases necesarias.
        entradaAudioScript = FindObjectOfType<EntradaAudio>();
        motorJuegoScript = FindObjectOfType<MotorJuego>();
    }

    // Update is called once per frame
    void Update()
    {
        //Si la bandera está activa, cuando la magia choca con el collider del jugador, se llama a actualizar la nota.
        if(analizaNota) entradaAudioScript.FrecNota();
        //También es posible usar las teclas.
        if (Input.GetKeyDown(KeyCode.D) && analizaNota){
            notaScript[1].ActivaNotaJugador();
        }
    }

    //Cuando la magia entra en el collider se analiza la nota.
    private void OnTriggerStay(Collider obj)
    {        
        //Activa la bandera
        analizaNota = true;

        //Se comprieba si la nota tocada por el jugador coincide con la nota actual del PC.
        if (motorJuegoScript.IsNotaOk())
        {
            motorJuegoScript.notaOk = false;  //Desactiva la bandera de notaOK. (Puesto que ya se ha chequeado)
            analizaNota = false; //Desactiba la bandera de análisis de nota.
            Destroy(obj.gameObject); // Se destruye el objeto.
            RestaVidaPc(); //Resta vida al PC.
            StartCoroutine(motorJuegoScript.EncuentraMagiaEnumerator()); //Comprueba si quedan magias en escena.
        }
    }

    private void OnTriggerExit(Collider obj)
    {
    
        analizaNota = false;  //Desactiba la bandera de análisis de nota.      
        Destroy(obj.gameObject); //Se destruye el objeto.
        RestaVidaJugador(); //Resta vida al jugador.
        StartCoroutine(motorJuegoScript.EncuentraMagiaEnumerator());   //Comprueba si quedan magias en escena.    

    }

    // Actualiza el valor de la nota.
    public void AnalizaNota(bool ok)
    {
        analizaNota = true;
    }

    //Resta vida al jugador y actualiza el texto en pantalla.
    // Si la vida se acaba el juego termina.
    void RestaVidaJugador()
    {
        vidaJugador -= 10;
        txtVidaJugador.text = vidaJugador.ToString();
        if (vidaJugador <= 0)
        {
            motorJuegoScript.GameOver(false);
        }
    }

    //Resta vida al PC y actualiza el texto en pantalla.
    // Si la vida se acaba el juego termina.
    void RestaVidaPc()
    {
        vidaPc -= 10;
        txtVidaPc.text = vidaPc.ToString();
        if(vidaPc <= 0)
        {
            motorJuegoScript.GameOver(true);
        }
    }
}
