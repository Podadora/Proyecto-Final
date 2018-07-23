using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sonido : MonoBehaviour {
	public Slider MainSlider;

	void Start()
	{
		MainSlider = GetComponent<Slider> ();		//tomo el slider para volcar el valor en el game manager
	}

	public void BarraSonido()
	{
		Debug.Log ("Editando el audio");
		GameManager.Instance.Volumen = MainSlider.value;		// vuelco el valor
	}
}
