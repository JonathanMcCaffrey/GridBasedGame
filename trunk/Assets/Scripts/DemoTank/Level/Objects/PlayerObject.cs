using UnityEngine;
using System.Collections;

public class PlayerObject : MonoBehaviour {
	
	public static PlayerObject instance = null;
	
	void Awake() {
		if (instance) {
			Destroy(instance);
		}

		instance = this;
	}
}
