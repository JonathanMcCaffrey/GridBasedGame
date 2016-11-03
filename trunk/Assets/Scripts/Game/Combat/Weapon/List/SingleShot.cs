using UnityEngine;
using System.Collections;

public class SingleShot : MonoBehaviour, ITapFire {
	public WeaponProperties weaponProperties;


	public SingleShot() {
		weaponProperties
			.setDamage (1)
			.setReload (0)
			.setAmmo (1)
			.setFiringSpeed (1)
			.setMaxRange (10)
			.setRangeDropOff (10);

	}

	public void tapFire() {

	}
}
