using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	
	private WeaponProperties weaponProperties = null;
	private TargetProperties targetProperties = null;


	public static void SetProjectile(WeaponProperties weaponProperties, TargetProperties targetProperties, GameObject projectileObject) {
		
		projectileObject.AddComponent<Projectile> ();

		projectileObject.AddComponent<SpriteRenderer> ();
		SpriteRenderer spriteRenderer = projectileObject.GetComponent<SpriteRenderer> ();


		//TODO Make this a model
		Sprite sprite = Resources.Load(weaponProperties.getModelPath (), typeof(Sprite)) as Sprite;
		spriteRenderer.sprite = sprite;

		projectileObject.AddComponent<BoxCollider2D> ();
		projectileObject.GetComponent<BoxCollider2D>().size = new Vector2(sprite.rect.width, sprite.rect.height);
		projectileObject.AddComponent<Rigidbody2D> ();

		projectileObject.GetComponent <Projectile> ().Initialize (weaponProperties, targetProperties);


	}

	public void Initialize(WeaponProperties weaponProperties, TargetProperties targetProperties) {
		this.weaponProperties = weaponProperties;
		this.targetProperties = targetProperties;

		gameObject.transform.position = targetProperties.getStartPosition ();

		gameObject.GetComponent<Rigidbody2D> ().AddRelativeForce (targetProperties.getTargetPosition () - targetProperties.getStartPosition (), ForceMode2D.Force);
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (weaponProperties == null || targetProperties == null) {
			//TODO Assert this error state
			Cleanup ();
		}

		gameObject.GetComponent<Rigidbody2D> ().AddRelativeForce (targetProperties.getProjectileDirection());

	}

	void OnCollision2D(Collider2D collider) {

		GameEntity gameEntity = collider.GetComponent<GameEntity> ();

		if (gameEntity != null) { 
			if(targetProperties.getAllegianceType() == AllegianceType.None || 
				targetProperties.getAllegianceType() != gameEntity.getAllegianceType()) {

				gameEntity.Damage ();
				this.Cleanup ();
			}
		}
	}

	public void Cleanup() {
		Destroy (this);
	}
}