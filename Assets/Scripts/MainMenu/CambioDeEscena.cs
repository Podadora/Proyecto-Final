using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioDeEscena : MonoBehaviour {




	public void IrAEscena(string nombreEscena)	// Funcion que cambia de escena y usa como referencia un string
	{		
		SceneManager.LoadScene (nombreEscena);
		GameManager.Instance.CondicionGanar = false;
		GameManager.Instance.CondicionPerder = false;

	}
}
