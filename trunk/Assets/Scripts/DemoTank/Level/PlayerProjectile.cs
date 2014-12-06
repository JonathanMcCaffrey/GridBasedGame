using UnityEngine;
using System.Collections;

public class PlayerProjectile : MonoBehaviour {
	void OnCollisionEnter2D(Collision2D col) {
		Destroy (gameObject);
	}
}
