using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {
	public GameObject turretHead;
	public GameObject bulletSpawn;
	public GameObject turretBullet;

	public float fireRate = 1.4f;
	public float sightRange;

	private static GameObject bulletContainer = null;

	public Sprite destroyedTexture;

	bool isActive = true;

	void Awake () {
		if (!bulletContainer) {
			bulletContainer = new GameObject("BulletContainer");
		}

	}

	void OnCollisionEnter2D(Collision2D col) {
		CollisionLogic ();
	}

	void OnTriggerEnter2D(Collider2D col) {
		health = -4;
		CollisionLogic ();

	}

	int health = 4;
	void CollisionLogic () {
		if (isActive) {
			health--;

			if(health < 0) {
				isActive = false;
				Destroy(turretHead.gameObject);

				gameObject.GetComponent<SpriteRenderer>().sprite = destroyedTexture;

				var col = gameObject.GetComponent<BoxCollider2D>();
				if(col) {
					Destroy(col);
				}
			}
		}
	}

//	float FORCE = 600;

//	float currentTime = 0;

	float currentAngle = int.MinValue;
	void Update () {


	}
}
