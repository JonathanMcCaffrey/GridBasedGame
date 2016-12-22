using UnityEngine;
using System.Collections;

public class InitializeGame : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		

		Layers.Create();

		Player.Create ();


	}
}
