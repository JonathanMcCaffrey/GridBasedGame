using UnityEngine;
using System.Collections;

public class FinishTile : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.GetComponent<PlayerObject> ()) {
			Application.LoadLevel("MainMenu");
		}
	}
}
