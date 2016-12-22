using UnityEngine;
using System.Collections;
using System;

public class EquippableItem {

	Guid guid;

	void Start () {

		//Serialize the guid, and everything else
		guid = System.Guid.NewGuid ();
	
	}

}
