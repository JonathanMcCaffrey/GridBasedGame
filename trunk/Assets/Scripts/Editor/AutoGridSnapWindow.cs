using UnityEngine;
using UnityEditor;
 
// While this window is open editor transform movement/rotation will auto snap to grid
public class AutoGridSnapWindow : EditorWindow
{
	[MenuItem("Edit/Commands/Toggle Auto Grid Snapping %_l")]
    static void ToggleGridSnap()
    {
        bool doSnap = EditorPrefs.GetBool("AutoGridSnap.doSnap");
        doSnap = !doSnap;
        EditorPrefs.SetBool("AutoGridSnap.doSnap", doSnap);

        AutoGridSnapWindow window = (AutoGridSnapWindow)EditorWindow.GetWindow(typeof(AutoGridSnapWindow));
        window.Repaint();
    }

    [MenuItem( "Edit/Auto Snap" )]
    static void Init()
    {
		// Create window
	    AutoGridSnapWindow window = (AutoGridSnapWindow)EditorWindow.GetWindow( typeof( AutoGridSnapWindow ) );
	    window.maxSize = new Vector2( 300, 200 );
    }
     
    public void OnGUI()
    {
        // Allow for on the fly value changes
	    EditorPrefs.SetBool("AutoGridSnap.doSnap", EditorGUILayout.Toggle( "Auto Snap", EditorPrefs.GetBool("AutoGridSnap.doSnap", false)));
		EditorPrefs.SetFloat (AutoGridSnap.MoveSnapXKey, EditorGUILayout.FloatField ("X Snap Value", EditorPrefs.GetFloat (AutoGridSnap.MoveSnapXKey, 1.0f)));
		EditorPrefs.SetFloat (AutoGridSnap.MoveSnapYKey, EditorGUILayout.FloatField ("Y Snap Value", EditorPrefs.GetFloat (AutoGridSnap.MoveSnapYKey, 1.0f)));
		EditorPrefs.SetFloat (AutoGridSnap.MoveSnapZKey, EditorGUILayout.FloatField ("Z Snap Value", EditorPrefs.GetFloat (AutoGridSnap.MoveSnapZKey, 1.0f)));
		EditorPrefs.SetFloat("AutoGridSnap.RotationSnap", EditorGUILayout.FloatField( "Rotate Snap", EditorPrefs.GetFloat("AutoGridSnap.RotationSnap", 45.0f)));
	}
}