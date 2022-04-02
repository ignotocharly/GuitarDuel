using UnityEngine;
using UnityEngine.UI; 
using System.Collections.Generic; 

[RequireComponent(typeof(AudioSource))]
public class EntradaAudio : MonoBehaviour {

	public float umbralDeteccion = 0.007f;
	public float frecuencia = 0.0f;
	public int audioSampleRate;
	public string entradaAudio;

	public FFTWindow fftWindow;
	public Dropdown entradaDropdown;
	public Slider umbralSlider;

	private List<string> options = new List<string>();
	public int samples; 
	public AudioSource audioSource;
	private float frecuenciaFundamental = 0.0f;

	public Text textoNota;

	public GameObject notaGO;
	public Nota[] notaScript;

	
	void Start() {
		//Inciampos los valores y componentes que necesitamos.
		audioSource = GetComponent<AudioSource>();
		umbralDeteccion = 0.01f; // Se fija un umbral de deteccion para las notas.
		audioSampleRate = 48000; // 48kHz de sampleo de la señal permite analizar frecuencias de hasta 24kHz.
		samples = 1024*2; // Cantidad de puntos de sampleo tomados para el analisis espectral.
		

		// Se muestran por consola todos los dispositivos de entrada de audio. 
		foreach (var dispositivo in Microphone.devices)
		{
			print("Name: " + dispositivo);
		}

		// Se obtienen todos los dispositivos de entrada de audio del PC.
		// Por defecto se activa el primero en encontrarse.
		foreach (string dispositivo in Microphone.devices) {
			if (entradaAudio == null) {
				entradaAudio = dispositivo;
			}
			options.Add(dispositivo);
		}
		entradaAudio = options[PrefJugadorManager.ObtenEntradaAudio ()];
		umbralDeteccion = PrefJugadorManager.ObtenUmbral ();

		//Se añaden los dispositivos de entrada de audio a la lista desplegable y se conecta con el resultado.
		entradaDropdown.AddOptions(options);
		entradaDropdown.onValueChanged.AddListener(delegate {
			ControladorEntradaAudio(entradaDropdown);
		});

		//Se conecta el slider con el valor de umbral.
		umbralSlider.onValueChanged.AddListener(delegate {
			ControladorUmbral(umbralSlider);
		});

		//Se actualiza la entrada de audio.
		ActualizaEntradaAudio();
	}

	//Función para actualizar el dispositivo de audio.
	public void ActualizaEntradaAudio(){
		
		audioSource.Stop(); 
		//Se utiliza la entrada de audio como clip de la fuente de audio.
		audioSource.clip = Microphone.Start(entradaAudio, true, 1, audioSampleRate);
		audioSource.loop = true; 
		
		Debug.Log(Microphone.IsRecording(entradaAudio).ToString());

		//Se comprueba se la entrada de audio está grabando y se espera hasta que lo esté.
		if (Microphone.IsRecording (entradaAudio)) { 
			while (!(Microphone.GetPosition (entradaAudio) > 0)) {
			} 
			//Se comienza la reproducción.
			audioSource.Play (); 
		} else {
			//Si no hay entrada de audio funcionando se dice por consola.
			Debug.Log (entradaAudio + "La entrada de audio no funciona");
		}
	}

	//Seleccion la entrada de audio y se llama a actualizar.
	public void ControladorEntradaAudio(Dropdown mic){
		entradaAudio = options[mic.value];
		ActualizaEntradaAudio();
	}

	//Se marca el valor del umbral mediante el slider.
	public void ControladorUmbral(Slider umbralSlider){
		umbralDeteccion = umbralSlider.value;
	}
	
	//Función para calcular el volumen medio de la señal de audio obteniendo la media de las mustras tomadas.
	public float CalcularVolumenMedio()
	{ 
		float[] data = new float[128];
		float a = 0;
		audioSource.GetOutputData(data,0);
		foreach(float s in data)
		{
			a += Mathf.Abs(s);
		}
		return a/256;
	}
	/*
	//función que comprueba si hay aduio en la entrada. Actualmente no se utiliza.
	public bool MicroSuena()
    {
		float volumenMedioActual = CalcularVolumenMedio();
		if (volumenMedioActual > umbralDeteccion)
		{
			return true;
	    }
		else 
		{
			return false;
		}
	}*/

	// Con esta función se obtiene la frecuencia fundamental de la nota capturada.
	// Se obtienen una serie de muestras, mediante un analisis de espectro basado en el
	// algoritmo  de Transformación de Fourier.

float SacarFrecuenciaFundamental()
	{
		
		float[] data = new float[samples];
		audioSource.GetSpectrumData(data,0,fftWindow);
		float s = 0.0f;
		int i = 0;
		for (int j = 1; j < samples; j++)
		{
			// Se guardan en un array y se compara muestra a muestra si su valor supera el umbral
			// de detección.
			if (data[j] > umbralDeteccion) // volumn must meet minimum umbral
			{
				// Tras ello se compara la frecuenca y como resultado se obtiene la frecuencia con mayor amplitud.
				if ( s < data[j] )
				{
					s = data[j];
					i = j;
				}
				
			}
		}
		// Para saber cual es la frecuencia fundamental hace falta adecuar el valor obtenido a la frecuencia de muestreo, 48kHz
		// y el numero de samples tomados.
		frecuenciaFundamental = i * audioSampleRate / samples;
		return frecuenciaFundamental;
	}

	// Una vez obtenida la frecuencia fundamental se obtiene la nota comparándola utilizando
	// una tabla de conversion Frecuencia <=> nota.
	public void FrecNota()
    {
		int freq = (int)SacarFrecuenciaFundamental();
		int numNota;
		
		// Se compara si la frecuencia esta dentro del rango y se lanza la nota correspondiente,
		// para comaprar con el listado de notas del PC y para visualizarla en el mastil virtual.
		if (freq > 513 && freq < 533)
		{
			textoNota.text = "C5";
			numNota = 0;
			notaScript[numNota].ActivaNotaJugador();
        }		
		else if(freq >= 577 && freq <= 597)
		{
			textoNota.text = "D5";
			numNota = 1;
			notaScript[numNota].ActivaNotaJugador();
		}	
		else if(freq >= 649 && freq <= 669)
		{
			textoNota.text = "E5";
			numNota = 2;
			notaScript[numNota].ActivaNotaJugador();
		}		
		else if(freq >= 689 && freq <= 710)
		{
			textoNota.text = "F5";
			numNota = 3;
			notaScript[numNota].ActivaNotaJugador();
		}
		else if(freq >= 773 && freq <= 794)
		{
			textoNota.text = "G5";
			numNota = 4;
			notaScript[numNota].ActivaNotaJugador();
		}
		else if(freq >= 870 && freq <= 890)
		{
			textoNota.text = "A5";
			numNota = 5;
			notaScript[numNota].ActivaNotaJugador();
		}
		else if(freq >= 977 && freq <= 997)
		{
			textoNota.text = "B5";
			numNota = 6;
			notaScript[numNota].ActivaNotaJugador();
		}
		else if(freq > 1036 && freq < 1056)
		{
			textoNota.text = "C6";
			numNota = 7;
			notaScript[numNota].ActivaNotaJugador();
        }		
		// Si la nota no se encuentra en la lista no se activa nada en el mástil de reperesentación.
		else
		{
			numNota = 8;
			textoNota.text = "";
		}
	}

}