using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Magia : MonoBehaviour
{
    
    public Metronomo metronomoScript;
    public MagiaParticula particulaScript;

    public ParticleSystem particulas;

    public Material particulasMat;

    GameObject RefMatAzulGO;
    GameObject RefMatRojoGO;

    Material colorMagia;

    public Material materialRojo;
    public Material materialAzul;

    // Start is called before the first frame update    
    float tiempoCompas;
    float tiempoLlegada;
    float tiempoIntervalo;
    float tiempoParpadeo;


    Color colorRojo;
    Color colorVerde;
    Color colorAzul;
    Color transparente;

    bool cambiaColor;
    bool empiezaParpadeo;

    

    void Start()
    {
        metronomoScript = FindObjectOfType<Metronomo>();
        colorMagia = GetComponent<MeshRenderer>().material;

        particulaScript= FindObjectOfType<MagiaParticula>();

        particulas = GetComponentInChildren<ParticleSystem>();
        particulasMat = particulas.GetComponent<ParticleSystemRenderer>().trailMaterial;

        RefMatRojoGO = GameObject.Find("RefMatRojo");
        RefMatAzulGO = GameObject.Find("RefMatAzul");

        materialRojo = RefMatRojoGO.GetComponent<Renderer>().material;
        materialAzul = RefMatAzulGO.GetComponent<Renderer>().material;

        SetMaterialAzul();

        tiempoCompas = metronomoScript.tiempoCompas;
        tiempoIntervalo = metronomoScript.tiempoIntervalo;
        tiempoLlegada = tiempoCompas + tiempoIntervalo;

        tiempoParpadeo = 0.1f;
        colorVerde = new Color(0f, 1f, 0f, 1f);
        colorAzul = new Color(0f, 0f, 1f, 1f);

        colorRojo = new Color(1f, 0f, 0f, 1f);
        transparente = new Color(1f, 0f, 0f, 0f);

        empiezaParpadeo = true;
        colorMagia.color = colorAzul;

        // Se etiqueta el la instancia lanzada.
        this.gameObject.tag = "Magia";
        

        // Se utiliza la librería LeanTween paralanzar la magia hacia el jugador, y cambiar de color,
        // en el tiempo determinado basado en un compas completo sincronizadamente.
        LeanTween.moveX(gameObject, -2.5f, tiempoLlegada);
    }

    private void Update()
    {
        if (compruebaPosicion() <= -0.5f && empiezaParpadeo == true)
        {
            SetMaterialRojo();
            particulasMat.color = colorRojo;
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
            particulasMat.color = colorRojo;

            //particulaScript.SetParticulaColor(colorRojo);
            cambiaColor = false;
        }
        else
        {
            print("transparente");
            //particulaScript.SetParticulaColor(transparente);
            colorMagia.color = transparente;
            particulasMat.color = transparente;
            cambiaColor = true;
        }
        StartCoroutine(Parpadea());
    }

    void SetMaterialRojo()
    {
        particulas.GetComponent<ParticleSystemRenderer>().trailMaterial = materialRojo;
        particulasMat = particulas.GetComponent<ParticleSystemRenderer>().trailMaterial;
    }
    void SetMaterialAzul()
    {
        particulas.GetComponent<ParticleSystemRenderer>().trailMaterial = materialAzul;
        particulasMat = particulas.GetComponent<ParticleSystemRenderer>().trailMaterial;
    }
   
}

