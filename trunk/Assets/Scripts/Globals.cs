// Deprecated

using UnityEngine;
using System.Collections;

public static class Globals {
	private static float GRID_SIZE = 1.0f;

	public static Vector3 Round(Vector3 aVector) {
		float x = Mathf.Round (aVector.x);
		float y = Mathf.Round (aVector.y);
		float z = Mathf.Round (aVector.z);
		return new Vector3 (x, y, aVector.z);
	}

	public static float Round(float aPoint) {
		float point = Mathf.Round (aPoint);
		return point;
	}
	
	public static bool IsMiddle(Vector3 aLeftVector, Vector3 aRightVector) {
		return Round(aLeftVector.x) == Round(aRightVector.x) && Round(aLeftVector.y) == Round(aRightVector.y);
	}
	
	
	public static bool IsTopLeft(Vector3 aLeftVector, Vector3 aRightVector) {
		return Round(aLeftVector.x) == Round(aRightVector.x + GRID_SIZE) && Round(aLeftVector.y) == Round(aRightVector.y - GRID_SIZE);
		
	}
	
	public static bool IsTopRight(Vector3 aLeftVector, Vector3 aRightVector) {
		return Round(aLeftVector.x) == Round(aRightVector.x - GRID_SIZE) && Round(aLeftVector.y) == Round(aRightVector.y - GRID_SIZE);
		
	}
	
	public static bool IsBottomLeft(Vector3 aLeftVector, Vector3 aRightVector) {
		return Round(aLeftVector.x) == Round(aRightVector.x + GRID_SIZE) && Round(aLeftVector.y) == Round(aRightVector.y + GRID_SIZE);
		
	}
	
	public static bool IsBottomRight(Vector3 aLeftVector, Vector3 aRightVector) {
		return Round(aLeftVector.x - GRID_SIZE) == Round(aRightVector.x) && Round(aLeftVector.y) == Round(aRightVector.y + GRID_SIZE);
	}
	
	
	public static bool IsBottom(Vector3 aLeftVector, Vector3 aRightVector) {
		return Round(aLeftVector.x) == Round(aRightVector.x) && Round(aLeftVector.y) == Round(aRightVector.y + GRID_SIZE);
		
	}
	
	public static bool IsTop(Vector3 aLeftVector, Vector3 aRightVector) {
		return Round(aLeftVector.x) == Round(aRightVector.x) && Round(aLeftVector.y) == Round(aRightVector.y - GRID_SIZE);
	}
	
	public static bool IsRight(Vector3 aLeftVector, Vector3 aRightVector) {
		return Round(aLeftVector.x) == Round(aRightVector.x - GRID_SIZE) && Round(aLeftVector.y) == Round(aRightVector.y);
	}
	
	public static bool IsLeft(Vector3 aLeftVector, Vector3 aRightVector) {
		return Round(aLeftVector.x) == Round(aRightVector.x + GRID_SIZE) && Round(aLeftVector.y) == Round(aRightVector.y);
	}
	
	public static Vector3 Add(Vector3 aLeftVector, Vector3 aRightVector) {
		return new Vector3(aLeftVector.x + aRightVector.x , aLeftVector.y + aRightVector.y, aLeftVector.z);
	}
	
	public static Vector3 Add(Vector3 aLeftVector, float x = 0.0f, float y = 0.0f) {
		return new Vector3(aLeftVector.x + x , aLeftVector.y + y, aLeftVector.z);
	}
	
	public static bool GridVectorTouchingGridVector(Vector3 aLeftVector, Vector3 aRightVector) {
		
		if (IsTop (aLeftVector, aRightVector) ||
		    //  IsTopLeft (aLeftVector, aRightVector) ||
		    // IsTopRight (aLeftVector, aRightVector) ||
		    IsBottom (aLeftVector, aRightVector) ||
		    //  IsBottomLeft (aLeftVector, aRightVector) ||
		    // IsBottomRight (aLeftVector, aRightVector) ||
		    IsRight (aLeftVector, aRightVector) ||
		    IsLeft (aLeftVector, aRightVector)) {
			return true;
			
		} else {
			return false;
		}
	}
};

