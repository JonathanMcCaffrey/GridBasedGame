using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public static class AutoGridSnap
{
	public const string MoveSnapXKey = "AutoGridSnap.MoveSnapX";
	public const string MoveSnapYKey = "AutoGridSnap.MoveSnapY";
	public const string MoveSnapZKey = "AutoGridSnap.MoveSnapZ";
	
	// Vars
	private static Vector3 prevPosition;
	private static Vector3 prevRotation;
	private static bool doSnap = true;
	private static float snapValueX = 1;
	private static float snapValueY = 1;
	private static float snapValueZ = 1;
	private static float snapValueRot = 45;
	
	static AutoGridSnap()
	{
		EditorApplication.update += Update;	
	}
	
	public static void Update()
	{
		doSnap = EditorPrefs.GetBool("AutoGridSnap.doSnap", false);
		snapValueX = EditorPrefs.GetFloat(MoveSnapXKey, 1.0f);
		snapValueY = EditorPrefs.GetFloat(MoveSnapYKey, 1.0f);
		snapValueZ = EditorPrefs.GetFloat(MoveSnapZKey, 1.0f);
		snapValueRot = EditorPrefs.GetFloat("AutoGridSnap.RotationSnap", 45.0f);
		
		// Check if we should snap
		if ( doSnap
		    && !EditorApplication.isPlaying
		    && Selection.transforms.Length > 0
		    && (Selection.transforms[0].position != prevPosition || Selection.transforms[0].eulerAngles != prevRotation) )
		{
			AutoSnap();
			prevPosition = Selection.transforms[0].position;
			prevRotation = Selection.transforms[0].eulerAngles;
		}
	}
	
	private static void AutoSnap()
	{
		// Snap the transforms
		foreach ( Transform transform in Selection.transforms )
		{
			// Setup editor undo
			Undo.RecordObject(transform, "Auto Snap");
			
			Vector3 t = transform.localPosition;
			t.x = SnapRound( t.x, snapValueX );
			t.y = SnapRound( t.y, snapValueY );
			t.z = SnapRound( t.z, snapValueZ );
			transform.localPosition = t;
			
			Vector3 r = transform.localEulerAngles;
			r.x = SnapRound( r.x, snapValueRot ) % 360.0f;
			r.y = SnapRound( r.y, snapValueRot ) % 360.0f;
			r.z = SnapRound( r.z, snapValueRot ) % 360.0f;
			transform.localEulerAngles = r;
			
			// If a change was made, register as an undo
			EditorUtility.SetDirty(transform);
			Undo.RecordObject(transform, "Auto Snap");
		}
	}
	
	private static float SnapRound( float input, float snapValue )
	{
		return snapValue * Mathf.Round( ( input / snapValue ) );
	}
}