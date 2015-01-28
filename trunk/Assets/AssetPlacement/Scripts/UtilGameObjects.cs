using UnityEngine;
using System.Collections;

namespace Utils {
	public class GameObjectFunctions {
		public static bool HasMesh (GameObject placedAsset) {
			if (placedAsset.GetComponentsInChildren<MeshFilter> ().Length > 0) { 
				return true;
			}
			
			return placedAsset.GetComponent<MeshFilter> () && placedAsset.GetComponent<MeshFilter> ().sharedMesh;
		}

		public static void GetMinMaxPointFromMeshFilter (ref Vector2 minPoint, ref Vector2 maxPoint, MeshFilter meshFilter) {
			Vector3[] vertexPositions = meshFilter.sharedMesh.vertices;
			foreach (var position in vertexPositions) {
				minPoint.x = Mathf.Min (minPoint.x, position.x * meshFilter.gameObject.transform.localScale.x);
				minPoint.y = Mathf.Min (minPoint.y, position.y * meshFilter.gameObject.transform.localScale.y);
				maxPoint.x = Mathf.Max (maxPoint.x, position.x * meshFilter.gameObject.transform.localScale.x);
				maxPoint.y = Mathf.Max (maxPoint.y, position.y * meshFilter.gameObject.transform.localScale.y);
			}
		}

		public static void GetMinMaxPointFromGameObject (GameObject placedAsset, ref Vector2 maxPoint, ref Vector2 minPoint) {
			minPoint = new Vector2 (int.MaxValue, int.MaxValue);
			maxPoint = new Vector2 (int.MinValue, int.MinValue);
			var selfFilter = placedAsset.GetComponent<MeshFilter> ();
			if (selfFilter && selfFilter.sharedMesh) {
				GetMinMaxPointFromMeshFilter (ref minPoint, ref maxPoint, selfFilter);
			}
			var childrenFilters = placedAsset.GetComponentsInChildren<MeshFilter> ();
			foreach (var meshFilter in childrenFilters) {
				GetMinMaxPointFromMeshFilter (ref minPoint, ref maxPoint, meshFilter);
			}
		}
		
		public static Rect CreateRectFromMeshes (GameObject placedAsset) {
			Vector2 maxPoint = Vector2.zero; Vector2 minPoint = Vector2.zero;
			GetMinMaxPointFromGameObject (placedAsset, ref maxPoint, ref minPoint);
			
			
			
			Rect collisionSize = new Rect (0, 0, 0, 0);
			collisionSize.width = maxPoint.x - minPoint.x;
			collisionSize.height = maxPoint.y - minPoint.y;
			
			return collisionSize;
		}
	}
}
