using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class SeguirCamara : MonoBehaviour {
	
	public Player target;			
	public AudioSource FuenteAud;
	Vector3 OffsetNegativo;
	public Vector3 Offset;			// Variable distancia entre la camara y yo

	void Start()
	{
		FuenteAud = gameObject.GetComponent<AudioSource>();

	}
	void Update () 
	{
		FuenteAud.volume = GameManager.Instance.Volumen;
		if (target == null)
			return;
		if(GameManager.Instance.Ataquen == true) 	
		{
			
			transform.rotation = Quaternion.Euler (55, 180, 0);  // busque por todos lados como girar la camara con el trigger y parece que tenes que entregarle la variable en Euler para que tome bien la rotacion
			Offset = new Vector3(Offset.x,Offset.y,-Offset.z);
		}else
		transform.position = target.transform.position + Offset;  //  Ajusto la variable Offset en play mode y uso los valores para dejarla acomodada desde un principio
		
	}
}
