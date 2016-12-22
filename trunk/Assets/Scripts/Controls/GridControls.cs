// Deprecated

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridControls : MonoBehaviour {
	private List<Vector3> inputList = new List<Vector3>();
	private List<GameObject> arrowList = new List<GameObject>();

	//public GameObject mArrowPrefab = null;

	private bool mouseDown = false;

	const float FORCE = 40;


	private float arrowZLayer = 500;

	private GameObject playerControlsContainer = null;
	private GameObject arrowContainer = null;

	void Awake() {

		playerControlsContainer = new GameObject("PlayerControlsContainer");
		DontDestroyOnLoad(playerControlsContainer);


		arrowContainer = new GameObject("ArrowContainer");


		arrowContainer.transform.parent = playerControlsContainer.transform;	
	}

	void RemovePoint() {
		if (inputList.Count > 0) {

			transform.position = new Vector3 (inputList [1].x, inputList [1].y, transform.position.z);
			inputList.RemoveAt (0);
			if (arrowList [0]) {
				Destroy (arrowList [0]);
			}
			arrowList [0] = null;
			arrowList.RemoveAt (0);
			RefreshNodeLinks ();
		}
	}

	void RefreshNodeLinks() {
		for (int arrowIndex = 0; arrowIndex < arrowList.Count; arrowIndex++) {
			if ((arrowIndex - 1 < 0) && arrowList [arrowIndex]) {
				arrowList [arrowIndex].GetComponentInChildren<ArrowController> ().prev = null;
			}
			else if((arrowIndex - 1 >= 0) && arrowList [arrowIndex - 1]) {
				arrowList [arrowIndex].GetComponentInChildren<ArrowController> ().prev = arrowList [arrowIndex - 1];
			}
			if ((arrowIndex + 1 >= arrowList.Count) && arrowList [arrowIndex]) {
				arrowList [arrowIndex].GetComponentInChildren<ArrowController> ().next = null;
			}
			else if((arrowIndex + 1 <= arrowList.Count) && arrowList [arrowIndex + 1]) {
				arrowList [arrowIndex].GetComponentInChildren<ArrowController> ().next = arrowList [arrowIndex + 1];
			}

			arrowList [arrowIndex].GetComponentInChildren<ArrowController> ().Start ();
		}
	}

	void UpdateMovement () {
		if (inputList.Count > 1) {
			GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			float xDiff = transform.position.x - inputList [1].x;
			float yDiff = transform.position.y - inputList [1].y;
			float distance = Mathf.Sqrt (xDiff * xDiff + yDiff * yDiff);
			if (distance < 0.1f) {
				RemovePoint ();
				return;
			}
			if (transform.position.x < inputList [1].x) {
				GetComponent<Rigidbody2D>().AddForce (new Vector2 (FORCE, 0), ForceMode2D.Force);
				transform.rotation = Quaternion.Euler (0, 0, -90);
			}
			if (transform.position.x > inputList [1].x) {
				GetComponent<Rigidbody2D>().AddForce (new Vector2 (-FORCE, 0), ForceMode2D.Force);
				transform.rotation = Quaternion.Euler (0, 0, 90);
			}
			if (transform.position.y < inputList [1].y) {
				GetComponent<Rigidbody2D>().AddForce (new Vector2 (0, FORCE), ForceMode2D.Force);
				transform.rotation = Quaternion.Euler (0, 0, 0);
			}
			if (transform.position.y > inputList [1].y) {
				GetComponent<Rigidbody2D>().AddForce (new Vector2 (0, -FORCE), ForceMode2D.Force);
				transform.rotation = Quaternion.Euler (0, 0, 180);
			}
		}
	}

	void CreateArrow (Vector2 point) {
		inputList.Add (point);

		var arrowTemp = ArrowController.Create ();
		arrowTemp.name = "PlayerArrow";
		arrowTemp.transform.parent = arrowContainer.transform;
		arrowList.Add (arrowTemp);

		arrowTemp.transform.position = point;
		RefreshNodeLinks ();
	}

	void CapturePlayerControls () {
		if (Input.GetMouseButtonDown (0)) {
			mouseDown = true;
		}
		if (mouseDown) {
			Vector2 mouseVector = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);
			Vector2 mouseGridVector  =  Globals.Round (mouseVector);
			Vector2 playerGridVector = Globals.Round (transform.position);

			Collider2D touched = Physics2D.OverlapPoint (new Vector2(mouseGridVector.x, mouseGridVector.y)); 
			if (touched) {
				if (touched.gameObject.layer == Tags.Wall || touched.gameObject.layer == Tags.Breakable) {
					return;
				}
			}
			if (inputList.Count == 0) {
				if (Globals.IsMiddle (mouseGridVector, playerGridVector)) {
					CreateArrow (mouseGridVector);		
					return;
				}
			}
			else {
				for (int inputIndex = 0; inputIndex < inputList.Count; inputIndex++) {
					if (Globals.IsMiddle (mouseGridVector, inputList [inputIndex])) {
						if (inputIndex < inputList.Count - 1 && inputIndex > 1) {
							for (int arrowIndex = inputIndex; arrowIndex < arrowList.Count; arrowIndex++) {
								Destroy (arrowList [arrowIndex]);
								arrowList [arrowIndex] = null;
							}
							inputList.RemoveRange (inputIndex, inputList.Count - inputIndex);
							arrowList.RemoveRange (inputIndex, arrowList.Count - inputIndex);
							RefreshNodeLinks ();
							return;
						}
					}
				}
				if (inputList.Count > 0) {
					if (Globals.GridVectorTouchingGridVector (inputList [inputList.Count - 1], mouseGridVector)) {
						CreateArrow (mouseGridVector);
						return;
					}
				}
			}
		}
		if (Input.GetMouseButtonUp (0)) {
			mouseDown = false;
		}

	}

	void Update () {
		UpdateMovement ();
		CapturePlayerControls ();
	}
}