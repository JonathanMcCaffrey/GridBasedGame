using UnityEngine;
using System.Collections;

/**
 * Information on where a projectile is going
 */
public class TargetProperties : MonoBehaviour {

	private Vector3 startPosition; //< Start position of a projectile
	private Vector3 targetPosition; //< Target position of a projectile
	private AllegianceType allegianceType; //< Targets that the projectile cannot hit

	//List<Beziers> //< Positions bullet will Bezier to
	//etc.

	public Vector3 getStartPosition() {
		return startPosition;
	}
	public WeaponProperties setStartPosition(Vector3 startPosition) {
		this.startPosition = startPosition; 
		return this;
	}

	public Vector3 getTargetPosition() {
		return targetPosition;
	}
	public WeaponProperties setTargetPosition(Vector3 targetPosition) {
		this.targetPosition = targetPosition; 
		return this;
	}

	public AllegianceType getAllegianceType() {
		return allegianceType;
	}
	public WeaponProperties setAllegianceType(AllegianceType allegianceType) {
		this.allegianceType = allegianceType; 
		return this;
	}

	public Vector3 getProjectileDirection() {
		return Vector3.Normalize (targetPosition - startPosition);
	}
}
