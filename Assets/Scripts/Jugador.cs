using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Jugador : MonoBehaviour
{

    EntradaAudio entradaAudioScript;
    MotorJuego motorJuegoScript;

    MagiaGen magiaGenScript;

    public GameObject gameObjectPadre;
    public GameObject VidaJugadorGO;

    public AudioSource sonidoImpacto;

    public Scrollbar vidaJugadorBarra;

    public GameObject VidaPcGO;
    public Scrollbar vidaPcBarra;


    public Nota[] notaScript;

    public bool analizaNota;
   
    public float vidaJugador;
    public int vidaPc;

    public Text txtVidaJugador;
    public Text txtVidaPc;

    public Animator anima;
    public float tocaGuitarra;

    // Start is called before the first frame update
    void Start()
    {
        //Se traen las clases necesarias.
        entradaAudioScript = FindObjectOfType<EntradaAudio>();
        motorJuegoScript = FindObjectOfType<MotorJuego>();
        magiaGenScript = FindObjectOfType<MagiaGen>(); ;

        sonidoImpacto = GetComponent<AudioSource>();

        VidaJugadorGO = GameObject.Find("VidaJugador");
        vidaJugadorBarra = VidaJugadorGO.GetComponent<Scrollbar>();

        VidaPcGO = GameObject.Find("VidaPC");
        vidaPcBarra = VidaPcGO.GetComponent<Scrollbar>();

        anima = GetComponent<Animator>();
        tocaGuitarra = 0;

    }
    
    // Update is called once per frame
    void Update()
    {
        //Si la bandera está activa, cuando la magia choca con el collider del jugador, se llama a actualizar la nota.
        if(analizaNota) entradaAudioScript.FrecNota();
        //También es posible usar las teclas.
        if (Input.GetKeyDown(KeyCode.D))
        {
            //notaScript[1].ActivaNotaJugador();
            StartCoroutine("TocaGuitarra");
            print("probando");
            
        }
        
        if (entradaAudioScript.MicroSuena())
        {
            StartCoroutine("TocaGuitarra");
        }
    }

    //Cuando la magia entra en el collider se analiza la nota.
    private void OnTriggerStay(Collider obj)
    {        
        //Activa la bandera
        analizaNota = true;

        gameObjectPadre = obj.gameObject.transform.parent.gameObject;

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
        analizaNota = false;  //Desactiva la bandera de análisis de nota.    

        sonidoImpacto.Play();
       

        Destroy(gameObjectPadre); //Se destruye el objeto.        
        
        RestaVidaJugador(); //Resta vida al jugador.
        StartCoroutine(motorJuegoScript.EncuentraMagiaEnumerator());   //Comprueba si quedan magias en escena. 
        magiaGenScript.InstanciaMagiaExplosion();
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
        vidaJugador -= 10f;
        vidaJugadorBarra.size -= 0.1f;
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
        vidaPcBarra.size -= 0.1f;
        txtVidaPc.text = vidaPc.ToString();
        if(vidaPc <= 0)
        {
            motorJuegoScript.GameOver(true);
        }
    }


    IEnumerator TocaGuitarra()
    {
        anima.SetFloat("PLAY", 1f);
        yield return new WaitForSeconds(0.2f);
        anima.SetFloat("PLAY", 0f);
    }
}
