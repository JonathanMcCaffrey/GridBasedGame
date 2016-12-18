using UnityEngine;
using System.Collections;

public class InitializeGame : MonoBehaviour {

	GameObject gameLayer;

	// Use this for initialization
	void Start () {
		gameLayer = new GameObject ("gameLayer");

		Transform projectileLayerTransform = addNewLayer ("projectileLayer");
		addNewLayer ("enemyProjectileLayer", projectileLayerTransform);
		addNewLayer ("playerProjectileLayer", projectileLayerTransform);

		addNewLayer ("objectLayer");
		addNewLayer ("playerLayer");
		addNewLayer ("enemyLayer");
		addNewLayer ("pickupLayer");


	}

	/**
	 * Create a layer with Name, attached to the gameLayer transform. Returns the new layers transform 
	 */
	Transform addNewLayer(string withLayerName) {
		GameObject newLayer = new GameObject (withLayerName);
		gameLayer.transform.parent = newLayer.transform;

		return newLayer.transform;
	}

	/**
	 * Create a layer with Name, attached to the passed in transform. Returns the new layers transform 
	 */
	Transform addNewLayer(string withLayerName, Transform toTransform) {
		GameObject newLayer = new GameObject (withLayerName);
		newLayer.transform.parent = toTransform;

		return newLayer.transform;
	}
}
