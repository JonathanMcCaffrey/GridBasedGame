using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelLayers : MonoBehaviour {
	
	public GameObject mEnemyLayer = null;
	public GameObject mFloorLayer = null;
	public GameObject mWallLayer = null;
	
	public static LevelLayers instance = null;
	void Awake() {
		if (instance) {
			Destroy (gameObject);
		} else {
			DontDestroyOnLoad(gameObject);
			instance = this;
		}
	}
	
	
	
	
}
