using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;


[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]

public class Enemy : MonoBehaviour {
	AudioSource FuenteAudio;
	public AudioClip [] Clip;
	bool BandMuerte;
	Player Target;
	NavMeshAgent NavAgE; // Nav Agent del GameObj  Enemy
	public Animator AnimEnemy;
	public Transform FixAnim;	// genero un transform para podes usarlo en codigo y resetear la posicion del objeto ya que la animacion me lo deja unas unidades mas abajo y se termina bugueando

	bool Danio = false;			// bool para ayudarme a solucionar un asunto con la animacion de Danio al caminar.
	float DanioTime ;			// float para el mismo problema con la animacion de danio



	//
	// Stats Enemy
	//
	public float DMG = 5;
	public float HP = 100;
	public float Armadura;
	public float DistDet = 8;   // Distancia de deteccion del Enemy
	public float DistAtaq = 1;   // Distancia de ataque del Enemy
	float DistObj;  // Distancia del Objetivo
	bool Seguir;
	bool Atacar;
	bool Idle;
	bool Muerto;


	// Use this for initialization
	void Start () {
		
		FuenteAudio = gameObject.GetComponent<AudioSource> ();
		AnimEnemy = GetComponentInChildren <Animator> ();
		Target = FindObjectOfType <Player> ();
		NavAgE = GetComponent <UnityEngine.AI.NavMeshAgent> ();
		//FixAnim = gameObject.GetComponentInChildren <Transform> ();


	}
	
	// Update is called once per frame
	void Update (	) {
		FuenteAudio.volume = GameManager.Instance.Volumen;

		


		if (AnimEnemy.GetBool("Danio"))			//Condicional para hacer durar el estado de Danio unos 3 segs y ser perceptible por la animacion.
		{
			if (!Danio)										//Checkeo bandera para setear el tiempo
			{	
				Debug.Log ("aass");
				float Var = 3f;								//genero variable de 3 "seg"
				DanioTime = Time.deltaTime + Var;
				Danio = true;								//Bandera
			}
			else if ( DanioTime <= Time.time)				//si ya paso el tiempo
			{
				AnimEnemy.SetBool ("Danio", false);			
				Danio = false;								//bandera en false para resetear el condicional.
			}
		}

		DistObj = Vector3.Distance (NavAgE.transform.position, Target.transform.position); //Tomo Distancia entre objetivo

		//
		// Condicionales para cambiar de estado
		//
		if (DistObj < DistDet) {
			if (DistObj < DistAtaq) {
				AnimEnemy.SetBool ("Atacar", true);
				AnimEnemy.SetBool ("Seguir", false);
				AnimEnemy.SetBool ("Idle", false);
				Atacar = true;
				Seguir = false;
				Idle = false;
			} else {
				AnimEnemy.SetBool ("Atacar", false);
				AnimEnemy.SetBool ("Seguir", true);
				AnimEnemy.SetBool ("Idle", false);
				Seguir = true;
				Atacar = false;
				Idle = false;
			}
						
		} 
		else
		{
			AnimEnemy.SetBool ("Atacar", false);
			AnimEnemy.SetBool ("Seguir", true);
			AnimEnemy.SetBool ("Idle", false);
			Idle = true;
			Atacar = false;
			Seguir = false;
		}

		if (Seguir  && !Muerto)   // En Caso de estar siguiendo
		{
			Debug.Log ("Seguir");
			FuenteAudio.Stop ();
			FixAnim.transform.localPosition = Vector3.zero;					// Luego de finalizar cada ataque volvera a la posicion de seguir o Idle y ahi se acomodara el personaje
			NavAgE.SetDestination (Target.transform.position);
		}
		if (Atacar  && !Muerto)		// En Caso de estar Atacando
		{
			FuenteAudio.Stop ();
			Debug.Log ("Atacar");
			NavAgE.velocity = Vector3.zero;

			transform.LookAt (new Vector3(Target.transform.position.x, transform.position.y, Target.transform.position.z));

		}
		if (Idle)
		{	
			//FuenteAudio.clip = Clip [0];
			//FuenteAudio.loop = true;				//solian hacer ruido con los escudos pero era bastante molesto
			//FuenteAudio.Play();
			AnimEnemy.SetBool ("Idle", true);
		}
			
		if (Muerto && !BandMuerte) {
			AnimEnemy.SetBool ("Muerte", true);
			BandMuerte = true;
			
		} else
			AnimEnemy.SetBool ("Muerte", false);
	}
	//
	//Gizmos
	//
	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;	
		Gizmos.DrawWireSphere (transform.position, DistDet);
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, DistAtaq);
	}

	void OnTriggerEnter (Collider Otro)
	{
		Debug.Log ("Choquewnnolopuecre");
		if (Otro.tag == "Proyectil") 
		{
			Otro.transform.SetParent (gameObject.transform);  // si es un proyectil se clavara en el enemigo


			HP = HP - (Target.DMGbase + 5);					// +5 de danio a la base por proyectil
			AnimEnemy.SetBool("Danio",true);
			if (HP <= 0 )
			{	
				StartCoroutine (CorrutinaMuerte ());		
				Muerto = true;
				NavAgE.velocity = Vector3.zero;
				NavAgE.isStopped = true;
				//Destroy (this.gameObject);
			}
		}
		if (Otro.tag == "Spell") 
		{
			HP = HP - (Target.DMGbase + 15);				// +15 de danio a la base por ser Spell
		}
	}

	IEnumerator CorrutinaMuerte ()
	{
		GameManager.Instance.Gold += 10;
		FuenteAudio.loop = false;
		FuenteAudio.clip = Clip [1];
		FuenteAudio.Play ();
		AnimEnemy.SetBool("Muerte",false);
		Debug.Log ("CorrutinaMuerte");
		yield return new WaitForSeconds (3f);
		Destroy (this.gameObject);
	}
}
