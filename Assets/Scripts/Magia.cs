using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Magia : MonoBehaviour
{
    
    public Metronomo metronomoScript;
    // Start is called before the first frame update    
    float tiempoCompas;
    float tiempoLlegada;
    float tiempoIntervalo;
    float tiempoParpadeo;

    Material colorMagia;

    Color colorRojo;
    Color colorVerde;
    Color transparente;

    bool cambiaColor;
    bool empiezaParpadeo;

    void Start()
    {
        metronomoScript = FindObjectOfType<Metronomo>();
        colorMagia = GetComponent<MeshRenderer>().material;

        tiempoCompas = metronomoScript.tiempoCompas;
        tiempoIntervalo = metronomoScript.tiempoIntervalo;
        tiempoLlegada = tiempoCompas + tiempoIntervalo;

        tiempoParpadeo = 0.1f;
        colorVerde = new Color(0f, 1f, 0f, 1f);
        colorRojo = new Color(1f, 0f, 0f, 1f);
        transparente = new Color(1f, 0f, 0f, 0f);

        empiezaParpadeo = true;
        colorMagia.color = colorVerde;

        // Se etiqueta el la instancia lanzada.
        this.gameObject.tag = "Magia";

        

        // Se utiliza la librería LeanTween paralanzar la magia hacia el jugador, y cambiar de color,
        // en el tiempo determinado basado en un compas completo sincronizadamente.
        LeanTween.moveX(gameObject, -2.5f, tiempoLlegada);
    }

    private void Update()
    {
        if (compruebaPosicion() <= -1.5f && empiezaParpadeo == true)
        {
            StartCoroutine(Parpadea());
            empiezaParpadeo = false;
        }
    }

    float compruebaPosicion()
    {
        float posicion;
        posicion = this.gameObject.transform.position.x;
        return posicion;
    }

    IEnumerator Parpadea()
    {
        yield return new WaitForSeconds(tiempoParpadeo);

        if (cambiaColor)
        {
            print("rojo");            
            colorMagia.color = colorRojo;
            cambiaColor = false;
        }
        else
        {
            print("transparente");
            colorMagia.color = transparente;
            cambiaColor = true;
        }
        StartCoroutine(Parpadea());
    }
}
