using UnityEngine;
using System.Collections;

public class DecoyProjectile : MonoBehaviour {
	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.layer == 11 || col.gameObject.layer == 9 || col.gameObject.layer == 10) {
			
			Destroy (gameObject);
		}
	}
	
	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.layer == 11 || col.gameObject.layer == 9 || col.gameObject.layer == 10) {
			
			Destroy (gameObject);
		}
	}

	void Awake() {
		DecoyManager.Add (this);
	}

	void OnDestroy() {
		DecoyManager.Remove (this);
	}
}
