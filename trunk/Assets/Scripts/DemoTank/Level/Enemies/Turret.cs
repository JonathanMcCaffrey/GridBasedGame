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

	float currentAngle = int.MinValue;
	void Update () {
		if (!isActive) {
			return;
		}

		var Target = DecoyManager.GetClosestToPlayer (250);
		if(!Target && PlayerObject.instance) {
			Target = PlayerObject.instance.gameObject;
		}

		if (!Target) {
			return;
		}

		float x = gameObject.transform.position.x - Target.transform.position.x;
		float y = gameObject.transform.position.y - Target.transform.position.y;
	
		float targetAngle = Mathf.Atan2 (y, x);
		if (currentAngle == int.MinValue) {
			currentAngle = targetAngle;
		}

		float rotationPower = Mathf.PI / 2.0f;

		float direction = currentAngle < targetAngle ? 1 : -1;
		if (Mathf.Abs (targetAngle - currentAngle) > Mathf.PI / 2.0f) {
			direction *= -1;
		}

		float moveTo = Time.deltaTime * direction * rotationPower;

		Debug.Log ("Move to: " + moveTo);
		Debug.Log ("Direction: " + direction);


		currentAngle += moveTo;

		if (direction > 0.0f) {
			if(currentAngle > targetAngle) { 
				currentAngle = targetAngle;

			}
		} else {
			if(currentAngle < targetAngle) { 
				currentAngle = targetAngle;
			}
		}

		if(currentAngle != targetAngle) {
	//		Debug.Log("Not Equal" + (currentAngle - targetAngle).ToString());
		}

		turretHead.transform.rotation = Quaternion.AngleAxis ((float)currentAngle * Mathf.Rad2Deg + 90, Vector3.forward);

		currentTime += Time.deltaTime;

		if (currentTime > fireRate) {
			float x2 = bulletSpawn.transform.position.x - Target.transform.position.x;
			float y2 = bulletSpawn.transform.position.y - Target.transform.position.y;

			float angle2 = currentAngle; //Mathf.Atan2 (y2, x2);

			currentTime = 0;

			GameObject bullet = GameObject.Instantiate (turretBullet, bulletSpawn.transform.position, turretHead.transform.rotation) as GameObject;
		
			bullet.name = "PlayerBullet";
			bullet.transform.parent = bulletContainer.transform;
			bullet.GetComponent<Rigidbody2D>().AddForce (new Vector2 (-Mathf.Cos (angle2) * FORCE, -Mathf.Sin (angle2) * FORCE));

			Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());


		}
	}
}
