using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ControlEscenas : MonoBehaviour
{
    public Image fundido;
    public string[] escenas;

    // Start is called before the first frame update
    void Start()
    {
        // Se realiza un fundido de medio segundo.
        fundido.CrossFadeAlpha(0, 0.5f, false);
    }

    // Se realiza un fundido de salida y se inicia el cambio entre escenas.
    public void FadeOut(int s)
    {
        fundido.CrossFadeAlpha(1, 0.5f, false);
        StartCoroutine(CambioEscena(escenas[s]));
    }

    // Se realiza el cambio de escena tras un segundo de espera.
    IEnumerator CambioEscena( string escena)
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(escena);
    }

}
