using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	
	public GameObject mainPlayer = null;

	void FollowPlayerCamera () {
		if (mainPlayer) {
			gameObject.transform.position = new Vector3 (mainPlayer.transform.position.x, mainPlayer.transform.position.y, -500);
		}
	}
	
	void Update () {
		FollowPlayerCamera ();
	}
}
