using UnityEngine;
using System.Collections;

public static class Globals {
	public static float GRID = 100.0f;
	public static float GRID_SIZE = 1.0f;
	
	
	public static Vector3 VectorToGridVector(Vector3 aVector) {
		float x = ((int)((aVector.x * 100.0f) / GRID)) * GRID / 100.0f;
		x = (float) decimal.Round ((decimal)x, 2);
		
		
		
		float y = ((int)((aVector.y * 100.0f) / GRID)) * GRID / 100.0f;
		y = (float)decimal.Round ((decimal)y, 2);
		
		return new Vector3 (x, y, 0);
	}
	
	public static int GridRound(float aFloat) {
		return (int)(aFloat * 100.0f);
	}
	
	public static bool IsMiddle(Vector3 aLeftVector, Vector3 aRightVector) {
		return GridRound(aLeftVector.x) == GridRound(aRightVector.x) && GridRound(aLeftVector.y) == GridRound(aRightVector.y);
	}
	
	
	public static bool IsTopLeft(Vector3 aLeftVector, Vector3 aRightVector) {
		return GridRound(aLeftVector.x) == GridRound(aRightVector.x + GRID_SIZE) && GridRound(aLeftVector.y) == GridRound(aRightVector.y - GRID_SIZE);
		
	}
	
	public static bool IsTopRight(Vector3 aLeftVector, Vector3 aRightVector) {
		return GridRound(aLeftVector.x) == GridRound(aRightVector.x - GRID_SIZE) && GridRound(aLeftVector.y) == GridRound(aRightVector.y - GRID_SIZE);
		
	}
	
	public static bool IsBottomLeft(Vector3 aLeftVector, Vector3 aRightVector) {
		return GridRound(aLeftVector.x) == GridRound(aRightVector.x + GRID_SIZE) && GridRound(aLeftVector.y) == GridRound(aRightVector.y + GRID_SIZE);
		
	}
	
	public static bool IsBottomRight(Vector3 aLeftVector, Vector3 aRightVector) {
		return GridRound(aLeftVector.x - GRID_SIZE) == GridRound(aRightVector.x) && GridRound(aLeftVector.y) == GridRound(aRightVector.y + GRID_SIZE);
	}
	
	
	public static bool IsBottom(Vector3 aLeftVector, Vector3 aRightVector) {
		return GridRound(aLeftVector.x) == GridRound(aRightVector.x) && GridRound(aLeftVector.y) == GridRound(aRightVector.y + GRID_SIZE);
		
	}
	
	public static bool IsTop(Vector3 aLeftVector, Vector3 aRightVector) {
		return GridRound(aLeftVector.x) == GridRound(aRightVector.x) && GridRound(aLeftVector.y) == GridRound(aRightVector.y - GRID_SIZE);
	}
	
	public static bool IsRight(Vector3 aLeftVector, Vector3 aRightVector) {
		return GridRound(aLeftVector.x) == GridRound(aRightVector.x - GRID_SIZE) && GridRound(aLeftVector.y) == GridRound(aRightVector.y);
	}
	
	public static bool IsLeft(Vector3 aLeftVector, Vector3 aRightVector) {
		return GridRound(aLeftVector.x) == GridRound(aRightVector.x + GRID_SIZE) && GridRound(aLeftVector.y) == GridRound(aRightVector.y);
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

