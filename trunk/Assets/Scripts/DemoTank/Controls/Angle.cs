using UnityEngine;
using System.Collections;

public class Angle : MonoBehaviour {

	public float angle;

	public float getDegreeValue() {
		return angle % 360;
	}
	
	public float getRadValue() {
		return angle * Mathf.Deg2Rad;
	}

	public void setDegreeValue(float angle) {
		this.angle = angle;
		Refresh ();
	}

	public Vector3 getDirection() {
		float y = Mathf.Sin (angle * Mathf.Deg2Rad) * 3;
		float x = Mathf.Cos (angle * Mathf.Deg2Rad) * 3;
		
		return new Vector3 (x, y, 0);
	}


	public bool IsWithin(float angle, float give) {
		if (this.angle < angle + give && this.angle > angle - give) {
			return true;
		} 
		return false;
	}

	void Update () {
		Refresh ();
	}

	void OnDrawGizmos() {
		Refresh ();

		Gizmos.color = Color.yellow;

		float y = getDirection().y * 3;
		float x = getDirection().x * 3;
		
		Vector3 vec = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z - 1);
		
		Gizmos.DrawLine(transform.position, vec);
	}

	void Refresh() {
		gameObject.transform.rotation = Quaternion.Euler (0, 0, angle - 90);
	}
}
