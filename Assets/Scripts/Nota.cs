using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Nota : MonoBehaviour
{
    public int notaId;
    public AudioClip sonido;
    private SpriteRenderer notaSprite;
    private Image notaImage;
    //private Color sumaColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

    
    public MotorJuego motorScript;
    public EntradaAudio entradaAudioScript;
    public MagiaGen magiaGenScript;


    private void Start()
    {
        motorScript = FindObjectOfType< MotorJuego >();
        entradaAudioScript = FindObjectOfType< EntradaAudio >();
        magiaGenScript = FindObjectOfType<MagiaGen>();

        //notaSprite = GetComponent<SpriteRenderer>();
        notaImage = GetComponent<Image>();
        notaImage.enabled = true;
    }

    public void ActivaNota()
    {
        AudioSource.PlayClipAtPoint(sonido, Vector3.zero, 1.0f);
        notaImage.color = Color.green;
        magiaGenScript.InstanciaMagia();
        Invoke("DesactivaNota", 0.5f);
    }

    public void ActivaNotaJugador()
    {
        if (motorScript.turnoPc == false)
        {
            motorScript.TurnoJugador(notaId);
        }
        notaImage.color = Color.green;
        Invoke("DesactivaNota", 0.5f);
    }

    public void DesactivaNota()
    {
        notaImage.color = Color.white;
        //notaImage.enabled = false;
    }
    public void DesactivaNotaJugador()
    {
        notaImage.color = Color.white;
        //notaImage.enabled = false;
    }
}
