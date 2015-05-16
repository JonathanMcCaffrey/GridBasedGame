using UnityEngine;
using System.Collections;

public class BulletProjectile : MonoBehaviour {
	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.layer == Tags.Player || 
		    col.gameObject.layer == Tags.Wall || 
		    col.gameObject.layer == Tags.Breakable || 
		    col.gameObject.layer == Tags.Enemy) {
			
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.layer == Tags.Player || 
		    col.gameObject.layer == Tags.Wall || 
		    col.gameObject.layer == Tags.Breakable || 
		    col.gameObject.layer == Tags.Enemy) {
			
			Destroy (gameObject);
		}
	}
}
