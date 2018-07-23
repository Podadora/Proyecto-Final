using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovimientoPlayerFPS : MonoBehaviour {

	public float velocidadFrontal = 5;
	public float velocidadLateral = 5;
	public float velocidadEspalda = 3;
	public float fuerzaSalto = 10;
	public int multiplicadorCorrida = 2;

	private bool tocandoElPiso = false;
	private int multiplicadorInterno = 1;
	Rigidbody rbody;

	void Start () {
		// Obtenemos el RigidBody para hacer el salto posteriormente;
		rbody = GetComponent <Rigidbody> ();
	}

	void FixedUpdate () 
	{
		// Si nuestro player esta tocando el piso, le permitimos hacer el salto
		if(tocandoElPiso == true)
		{
			// Detectamos cuando toca la barra espaciadora
			if(Input.GetKeyDown(KeyCode.Space))
			{
				// Le aplicamos una fuerza hacia arriba (Vector3.Up) y la multiplicamos por nuestra fuerza de salto
				rbody.AddForce (Vector3.up * fuerzaSalto, ForceMode.Impulse);
			}
		}	

		// Detectamos si el player esta manteniendo apretada la tecla de shift
		if (Input.GetKey (KeyCode.LeftShift)){
			// Seteamos el multiplicador interno a nuestro multiplicador de corrida
			multiplicadorInterno = multiplicadorCorrida;
		} 
		// Cuando el player suelta el shift volvemos el multiplicador a 1
		else if (Input.GetKeyUp (KeyCode.LeftShift)){
			
			multiplicadorInterno = 1;
		}

		if (Input.GetKey (KeyCode.W)){

			transform.Translate (0f, 0f, velocidadFrontal * Time.deltaTime * multiplicadorInterno);
		}

		if (Input.GetKey (KeyCode.S)){
			transform.Translate (0f,0f, -velocidadEspalda * Time.deltaTime * multiplicadorInterno);
		}

		if (Input.GetKey (KeyCode.A)){
			transform.Translate (-velocidadLateral * Time.deltaTime * multiplicadorInterno,0f , 0f);
		}

		if (Input.GetKey (KeyCode.D)){
			transform.Translate (velocidadLateral * Time.deltaTime * multiplicadorInterno,0f, 0f);
		}


	}

	void OnCollisionEnter(Collision col)
	{
		// Detectamos si el player se encuentra chocando con nuestro layer llamado Piso
		if(col.gameObject.layer == LayerMask.NameToLayer("Piso"))
		{
			// Seteamos el bool tocando el Piso a verdadero ya que 
			tocandoElPiso = true;
		}
	}


	void OnCollisionExit(Collision col)
	{
		// Si el player deja de tocar el layer piso, ponemos el bool acorde
		if (col.gameObject.layer == LayerMask.NameToLayer ("Piso")) {
			tocandoElPiso = false;
		}
	}
}
