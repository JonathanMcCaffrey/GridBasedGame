using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridControls : MonoBehaviour {
	private List<Vector3> mInputList = new List<Vector3>();
	private List<GameObject> mArrowList = new List<GameObject>();
	
	public GameObject mArrowPrefab = null;
	
	private bool mMouseDown = false;
	
	float FORCE = 40;
	
	private bool mMouseWasDown = false;
	private Vector3 mLastMousePosition = Vector3.zero;
	
	private float arrowZLayer = 500;
	
	private static GameObject playerControlsContainer = null;
	private static GameObject arrowContainer = null;
	
	void Awake() {
		if (!playerControlsContainer) {
			playerControlsContainer = new GameObject("PlayerControlsContainer");
			DontDestroyOnLoad(playerControlsContainer);
		}
		
		if (!arrowContainer) {
			arrowContainer = new GameObject("ArrowContainer");
		}
		
		arrowContainer.transform.parent = playerControlsContainer.transform;	
	}
	
	void RemovePoint() {
		if (mInputList.Count > 0) {
			
			transform.position = new Vector3 (mInputList [1].x, mInputList [1].y, transform.position.z);
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
				mArrowList [arrowIndex].GetComponentInChildren<ArrowController> ().mPrev = null;
			}
			else if((arrowIndex - 1 >= 0) && mArrowList [arrowIndex - 1]) {
				mArrowList [arrowIndex].GetComponentInChildren<ArrowController> ().mPrev = mArrowList [arrowIndex - 1];
			}
			if ((arrowIndex + 1 >= mArrowList.Count) && mArrowList [arrowIndex]) {
				mArrowList [arrowIndex].GetComponentInChildren<ArrowController> ().mNext = null;
			}
			else if((arrowIndex + 1 <= mArrowList.Count) && mArrowList [arrowIndex + 1]) {
				mArrowList [arrowIndex].GetComponentInChildren<ArrowController> ().mNext = mArrowList [arrowIndex + 1];
			}
			
			mArrowList [arrowIndex].GetComponentInChildren<ArrowController> ().Start ();
		}
	}
	
	void UpdateMovement () {
		if (mInputList.Count > 1) {
			GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			float xDiff = transform.position.x - mInputList [1].x;
			float yDiff = transform.position.y - mInputList [1].y;
			float distance = Mathf.Sqrt (xDiff * xDiff + yDiff * yDiff);
			if (distance < 0.1f) {
				RemovePoint ();
				return;
			}
			if (transform.position.x < mInputList [1].x) {
				GetComponent<Rigidbody2D>().AddForce (new Vector2 (FORCE, 0), ForceMode2D.Force);
				transform.rotation = Quaternion.Euler (0, 0, -90);
			}
			if (transform.position.x > mInputList [1].x) {
				GetComponent<Rigidbody2D>().AddForce (new Vector2 (-FORCE, 0), ForceMode2D.Force);
				transform.rotation = Quaternion.Euler (0, 0, 90);
			}
			if (transform.position.y < mInputList [1].y) {
				GetComponent<Rigidbody2D>().AddForce (new Vector2 (0, FORCE), ForceMode2D.Force);
				transform.rotation = Quaternion.Euler (0, 0, 0);
			}
			if (transform.position.y > mInputList [1].y) {
				GetComponent<Rigidbody2D>().AddForce (new Vector2 (0, -FORCE), ForceMode2D.Force);
				transform.rotation = Quaternion.Euler (0, 0, 180);
			}
		}
	}

	void CreateArrow (Vector2 point) {
		mInputList.Add (point);
		var arrowTemp = GameObject.Instantiate (mArrowPrefab, new Vector3(point.x, point.y, arrowZLayer), Quaternion.identity) as GameObject;
		arrowTemp.name = "PlayerArrow";
		arrowTemp.transform.parent = arrowContainer.transform;
		mArrowList.Add (arrowTemp);
		RefreshNodeLinks ();
	}
	
	void CapturePlayerControls () {
		if (Input.GetMouseButtonDown (0)) {
			mMouseDown = true;
		}
		if (mMouseDown) {
			Vector2 mouseVector = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);
			Vector2 mouseGridVector  =  Globals.Round (mouseVector);
			Vector2 playerGridVector = Globals.Round (transform.position);
			
			Collider2D touched = Physics2D.OverlapPoint (new Vector2(mouseGridVector.x, mouseGridVector.y)); 
			if (touched) {
				if (touched.gameObject.layer == Tags.Wall || touched.gameObject.layer == Tags.Breakable) {
					return;
				}
			}
			if (mInputList.Count == 0) {
				if (Globals.IsMiddle (mouseGridVector, playerGridVector)) {
					CreateArrow (mouseGridVector);		
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
						CreateArrow (mouseGridVector);
						return;
					}
				}
			}
		}
		if (Input.GetMouseButtonUp (0)) {
			mMouseDown = false;
		}
		
	}
	
	void Update () {
		UpdateMovement ();
		CapturePlayerControls ();
	}
}