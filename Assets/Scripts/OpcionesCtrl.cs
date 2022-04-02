using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OpcionesCtrl : MonoBehaviour {

	public Dropdown entradaAudio; //Se define una variable lista despelgable para elegir entrada;
	public Slider umbralSlider; // Se definen sliders para la sensibilidad y el umbral de deteccion;
	public GameObject settingsPanel; // Se define el GO de panel de preferencias;
	public GameObject openButton; // Se define GO para el boton de abrir panel.

	private bool panelActivo = false; // Boleana que indicara si está abierto el panel de preferencias.

	// Use this for initialization
	void Start () {
		//Llamadas para traer el puerto de entrada de guitarra y lso valores de sensibilidad y umbral.
		entradaAudio.value = PrefJugadorManager.ObtenEntradaAudio (); 
		umbralSlider.value = PrefJugadorManager.ObtenUmbral ();
	}
	
	//Metodo para guardar las preferencias;
	public void Guardar (){
		PrefJugadorManager.SetEntradaAudio (entradaAudio.value);
		PrefJugadorManager.SetUmbral (umbralSlider.value);

		panelActivo = !panelActivo;
		settingsPanel.GetComponent<Animator> ().SetBool ("PanelActive",panelActivo);
	}

	//Metodo que marca los valores por defecto. 
	public void SetDefaults(){
		entradaAudio.value = 0;
		umbralSlider.value = 0.007f;
	}


	public void AbrirPreferencias(){
		panelActivo = !panelActivo;
		settingsPanel.GetComponent<Animator> ().SetBool ("PanelActive",panelActivo);
	}

	public void SacarPanelpreferencias(){
		
		if (!panelActivo) {
			AbrirPreferencias ();
		} else {
			Guardar ();
		}
	}
}
