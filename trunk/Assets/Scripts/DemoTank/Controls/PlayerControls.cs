using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor;

public class PlayerControls : MonoBehaviour {
	public GameObject mPlayer = null;
	public GameObject mPlayerBase = null;
	public GameObject mPlayerGun = null;
	
	public GameObject mPlayerBullet = null;
	public GameObject mBulletSpawn = null;
	
	public bool mIsOn = true;
	
	private List<Vector3> mInputList = new List<Vector3>();
	private List<GameObject> mArrowList = new List<GameObject>();
	
	public GameObject mArrowPrefab = null;
	public GameObject mTestPrefab = null;
	
	private bool mMouseDown = false;
	
	float SPEED = 0.8f;
	float FORCE = 900;
	
	private bool mMouseWasDown = false;
	private Vector3 mLastMousePosition = Vector3.zero;
	
	void RemovePoint() {
		if (mInputList.Count > 0) {
			
			mPlayer.transform.position = new Vector3 (mInputList [1].x, mInputList [1].y, mPlayer.transform.position.z);
			mInputList.RemoveAt (0);
			if (mArrowList [0]) {
				Destroy (mArrowList [0]);
			}
			mArrowList [0] = null;
			mArrowList.RemoveAt (0);
			RefreshNodeLinks ();
		}
	}
	
	void RefreshNodeLinks() {
		for (int arrowIndex = 0; arrowIndex < mArrowList.Count; arrowIndex++) {
			if ((arrowIndex - 1 < 0) && mArrowList [arrowIndex]) {
				mArrowList [arrowIndex].GetComponent<ArrowController> ().mPrev = null;
			}
			else if((arrowIndex - 1 >= 0) && mArrowList [arrowIndex - 1]) {
				mArrowList [arrowIndex].GetComponent<ArrowController> ().mPrev = mArrowList [arrowIndex - 1];
			}
			if ((arrowIndex + 1 >= mArrowList.Count) && mArrowList [arrowIndex]) {
				mArrowList [arrowIndex].GetComponent<ArrowController> ().mNext = null;
			}
			else if((arrowIndex + 1 <= mArrowList.Count) && mArrowList [arrowIndex + 1]) {
				mArrowList [arrowIndex].GetComponent<ArrowController> ().mNext = mArrowList [arrowIndex + 1];
			}
			
			mArrowList [arrowIndex].GetComponent<ArrowController> ().Start ();
		}
	}
	
	void UpdateGunControls () {
		Vector3 mouseVector = new Vector3 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y, 0);
		float xDiff = mPlayerGun.transform.position.x - mouseVector.x;
		float yDiff = mPlayerGun.transform.position.y - mouseVector.y;
		float distance = Mathf.Sqrt (xDiff * xDiff + yDiff * yDiff);
		float angle = Mathf.Atan2 (mPlayerGun.transform.position.x - mouseVector.x, mPlayerGun.transform.position.y - mouseVector.y);
		if (distance > 1.1f) {
			mPlayerGun.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, -angle * 57.2957795f - 180.0f));
		}
		if (Input.GetMouseButtonUp (0)) {
			if (mMouseWasDown && (mLastMousePosition == Input.mousePosition)) {
				print ("CreateBullet");
				GameObject bullet = GameObject.Instantiate (mPlayerBullet, mBulletSpawn.transform.position, mPlayerGun.transform.rotation) as GameObject;
				bullet.rigidbody2D.AddForce (new Vector2 (Mathf.Sin (angle - (180.0f / 57.2957795f)) * FORCE / 2, Mathf.Cos (angle - (180.0f / 57.2957795f)) * FORCE / 2));
			}
			mMouseWasDown = false;
		}
		if (Input.GetMouseButtonDown (0)) {
			mMouseWasDown = true;
			mLastMousePosition = Input.mousePosition;
		}
	}
	
	void UpdateTankMovement () {
		if (mPlayer && mInputList.Count > 1) {
			rigidbody2D.velocity = Vector2.zero;
			float xDiff = mPlayer.transform.position.x - mInputList [1].x;
			float yDiff = mPlayer.transform.position.y - mInputList [1].y;
			float distance = Mathf.Sqrt (xDiff * xDiff + yDiff * yDiff);
			if (distance < 0.1f) {
				RemovePoint ();
				return;
			}
			if (mPlayer.transform.position.x < mInputList [1].x) {
				rigidbody2D.AddForce (new Vector2 (Time.deltaTime * FORCE, 0), ForceMode2D.Force);
				//mPlayer.transform.position = Globals.Add(mPlayer.transform.position, Time.deltaTime * SPEED, 0);
				mPlayerBase.transform.rotation = Quaternion.Euler (0, 0, -90);
				if (mPlayer.transform.position.x >= mInputList [1].x) {
					//	RemovePoint ();
					//	return;
				}
			}
			if (mPlayer.transform.position.x > mInputList [1].x) {
				//mPlayer.transform.position = Globals.Add(mPlayer.transform.position, -Time.deltaTime * SPEED, 0);
				rigidbody2D.AddForce (new Vector2 (-Time.deltaTime * FORCE, 0), ForceMode2D.Force);
				mPlayerBase.transform.rotation = Quaternion.Euler (0, 0, 90);
				if (mPlayer.transform.position.x <= mInputList [1].x) {
					//	RemovePoint();
					//	return;
				}
			}
			if (mPlayer.transform.position.y < mInputList [1].y) {
				//mPlayer.transform.position = Globals.Add(mPlayer.transform.position, 0, Time.deltaTime * SPEED);
				rigidbody2D.AddForce (new Vector2 (0, Time.deltaTime * FORCE), ForceMode2D.Force);
				mPlayerBase.transform.rotation = Quaternion.Euler (0, 0, 0);
				if (mPlayer.transform.position.y >= mInputList [1].y) {
					//	RemovePoint();
					//		return;
				}
			}
			if (mPlayer.transform.position.y > mInputList [1].y) {
				//mPlayer.transform.position = Globals.Add(mPlayer.transform.position, 0, -Time.deltaTime * SPEED);
				rigidbody2D.AddForce (new Vector2 (0, -Time.deltaTime * FORCE), ForceMode2D.Force);
				mPlayerBase.transform.rotation = Quaternion.Euler (0, 0, 180);
				if (mPlayer.transform.position.y <= mInputList [1].y) {
					//		RemovePoint();
					//	return;
				}
			}
		}
	}
	
	void CapturePlayerControls () {
		if (mIsOn && mPlayer) {
			if (Input.GetMouseButtonDown (0)) {
				mMouseDown = true;
			}
			if (mMouseDown) {
				Vector3 mouseVector = new Vector3 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y, 0);
				Vector3 mouseGridVector = Globals.VectorToGridVector (mouseVector);
				Vector3 playerGridVector = Globals.VectorToGridVector (mPlayer.transform.position);
				Collider2D touched = Physics2D.OverlapPoint (new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y));
				if (touched) {
					if (touched.gameObject.layer == 9) {
						return;
					}
				}
				if (mInputList.Count == 0) {
					if (Globals.IsMiddle (mouseGridVector, playerGridVector)) {
						mInputList.Add (new Vector3 (mouseGridVector.x, mouseGridVector.y, mouseGridVector.z));
						mArrowList.Add (GameObject.Instantiate (mArrowPrefab, mouseGridVector, Quaternion.identity) as GameObject);
						RefreshNodeLinks ();
						return;
					}
				}
				else {
					for (int inputIndex = 0; inputIndex < mInputList.Count; inputIndex++) {
						if (Globals.IsMiddle (mouseGridVector, mInputList [inputIndex])) {
							if (inputIndex < mInputList.Count - 1 && inputIndex > 1) {
								for (int arrowIndex = inputIndex; arrowIndex < mArrowList.Count; arrowIndex++) {
									Destroy (mArrowList [arrowIndex]);
									mArrowList [arrowIndex] = null;
								}
								mInputList.RemoveRange (inputIndex, mInputList.Count - inputIndex);
								mArrowList.RemoveRange (inputIndex, mArrowList.Count - inputIndex);
								RefreshNodeLinks ();
								return;
							}
						}
					}
					if (mInputList.Count > 0) {
						if (Globals.GridVectorTouchingGridVector (mInputList [mInputList.Count - 1], mouseGridVector)) {
							mInputList.Add (new Vector3 (mouseGridVector.x, mouseGridVector.y, mouseGridVector.z));
							mArrowList.Add (GameObject.Instantiate (mArrowPrefab, mouseGridVector, Quaternion.identity) as GameObject);
							RefreshNodeLinks ();
							return;
						}
					}
				}
			}
			if (Input.GetMouseButtonUp (0)) {
				mMouseDown = false;
			}
		}
	}
	
	void Update () {
		UpdateGunControls(); 
		UpdateTankMovement ();
		CapturePlayerControls ();
	}
	
	
	void OnCollisionEnter2D(Collision2D col) {
		print ("Collision Event");
		this.rigidbody2D.velocity = Vector3.zero;
	}
}