#if UNITY_EDITOR

using UnityEngine;
using System.Collections;

namespace Utils {
	public class GameObjectUtils {
		public static bool HasMesh (GameObject placedAsset) {
			if (placedAsset.GetComponentsInChildren<MeshFilter> ().Length > 0) { 
				return true;
			}
			
			return placedAsset.GetComponent<MeshFilter> () && placedAsset.GetComponent<MeshFilter> ().sharedMesh;
		}

		public static void GetMinMaxPointFromMeshFilter (ref Vector3 minPoint, ref Vector3 maxPoint, MeshFilter meshFilter) {
			Vector3[] vertexPositions = meshFilter.sharedMesh.vertices;
			foreach (var position in vertexPositions) {

				Vector3 fixedVertex = new Vector3(position.x * meshFilter.gameObject.transform.lossyScale.x,
				                                  position.y * meshFilter.gameObject.transform.lossyScale.y,
				                                  position.z * meshFilter.gameObject.transform.lossyScale.z);

				fixedVertex = meshFilter.gameObject.transform.rotation * fixedVertex;

				fixedVertex = new Vector3(fixedVertex.x + meshFilter.gameObject.transform.position.x,
				                          fixedVertex.y + meshFilter.gameObject.transform.position.y,
				                          fixedVertex.z + meshFilter.gameObject.transform.position.z);


				minPoint.x = Mathf.Min (minPoint.x, fixedVertex.x);
				minPoint.y = Mathf.Min (minPoint.y, fixedVertex.y);
				minPoint.z = Mathf.Min (minPoint.z, fixedVertex.z);

				maxPoint.x = Mathf.Max (maxPoint.x, fixedVertex.x);
				maxPoint.y = Mathf.Max (maxPoint.y, fixedVertex.y);
				maxPoint.z = Mathf.Max (maxPoint.z, fixedVertex.z);
			}
		}

		public static void GetMaxMinPointFromGameObject (GameObject placedAsset, ref Vector3 maxPoint, ref Vector3 minPoint) {
			minPoint = new Vector3 (int.MaxValue, int.MaxValue, int.MaxValue);
			maxPoint = new Vector3 (int.MinValue, int.MinValue, int.MinValue);
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
			Vector3 maxPoint = Vector3.zero; Vector3 minPoint = Vector3.zero;
			GetMaxMinPointFromGameObject (placedAsset, ref maxPoint, ref minPoint);

			Rect collisionSize = new Rect (0, 0, 0, 0);
			collisionSize.width = maxPoint.x - minPoint.x;
			collisionSize.height = maxPoint.y - minPoint.y;
			
			return collisionSize;
		}
	}
}

#endif
