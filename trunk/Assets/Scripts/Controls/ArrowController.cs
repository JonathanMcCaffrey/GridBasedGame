
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArrowController : MonoBehaviour {
	
	public GameObject dot = null;
	public GameObject start = null;
	public GameObject end = null;
	public GameObject head = null;
	
	public GameObject prev = null;
	public GameObject next = null;

	public static GameObject Create() {

		return Instantiate(Resources.Load("Prefabs/Arrow/ArrowContainer", typeof(GameObject))) as GameObject;

	}
	
	
	public void Start () {
		ArrowTag [] arrowTags =  gameObject.GetComponentsInChildren<ArrowTag> ();
		foreach (var tag in arrowTags) {
			if (tag.gameObject.name == "ArrowDot") {
				dot = tag.gameObject;
			}
			if (tag.gameObject.name == "ArrowStart") {
				start = tag.gameObject;
			}
			if (tag.gameObject.name == "ArrowEnd") {
				end = tag.gameObject;
			}
			if (tag.gameObject.name == "ArrowHead") {
				head = tag.gameObject;
			}
		}

		if (prev) {
			float angle = Mathf.Atan2 ((prev.gameObject.transform.position.x - gameObject.transform.position.x),
			                           (gameObject.transform.position.y - prev.gameObject.transform.position.y));
			
			start.transform.rotation = Quaternion.Euler(0,0, angle * 57.2957795f + 90.0f);
			start.transform.localScale = Vector3.one;
			head.transform.localScale = Vector3.zero;
		} else {
			start.transform.localScale = Vector3.zero;
		}

		
		if (next) {
			float angle = Mathf.Atan2 ((next.gameObject.transform.position.x - gameObject.transform.position.x),
			                           (gameObject.transform.position.y - next.gameObject.transform.position.y));
			
			end.transform.rotation = Quaternion.Euler(0,0, angle * 57.2957795f + 90.0f);
			end.transform.localScale = Vector3.one;
		} else {
			end.transform.localScale = Vector3.zero;
			
			head.transform.localScale = Vector3.one;
			start.transform.localScale = Vector3.zero;
			head.transform.rotation = start.transform.rotation;
			
		}
		
		if (prev && !next) {
			dot.transform.localScale = Vector3.zero;
		} else {
			dot.transform.localScale = Vector3.one;
		}
		
		if (!prev && !next) {
			head.transform.localScale = Vector3.zero;
			start.transform.localScale = Vector3.zero;
			end.transform.localScale = Vector3.zero;
		}
	}
}
