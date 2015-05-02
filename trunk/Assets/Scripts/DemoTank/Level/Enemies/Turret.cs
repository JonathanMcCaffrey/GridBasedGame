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
	void CollisionLogic ()
	{
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

	float FORCE = 600;

	float currentTime = 0;
	void Update () {
		if (!isActive) {
			return;
		}

		float x = gameObject.transform.position.x - PlayerObject.instance.gameObject.transform.position.x;
		float y = gameObject.transform.position.y - PlayerObject.instance.gameObject.transform.position.y;
	
		Debug.Log ("String x:" + x);
		Debug.Log ("String y:" + y);


		float angle = Mathf.Atan2 (y, x);

		turretHead.transform.rotation = Quaternion.AngleAxis (angle  * Mathf.Rad2Deg + 90, Vector3.forward);

		currentTime += Time.deltaTime;

		if (currentTime > fireRate) {
			float x2 = bulletSpawn.transform.position.x - PlayerObject.instance.gameObject.transform.position.x;
			float y2 = bulletSpawn.transform.position.y - PlayerObject.instance.gameObject.transform.position.y;

			float angle2 = Mathf.Atan2 (y2, x2);

			currentTime = 0;

			GameObject bullet = GameObject.Instantiate (turretBullet, bulletSpawn.transform.position, turretHead.transform.rotation) as GameObject;
		
			bullet.name = "PlayerBullet";
			bullet.transform.parent = bulletContainer.transform;
			bullet.GetComponent<Rigidbody2D>().AddForce (new Vector2 (-Mathf.Cos (angle2) * FORCE, -Mathf.Sin (angle2) * FORCE));

			Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());


		}
	}
}
