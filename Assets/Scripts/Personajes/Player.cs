using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]

public class Player : MonoBehaviour {
	float DispTime ;				// Variable para controlar los ataques 
	public float Reit = 30f;		// Atk Speed
	public Camera Cam; 				// Camara para Raycast
	public GameObject Proyectil; 	// Proyectil actual
	GameObject PuntDisp;			// Punto de donde sale el proyectil
	Transform Referencia;			// Transform de PuntDisp
	public Animator AnimPlay;		// Animator del player
	RaycastHit hit;					// Variabla  Hit
	gui Gui;						// Variable para la UI
	bool BandDisp = false;			// Variable para control el primer disparo y que no salga instantaneamente cuando clickeo


	//
	// Stats Player
	//
	public float HPMAX = 100;
	public float HP;
	public float DMGbase = 10;
	public float Rango = 6;				// Rango Disparo 	


	NavMeshAgent NavAgP; 			// Nav Agent del GameObj Player
	Transform EnemObj;				// Objectivo del player al disparar
	//
	// Estados del Player
	//

	bool Attacking;
	bool Running;
	bool Idle;


	void Start(){
		//
		//	Inicializacion de variables
		//
		HP = HPMAX;
		PuntDisp = GameObject.FindGameObjectWithTag ("PuntoDisparo");
		NavAgP = GetComponent <UnityEngine.AI.NavMeshAgent> ();
		AnimPlay = GetComponentInChildren <Animator> ();
		AnimPlay.SetBool ("Idle", true);
		Referencia = PuntDisp.GetComponent<Transform> ();
		Gui = FindObjectOfType<gui> ();
			

	} 

	void Update() {
		 
					
		//
		//Ray Cast a la posicion del puntero como referencia para moverse o atacar
		//

		if (Input.GetMouseButton(0) ) {
			
			Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
			Debug.DrawRay (ray.origin, ray.direction);
			if (Physics.Raycast(ray,out hit)) 
			{
				if (hit.collider.tag == "Enemy") // Si choca
				{
					EnemObj = hit.transform; 	// Guardo Referencia del enemigo al que le apunta

					NavAgP.isStopped = true;
					Debug.Log("Toco Enemy");
					Attacking = true;
					Idle = false;
					Running = false;
					AnimPlay.SetBool ("Running", true);
					AnimPlay.SetBool ("Idle", false);
					AnimPlay.SetBool ("Attacking", false);
				}
				if (hit.collider.tag == "Floor") // Si Choca
				{
					
					//
					// Movimiento hacia el punto Objetivo
					//
					Debug.Log("Toco Floor");
					AnimPlay.SetBool ("Running", true);
					AnimPlay.SetBool ("Idle", false);
					AnimPlay.SetBool ("Attacking", false);
					Running = true;
					Attacking = false;
					Idle = false;
				}
			}
		}

		//
		// Condicional de ataque confirma si esta en rango.
		//

		if(Attacking)
		{
			

			if (EnemObj == null)
			{	
				Attacking = false;
				Idle = true;
				return;
			}
			NavAgP.velocity = Vector3.zero;
			float DistObjetivo = Vector3.Distance (transform.position, EnemObj.transform.position);
				if(DistObjetivo > Rango)
			{
				Debug.Log("Fuera de Rango");
				transform.LookAt(EnemObj.transform);
				NavAgP.SetDestination(EnemObj.transform.position);
					
			}
			else
			{
			Debug.Log("Attacking");
			AnimPlay.SetBool ("Attacking", true);
			AnimPlay.SetBool ("Running", false);
			Debug.Log ("Nonull");
			if (DispTime <= Time.time)
			{
				transform.LookAt(new Vector3( EnemObj.transform.position.x, Referencia.transform.position.y, EnemObj.transform.position.z));
					Disparar (Referencia);
					DispTime = Time.time + Reit;
			
			}
		}
	}
		//
		// Condicional de movimiento da las coordenadas y checkea la distancia con el objetivo para 
		//
	if (Running)
	{	
		NavAgP.isStopped = false;	// Activo NavAgP
		BandDisp = false;		//Bandera de primer disparo
		NavAgP.SetDestination(hit.point); //Punto destino
		EnemObj = null;		//Quito Target
		AnimPlay.SetBool ("Running", true);
		float DistObjetivo = Vector3.Distance (transform.position, hit.point);	// Distancia entre el objetivo y yo
		if (DistObjetivo < 30f)				// Distancia de reposo del objetivo para cambiar Booleanos
		{
			AnimPlay.SetBool ("Running", false);
			AnimPlay.SetBool ("Idle", true);
			Running = false;
			Idle = true;
		}
	}
		//
		// Condicional IDLE
		//
	if (Idle)
	{
		AnimPlay.SetBool ("Idle", true);
		AnimPlay.SetBool ("Attacking", false);
		AnimPlay.SetBool ("Running", false);
		}
	}
	void OnTriggerEnter( Collider Otro)
	{
		Debug.Log ("ColisionoEspada");									//Si me choca una espada
		if (Otro.tag == "Arma")
		{
			Debug.Log ("ColisionoConArma");

			HP -= 5;
			float PorcAct = Mathf.Clamp( HP/ HPMAX, 0,1);
			Gui.ActHP (PorcAct);
		}
		if (Otro.tag == "Spell")									//Si me choca una spell
		{
			HP -= 15;
			float PorcAct = Mathf.Clamp( HP/ HPMAX, 0,1);			// Limito el minimo de Hp a 0
			Gui.ActHP (PorcAct);
			Destroy (Otro.gameObject);
		}
		if (HP <= 0) 
		{
			AnimPlay.SetBool ("Muerte", true);
			StartCoroutine (Muerte ());
		}
	}

	void Disparar(Transform Ang)
	{	

		if (BandDisp)
		{
			Instantiate(Proyectil, Ang.position, Ang.rotation);

		}
		else
		{
			
			if (DispTime <= Time.time)
			{
				BandDisp = true;
			}
		}
	}
	IEnumerator Muerte ()
	{
		yield return new WaitForSeconds (3);
		AnimPlay.SetBool ("Muerte", false);
		GameManager.Instance.CondicionPerder = true;
	}
}
	