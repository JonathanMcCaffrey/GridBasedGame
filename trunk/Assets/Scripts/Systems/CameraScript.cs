using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	
	public GameObject mainPlayer = null;

	float cameraZLayer = -100;

	void FollowPlayerCamera () {
		if (mainPlayer == null && Player.instance) {
			mainPlayer = Player.instance.gameObject;
		}
		
		if (mainPlayer) {
			gameObject.transform.position = new Vector3 (mainPlayer.transform.position.x, mainPlayer.transform.position.y, cameraZLayer);
		}
	}
	
	void Update () {
		FollowPlayerCamera ();
	}
}
