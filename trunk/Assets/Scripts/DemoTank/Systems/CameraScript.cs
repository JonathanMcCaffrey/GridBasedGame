using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	
	public GameObject mainPlayer = null;

	float cameraZLayer = 0;

	void FollowPlayerCamera () {
		if (mainPlayer == null && PlayerObject.instance) {
			mainPlayer = PlayerObject.instance.gameObject;
		}
		
		if (mainPlayer) {
			gameObject.transform.position = new Vector3 (mainPlayer.transform.position.x, mainPlayer.transform.position.y, cameraZLayer);
		}
	}
	
	void Update () {
		FollowPlayerCamera ();
	}
}
