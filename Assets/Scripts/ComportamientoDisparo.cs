using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportamientoDisparo : MonoBehaviour {

	public Rigidbody Disp;
	public float VelDisp = 2f;	
	public float DurDisp = 3f;
	public float Caida= 2;
	bool Decaida = true;


	// Use this for initialization
	void Start () 
	{
		DurDisp = Time.time + DurDisp;
		Disp = gameObject.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Decaida)
		{
		Disp.velocity = transform.forward * VelDisp;
		if (DurDisp <= Time.time)
		
			transform.Rotate (Caida*Time.deltaTime, 0, 0);
				
		}

	}

	void OnTriggerEnter(Collider Otro)
	{
		if (Otro.tag == "Enemy")
		{
			
			Decaida = false;									//Quita la decaida
			Disp.velocity = Vector3.zero;						//La flecha se detiene

			// Stops GameObject2 moving
			Debug.Log("Choco ");
		}
	}
}
