using UnityEngine;
using System.Collections;

public class StartTile : MonoBehaviour {
	
	public GameObject playerPrefab = null;
	
	private static StartTile instance;

	void Awake () {
		if (instance) {
			Destroy(instance);
		}

		instance = this;

		if (playerPrefab) {
			var player = GameObject.Instantiate(playerPrefab) as GameObject;
			if(!player.GetComponent<PlayerObject>()) {
				Debug.Log("Error, player doesn't exist");
			}

			var collider = this.gameObject.GetComponent<BoxCollider2D>();



			player.transform.position = new Vector3(this.gameObject.transform.position.x,
			                                        this.gameObject.transform.position.y,
			                                        250);

		}
	}
}
