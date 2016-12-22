using UnityEngine;
using System.Collections;

public class StartTile : MonoBehaviour {

	public GameObject playerPrefab = null;

	private static StartTile instance = null;

	void OnCollisionEnter2D(Collision2D col) {

	}

	void Awake () {
		if (instance) {
			Destroy(instance);
		}

		instance = this;

		var player = Player.Create ();


		player.name = "Player";

		var temp =  new Vector3((int)(this.gameObject.transform.position.x),
			(int)(this.gameObject.transform.position.y),
			250);

		player.transform.position = temp;

	}
}
