using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponControls : MonoBehaviour, InputEventListener {

	WeaponSystem[] weaponSystems;

	public void Start() {
		//TODO Add weapons to the player
		weaponSystems = GameObject.Find ("player").GetComponents<WeaponSystem>();

		InputEvents.Add (this);
	}

	public void onDestroy() {
		InputEvents.Remove (this);
	}


	public void onInputEvent(InputPackage inputPackage) {
		if (inputPackage.getInputEventType () == InputEventType.OnTap) {
			foreach(WeaponSystem weaponSystem in weaponSystems) {
				weaponSystem.Fire (inputPackage);
			}
		}
	}

}
