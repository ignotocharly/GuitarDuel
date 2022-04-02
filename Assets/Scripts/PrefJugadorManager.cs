using UnityEngine;
using System.Collections;

public class PrefJugadorManager : MonoBehaviour {

	const string MICROPHONE_KEY = "entradaAudio";
	const string THRESHOLD_KEY = "umbral";

	public static void SetEntradaAudio (int mic) {
		PlayerPrefs.SetInt (MICROPHONE_KEY, mic);
	}

	public static int ObtenEntradaAudio (){
		return PlayerPrefs.GetInt (MICROPHONE_KEY);
	}


	public static void SetUmbral (float umbral) {
		if (umbral >= 0f && umbral <= 1f) {
			PlayerPrefs.SetFloat (THRESHOLD_KEY, umbral);
		} else {
			Debug.LogError("Umbral fuera de rango");
		}
	}

	public static float ObtenUmbral (){
		return PlayerPrefs.GetFloat (THRESHOLD_KEY);
	}
}
