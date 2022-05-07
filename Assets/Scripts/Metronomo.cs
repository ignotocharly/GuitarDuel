using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Metronomo : MonoBehaviour
{
    
    public MotorJuego motorScript;
    public AudioSource clickMetronomo;

    public AudioSource audioIntro;
    public AudioSource audioLoop;

    //Se crean las variables necesarias.
    public int bpm = 100; //Se fija el tempo de la canci�n. Podr�a variarse seg�n dificultad.
    public Text txtMetronomo; //Texto en pantalla que muestra los tiempos del comp�s.
    public int beat1; 
    public bool beat1Check; //Bandera que se activa en el primer tiempo del comp�s. 
    public float tiempoIntervalo; //Almacenar� el tiempo entre negras del comp�s.
    public float beatTiempo; 
    public float tiempo; 
    public float tiempoCompas;
    public bool playIntro;


    public bool metronomoCuenta;


    // Start is called before the first frame update
    void Start()
    {
        //Se traen las clases y objetos.
        motorScript = FindObjectOfType<MotorJuego>();
        clickMetronomo = this.gameObject.GetComponent<AudioSource>();

        metronomoCuenta = false;

        playIntro = true;       


        tiempoIntervalo = 60f / bpm; // Se calcula el espacio de tiempo entre notas.
        tiempoCompas = tiempoIntervalo * 4f; // Se calcula la duraci�n de un comp�s.
        
        // Empieza el juego.
        motorScript.StartJuego();        
        StartCoroutine(audioLoopPlay());
        //StartCoroutine(audioIntroPlay());


    }

    // Update is called once per frame
    void Update()
    {
        // Si se indica se actualiza el metronomo.
        
        if (metronomoCuenta) MetronomoCuenta();
        //MetronomoStart();
        // Actualiza la variable de tiempo.
        tiempo += Time.deltaTime;
    }


    void MetronomoCuenta()
    {

        beatTiempo += Time.deltaTime;
        // beatTiempo = 1
        if (beatTiempo >= tiempoIntervalo)
        //
        {
            beatTiempo -= tiempoIntervalo;
            if (beat1 != 4) {
                beat1++;
                beat1Check = false;
                //clickMetronomo.Play();
                if(playIntro) audioIntro.Play();
                playIntro = false;                
            }

            else
            {
                beat1 = 1;
                beat1Check = true;
                //clickMetronomo.Play();            
            }
            txtMetronomo.text = (beat1).ToString();
        }   
    }

    public bool IsBeat1()
    {
        return beat1Check;
    }

    public bool MetronomoStart()
    {
        return metronomoCuenta = true;
    }

    public bool MetronomoStop()
    {
        return metronomoCuenta = false;
    }



    IEnumerator audioLoopPlay()
    {
        yield return new WaitUntil(IsBeat1);
        audioLoop.Play();
        //playLoop = false;
    }
    /*
    IEnumerator audioIntroPlay()
    {
        yield return new WaitUntil(() => metronomoCuenta == true);
        audioIntro.Play();
    }*/
}
