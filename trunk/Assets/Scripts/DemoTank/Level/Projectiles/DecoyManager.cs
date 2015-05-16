using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DecoyManager {
	List<DecoyProjectile> children = new List<DecoyProjectile>();

	public static void Add(DecoyProjectile child) {
		if(!get().children.Contains(child)) {
				get().children.Add(child);
		}
	}

	public static void Remove(DecoyProjectile child) {
		if(get().children.Contains(child)) {
			get().children.Remove(child);
		}
	}

	public static GameObject GetClosestToPlayer(float withinRange = float.MaxValue) {
		if (get ().children.Count > 1) {
			if(!PlayerObject.instance) {
				return null;
			}

			Vector2 origin = PlayerObject.instance.gameObject.transform.position;
			GameObject currentClosest = get ().children [0].gameObject;
			foreach(var child in get().children) {
				currentClosest = ClosestEquation (currentClosest, child.gameObject, origin, withinRange);
			}

			return currentClosest;
		} else if (get ().children.Count == 1) {
			return get ().children [0].gameObject;
		} else {
			return null;
		}
	}

	private static DecoyManager instance = null;
	private static DecoyManager get() {
		if (instance == null) {
			instance = new DecoyManager();
		}

		return instance;
	}

	private static GameObject ClosestEquation (GameObject left, GameObject right, Vector2 position, float max) {
		float ldx = position.x - left.transform.position.x;
		float ldy = position.y - left.transform.position.y;
		float rdx = position.x - right.transform.position.x;
		float rdy = position.y - right.transform.position.y;
		float ldd = Mathf.Sqrt (ldx * ldx + ldy * ldy);
		float rdd = Mathf.Sqrt (rdx * rdx + rdy * rdy);

		if (ldd < max || rdd < max) {
			return ldd <= rdd ? left : right;
		} else {
			return null;
		}
	}
}
