using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;


[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]

public class EnemySorc : MonoBehaviour { 
	AudioSource FuenteAudio;
	public AudioClip [] Clip;

	bool BandMuerte;
	GameObject SpellBK;		//	Backup del spell
	public Transform PuntDisp1;			// PUnto de donde sale disparado el spell
	bool Spelling;				// Boleano para saber cuando termina de tirar el spell
	public GameObject Spell;	// Spell a tirar con Particulas
	Player Target;				// Player
	NavMeshAgent NavAgE; 		// Nav Agent del GameObj  Enemy
	public Animator AnimEnemy;	// Animator del Enemy
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
	void Update () {
		FuenteAudio.volume = GameManager.Instance.Volumen;
		if (AnimEnemy.GetBool("Danio"))			//Condicional para hacer durar el estado de Danio unos 3 segs y ser perceptible por la animacion.
		{
			Debug.Log (Time.deltaTime);
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
			StopCoroutine (CorrutinaSpell ());
			Debug.Log ("Seguir");
			FixAnim.transform.localPosition = Vector3.zero;					// Luego de finalizar cada ataque volvera a la posicion de seguir o Idle y ahi se acomodara el personaje
			NavAgE.SetDestination (Target.transform.position);
		}
		if (Atacar  && !Muerto)		// En Caso de estar Atacando
		{
			if(!Spelling)
			{
			StartCoroutine (CorrutinaSpell());
			}
			Debug.Log ("Atacar");
			NavAgE.velocity = Vector3.zero;
			transform.LookAt (new Vector3(Target.transform.position.x, transform.position.y, Target.transform.position.z));

		}
		if (Idle  && !Muerto)		// En Caso de no tener target
		{
			StopCoroutine (CorrutinaSpell ());

			Debug.Log ("Idle");
			//FixAnim.transform.localPosition = Vector3.zero;					// Luego de finalizar cada ataque volvera a la posicion de seguir o Idle y ahi se acomodara el personaje
			//NavAgE.SetDestination (Base.transform.position);

		}
		if (Muerto && !BandMuerte) {
			StopCoroutine (CorrutinaSpell ());
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
		GameManager.Instance.Gold += 20;
		AnimEnemy.SetBool("Muerte",false);
		Debug.Log ("CorrutinaMuerte");
		yield return new WaitForSeconds (3f);
		Destroy (this.gameObject);
	}

	IEnumerator CorrutinaSpell()			//Lanzamiento de hechizo
	{
		Spelling = true;
		Debug.Log ("Spell Corrutina");
		FuenteAudio.Stop ();
		FuenteAudio.loop = false;
		FuenteAudio.clip = Clip [0];
		FuenteAudio.Play ();
		SpellBK = Instantiate (Spell, PuntDisp1.position, PuntDisp1.rotation);		//Instancio Gameobject con particulas collider y rigidbody
		yield return new WaitForSeconds (2.8f);				//Termina la carga
		FuenteAudio.Stop();
		FuenteAudio.clip = Clip [1];
		FuenteAudio.Play();
		Rigidbody SpellRigi = SpellBK.GetComponent<Rigidbody> (); //Toma el rigibody para darle velocity
		SpellRigi.velocity = transform.forward * 100;			//le da velocity
		yield return new WaitForSeconds (2f);				//le da tiempo a terminar la animacion


		Spelling = false;
	}
}
