using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gui : MonoBehaviour {

	GameObject PercH; 	//Porcentaje de vida
	GameObject PercM;	//Porcentaje de mana
	Transform Health;		//Transform del HP
	Transform Mana;		//Transform del MP

	// Use this for initialization
	void Start () {
		PercH = GameObject.FindGameObjectWithTag ("HPPERC");
		Health = PercH.GetComponent <Transform> ();
		PercM = GameObject.FindGameObjectWithTag ("MPPERC");
		Mana = PercM.GetComponent <Transform> ();

		
	}
	
	// Update is called once per frame
	void Update () {	
	}

	public void ActHP (float PorcentajeActual)
	{	
		Debug.Log("LLegue a ACTHP");
		Health.transform.localScale = new Vector3 (PorcentajeActual, 1, 1);
	}

	public void ActMP (float PorcentajeActual)
	{	
		Debug.Log ("LLegue a ACTMP");	
		Mana.transform.localScale = new Vector3 (PorcentajeActual, 1, 1);
	}
}
