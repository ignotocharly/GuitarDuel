using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Metronomo : MonoBehaviour
{
    
    public MotorJuego motorScript;
    public AudioSource clickMetronomo;

    //Se crean las variables necesarias.
    public int bpm = 100; //Se fija el tempo de la canción. Podría variarse según dificultad.
    public Text txtMetronomo; //Texto en pantalla que muestra los tiempos del compás.
    public int beat1; 
    public bool beat1Check; //Bandera que se activa en el primer tiempo del compás. 
    public float tiempoIntervalo; //Almacenará el tiempo entre negras del compás.
    public float beatTiempo; 
    public float tiempo; 
    public float tiempoCompas;

    bool metronomoCuenta;


    // Start is called before the first frame update
    void Start()
    {
        //Se traen las clases y objetos.
        motorScript = FindObjectOfType<MotorJuego>();
        clickMetronomo = this.gameObject.GetComponent<AudioSource>();


        tiempoIntervalo = 60f / bpm; // Se calcula el espacio de tiempo entre notas.
        tiempoCompas = tiempoIntervalo * 4f; // Se calcula la duración de un compás.
        
        // Empieza el juego.
        motorScript.StartJuego();

    }

    // Update is called once per frame
    void Update()
    {
        // Si se indica se actualiza el metronomo.
        if(metronomoCuenta) MetronomoCuenta();
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
                clickMetronomo.Play();
            }
            else
            {
                beat1 = 1;
                beat1Check = true;
                clickMetronomo.Play();
            }

            txtMetronomo.text = (beat1).ToString();
        }   
    }

    public bool IsBeat1()
    {
        return beat1Check;
    }

    public void MetronomoStart()
    {
        metronomoCuenta = true;
    }

    public void MetronomoStop()
    {
        metronomoCuenta = false;
    }
}
