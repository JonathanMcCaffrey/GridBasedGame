using UnityEngine;
using System.Collections;

public class LevelGrid : MonoBehaviour {
	
	public float mGridSize = 50.0f;
	
	public float mWidth = 1000.0f;
	public float mHeight = 1000.0f;
	public Color mColour = Color.blue;

	int mMax = 1000;

	void CreateEditorGrid () {
		Gizmos.color = this.mColour;
		for (float x = 0; x < mWidth; x += 1) {
			Gizmos.DrawLine (new Vector3 (mGridSize * x, -mMax, 2), new Vector3 (mGridSize * x, mMax, 2));
		}
		for (float y = 0; y < mHeight; y += 1) {
			Gizmos.DrawLine (new Vector3 (mMax, mGridSize * y, 1), new Vector3 (-mMax, mGridSize * y, 1));
		}
	}
	
	void OnDrawGizmos() {
		CreateEditorGrid ();
	}
}
