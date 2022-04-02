using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagiaGen : MonoBehaviour
{
    public GameObject Magia;
    public Transform PosInicial;
    // Start is called before the first frame update

    Metronomo metronomoScript;
    float tiempoCompas;

    private void Start()
    {
        //Se traen las clases y variables necesarias de otros scripts.
        metronomoScript = FindObjectOfType<Metronomo>();
        tiempoCompas = metronomoScript.tiempoCompas;
    }

    // Función para instanciar magia.
    public void InstanciaMagia()
    {
        StartCoroutine(Espera1Compas()); //Espera un compas tras emitir la nota para la emisión de la magia.
    }

    IEnumerator Espera1Compas()
    {
        yield return new WaitForSecondsRealtime(tiempoCompas); //Espera un compas.
        Instantiate(Magia, PosInicial); //Instancia la magia.
    }

}
