// Deprecated

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor;

public class GunMouseControls : MonoBehaviour {
	public GameObject mGun = null;
	
	public GameObject mBullet = null;
	public GameObject mDecoy = null;

	public GameObject mBulletSpawn = null;

	float FORCE = 400;


	private bool mMouseWasDown = false;
	private Vector3 mLastMousePosition = Vector3.zero;

	private static GameObject bulletContainer = null;

	void Awake() {
		bulletContainer = GameObject.Find ("BulletContainer");

		if (!bulletContainer) {
			bulletContainer = new GameObject("BulletContainer");
		}
	}

	void UpdateGunControls () {
		Vector3 mouseVector = new Vector3 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y, 0);
		float xDiff = mGun.transform.position.x - mouseVector.x;
		float yDiff = mGun.transform.position.y - mouseVector.y;
		float distance = Mathf.Sqrt (xDiff * xDiff + yDiff * yDiff);
		float angle = Mathf.Atan2 (mGun.transform.position.x - mouseVector.x, mGun.transform.position.y - mouseVector.y);
		if (distance > 1.1f) {
			mGun.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, -angle * 57.2957795f - 180.0f));
		}
		if (Input.GetMouseButtonUp (0)) {
			if (mMouseWasDown && (mLastMousePosition == Input.mousePosition)) {
				
				GameObject bullet = GameObject.Instantiate (alternativeFire
				                                             ? mDecoy : mBullet, 
				                                            mBulletSpawn.transform.position, 
				                                            mGun.transform.rotation) as GameObject;
				bullet.name = "PlayerBullet";
				bullet.transform.parent = bulletContainer.transform;
				bullet.GetComponent<Rigidbody2D>().AddForce (new Vector2 (Mathf.Sin (angle - (180.0f / 57.2957795f)) * FORCE, Mathf.Cos (angle - (180.0f / 57.2957795f)) * FORCE));
		
				Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), Player.instance.gameObject.GetComponent<Collider2D>());
			}
			mMouseWasDown = false;
		}
		if (Input.GetMouseButtonDown (0)) {
			mMouseWasDown = true;
			mLastMousePosition = Input.mousePosition;
		}
	}

	bool alternativeFire = false;
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			alternativeFire = true;
		} 

		if (Input.GetKeyUp (KeyCode.Space)) {
			alternativeFire = false;
		} 

		UpdateGunControls(); 
	}

}