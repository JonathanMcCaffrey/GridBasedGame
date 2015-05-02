using UnityEngine;
using System.Collections;

public class PlayerProjectile : MonoBehaviour {
	void OnCollisionEnter2D(Collision2D col) {
		Debug.Log ("Col");
		
		if (col.gameObject.layer == 11 || col.gameObject.layer == 9 || col.gameObject.layer == 10) {
			
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		Debug.Log ("Col");
		
		if (col.gameObject.layer == 11 || col.gameObject.layer == 9 || col.gameObject.layer == 10) {
			
			Destroy (gameObject);
		}
	}
}
