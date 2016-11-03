using UnityEngine;
using System.Collections;

public class WeaponProperties : MonoBehaviour {

	private float damage = 1;
	private float reload = 0;
	private float ammo = 1;
	private float firingSpeed = 1;
	private float maxRange = 10;
	private float rangeDropOff = 10;


	public WeaponProperties setDamage(float damage) {
		this.damage = damage; 
		return this;
	}

	public WeaponProperties setReload(float reload) {
		this.reload = reload; 
		return this;
	}

	public WeaponProperties setAmmo(float ammo) {
		this.ammo = ammo; 
		return this;
	}

	public WeaponProperties setFiringSpeed(float firingSpeed) {
		this.firingSpeed = firingSpeed; 
		return this;
	}

	public WeaponProperties setMaxRange(float maxRange) {
		this.maxRange = maxRange; 
		return this;
	}

	public WeaponProperties setRangeDropOff(float rangeDropOff) {
		this.rangeDropOff = rangeDropOff; 
		return this;
	}
}
