using UnityEngine;
using System.Collections;

/*
 * Contains information of weapon
 * 
 */
public class WeaponProperties : MonoBehaviour {

	private float damage = 1;
	private float reload = 0;
	private float ammo = 1;
	private float firingSpeed = 1;
	private float maxRange = 10;
	private float rangeDropOff = 10;

	//TODO Make this be a 3D model
	private string modelPath = "Textures/DemoTank/ProjectileBullet";

	public float getDamage() {
		return damage;
	}
	public WeaponProperties setDamage(float damage) {
		this.damage = damage; 
		return this;
	}

	public float getReload() {
		return reload;
	}
	public WeaponProperties setReload(float reload) {
		this.reload = reload; 
		return this;
	}

	public float getAmmo() {
		return ammo;
	}
	public WeaponProperties setAmmo(float ammo) {
		this.ammo = ammo; 
		return this;
	}

	public float getFiringSpeed() {
		return firingSpeed;
	}
	public WeaponProperties setFiringSpeed(float firingSpeed) {
		this.firingSpeed = firingSpeed; 
		return this;
	}

	public float getMaxRange() {
		return maxRange;
	}
	public WeaponProperties setMaxRange(float maxRange) {
		this.maxRange = maxRange; 
		return this;
	}

	public float getRangeDropOff() {
		return rangeDropOff;
	}
	public WeaponProperties setRangeDropOff(float rangeDropOff) {
		this.rangeDropOff = rangeDropOff; 
		return this;
	}

	public string getModelPath() {
		return modelPath;
	}
	public WeaponProperties setModelPath(string modelPath) {
		this.modelPath = modelPath; 
		return this;
	}
}
