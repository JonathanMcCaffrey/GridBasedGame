using UnityEngine;
using System.Collections;

public class SingleShot : EquippableItem, ITapFire {

	private WeaponProperties weaponProperties = new WeaponProperties();
	public WeaponProperties getWeaponProperties() {
		return weaponProperties;
	}

	public SingleShot() {
		weaponProperties
			.setDamage (1)
			.setReload (0)
			.setAmmo (1)
			.setFiringSpeed (1)
			.setMaxRange (10)
			.setRangeDropOff (10);
	}

	public void tapFire(TargetProperties targetProperties) {
		ProjectileFactory.fireProjectile (weaponProperties, targetProperties);
	}
}
