using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEngine.UI;

public class GameManager :  MonoBehaviour{

	public static GameManager Instance;		// Genero Instancia del Gamenager
	public GameObject MenuPausa;
	public Canvas Menupausa;
	bool Pausa;
	public bool Ataquen;					// Variable en modo historia para girar la camara y comenzar el spawn de bichos que protegen la "Gema"
	public Text TextoFinal;
	public float Volumen;
	public int Gold;
	public bool CondicionPerder; 			// Muerte
	public bool CondicionGanar;				// Destruccion de nexo 
	public Scene ScenaActual; 



	void Awake()					//Genero el Singleton
	{
		if (Instance == null) 
		{
			Instance = this;
			DontDestroyOnLoad (gameObject);
		} 
		else 
		{
			Destroy (gameObject);
		}
	}

		
	void Update () 
	{
		//configuracion de pausa

		ScenaActual = SceneManager.GetActiveScene ();
		MenuPausa = GameObject.FindGameObjectWithTag ("MenuPausa");
		Menupausa = MenuPausa.GetComponent<Canvas> ();
		TextoFinal = MenuPausa.GetComponentInChildren<Text> ();
		if (ScenaActual.name == "Level1" && Input.GetKeyDown(KeyCode.Escape))
		{
			if (Pausa)
			{
				Menupausa.enabled = false;
				Pausa = false;
				if (!CondicionGanar && !CondicionPerder)
				Time.timeScale = 1;

			}else
			{
				
				Pausa = true;
				Menupausa.enabled = true;
				Time.timeScale = 0;
			}
			


		}
		//Checkeo de condiciones
		if (CondicionGanar)
			Ganar();
		if (CondicionPerder)
			Perder();

	}



	void Ganar()
	{
		Menupausa.enabled = true;
		TextoFinal.text = "Felicidades has ganado! Hiciste"+ Gold + "de Puntos";
		Gold = 0;


	}

	void Perder()
	{
		Menupausa.enabled = true;
		TextoFinal.text = "Has Perdido! Vuelve a intentar";
		Gold = 0;


	}

	//Como nosotros estamos usando el score y el booleano para hacer calculos en
	//El update, si nosotros cambiamos de escena sin resetear los valores
	//Unity va a trabarse ya que va a estar preguntando constante mente por esos valores
	void Resetear ()
	{
		
	}
}
