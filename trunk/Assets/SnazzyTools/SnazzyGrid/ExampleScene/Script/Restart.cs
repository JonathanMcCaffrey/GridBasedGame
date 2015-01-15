using UnityEngine;
using System.Collections;

public class Restart : MonoBehaviour {

	// Use this for initialization
	Vector3 startPosition = new Vector3();

	void Start () {
		startPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < -20) transform.position = startPosition;
	}
}
