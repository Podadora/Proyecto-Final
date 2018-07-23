using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CambiodeEstado : MonoBehaviour {
	
	void OnTriggerEnter(Collider Otro)
	{
		if (Otro.tag == "Player") 
		{ 
			GameManager.Instance.Ataquen = true;
			Destroy (gameObject);
		}
	}
}
