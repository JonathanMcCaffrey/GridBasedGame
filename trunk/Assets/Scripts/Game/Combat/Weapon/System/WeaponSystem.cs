using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponSystem : MonoBehaviour {

	private List<ITapFire> tapFires = new List<ITapFire>();

	public WeaponSystem addTapFire(ITapFire tapFire) {
		tapFires.Add (tapFire);
		return this;
	}



	public void Fire() {

		foreach (ITapFire tapFire in tapFires) {
			tapFire.tapFire ();
		}

	}
}
