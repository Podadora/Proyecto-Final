using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nexo : MonoBehaviour {

	ParticleSystem Particula;
	Transform HPBAR;

	public float HPMAX = 100;
	public float HP;
	// Use this for initialization
	void Start () 
	{
		HP = HPMAX;
		Particula = gameObject.GetComponentInChildren<ParticleSystem> ();
		Particula.Stop ();
		HPBAR = GameObject.FindGameObjectWithTag ("VidaFinal").GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (HP <= 0) 
		{
			GameManager.Instance.CondicionGanar = true;
			Particula.Play ();
		}
	} 
	void OnTriggerEnter(Collider Otro)
	{
		if (Otro.tag == "Proyectil")
		{
			HP -= 10;
			float Res = Mathf.Clamp (HP / HPMAX, 0, 0.5f);
			HPBAR.localScale = new Vector3 (Res / 2, 0.1f, 1);
		}
	}
}
