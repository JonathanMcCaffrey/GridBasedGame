using UnityEngine;
using System.Collections;

public class FinishTile : MonoBehaviour {
	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.GetComponent<PlayerObject> ()) {
			Debug.Log("Finished Level");
			Application.LoadLevel("1");
		}
		
	}
}
