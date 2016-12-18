using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Handles controls of weapons
 */
public class WeaponSystem : MonoBehaviour {

	public GameEntity owner;

	private List<ITapFire> tapFires = new List<ITapFire>();

	public WeaponSystem addTapFire(ITapFire tapFire) {
		tapFires.Add (tapFire);
		return this;
	}
		
	public void Fire(InputPackage inputPackage) {

		Vector3 targetPositon = inputPackage.getMousePosition ();
		Vector3 startPosition = gameObject.transform.position;

		TargetProperties targetProperties = new TargetProperties ()
			.setStartPosition (startPosition)
			.setTargetPosition(targetPositon)
			.setAllegianceType(owner.getAllegianceType());


		if (inputPackage.getInputEventType () == InputEventType.OnTap) {
			foreach (ITapFire tapFire in tapFires) {
				tapFire.tapFire (targetProperties);
			}
		}

	}
}
