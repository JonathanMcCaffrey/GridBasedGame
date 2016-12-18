using UnityEngine;
using System.Collections;

public abstract class ProjectileFactory : MonoBehaviour {

	private ProjectileFactory(WeaponSystem weaponSystem) {

	}

	public static void fireProjectile(WeaponProperties weaponProperties, TargetProperties targetProperties) {

		GameObject newGameObject = new GameObject ();
		Projectile.SetProjectile (weaponProperties, targetProperties, newGameObject);
		newGameObject.transform.parent = GameObject.Find ("playerProjectileLayer").transform;

	}
}
