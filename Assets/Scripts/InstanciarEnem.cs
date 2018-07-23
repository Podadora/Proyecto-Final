using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanciarEnem : MonoBehaviour {



	public GameObject[] Enemigo;		// Array de GameObjects enemigos
	public Transform[] PuntosSpawn;		// 4 Puntos de SPawn
	public int LimEnem = 30;			// Limite total de enemigos
	public float TiempoEntreSpawn = 10f;	// Tiempo de spawn entre cada oleada
	bool Band;							// Bandera para spawnear una sola vez ya que necesito la variable Atacar activa en GameManager para otra evento

	// Use this for initialization


	void Start () 
	{
		PuntosSpawn = gameObject.GetComponentsInChildren<Transform> ();

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (GameManager.Instance.Ataquen)
			StartCoroutine(Invoca());
	}

	IEnumerator Invoca ()
	{
		if (!Band)
		{	
			GameManager.Instance.Ataquen = false;
			int Index = 1;
			for (int i = LimEnem; i >= 1; i--) 
			{
				Instantiate(Enemigo[Random.Range(0,2)], PuntosSpawn[Index].transform.position,PuntosSpawn[Index].transform.rotation); 
				Debug.Log ("Inoque a 1");
				if (Index >= 4) 
				{
					Index = 1;
					yield return new WaitForSeconds (TiempoEntreSpawn);
				} 
				else
				Index++;
			}
		
			Band = true;
		}

	}

}