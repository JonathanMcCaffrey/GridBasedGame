using UnityEditor;
using UnityEngine;
using System.Collections.Generic;


public class SnazzyToolsEditor: EditorWindow
{
	static public string installPath = ""; // Add your installation folder here, end with a slash like this "MyPackages/"
	static MeshRenderer snazzyGridRenderer;
	static GameObject snazzyGrid;
	static GameObject camReference;
	static GameObject snazzySettings;
	static GameObject _parent;
	static public SnazzySettings snazzy;

	static bool s;
	static Event key;
	static SnazzyToolsEditor window;

	static int controlID;
	static bool rotHandler = false;
	static bool scaleHandler = false;
	static bool snapLock = false;
	static string notify = "";
	static Camera sceneCamera;
	static float cameraXAxis;
	static Vector2 rotationOld;
	static bool keySnap;	
	
	float tipX = 0;
	int screenWidthOld = 0;
	Texture Button;
	string tooltip = "";
	bool tooltipOld;
	bool resizeWindow = false;
	
	[MenuItem("Window/SnazzyTools/SnazzyGrid")]
	public static void Init()
	{
		window = GetWindow(typeof(SnazzyToolsEditor)) as SnazzyToolsEditor;
		window.title = "SG";
		window.minSize = new Vector2(771,566);
		window.minSize = new Vector2(66,110);
		window.Show();
		window.LoadButtons();
	}

	void OnEnable()
	{
		// Debug.Log("Enable");
		InitReferences();
		SceneView.onSceneGUIDelegate -= OnScene;
		EditorApplication.hierarchyWindowItemOnGUI -= OnHierarchy;

		SceneView.onSceneGUIDelegate += OnScene;
		EditorApplication.hierarchyWindowItemOnGUI += OnHierarchy;
		GetOldPSR();;
	}
	
	void OnDisable()
	{
		// Debug.Log("Disable");
		ClearSnazzyGrid();
		SceneView.onSceneGUIDelegate -= OnScene;
		EditorApplication.hierarchyWindowItemOnGUI -= OnHierarchy;
	}
	
	public void OnDestroy()
	{
		// Debug.Log ("Destroy");
		ClearSnazzyGrid();
		SceneView.onSceneGUIDelegate -= OnScene;
		EditorApplication.hierarchyWindowItemOnGUI -= OnHierarchy;
	}
	
	static void OnScene(SceneView sceneview)
	{
		if (snazzy == null) {InitReferences();}
		key = Event.current;

		sceneCamera = Camera.current;
//		if (sceneCamera != null) 
//			Handles.Label (sceneCamera.transform.position+(sceneCamera.transform.forward*1),new GUIContent(sceneCamera.transform.rotation.eulerAngles.ToString()));
		
		if (snazzy.lookAtSelect) {
			if (Selection.activeTransform != null) {
				sceneview.pivot = Selection.activeTransform.position;
				if (key.type == EventType.MouseDown) {
					if (Mathf.Abs (sceneCamera.transform.localEulerAngles.z) > 175 && Mathf.Abs (sceneCamera.transform.localEulerAngles.z) < 185) 
						rotationOld = new Vector2(-sceneCamera.transform.localEulerAngles.x-sceneCamera.transform.localEulerAngles.z,sceneCamera.transform.localEulerAngles.y+sceneCamera.transform.localEulerAngles.z);
					else 
						rotationOld = new Vector2(sceneCamera.transform.localEulerAngles.x,sceneCamera.transform.localEulerAngles.y);
				}
				else if (key.button == 1) {
					sceneCamera.transform.localEulerAngles = new Vector3(DeltaAngle(sceneCamera.transform.localEulerAngles.x),DeltaAngle (sceneCamera.transform.localEulerAngles.y),DeltaAngle (sceneCamera.transform.localEulerAngles.z));
					rotationOld += new Vector2(key.delta.y/10,key.delta.x/10);
					Quaternion rotation = Quaternion.Euler(rotationOld.x,rotationOld.y,0.0f);
					Vector3 position = rotation * new Vector3(0,0,-2) + Selection.activeTransform.position;
					
					sceneCamera.transform.rotation = rotation;
					sceneCamera.transform.position = position;
				}	
			}
			if (key.button == 2) {
				snazzy.lookAtSelect = false;
				SnazzyWindowRepaint();
			}
		}

		// Display Scene Notification
		if (notify.Length > 0) { 
			sceneview.ShowNotification(new GUIContent(notify));
			notify = "";
		}
		
		Key(key,true);

		if (Selection.activeTransform != null) {
			snazzyGrid.transform.position = Selection.activeTransform.position;
			if (Selection.activeTransform != snazzy.oldTransform) snazzy.oldPosition = Selection.activeTransform.position;
		}
		
		if (snazzy.lockChild) {
			if (Selection.activeTransform != snazzy.oldTransform) ParentChildren();	
		}

		if (snazzy.showGrid) {
			if (!snazzy.gridIsEnabled) ShowGrid();
		}
		else {
			if (snazzy.gridIsEnabled) HideGrid();
		}
		if (Selection.transforms.Length == 0) {
			if (snazzy.gridIsEnabled) HideGrid();
		}
		
		if (snazzyGrid != null)	{
			if (snazzy.mov2) snazzyGridRenderer.sharedMaterial.SetFloat("_Scale", snazzy.gridIncrement2*snazzy.gridSize);
				else snazzyGridRenderer.sharedMaterial.SetFloat("_Scale", snazzy.gridIncrement*snazzy.gridSize);
			
			snazzyGrid.transform.localScale = new Vector3(snazzy.objectScale*snazzy.moveAmount,snazzy.objectScale*snazzy.moveAmount,snazzy.objectScale*snazzy.moveAmount);
			snazzyGridRenderer.sharedMaterial.SetFloat("_SnapAreaSize", snazzy.objectScale/(snazzy.objectScale*snazzy.snapAreaSize));
			snazzyGridRenderer.sharedMaterial.SetFloat("_ObjectScale", snazzy.objectScale*snazzy.moveAmount);
			snazzyGridRenderer.sharedMaterial.SetFloat("_GridSteps", snazzy.gridSize);
			if (Camera.current != null) {
				Vector3 cameraForward = Camera.current.transform.forward;
				snazzyGridRenderer.sharedMaterial.SetFloat("_X", cameraForward.x);
				snazzyGridRenderer.sharedMaterial.SetFloat("_Y", cameraForward.y);
				snazzyGridRenderer.sharedMaterial.SetFloat("_Z", cameraForward.z);
				snazzyGridRenderer.sharedMaterial.SetFloat("_Fresnel", snazzy.viewDependency);
			}
		}
	}

	static public void CheckSnap()
	{
		if (key == null) return;
		
		controlID = GUIUtility.hotControl;
		
		if (Selection.activeTransform != null) 
		{
			if (key.keyCode == KeyCode.V) snapLock = true;
			
			if (snazzy.autoSnap) {
				if (Selection.activeTransform == snazzy.oldTransform && !snapLock) {
					if (snazzy.autoSnapRot) {
						if (controlID != 0 && !rotHandler) {
							if (Selection.activeTransform.eulerAngles != snazzy.oldRotation) rotHandler = true;
						}
						else if (controlID == 0 && rotHandler) {
							SnapRot(snazzy.rotAmount,snazzy.autoSnapRotX,snazzy.autoSnapRotY,snazzy.autoSnapRotZ);
							rotHandler = false;
						}
					}
					
					if (snazzy.autoSnapPos)
					{
						if (Selection.activeTransform.position != snazzy.oldPosition && !rotHandler && (controlID != 0 || keySnap)) {
							// Debug.Log (snazzy.oldPosition+" : "+Selection.activeTransform.position);
							SnapPos(snazzy.autoSnapPosX,snazzy.autoSnapPosY,snazzy.autoSnapPosZ);
						}
					}
					
					if (snazzy.autoSnapScale) {
						if (Selection.activeTransform.localScale != snazzy.oldScale && (controlID != 0 || keySnap)) {
							if (Selection.transforms.Length == 1) {
								SnapScale(snazzy.autoSnapScaleX,snazzy.autoSnapScaleY,snazzy.autoSnapScaleZ);
								scaleHandler = false;
							}
							else if (controlID != 0 && !scaleHandler) scaleHandler = true;
						}
						else if (controlID == 0 && scaleHandler) {
							SnapScale(snazzy.autoSnapScaleX,snazzy.autoSnapScaleY,snazzy.autoSnapScaleZ);
							scaleHandler = false;
						}
					}
				}
				GetOldPSR();
			}
			
			snazzy.oldTransform = Selection.activeTransform;
		}
	}
	
	public void OnGUI()
	{
		if (snazzy == null) InitReferences ();
		if (AngleLabel == null) LoadButtons ();
		EditorGUI.DrawPreviewTexture(new Rect(0,0,Screen.width,Screen.height),BackGround);
		// Debug.Log (Screen.width+","+Screen.height);
		
		key = Event.current;
		
		if (Screen.width > 275) {
			GUI.color = new Color(1,1,1,2f-(tipX/135f));
			
			GUI.DrawTexture(new Rect(tipX,0,765+6,562),Quicktips);
			
			if (Vector2.Distance(new Vector2(491+tipX,42),key.mousePosition) < 40) {
				GUI.DrawTexture(new Rect(451+tipX,2,80,80),ManualHover);
				if (key.type == EventType.mouseDown) Application.OpenURL(Application.dataPath+"/"+installPath+"SnazzyTools/SnazzyGrid/SnazzyGridManual.pdf");
			}
			
			if (Vector2.Distance(new Vector2(706+tipX,213),key.mousePosition) < 40) {
				GUI.DrawTexture(new Rect(666+tipX,173,81,80),ForumHover);
				if (key.type == EventType.mouseDown) Application.OpenURL ("http://forum.unity3d.com/threads/snazzytools-snazzygrid-much-more-than-just-a-grid.253799/");
			}
			
			Repaint ();
			
			GUI.color = Color.white;
			GUI.DrawTexture(new Rect(Screen.width-40,0,40,Screen.height),GradientFade);
			
			if (screenWidthOld != Screen.width) {
				if (Screen.width > 70 || tipX > 0) {
					
					tipX = (400f-(((float)Screen.width/1.1f))/2f);
					if (tipX < 64) tipX = 64;
					Repaint();
				}
			}
		}
		
		if (screenWidthOld != Screen.width) {
			tooltipOld = snazzy.tooltip;
			snazzy.tooltip = false;
			resizeWindow = true;
		}
		else {
			if (resizeWindow) {
				snazzy.tooltip = tooltipOld;	
				resizeWindow = false;
			}
		}
		
		Key(key,false);
			
		screenWidthOld = Screen.width;

		int y = 0;
		
		if (snazzy.tooltip) tooltip = "Double-Click this logo to open the manual.\n\nRight-Click to go to the SnazzyGrid forum.\n\nHi, SnazzyGrid is designed to save you some time and make the work with Unity generally more enjoyable. If you find SnazzyTools helpful we would appreciate if you spend some of your saved time to leave a rating/review on the Assetstore. Also, if you have ideas on how to improve SnazzyTools please feel encouraged to visit us on the Unity Forum thread (Link in Settings/Help). We wish you a snazzy workflow and good luck with your work.";
			else tooltip = "";
		EditorGUI.LabelField(new Rect(-1,0+y,64+6,43+6),new GUIContent(SnazzyTitle,tooltip));
		if (new Rect(-1,0+y,64+6,43+6).Contains(key.mousePosition)) {
			if (key.clickCount == 2) Application.OpenURL(Application.dataPath+"/"+installPath+"SnazzyTools/SnazzyGrid/SnazzyGridManual.pdf");
			if (key.button == 1 && key.type == EventType.mouseDown) Application.OpenURL ("http://forum.unity3d.com/threads/snazzytools-snazzygrid-much-more-than-just-a-grid.253799/");
		}

		/*
		if (GUILayout.Button ("Display")) {
			GameObject[] test = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
			foreach (GameObject test1 in test) {
				if (test1.name == "##SnazzyGrid##" || test1.name == "##SnazzySettings##") {
					Debug.Log("found: "+test1.name);p
					test1.hideFlags = HideFlags.None;
				}
			}
		}
		if (GUILayout.Button ("Destroy")) {
			GameObject[] test = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
			foreach (GameObject test1 in test) {
				if (test1.name == "##SnazzyGrid##" || test1.name == "##SnazzySettings##") {
					Debug.Log("found: "+test1.name);
					DestroyImmediate(test1);
				}
			}
		}
		*/

		if (snazzy.showGrid) Button = GridToggleOn; else Button = GridToggleOff;
		if (snazzy.tooltip) tooltip = "Shows/hides the grid.\n<Hotkey: "+GetSpecialKey(snazzy.gridKeySpecial,snazzy.gridKey,false)+">";
		if (GUI.Button (new Rect(5-1,43+y,54+6,16+6),new GUIContent(Button,tooltip),EditorStyles.label)) {
			snazzy.showGrid = !snazzy.showGrid;
			SceneView.RepaintAll();
		}
		
		GUI.changed = false;
		snazzy.viewDependency = GUI.HorizontalSlider (new Rect(5+2,59+y,54-4,17),snazzy.viewDependency,1,10);
		y -= 2;
		snazzy.snapAreaSize = GUI.HorizontalSlider (new Rect(5+2,76+y,54-4,17),snazzy.snapAreaSize,0,1);
		
		y += 17;
		
		#region Grid
		if (snazzy.tooltip) tooltip = "Cycles through some grid size preset values.\n\nRight-Click to cycle backwards, Left/Middle-Click to cycle forward.";
		if (GUI.Button (new Rect(5-1,76+y,16+6,16+6),new GUIContent(GridSizePreset,tooltip),EditorStyles.label)) {
			if (key.button == 1 && key.button != 2) --snazzy.gridSizeIndex; else ++snazzy.gridSizeIndex;
			if (snazzy.gridSizeIndex < 0) snazzy.gridSizeIndex = snazzy.gridSizePresets.Count-1;
			else if (snazzy.gridSizeIndex > snazzy.gridSizePresets.Count-1) {snazzy.gridSizeIndex = 0;}
			snazzy.gridSize = snazzy.gridSizePresets[snazzy.gridSizeIndex];
			SceneView.RepaintAll();
		}
		
		GUI.SetNextControlName("GridSize");
		snazzy.gridSize = EditorGUI.FloatField(new Rect(21+4,76+1+y,38-3,16),snazzy.gridSize);
		if (GUI.GetNameOfFocusedControl() == "GridSize") {
			if (key.keyCode == KeyCode.Return) {GUIUtility.keyboardControl = 0;Repaint ();}
			else snazzy.lockKeys = true;
		}
		if (snazzy.gridSize < 0.0001f) snazzy.gridSize = 0.0001f;		
		if (snazzy.tooltip) tooltip = "The increments define how many grid units the selected object/s will move when you use the Move-Buttons/Hotkeys.";
		GUI.Label(new Rect(-1,92+y,64+6,16+6),new GUIContent(IncrementLabel,tooltip));
		
		if (snazzy.mov2) Button = IncrementPreset1ToggleOff; else Button = IncrementPreset1;
		if (snazzy.tooltip) tooltip = "Left-Click toggles between first and second move increment.\n\nLeft/Middle-Click cycles forward through some preset values, Right-Click to cycle backwards.";
		if (GUI.Button (new Rect(5-1,108+y,16+6,16+6),new GUIContent(Button,tooltip),EditorStyles.label)) {
			if (key.button == 0 && snazzy.mov2) snazzy.mov2 = false;
			else {
				if (key.button == 1) --snazzy.gridIncrementIndex; 
				else ++snazzy.gridIncrementIndex;
				
				if (snazzy.gridIncrementIndex < 0) snazzy.gridIncrementIndex = snazzy.gridIncrementPresets.Count-1;
				else if (snazzy.gridIncrementIndex > snazzy.gridIncrementPresets.Count-1) {snazzy.gridIncrementIndex = 0;}
				snazzy.gridIncrement = snazzy.gridIncrementPresets[snazzy.gridIncrementIndex];	
			}
			SceneView.RepaintAll();
		}
		if (!snazzy.mov2) Button = IncrementPreset2ToggleOff; else Button = IncrementPreset2;
		GUI.SetNextControlName("GridIncrement");
		snazzy.gridIncrement = EditorGUI.FloatField(new Rect(21+4,108+1+y,38-3,16),snazzy.gridIncrement);
		if (GUI.GetNameOfFocusedControl() == "GridIncrement") {
			if (key.keyCode == KeyCode.Return) {GUIUtility.keyboardControl = 0;Repaint ();}
			else snazzy.lockKeys = true;
		}
		if (snazzy.gridIncrement < 1) snazzy.gridIncrement = 1;
		if (snazzy.tooltip) tooltip = "Left-Click toggles between first and second move increment.\n\nLeft/Middle-Click cycles forward through some preset values, Right-Click to cycle backwards.";
		if (GUI.Button (new Rect(5-1,127+y,16+6,16+6),new GUIContent(Button,tooltip),EditorStyles.label)) {
			if (key.button == 0 && !snazzy.mov2) snazzy.mov2 = true;
			else {
				if (key.button == 1) --snazzy.gridMultiIndex;
				else ++snazzy.gridMultiIndex;
				
				if (snazzy.gridMultiIndex < 0) snazzy.gridMultiIndex = snazzy.gridMultiPresets.Count-1;
				else if (snazzy.gridMultiIndex > snazzy.gridMultiPresets.Count-1) {snazzy.gridMultiIndex = 0;}
				snazzy.gridIncrement2 = snazzy.gridMultiPresets[snazzy.gridMultiIndex];
			}
			SceneView.RepaintAll();
		}
		GUI.SetNextControlName("GridIncrement2");
		snazzy.gridIncrement2 = EditorGUI.FloatField(new Rect(21+4,127+1+y,38-3,16),snazzy.gridIncrement2);
		if (GUI.GetNameOfFocusedControl() == "GridIncrement2") {
			if (key.keyCode == KeyCode.Return) {GUIUtility.keyboardControl = 0;Repaint ();}
			else snazzy.lockKeys = true;
		}
		if (snazzy.gridIncrement2 < 1) snazzy.gridIncrement2 = 1;
	
		if (GUI.changed) SceneView.RepaintAll();	
		
		if (snazzy.tooltip) tooltip = "The angles define how many degrees the selected object/s will move when you use the Rotate-Buttons/Hotkeys";
		GUI.Label(new Rect(-1,143+y,39+6,16+6),new GUIContent(AngleLabel,tooltip));
		if (snazzy.rotationMode) Button = RotationModeToggleOn; else Button = RotationModeToggleOff;

		if (snazzy.tooltip) tooltip = "When enabled rotates each selected object around its own pivot (using the rotation keys).";
		if (GUI.Button (new Rect(38,144+y,22+6,14+6),new GUIContent(Button,tooltip),EditorStyles.label)) {
			snazzy.rotationMode = !snazzy.rotationMode;
		}
		#endregion
		
		#region Angle
		if (snazzy.angle2) Button = AnglePreset1ToggleOff; else Button = AnglePreset1;
		if (snazzy.tooltip) tooltip = "Left-Click toggles between first and second rotation angle.\n\nLeft/Middle-Click cycles forward through some preset values, Right-Click to cycle backwards.";
		if (GUI.Button (new Rect(5-1,159+y,16+6,16+6),new GUIContent(Button,tooltip),EditorStyles.label)) {
			if (key.button == 0 && snazzy.angle2) snazzy.angle2 = false;
			else {
				if (key.button == 1) --snazzy.rotIndex;
				else ++snazzy.rotIndex;
				
				if (snazzy.rotIndex < 0) snazzy.rotIndex = snazzy.rotPresets.Count-1;
				else if (snazzy.rotIndex > snazzy.rotPresets.Count-1) snazzy.rotIndex = 0;
				snazzy.rotIncrement = snazzy.rotPresets[snazzy.rotIndex];
			}
		}
		GUI.SetNextControlName("RotIncrement");
		snazzy.rotIncrement = EditorGUI.FloatField(new Rect(21+4,159+1+y,38-3,16),snazzy.rotIncrement);
		if (GUI.GetNameOfFocusedControl() == "RotIncrement") {
			if (key.keyCode == KeyCode.Return) {GUIUtility.keyboardControl = 0;Repaint ();}
			else snazzy.lockKeys = true;
		}
		if (snazzy.rotIncrement < 1) snazzy.rotIncrement = 1;		
		
		if (!snazzy.angle2) Button = AnglePreset2ToggleOff; else Button = AnglePreset2;
		if (snazzy.tooltip) tooltip = "Left-Click toggles between first and second rotation angle.\n\nLeft/Middle-Click cycles forward through some preset values, Right-Click to cycle backwards.";
		if (GUI.Button (new Rect(5-1,178+y,16+6,16+6),new GUIContent(Button,tooltip),EditorStyles.label)) {
			if (key.button == 0 && !snazzy.angle2) snazzy.angle2 = true;
			else {
				if (key.button == 1) --snazzy.rotMultiIndex;
				else ++snazzy.rotMultiIndex;
				
				if (snazzy.rotMultiIndex < 0) snazzy.rotMultiIndex = snazzy.rotMultiPresets.Count-1;
				else if (snazzy.rotMultiIndex > snazzy.rotMultiPresets.Count-1) snazzy.rotMultiIndex = 0;	
				snazzy.rotIncrement2 = snazzy.rotMultiPresets[snazzy.rotMultiIndex];
			}
		}
		GUI.SetNextControlName("RotIncrement2");
		snazzy.rotIncrement2 = EditorGUI.FloatField(new Rect(21+4,178+1+y,38-3,16),snazzy.rotIncrement2);
		if (GUI.GetNameOfFocusedControl() == "RotIncrement2") {
			if (key.keyCode == KeyCode.Return) {GUIUtility.keyboardControl = 0;Repaint ();}
			else snazzy.lockKeys = true;
		}
		if (snazzy.rotIncrement2 < 1) snazzy.rotIncrement2 = 1;	

		if (snazzy.rotAxis == 0) Button = RotationXToggleOn; else Button = RotationXToggleOff;
		if (snazzy.tooltip) tooltip = "Rotate the selected object/s around the X axis. \n<Axis select hotkey: "+GetSpecialKey (snazzy.rotXYZKeySpecial,snazzy.rotXYZKey,false)+">";
		if (GUI.Button (new Rect(5-1,197+y,16+6,16+6),new GUIContent(Button,tooltip),EditorStyles.label)) {
			snazzy.rotAxis = 0;
		}
		
		if (snazzy.rotAxis == 1) Button = RotationYToggleOn; else Button = RotationYToggleOff;
		if (snazzy.tooltip) tooltip = "Rotate the selected object/s around the Y axis.\n<Axis select hotkey: "+GetSpecialKey (snazzy.rotXYZKeySpecial,snazzy.rotXYZKey,false)+">";
		if (GUI.Button (new Rect(5-1+19,197+y,16+6,16+6),new GUIContent(Button,tooltip),EditorStyles.label)) {
			snazzy.rotAxis = 1;
		}
		
		if (snazzy.rotAxis == 2) Button = RotationZToggleOn; else Button = RotationZToggleOff;
		if (snazzy.tooltip) tooltip = "Rotate the selected object/s around the Z axis.\n<Axis select hotkey: "+GetSpecialKey (snazzy.rotXYZKeySpecial,snazzy.rotXYZKey,false)+">";
		if (GUI.Button (new Rect(5-1+38,197+y,16+6,16+6),new GUIContent(Button,tooltip),EditorStyles.label)) {
			snazzy.rotAxis = 2;
		}
		
		GUI.Label(new Rect(-1,213+y,64+6,13+6),new GUIContent(Spacer));
		#endregion
		
		#region MoveKeys
		if (snazzy.tooltip) tooltip = "Click to rotate the selected object/s to the left.\n<Hotkeys: "+GetSpecialKey(snazzy.rotLeftKeySpecial,snazzy.rotLeftKey,true)+">";
		if (GUI.Button (new Rect(5-1,226+y,16+6,16+6),new GUIContent(RotateLeft,tooltip),EditorStyles.label)) {
			RotateSelectionLeft();
		}
		
		if (snazzy.tooltip) tooltip = "Click to move the selected object/s up.\n<Hotkeys: "+GetSpecialKey(snazzy.upKeySpecial,snazzy.upKey,true)+">";		
		if (GUI.Button (new Rect(24-1,226+y,16+6,16+6),new GUIContent(MoveUp,tooltip),EditorStyles.label)) {
			MoveSelectionUp();
		}
		
		if (snazzy.tooltip) tooltip = "Click to rotate the selected object/s to the right.\n<Hotkeys: "+GetSpecialKey (snazzy.rotRightKeySpecial,snazzy.rotRightKey,true)+">";
		if (GUI.Button (new Rect(43-1,226+y,16+6,16+6),new GUIContent(RotateRight,tooltip),EditorStyles.label)) {
			RotateSelectionRight();
		}
		
		if (snazzy.tooltip) tooltip = "Click to move the selected object/s to the left.\n<Hotkeys: "+GetSpecialKey (snazzy.leftKeySpecial,snazzy.leftKey,true)+">";
		if (GUI.Button (new Rect(5-1,245+y,16+6,16+6),new GUIContent(MoveLeft,tooltip),EditorStyles.label)) {
			MoveSelectionLeft();
		}
		
		if (snazzy.tooltip) tooltip = "Click to move the selected object/s down.\n<Hotkeys: "+GetSpecialKey(snazzy.downKeySpecial,snazzy.downKey,true)+">";
		if (GUI.Button (new Rect(24-1,245+y,16+6,16+6),new GUIContent(MoveDown,tooltip),EditorStyles.label)) {
			MoveSelectionDown();
		}
		
		if (snazzy.tooltip) tooltip = "Click to move the selected object/s to the right.\n<Hotkeys: "+GetSpecialKey (snazzy.rightKeySpecial,snazzy.rightKey,true)+">";
		if (GUI.Button (new Rect(43-1,245+y,16+6,16+6),new GUIContent(MoveRight,tooltip),EditorStyles.label)) {
			MoveSelectionRight();
		}
		
		if (snazzy.tooltip) tooltip = "Click to move the selected object/s forward.\n<Hotkeys: "+GetSpecialKey (snazzy.frontKeySpecial,snazzy.frontKey,true)+">";
		if (GUI.Button (new Rect(5-1,264+y,16+6,16+6),new GUIContent(MoveForward,tooltip),EditorStyles.label)) {
			MoveSelectionForward();
		}
		
		if (snazzy.tooltip) tooltip = "Click to move the selected object/s back.\n<Hotkeys: "+GetSpecialKey (snazzy.backKeySpecial,snazzy.backKey,true)+">";
		if (GUI.Button (new Rect(24-1,264+y,16+6,16+6),new GUIContent(MoveBack,tooltip),EditorStyles.label)) {
			MoveSelectionBack();
		}
		
		if (snazzy.tooltip) tooltip = "Duplicates the selected object/s.\n<Hotkey: "+GetSpecialKey (snazzy.duplicateKeySpecial,snazzy.duplicateKey,false)+">";
		if (GUI.Button (new Rect(43-1,264+y,16+6,16+6),new GUIContent(Duplicate,tooltip),EditorStyles.label)) {
			DuplicateSelection(false);
		}	
		
		if (snazzy.lookAtSelect) Button = FocusToggleOn; else Button = FocusToggleOff;
		if (snazzy.tooltip) tooltip = "Keeps the selected object in camera focus.\n<Hotkey: "+GetSpecialKey(snazzy.focusKeySpecial,snazzy.focusKey,false)+">\n\nMiddle-Click to disable focus.\n\nRight-Click and drag to rotate around the selected object.";
		if (GUI.Button (new Rect(5-1,283+y,54+6,16+6),new GUIContent(Button,tooltip),EditorStyles.label)) {
			snazzy.lookAtSelect = !snazzy.lookAtSelect;
			SceneView.RepaintAll();
		}
	
		y += 22;
			
		GUI.changed = false;
		if (GetHorizontalMode()) GUI.color = Color.green; else GUI.color = Color.white;
		snazzy.cameraAngleOffset = GUI.HorizontalSlider (new Rect(7,277+y,54-4,17),snazzy.cameraAngleOffset,45,0);
		if (snazzy.cameraAngleOffset >= 45) snazzy.cameraAngleOffset = 90;
		
		if (GUI.changed) {
			if (key.button == 1) {
				if (sceneCamera != null) {
					snazzy.cameraAngleOffset = Mathf.Abs(Mathf.DeltaAngle(0,sceneCamera.transform.localEulerAngles.x));
				}
			}
			notify = "Free- to Horizontal Mode switching angle "+Mathf.Round (snazzy.cameraAngleOffset).ToString()+(char)176;
			SceneView.RepaintAll();
		}
		GUI.color = Color.white;
		
		y += 2;
		// GUI.Label(new Rect(-1,280+y,64+6,13+6),new GUIContent(Spacer));
		#endregion
		
		#region Snap
		
		// Transform Snap
		if (snazzy.autoSnap) Button = SnapToggleOn; else Button = SnapToggleOff;
		if (snazzy.tooltip) tooltip = "Left-Click enables/disables auto snapping. Single tap hotkey will toggle, hold will toggle auto snapping back upon releasing hotkey.\nSingle tap <HotKey: "+GetSpecialKey(snazzy.snapKeySpecial,snazzy.snapKey,false)+">\n\nMiddle-Click will snap the transform of the selected object/s.\nDouble tap <Hotkey: "+GetSpecialKey(snazzy.snapKeySpecial,snazzy.snapKey,false)+">\n\nRight-Click will reset the transform of the selected object/s\n<Hotkey: "+GetSpecialKey(snazzy.resetTransformKeySpecial,snazzy.resetTransformKey,false)+">";
		if (GUI.Button (new Rect(5-1,293+y,54+6,16+6),new GUIContent(Button,tooltip),EditorStyles.label)) {
			if (key.button == 1) {
				ResetPosition (true,true,true);	
				ResetRotation (true,true,true);
				ResetScale (true,true,true);
			}
			else if (key.button == 2){
				#if !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
				Undo.RecordObjects(Selection.transforms,"Selection Snap Transform");
				#else
				Undo.RegisterUndo(Selection.transforms,"Selection Snap Transform");
				#endif
				SnapPos(true,true,true);
				SnapRot(snazzy.rotAmount,true,true,true);
				SnapScale(true,true,true);
			}
			else ToggleSnap();
		}
		// Position Snap
		if (snazzy.autoSnapPos) Button = PositionToggleOn; else Button = PositionToggleOff;
		if (snazzy.tooltip) tooltip = "Left-Click enables/disables position snapping. Single tap hotkey will toggle, hold will toggle position snapping back upon releasing hotkey.\nSingle tap <HotKey: "
			+GetSpecialKey(snazzy.snapPosKeySpecial,snazzy.snapPosKey,false)+">\n\nMiddle-Click will snap the position of the selected object/s.\nDouble tap <HotKey: "
				+GetSpecialKey(snazzy.snapPosKeySpecial,snazzy.snapPosKey,false)+">\n\nRight-Click will reset the position of the selected object/s.\n<Hotkey: "
				+GetSpecialKey(snazzy.resetPositionKeySpecial,snazzy.resetPositionKey,false)+">";
		if (GUI.Button (new Rect(5-1,312+y,54+6,16+6),new GUIContent(Button,tooltip),EditorStyles.label)) {
			if (key.button == 1) {
				ResetPosition (true,true,true);	
			}
			else if (key.button == 2) {
				#if !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
				Undo.RecordObjects(Selection.transforms,"Selection Snap Position");
				#else
				Undo.RegisterUndo(Selection.transforms,"Selection Snap Position");
				#endif
				SnapPos(true,true,true);
			}
			else {
				snazzy.autoSnapPos = !snazzy.autoSnapPos;
				if (snazzy.autoSnapPos) GetOldPSR();;
			}
		}
		
		if (snazzy.autoSnapPosX) Button = PositionXToggleOn; else Button = PositionXToggleOff;
		if (snazzy.tooltip) tooltip = "Left-Click enables/disables position snapping for X axis.\n\nMiddle-Click will snap position-X for selected object/s.\n\nRight-Click will reset only Position-X of the selected object/s.";
		if (GUI.Button (new Rect(5-1,331+y,16+6,16+6),new GUIContent(Button,tooltip),EditorStyles.label)) {
			if (key.button == 1) ResetPosition (true,false,false);
			else if (key.button == 2) {
				#if !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
				Undo.RecordObjects(Selection.transforms,"Selection Snap Position X Axis");
				#else
				Undo.RegisterUndo(Selection.transforms,"Selection Snap Position X Axis");
				#endif
				SnapPos (true,false,false);
			}				
 			else snazzy.autoSnapPosX = !snazzy.autoSnapPosX;
		}
		
		if (snazzy.autoSnapPosY) Button = PositionYToggleOn; else Button = PositionYToggleOff;
		if (snazzy.tooltip) tooltip = "Left-Click enables/disables position snapping for Y axis.\n\nMiddle-Click will snap position-Y for selected object/s.\n\nRight-Click will reset only Position-Y of the selected object/s.";
		if (GUI.Button (new Rect(5-1+19,331+y,16+6,16+6),new GUIContent(Button,tooltip),EditorStyles.label)) {
			if (key.button == 1) ResetPosition (false,true,false); 
			else if (key.button == 2) {
				#if !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
				Undo.RecordObjects(Selection.transforms,"Selection Snap Position Y Axis");
				#else
				Undo.RegisterUndo(Selection.transforms,"Selection Snap Position Y Axis");
				#endif
				SnapPos(false,true,false);
			}
			else snazzy.autoSnapPosY = !snazzy.autoSnapPosY;
		}
		
		if (snazzy.autoSnapPosZ) Button = PositionZToggleOn; else Button = PositionZToggleOff;
		if (snazzy.tooltip) tooltip = "Left-Click enables/disables position snapping for Z axis.\n\nMiddle-Click will snap position-Z for selected object/s.\n\nRight-Click will reset only Position-Z of the selected object/s.";
		if (GUI.Button (new Rect(5-1+38,331+y,16+6,16+6),new GUIContent(Button,tooltip),EditorStyles.label)) {
			if (key.button == 1) ResetPosition (false,false,true); 
			else if (key.button == 2) {
				#if !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
				Undo.RecordObjects(Selection.transforms,"Selection Snap Position Z Axis");
				#else
				Undo.RegisterUndo(Selection.transforms,"Selection Snap Position Z Axis");
				#endif
				SnapPos (false,false,true);
			}
			else snazzy.autoSnapPosZ = !snazzy.autoSnapPosZ;
		}
		
		if (!snazzy.autoSnapPos) {
			GUI.Label(new Rect(5-1,331+y,57+6,16+6),new GUIContent(XYZMask));
		}
		
		// Rotation Snap
		if (snazzy.autoSnapRot) Button = RotationToggleOn; else Button = RotationToggleOff;
		if (snazzy.tooltip) tooltip = "Left-Click enables/disables rotation snapping.\nSingle tap hotkey will toggle, hold will toggle rotation snapping back upon releasing hotkey.\nSingle tap <HotKey: "
			+GetSpecialKey(snazzy.snapRotKeySpecial,snazzy.snapRotKey,false)+">\n\nMiddle-Click will snap the position of the selected object/s.\nDouble tap <HotKey: "
				+GetSpecialKey(snazzy.snapRotKeySpecial,snazzy.snapRotKey,false)+">\n\nRight-Click will reset the rotation of the selected object/s.\n<Hotkey: "+
				GetSpecialKey(snazzy.resetRotationKeySpecial,snazzy.resetRotationKey,false)+">";
		if (GUI.Button (new Rect(5-1,350+y,54+6,16+6),new GUIContent(Button,tooltip),EditorStyles.label)) {
			if (key.button == 1) ResetRotation (true,true,true);	
			else if (key.button == 2) {
				#if !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
				Undo.RecordObjects(Selection.transforms,"Selection Snap Rotation");
				#else
				Undo.RegisterUndo(Selection.transforms,"Selection Snap Rotation");
				#endif
				SnapRot(snazzy.rotAmount,true,true,true);
			}
			else {
				snazzy.autoSnapRot = !snazzy.autoSnapRot;
				if (snazzy.autoSnapRot) GetOldPSR();;
			}
		}
		
		if (snazzy.autoSnapRotX) Button = RotationXToggleOn; else Button = RotationXToggleOff;
		if (snazzy.tooltip) tooltip = "Left-Click enables/disables rotation snapping for X axis.\n\nMiddle-Click will snap rotation-X for selected object/s.\n\nRight-Click will reset only Rotation-X of the selected object/s.";
		if (GUI.Button (new Rect(5-1,369+y,16+6,16+6),new GUIContent(Button,tooltip),EditorStyles.label)) {
			if (key.button == 1) ResetRotation(true,false,false); 
			else if (key.button == 2) {
				#if !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
				Undo.RecordObjects(Selection.transforms,"Selection Snap Rotation X Axis");
				#else
				Undo.RegisterUndo(Selection.transforms,"Selection Snap Rotation X Axis");
				#endif
				SnapRot (snazzy.rotAmount,true,false,false);
			}
			else snazzy.autoSnapRotX = !snazzy.autoSnapRotX;
		}
		
		if (snazzy.autoSnapRotY) Button = RotationYToggleOn; else Button = RotationYToggleOff;
		if (snazzy.tooltip) tooltip = "Left-Click enables/disables rotation snapping for Y axis.\n\nMiddle-Click will snap rotation-Y for selected object/s.\n\nRight-Click will reset only Rotation-Y of the selected object/s.";
		if (GUI.Button (new Rect(5-1+19,369+y,16+6,16+6),new GUIContent(Button,tooltip),EditorStyles.label)) {
			if (key.button == 1) ResetRotation(false,true,false); 
			else if (key.button == 2) {
				#if !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
				Undo.RecordObjects(Selection.transforms,"Selection Snap Rotation Y Axis");
				#else
				Undo.RegisterUndo(Selection.transforms,"Selection Snap Rotation Y Axis");
				#endif
				SnapRot (snazzy.rotAmount,false,true,false);
			}
			else snazzy.autoSnapRotY = !snazzy.autoSnapRotY;
		}
		
		if (snazzy.autoSnapRotZ) Button = RotationZToggleOn; else Button = RotationZToggleOff;
		if (snazzy.tooltip) tooltip = "Left-Click enables/disables rotation snapping for Z axis.\n\nMiddle-Click will snap rotation-Z for selected object/s.\n\nRight-Click will reset only Rotation-Z of the selected object/s.";
		if (GUI.Button (new Rect(5-1+38,369+y,16+6,16+6),new GUIContent(Button,tooltip),EditorStyles.label)) {
			if (key.button == 1) ResetRotation(false,false,true); 
			else if (key.button == 2) SnapRot(snazzy.rotAmount,false,false,true);
			else snazzy.autoSnapRotZ = !snazzy.autoSnapRotZ;
		}
		
		if (!snazzy.autoSnapRot) GUI.Label(new Rect(5-1,369+y,57+6,16+6),new GUIContent(XYZMask));
		
		// Scale Snap
		if (snazzy.autoSnapScale) Button = ScaleToggleOn; else Button = ScaleToggleOff;
		if (snazzy.tooltip) tooltip = "Left-Click enables/disables scale snapping. Single tap hotkey will toggle, hold will toggle scale snapping back upon releasing hotkey.\nSingle tap <HotKey: "
			+GetSpecialKey(snazzy.snapScaleKeySpecial,snazzy.snapScaleKey,false)+">\n\nMiddle-Click will snap the rotation of the selected object/s.\nDouble tap <HotKey: "
				+GetSpecialKey(snazzy.snapScaleKeySpecial,snazzy.snapScaleKey,false)+"> \n\nRight-Click will reset the scale of the selected object/s.\n<Hotkey: "
				+GetSpecialKey (snazzy.resetScaleKeySpecial,snazzy.resetScaleKey,false)+">";
		if (GUI.Button (new Rect(5-1,388+y,54+6,16+6),new GUIContent(Button,tooltip),EditorStyles.label)) {
			if (key.button == 1) ResetScale (true,true,true);	
			else if (key.button == 2) {
				#if !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
				Undo.RecordObjects(Selection.transforms,"Selection Snap Scale");
				#else
				Undo.RegisterUndo(Selection.transforms,"Selection Snap Scale");
				#endif
				SnapScale(true,true,true);
			}
			else {
				snazzy.autoSnapScale = !snazzy.autoSnapScale;
				if (snazzy.autoSnapScale) GetOldPSR();;
			}
		}
		
		if (snazzy.autoSnapScaleX) Button = ScaleXToggleOn; else Button = ScaleXToggleOff;
		if (snazzy.tooltip) tooltip = "Left-Click enables/disables scale snapping for X axis.\n\nMiddle-Click will snap scale-X for selected object/s.\n\nRight-Click will reset only Scale-X of the selected object/s.";
		if (GUI.Button (new Rect(5-1,407+y,16+6,16+6),new GUIContent(Button,tooltip),EditorStyles.label)) {
			if (key.button == 1) ResetScale (true,false,false);
			else if (key.button	== 2) {
				#if !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
				Undo.RecordObjects(Selection.transforms,"Selection Snap Scale X Axis");
				#else
				Undo.RegisterUndo(Selection.transforms,"Selection Snap Scale X Axis");
				#endif
				SnapScale(true,false,false);
			}
			else snazzy.autoSnapScaleX = !snazzy.autoSnapScaleX;
		}
		
		if (snazzy.autoSnapScaleY) Button = ScaleYToggleOn; else Button = ScaleYToggleOff;
		if (snazzy.tooltip) tooltip = "Left-Click enables/disables scale snapping for Y axis.\n\nMiddle-Click will snap scale-Y for selected object/s.\n\nRight-Click will reset only Scale-Y of the selected object/s.";
		if (GUI.Button (new Rect(5-1+19,407+y,16+6,16+6),new GUIContent(Button,tooltip),EditorStyles.label)) {
			if (key.button == 1) ResetScale (false,true,false); 
			else if (key.button == 2) {
				#if !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
				Undo.RecordObjects(Selection.transforms,"Selection Snap Scale Y Axis");
				#else
				Undo.RegisterUndo(Selection.transforms,"Selection Snap Scale Y Axis");
				#endif
				SnapScale (false,true,false);
			}
			else snazzy.autoSnapScaleY = !snazzy.autoSnapScaleY;
		}
		
		if (snazzy.autoSnapScaleZ) Button = ScaleZToggleOn; else Button = ScaleZToggleOff;
		if (snazzy.tooltip) tooltip = "Left-Click enables/disables scale snapping for Z axis.\n\nMiddle-Click will snap scale-Z for selected object/s.\n\nRight-Click will reset only Scale-Z of the selected object/s.";
		if (GUI.Button (new Rect(5-1+38,407+y,16+6,16+6),new GUIContent(Button,tooltip),EditorStyles.label)) {
			if (key.button == 1) ResetScale (false,false,true); 
			else if (key.button == 2) {
				#if !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
				Undo.RecordObjects(Selection.transforms,"Selection Snap Scale Z Axis");
				#else
				Undo.RegisterUndo(Selection.transforms,"Selection Snap Scale Z Axis");
				#endif
				SnapScale (false,false,true);
			}
			else snazzy.autoSnapScaleZ = !snazzy.autoSnapScaleZ;
		}
		
		if (!snazzy.autoSnapScale) {
			GUI.Label(new Rect(5-1,407+y,57+6,16+6),new GUIContent(XYZMask));
		}
		
		if (!snazzy.autoSnap) {
			GUI.Label(new Rect(5-1,312+y,57+6,115+6),new GUIContent(SnapMask));
		}
		
		GUI.Label(new Rect(-1,423+y,64+6,13+6),new GUIContent(Spacer));
		#endregion
		
		#region Parent/Child
		if (snazzy.lockChild) Button = CCompToggleOn; else Button = CCompToggleOff;
		if (snazzy.tooltip) tooltip = "When enabled the Children of the selected object wont get affected by moving/rotating/scaling the Parent.\n<Hotkey: "+GetSpecialKey(snazzy.childCompensationKeySpecial,snazzy.childCompensationKey,false)+">";
		if (GUI.Button (new Rect(5-1,436+y,54+6,16+6),new GUIContent(Button,tooltip),EditorStyles.label)) {
			if (!snazzy.lockChild) UnparentChildren(); else ParentChildren();
		}
		if (snazzy.tooltip) tooltip = "Left-Click creates a new Parent for the selected object/s at the center of the selection.\n<Hotkey: "+GetSpecialKey(snazzy.parentToCenterKeySpecial,snazzy.parentToCenterKey,false)+">\n\nMiddle-Click creates a new Parent for the selected object/s at the Scene origin.\n<Hotkey: "+GetSpecialKey (snazzy.parentKeySpecial,snazzy.parentKey,false)+">\n\nRight-Click moves the parent of the selected object to the center position of the selection.\n<Hotkey: "+GetSpecialKey (snazzy.centerCurrentParentKeySpecial,snazzy.centerCurrentParentKey,false)+">";
		if (GUI.Button (new Rect(5-1,455+y,54+6,16+6),new GUIContent(Parent,tooltip),EditorStyles.label)) {
			if (key.button == 1) MoveParentToCenter(Selection.activeTransform.parent);
			else if (key.button == 2) ParentSelection();	
			else ParentSelectionToCenter();
		}
		
		if (snazzy.tooltip) tooltip = "Selects the Parent of the selected object/s.\n<Hotkey: "+GetSpecialKey(snazzy.hierarchyUpKeySpecial,snazzy.hierarchyUpKey,false)+">";
		if (GUI.Button (new Rect(5-1,474+y,16+6,16+6),new GUIContent(HierarchyUp,tooltip),EditorStyles.label)) SelectParent();
		
		if (snazzy.tooltip) tooltip = "Selects the Children of the selected object/s.\n<Hotkey: "+GetSpecialKey (snazzy.hierarchyDownKeySpecial,snazzy.hierarchyDownKey,false)+">";
		if (GUI.Button (new Rect(24-1,474+y,16+6,16+6),new GUIContent(HierarchyDown,tooltip),EditorStyles.label)) SelectChildren();
		
		if (snazzy.tooltip) tooltip = "Left-Click unparent the selected object/s.\n<HotKey: "+GetSpecialKey (snazzy.unparentKeySpecial,snazzy.unparentKey,false)+">";//+
			//"\n\nRight-Click unparent the selected object/s and delete the parent.\n<HotKey: "+GetSpecialKey(snazzy.unparentDeleteKeySpecial,snazzy.unparentDeleteKey,false)+">";
		if (GUI.Button (new Rect(43-1,474+y,16+6,16+6),new GUIContent(Unparent,tooltip),EditorStyles.label)) {
			//if (key.button == 1) UnparentSelectionDelete(); else UnparentSelection();
			UnparentSelection();
		}
		
		GUI.Label(new Rect(-1,490+y,64+6,13+6),new GUIContent(Spacer));
		
		#endregion
		
		#region Settings
		if (snazzy.settings) Button = SettingsToggleOn; else Button = SettingsToggleOff;
		if (GUI.Button (new Rect(5-1,503+y,54+6,16+6),new GUIContent(Button),EditorStyles.label)) {
			snazzy.settings = !snazzy.settings;
			if (snazzy.settings) {
				SnazzySettingsEditor editWindow = ScriptableObject.CreateInstance<SnazzySettingsEditor>();
				editWindow.title = "Snazzy Tools Settings";
				editWindow.minSize = new Vector3(415,272);
				editWindow.ShowUtility();
				editWindow.InitSnazzySettings();
			}
			else {
				SnazzySettingsEditor editWindow = GetWindow(typeof(SnazzySettingsEditor)) as SnazzySettingsEditor;
				if (editWindow != null) editWindow.Close();
			}
		}
		#endregion
	}

	static void ToggleSnap()
	{
		snazzy.autoSnap = !snazzy.autoSnap;
		if (snazzy.autoSnap) GetOldPSR();;
		SnazzyWindowRepaint();
	}

	static void ToggleMovSnap()
	{
		snazzy.autoSnapPos = !snazzy.autoSnapPos;
		if (snazzy.autoSnap && snazzy.autoSnapPos) GetOldPSR();;
		SnazzyWindowRepaint();
	}

	static void ToggleRotSnap()
	{
		snazzy.autoSnapRot = !snazzy.autoSnapRot;
		if (snazzy.autoSnap && snazzy.autoSnapRot) GetOldPSR();;
		SnazzyWindowRepaint();
	}

	static void ToggleScaleSnap()
	{
		snazzy.autoSnapScale = !snazzy.autoSnapScale;
		if (snazzy.autoSnap && snazzy.autoSnapScale) GetOldPSR();;
		SnazzyWindowRepaint();
	}
	

	static string GetSpecialKey(int index,KeyCode key,bool mov2RotKey)
	{
		string specialKey = "";
		if (key != KeyCode.None) specialKey += GetSpecialKey (index);		
		
		return specialKey+key;
	}

	static string GetSpecialKey(int index)
	{
		string specialKey = "";
		
		if ((index & 1) != 0) specialKey += "Shift-";
		if ((index & 2) != 0) specialKey += "Control-";	
		if ((index & 4) != 0) specialKey += "Alt-";
		
		return specialKey;
	}

	static void SnazzyWindowRepaint()
	{
		if (window == null) {window = GetWindow(typeof(SnazzyToolsEditor)) as SnazzyToolsEditor;}
		window.Repaint();	
	}
	
	static void GetOldPSR()
	{
		if (Selection.activeTransform != null) {
			if (!rotHandler) snazzy.oldPosition = Selection.activeTransform.position;
			snazzy.oldScale = Selection.activeTransform.localScale; 
			snazzy.oldRotation = Selection.activeTransform.eulerAngles;	
		}
	}
	
	static Vector3 GetCenterPosition()
	{
		Vector3 center = new Vector3(0,0,0);
		
		foreach (Transform transform in Selection.transforms) {
			center += transform.position;	
		}
		center /= Selection.transforms.Length;
		
		return center;
	}

//	static Vector3 getDirection(Vector3 direction,float rotation)
//	{
//		if (sceneCamera != null) {
//			camReference.transform.eulerAngles = sceneCamera.transform.eulerAngles;
//		}
//		Vector3 localRotation = camReference.transform.localEulerAngles;
//		localRotation.x = Mathf.DeltaAngle(0,localRotation.x);
//		notify = localRotation.x.ToString();
//		if (localRotation.x >= 0) {
//			if (localRotation.x >= rotation) {
//				direction.z *= -1; 
//			}
//			
//			if (localRotation.x < 45) localRotation.x += 45-rotation;
//		}
//		else {
//			if (localRotation.x <= -rotation) {
//				direction.z *= -1; 
//			}
//			if (localRotation.x > -45) localRotation.x -= 45-rotation;
//		}
//		camReference.transform.localEulerAngles = localRotation;
//		
//		SnapRot(camReference.transform,90,true,true,true);
//		return camReference.transform.TransformDirection(direction);
//	}
	
	static Vector3 getDirection(Vector3 direction,float rotation,float amount)
	{
		if (sceneCamera != null) {
			camReference.transform.eulerAngles = sceneCamera.transform.eulerAngles;
		}
		Vector3 localRotation = camReference.transform.localEulerAngles;
		bool yAxis = false;
		localRotation.x = Mathf.DeltaAngle(0,localRotation.x);
		
		if (Mathf.Abs (localRotation.x)-rotation > 0) {
			if (direction.y != 0) {direction.z = direction.y;direction.y = 0;}
			else if (direction.z != 0) {direction.y = direction.z;direction.z = 0;yAxis = true;}
			localRotation.x = 0;
			camReference.transform.localEulerAngles = localRotation;
			SnapRot(camReference.transform,90,false,true,false);
		}
		else {
			camReference.transform.localEulerAngles = localRotation;
			SnapRot(camReference.transform,90,true,true,true);
		}
		
		if (!yAxis) {
			Vector3 newDirection = SnapVector3 (camReference.transform.TransformDirection(direction),true,true,true,amount);
			return newDirection;
		}
		else 
			return direction;
	}

	bool GetHorizontalMode()
	{
		if (sceneCamera != null) {
			Vector3 localRotation = sceneCamera.transform.localEulerAngles;
			localRotation.x = Mathf.DeltaAngle(0,localRotation.x);
			
			if (localRotation.x >= 0) {
				if (localRotation.x >= snazzy.cameraAngleOffset) return true;
				else return false;
			}
			else {
				if (localRotation.x <= -snazzy.cameraAngleOffset) return true;
				else return false;
			}
		}
		return false;
	}	

	static void Move(Vector3 direction)
	{
		foreach (Transform transform in Selection.transforms) {
			transform.position += direction;	
		}
	}
	
	static void Rotate (float rotation)
	{
		if (Tools.pivotRotation == PivotRotation.Global) {
			foreach (Transform transform in Selection.transforms) 
			{
				switch (snazzy.rotAxis)	{
					case 0:
						transform.Rotate(new Vector3(rotation, 0, 0),Space.World);
						break;
					case 1:
						transform.Rotate(new Vector3(0, rotation, 0),Space.World);
						break;
					case 2:
						transform.Rotate(new Vector3(0, 0, rotation),Space.World);
						break;
				}
			}
		}
		else {
			foreach (Transform transform in Selection.transforms) 
			{
				switch (snazzy.rotAxis)
				{
					case 0:
						transform.Rotate(new Vector3(rotation, 0, 0),Space.Self);
						break;
					case 1:
						transform.Rotate(new Vector3(0, rotation, 0),Space.Self);
						break;
					case 2:
						transform.Rotate(new Vector3(0, 0, rotation),Space.Self);
						break;
				}
			}
		}
		ClampRotation();
	}
	
	static void SnapRot(float rotation,bool x,bool y,bool z)
	{
		foreach (Transform transform in Selection.transforms) {
			SnapRot(transform,rotation,x,y,z);
		}
		ResetHandler ();
	}

	static void UnityRotate(float rotation)
	{	
		Vector3 rotPoint = new Vector3(0,0,0);
		Vector3 rotAxis = new Vector3(0,0,0);

		if (Tools.pivotRotation == PivotRotation.Global) {
			if (snazzy.rotAxis == 0) rotAxis = Vector3.right;
			else if (snazzy.rotAxis == 1) rotAxis = Vector3.up;
			else if (snazzy.rotAxis == 2) rotAxis = Vector3.forward;
		}
		else {
			if (snazzy.rotAxis == 0) rotAxis = Selection.activeTransform.right;
			else if (snazzy.rotAxis == 1) rotAxis = Selection.activeTransform.up;
			else if (snazzy.rotAxis == 2) rotAxis = Selection.activeTransform.forward;
		}

		rotPoint = Tools.handlePosition;
		
		foreach (Transform transform in Selection.transforms) {
			transform.RotateAround(rotPoint,rotAxis,rotation);
		}
		ClampRotation();
	}

	static void ClampRotation()
	{
		Vector3 rotation;
		foreach (Transform transform in Selection.transforms) {
			rotation = transform.localEulerAngles;
			rotation.x = Mathf.DeltaAngle(0,rotation.x);
			rotation.y = Mathf.DeltaAngle(0,rotation.y);
			rotation.z = Mathf.DeltaAngle(0,rotation.z);
			transform.localEulerAngles = rotation;
		}
	}

	static float DeltaAngle(float angle)
	{
		if (angle > 0) while (angle > 360) angle -= 360;
		else while (angle < -360) angle += 360;
		return angle;
	}
	
	static void SnapScale(bool x,bool y,bool z)
	{
		Vector3 scale;
		
		foreach (Transform transform in Selection.transforms) 
		{
			scale = SnapVector3(transform.localScale,x,y,z,snazzy.gridSize);
			if (scale.x < snazzy.gridSize) {scale.x = snazzy.gridSize;}
			if (scale.y < snazzy.gridSize) {scale.y = snazzy.gridSize;}
			if (scale.z < snazzy.gridSize) {scale.z = snazzy.gridSize;}
			
			transform.localScale = scale;
		}
	}

	static void SnapRot(Transform transform,float rotation,bool snapX,bool snapY,bool snapZ)
	{
		Vector3 snapRotation = new Vector3();
		
		if (snapX) snapRotation.x = Mathf.Round(transform.eulerAngles.x/rotation)*rotation; else snapRotation.x = transform.eulerAngles.x;
		if (snapY) snapRotation.y = Mathf.Round(transform.eulerAngles.y/rotation)*rotation; else snapRotation.y = transform.eulerAngles.y;
		if (snapZ) snapRotation.z = Mathf.Round(transform.eulerAngles.z/rotation)*rotation; else snapRotation.z = transform.eulerAngles.z;
		
		transform.localEulerAngles = snapRotation;
	}

	static Vector3 SnapVector3(Vector3 snapVector3,bool snapX,bool snapY,bool snapZ,float amount)
	{
		if (snapX) snapVector3.x = Mathf.Round(snapVector3.x/amount)*amount; 
		if (snapY) snapVector3.y = Mathf.Round(snapVector3.y/amount)*amount; 
		if (snapZ) snapVector3.z = Mathf.Round(snapVector3.z/amount)*amount; 
		
		return snapVector3;
	}

	static Vector3 GetSnapRot(Quaternion rotation)
	{
		Vector3 snapRotation = new Vector3();

		snapRotation.x = Mathf.Round(rotation.eulerAngles.x/snazzy.rotIncrement)*snazzy.rotIncrement;
		snapRotation.y = Mathf.Round(rotation.eulerAngles.y/snazzy.rotIncrement)*snazzy.rotIncrement;
		snapRotation.z = Mathf.Round(rotation.eulerAngles.z/snazzy.rotIncrement)*snazzy.rotIncrement;

		return snapRotation;
	}

	static void ResetPosition (bool x,bool y,bool z)
	{
		Vector3 position = new Vector3();
		#if !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
		Undo.RecordObjects(Selection.transforms,"Selection Reset Position");
		#else
		Undo.RegisterUndo(Selection.transforms,"Selection Reset Position");
		#endif
		
		foreach (Transform transform in Selection.transforms) {
			position = transform.position;
			if (x) position.x = 0; 
			if (y) position.y = 0; 
			if (z) position.z = 0; 
			transform.localPosition = position;
		}
	}
	
	static void ResetRotation (bool x,bool y,bool z)
	{
		Vector3 rotation = new Vector3();
		#if !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
		Undo.RecordObjects(Selection.transforms,"Selection Reset Rotation");
		#else
		Undo.RegisterUndo(Selection.transforms,"Selection Reset Rotation");		
		#endif

		foreach (Transform transform in Selection.transforms) {
			rotation = transform.localEulerAngles;
			if (x) rotation.x = 0; 
			if (y) rotation.y = 0; 
			if (z) rotation.z = 0; 
			transform.localEulerAngles = rotation;
		}
		
		ResetHandler ();
	}
	
	static void ResetHandler ()
	{
		if (Selection.activeTransform != null) {
			if (Tools.pivotRotation == PivotRotation.Global) 
				Tools.handleRotation = Quaternion.identity;
			else Tools.handleRotation = Selection.activeTransform.rotation;	
		}
	}
	
	static void ResetScale (bool x,bool y,bool z)
	{
		Vector3 scale = new Vector3();
		
		#if !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
		Undo.RecordObjects(Selection.transforms,"Selection Reset Scale");
		#else
		Undo.RegisterUndo(Selection.transforms,"Selection Reset Scale");
		#endif
		foreach (Transform transform in Selection.transforms) {
			scale = transform.localScale;
			if (x) scale.x = 1; 
			if (y) scale.y = 1; 
			if (z) scale.z = 1; 
			
			transform.localScale = scale;
		}
	}
	
	static void SnapPos(bool x,bool y,bool z)
	{
		#if !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
		Undo.RecordObjects(Selection.transforms,"Selection Snap Transform");
		#else
		Undo.RegisterUndo(Selection.transforms,"Selection Snap Transform");
		#endif
		foreach (Transform transform in Selection.transforms) {
			transform.position = SnapVector3(transform.position,x,y,z,snazzy.gridSize);
		}
	}

	static void MoveSelectionLeft()
	{
		#if !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
		Undo.RecordObjects(Selection.transforms,"Selection Move Left");
		#else
		Undo.RegisterUndo(Selection.transforms,"Selection Move Left");
		#endif
		Move (getDirection(new Vector3(-snazzy.moveAmount,0,0),0,snazzy.moveAmount));
	}
	
	static void MoveSelectionRight()
	{
		#if !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
		Undo.RecordObjects(Selection.transforms,"Selection Move Right");
		#else
		Undo.RegisterUndo(Selection.transforms,"Selection Move Right");
		#endif
		Move (getDirection(new Vector3(snazzy.moveAmount,0,0),0,snazzy.moveAmount));
	}
	
	static void MoveSelectionUp()
	{
		#if !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
		Undo.RecordObjects(Selection.transforms,"Selection Move Up");
		#else
		Undo.RegisterUndo(Selection.transforms,"Selection Move Up");
		#endif
		Move (getDirection(new Vector3(0,snazzy.moveAmount,0),snazzy.cameraAngleOffset,snazzy.moveAmount));	
	}
	
	static void MoveSelectionDown()
	{
		#if !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
		Undo.RecordObjects(Selection.transforms,"Selection Move Down");
		#else
		Undo.RegisterUndo(Selection.transforms,"Selection Move Down");
		#endif
		Move (getDirection(new Vector3(0,-snazzy.moveAmount,0),snazzy.cameraAngleOffset,snazzy.moveAmount));
	}
	
	static void MoveSelectionForward()
	{
		#if !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
		Undo.RecordObjects(Selection.transforms,"Selection Move Forward");
		#else
		Undo.RegisterUndo(Selection.transforms,"Selection Move Forward");
		#endif
		Move (getDirection(new Vector3(0,0,snazzy.moveAmount),snazzy.cameraAngleOffset,snazzy.moveAmount));
	}
	
	static void MoveSelectionBack()
	{
		#if !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
		Undo.RecordObjects(Selection.transforms,"Selection Move Back");
		#else
		Undo.RegisterUndo(Selection.transforms,"Selection Move Back");
		#endif
		Move (getDirection(new Vector3(0,0,-snazzy.moveAmount),snazzy.cameraAngleOffset,snazzy.moveAmount));
	}
	
	static void RotateSelectionLeft()
	{
		#if !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
		Undo.RecordObjects(Selection.transforms,"Selection Rotate Left");
		#else
		Undo.RegisterUndo(Selection.transforms,"Selection Rotate Left");
		#endif
		if (!snazzy.rotationMode) UnityRotate (-snazzy.rotAmount); else Rotate (-snazzy.rotAmount); // RotateLeft
		if (snazzy.autoSnap && snazzy.autoSnapRot) {
			SnapRot(snazzy.rotAmount,snazzy.autoSnapRotX,snazzy.autoSnapRotY,snazzy.autoSnapRotZ);
		}		
	}
	
	static void RotateSelectionRight()
	{
		#if !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
		Undo.RecordObjects(Selection.transforms,"Selection Rotate Right");
		#else
		Undo.RegisterUndo(Selection.transforms,"Selection Rotate Right");
		#endif
		if (!snazzy.rotationMode) UnityRotate (snazzy.rotAmount); else Rotate (snazzy.rotAmount); // RotateRight	
		if (snazzy.autoSnap && snazzy.autoSnapRot) {
			SnapRot(snazzy.rotAmount,snazzy.autoSnapRotX,snazzy.autoSnapRotY,snazzy.autoSnapRotZ);
		}	
	}
	
	static void DuplicateSelection(bool scene)
	{
		if (!scene) EditorApplication.ExecuteMenuItem("Window/Hierarchy");
		EditorWindow.focusedWindow.SendEvent(EditorGUIUtility.CommandEvent("Duplicate"));
	}

	static void SelectParent()
	{
		if (Selection.activeTransform != null) {
			#if !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
			Undo.RecordObjects(Selection.transforms,"Select Parent");
			#else
			Undo.RegisterUndo(Selection.transforms,"Select Parent");
			#endif
			List<GameObject> parents = new List<GameObject>();
			bool add = false;

			foreach (Transform transform in Selection.transforms) {
				if (transform.parent != null) {
					add = true;
					foreach (GameObject parent in parents) {
						if (parent == transform.parent.gameObject) {add = false;break;}
					}
					if (add) parents.Add(transform.parent.gameObject);
				}
				else parents.Add (transform.gameObject);
			}
			Selection.objects = parents.ToArray();
		}	
	}
	
	static void ParentSelection()
	{
		if (Selection.transforms.Length > 0) { 
			GameObject obj = new GameObject();
			obj.name = "GameObject";
			Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name); 	 
			obj.transform.parent = Selection.activeTransform.parent;
			
			foreach (Transform transform in Selection.transforms) {  
				#if !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
				Undo.SetTransformParent (transform, obj.transform,"Change Parent "+transform.name);  
				#else
				Undo.RegisterSetTransformParentUndo (transform, obj.transform,"Change Parent "+transform.name);  
				transform.parent = obj.transform;
				#endif
			}
		
			Selection.activeTransform = obj.transform;	
		}
	}

	static void ParentSelectionToCenter()
	{
		if (Selection.transforms.Length > 0) { 
			GameObject obj = new GameObject();
			obj.name = "GameObject";
			Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name); 	 
			obj.transform.parent = Selection.activeTransform.parent;
			
			foreach (Transform transform in Selection.transforms) {  
				#if !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
				Undo.SetTransformParent (transform, obj.transform,"Change Parent "+transform.name);  
				#else
				Undo.RegisterSetTransformParentUndo (transform, obj.transform,"Change Parent "+transform.name);  
				transform.parent = obj.transform;
				#endif
			}
			MoveParentToCenter(obj.transform);
			
			Selection.activeTransform = obj.transform;	
		}
	}

	static void SelectChildren()
	{
		if (Selection.activeTransform != null) {
			#if !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
			Undo.RecordObjects(Selection.transforms,"Select Children");
			#else
			Undo.RegisterUndo(Selection.transforms,"Select Children");
			#endif
			List<GameObject> transforms = new List<GameObject>();
			foreach (Transform transform in Selection.transforms) {
				foreach (Transform transform1 in transform) {
					transforms.Add(transform1.gameObject);
				}
			}
			if (transforms.Count > 0) Selection.objects = transforms.ToArray();
		}	
	}
	
	static void UnparentSelection()
	{
		if (Selection.activeTransform != null) {
			foreach (Transform transform in Selection.transforms) {
				#if !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
				Undo.SetTransformParent (transform, null,"Parent change "+transform.name);  
				#else
				Undo.RegisterSetTransformParentUndo (transform, null,"Parent change "+transform.name);  
				transform.parent = null;
				#endif
			}
		}
	}

//	static void UnparentSelectionDelete()
//	{
//		if (Selection.activeTransform != null) {
//			#if !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
//			#else
//			Undo.RegisterSceneUndo("Parent Change");
//			#endif
//			
//			Transform parent = null;
//			if (Selection.activeTransform.parent != null) {
//				parent = Selection.activeTransform.parent;
//			}
//			else {
//				notify = "Selected object has no parent";
//				SceneView.RepaintAll();
//				return;
//			}	
//			foreach (Transform transform in Selection.transforms) {
//				#if !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
//				Undo.SetTransformParent (transform, null,"Parent change "+transform.name);  
//				#else
//				transform.parent = null;
//				#endif
//			}
//			if (parent != null) {
//				#if !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
//				GameObject tempObj;
//				
////				for (int i = 0;i < parent.childCount;++i) {
////					tempObj = parent.GetChild (0).gameObject;
////					//Undo.SetTransformParent (tempObj, null,"Parent change "); 
////					Undo.DestroyObjectImmediate(tempObj);
////					--i;
////				}
//				Undo.SetTransformParent (parent,Selection.activeTransform,"Parent change "+Selection.activeTransform.name);
//				//Undo.DestroyObjectImmediate(parent);
//				#else
//				DestroyImmediate(parent.gameObject);
//				#endif
//			}  
//		}
//	}

	static void MoveParentToCenter(Transform parent)
	{
		if (Selection.activeTransform == null) {
			notify = "Nothing selected";
			return;
		}
		if (parent == null) {
			notify = "Active selection does not have a parent";
			return;
		}
		
		#if !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
		Undo.RecordObjects(Selection.transforms,"Move parent to center");
		#else
		Undo.RegisterUndo(Selection.transforms,"Move parent to center");
		#endif
		Vector3 center = new Vector3(0,0,0);
		List<Vector3> oldPositions = new List<Vector3>();
		
		foreach (Transform transform in parent) {
			oldPositions.Add(transform.position);
		}
		
		center = GetCenterPosition ();
		
		parent.position = center;
		if (snazzy.autoSnapPos) {
			parent.position = SnapVector3(parent.position,snazzy.autoSnapPosX,snazzy.autoSnapPosY,snazzy.autoSnapPosY,snazzy.moveAmount);	
		}
		
		int i = 0;
		foreach (Transform transform in parent) {
			transform.position = oldPositions[i++];
		}	

		notify = parent.name+" moved to center of selection";	
	}
	
	static void UnparentChildren()
	{
		if (Selection.activeTransform != null) {
			if (Selection.transforms.Length > 1) {notify = "ChildCompensation works only with 1 selected Parent";}
			if (Selection.activeTransform.childCount > 0) {	
				_parent = new GameObject();  
				_parent.name = "##TempParent##";
				Undo.RegisterCreatedObjectUndo(_parent, "Create " + _parent.name); 
				
				int childCount = Selection.activeTransform.childCount;				

				for (int i = 0;i < childCount;++i) {
					#if !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
					Undo.SetTransformParent (Selection.activeTransform.GetChild(childCount-i-1),_parent.transform,"Change Parent "+Selection.activeTransform.GetChild(childCount-i-1).name);	
					#else
					Undo.RegisterSetTransformParentUndo (Selection.activeTransform.GetChild(childCount-i-1),_parent.transform,"Change Parent "+Selection.activeTransform.GetChild(childCount-i-1).name);	
					Selection.activeTransform.GetChild(childCount-i-1).parent = _parent.transform;
					#endif
				}
				snazzy.lockChild = true;
			}
		}
	}
	
	static void ParentChildren()
	{
		if (_parent != null) {
			#if !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
			#else		
			Undo.RegisterSceneUndo ("Change Parent");
			#endif
			int childCount =_parent.transform.childCount;				
			
			for (int i = 0;i < childCount;++i) {
				#if !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
				Undo.SetTransformParent (_parent.transform.GetChild(childCount-i-1),snazzy.oldTransform,"Change Parent "+_parent.transform.GetChild(childCount-i-1).name);
				#else
				_parent.transform.GetChild(childCount-i-1).parent = snazzy.oldTransform;
				#endif
			}
		
			#if !UNITY_3_4 && !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
			Undo.DestroyObjectImmediate(_parent);
			#else
			DestroyImmediate(_parent);
			#endif
		}
		snazzy.lockChild = false;
		SnazzyWindowRepaint();
	}

	static public void Key(Event key,bool fromScene)
	{
		if (Application.isPlaying) return;
		//Debug.Log("Do something special here!");

		keySnap = false;
		if (!key.isKey && key.type == EventType.Repaint) snapLock = false;
		if (key.type == EventType.KeyUp) {
			snazzy.keyDown = false;
			snazzy.lockKeys = false;
			snazzy.snapKeyDown = false;
			snazzy.snapPosKeyDown = false;
			snazzy.snapRotKeyDown = false;
			snazzy.snapScaleKeyDown = false;
			// Debug.Log ("keyup");
		}

		int specialKeyPressed = 0;
		if (key.shift) specialKeyPressed += 1;
		if (key.control) specialKeyPressed += 2;
		if (key.alt) specialKeyPressed += 4;

		if (snazzy.mov2) snazzy.moveAmount = snazzy.gridIncrement2*snazzy.gridSize; else snazzy.moveAmount = snazzy.gridIncrement*snazzy.gridSize;
		if (snazzy.angle2) snazzy.rotAmount = snazzy.rotIncrement2; else snazzy.rotAmount = snazzy.rotIncrement;
				
		if (key.type == EventType.keyDown) {
			
			// Repeat Keys
			if (!snazzy.lockKeys) {
				if (key.keyCode == snazzy.leftKey && snazzy.leftKey != KeyCode.None && snazzy.leftKeySpecial == specialKeyPressed) {
					MoveSelectionLeft();	
					key.Use();
					keySnap = true;
				}
				if (key.keyCode == snazzy.rightKey && snazzy.rightKey != KeyCode.None && snazzy.rightKeySpecial == specialKeyPressed) {
					MoveSelectionRight();
					key.Use();
					keySnap = true;
				}
				if (key.keyCode == snazzy.upKey && snazzy.upKey != KeyCode.None && snazzy.upKeySpecial == specialKeyPressed) {
					MoveSelectionUp();
					key.Use();
					keySnap = true;
				}
				if (key.keyCode == snazzy.downKey && snazzy.downKey != KeyCode.None && snazzy.downKeySpecial == specialKeyPressed) {
					MoveSelectionDown();
					key.Use();
					keySnap = true;
				}
				if (key.keyCode == snazzy.frontKey && snazzy.frontKey != KeyCode.None && snazzy.frontKeySpecial == specialKeyPressed) {
					MoveSelectionForward();
					key.Use();
					keySnap = true;
				}
				if (key.keyCode == snazzy.backKey && snazzy.backKey != KeyCode.None && snazzy.backKeySpecial == specialKeyPressed) {
					MoveSelectionBack();
					key.Use();
					keySnap = true;
				}
				if (key.keyCode == snazzy.rotLeftKey && snazzy.rotLeftKey != KeyCode.None && snazzy.rotLeftKeySpecial == specialKeyPressed) {
					RotateSelectionLeft();
					key.Use();
					keySnap = true;
				}
				if (key.keyCode == snazzy.rotRightKey && snazzy.rotRightKey != KeyCode.None && snazzy.rotRightKeySpecial == specialKeyPressed) {
					RotateSelectionRight();
					key.Use();
					keySnap = true;
				}
			
				// Non repeat Keys
				if (snazzy.keyDown) return;
				snazzy.keyDown = true;
				// Debug.Log ("keydown");	
				if (key.keyCode == snazzy.gridKey && snazzy.gridKey != KeyCode.None && snazzy.gridKeySpecial == specialKeyPressed) {
					snazzy.showGrid = !snazzy.showGrid;
					SceneView.RepaintAll();
					SnazzyWindowRepaint();
				}
				if (key.keyCode == snazzy.movToggleKey && snazzy.movToggleKey != KeyCode.None && snazzy.movToggleKeySpecial == specialKeyPressed) {
					snazzy.mov2 = !snazzy.mov2;
					SnazzyWindowRepaint();
				}
				if (key.keyCode == snazzy.angleToggleKey && snazzy.angleToggleKey != KeyCode.None && snazzy.angleToggleKeySpecial == specialKeyPressed) {
					snazzy.angle2 = !snazzy.angle2;
					SnazzyWindowRepaint();
				}
				if (key.keyCode == snazzy.rotXYZKey && snazzy.rotXYZKey != KeyCode.None && snazzy.rotXYZKeySpecial == specialKeyPressed) {
					++snazzy.rotAxis;
					if (snazzy.rotAxis > 2) snazzy.rotAxis = 0;
					SnazzyWindowRepaint();
				}
				if (key.keyCode == snazzy.duplicateKey && snazzy.duplicateKey != KeyCode.None && snazzy.duplicateKeySpecial == specialKeyPressed) {
					DuplicateSelection(fromScene);
				}
				if (key.keyCode == snazzy.focusKey && snazzy.focusKey != KeyCode.None && snazzy.focusKeySpecial == specialKeyPressed) {
					snazzy.lookAtSelect = !snazzy.lookAtSelect;
					SnazzyWindowRepaint();
				}

				if (key.keyCode == snazzy.snapKey && snazzy.snapKey != KeyCode.None && snazzy.snapKeySpecial == specialKeyPressed) {
					if (snazzy.snapKeyTapped == 0) ToggleSnap();
					++snazzy.snapKeyTapped;
					if (snazzy.snapKeyTapped > 1) {
						SnapPos(true,true,true);
						SnapRot(snazzy.rotAmount,true,true,true);
						SnapScale(true,true,true);
						
						ToggleSnap ();
						snazzy.snapKeyTapped = 0;
					} else {
						snazzy.snapKeyDown = true;
						snazzy.snapKeyDownStart = Time.realtimeSinceStartup;		
					}
				}
				
				if (key.keyCode == snazzy.snapPosKey && snazzy.snapPosKey != KeyCode.None && snazzy.snapPosKeySpecial == specialKeyPressed) {
					if (snazzy.snapPosKeyTapped == 0) ToggleMovSnap();
					++snazzy.snapPosKeyTapped;
					if (snazzy.snapPosKeyTapped > 1) {
						SnapPos(true,true,true);
						
						ToggleMovSnap ();
						snazzy.snapPosKeyTapped = 0;
					} else {
						snazzy.snapPosKeyDown = true;
						snazzy.snapPosKeyDownStart = Time.realtimeSinceStartup;		
					}
				}
				if (key.keyCode == snazzy.snapRotKey && snazzy.snapRotKey != KeyCode.None && snazzy.snapRotKeySpecial == specialKeyPressed) {
					if (snazzy.snapRotKeyTapped == 0) ToggleRotSnap();
					++snazzy.snapRotKeyTapped;
					if (snazzy.snapRotKeyTapped > 1) {
						SnapRot(snazzy.rotAmount,true,true,true);
						
						ToggleRotSnap ();
						snazzy.snapRotKeyTapped = 0;
					} else {
						snazzy.snapRotKeyDown = true;
						snazzy.snapRotKeyDownStart = Time.realtimeSinceStartup;		
					}
				}
				if (key.keyCode == snazzy.snapScaleKey && snazzy.snapScaleKey != KeyCode.None && snazzy.snapScaleKeySpecial == specialKeyPressed) {
					if (snazzy.snapScaleKeyTapped == 0) ToggleScaleSnap();
					++snazzy.snapScaleKeyTapped;
					if (snazzy.snapScaleKeyTapped > 1) {
						SnapScale(true,true,true);
						
						ToggleScaleSnap ();
						snazzy.snapScaleKeyTapped = 0;
					} else {
						snazzy.snapScaleKeyDown = true;
						snazzy.snapScaleKeyDownStart = Time.realtimeSinceStartup;		
					}
				}
				if (key.keyCode == snazzy.resetTransformKey && snazzy.resetTransformKey != KeyCode.None && snazzy.resetTransformKeySpecial == specialKeyPressed) {
					ResetPosition (true,true,true);
					ResetRotation (true,true,true);
					ResetScale (true,true,true);
				}
				if (key.keyCode == snazzy.resetPositionKey && snazzy.resetPositionKey != KeyCode.None && snazzy.resetPositionKeySpecial == specialKeyPressed) {
					ResetPosition (true,true,true);
				}
				if (key.keyCode == snazzy.resetRotationKey && snazzy.resetRotationKey != KeyCode.None && snazzy.resetRotationKeySpecial == specialKeyPressed) {
					ResetRotation (true,true,true);
				}
				if (key.keyCode == snazzy.resetScaleKey && snazzy.resetScaleKey != KeyCode.None && snazzy.resetScaleKeySpecial == specialKeyPressed) {
					ResetScale (true,true,true);
				}
				if (key.keyCode == snazzy.childCompensationKey && snazzy.childCompensationKey != KeyCode.None && snazzy.childCompensationKeySpecial == specialKeyPressed) {  
					if (!snazzy.lockChild) UnparentChildren(); else ParentChildren();
				}
				if (key.keyCode == snazzy.parentKey && snazzy.parentKey != KeyCode.None && snazzy.parentKeySpecial == specialKeyPressed) {  
					ParentSelection();
				}
	
				if (key.keyCode == snazzy.parentToCenterKey && snazzy.parentToCenterKey != KeyCode.None && snazzy.parentToCenterKeySpecial == specialKeyPressed) {
					ParentSelectionToCenter();
				}
				
				if (key.keyCode == snazzy.centerCurrentParentKey && snazzy.centerCurrentParentKey != KeyCode.None && snazzy.parentKeySpecial == snazzy.centerCurrentParentKeySpecial) {  
					if (Selection.activeTransform.parent != null) MoveParentToCenter(Selection.activeTransform.parent);
				}
				
				if (key.keyCode == snazzy.hierarchyUpKey && snazzy.hierarchyUpKey != KeyCode.None && snazzy.hierarchyUpKeySpecial == specialKeyPressed) {
					SelectParent();
				} 
				
				if (key.keyCode == snazzy.hierarchyDownKey && snazzy.hierarchyDownKey != KeyCode.None && snazzy.hierarchyDownKeySpecial == specialKeyPressed) {
					SelectChildren();
				}

				if (key.keyCode == snazzy.unparentKey && snazzy.unparentKey != KeyCode.None && snazzy.unparentKeySpecial == specialKeyPressed) {
					UnparentSelection();
				}

//				if (key.keyCode == snazzy.unparentDeleteKey && snazzy.unparentDeleteKey != KeyCode.None && snazzy.unparentDeleteKeySpecial == specialKeyPressed) {
//					UnparentSelectionDelete();
//				}
			}
		}
		CheckSnap();
	}
	public Texture BackGround;
	Texture AngleLabel,AnglePreset1,AnglePreset2,CCompToggleOff,CCompToggleOn,Duplicate,GridSizePreset,GridToggleOff,GridToggleOn,HierarchyDown,HierarchyUp,IncrementLabel,IncrementPreset1,ManualHover,ForumHover,
		IncrementPreset2,MoveBack,MoveDown,MoveForward,MoveLeft,MoveRight,MoveUp,Parent,PositionToggleOff,PositionToggleOn,PositionXToggleOff,PositionXToggleOn,PositionYToggleOff,PositionYToggleOn,
		PositionZToggleOff,PositionZToggleOn,RotateLeft,RotateRight,RotationToggleOff,RotationToggleOn,RotationXToggleOff,RotationXToggleOn,RotationYToggleOn,RotationYToggleOff,RotationZToggleOff,RotationZToggleOn,ScaleToggleOff,
		ScaleToggleOn,ScaleXToggleOff,ScaleXToggleOn,ScaleYToggleOff,ScaleYToggleOn,ScaleZToggleOff,ScaleZToggleOn,SettingsToggleOff,SettingsToggleOn,SnapToggleOff,SnapToggleOn,SnazzyTitle,Spacer,Unparent,
		SnapMask,XYZMask,FocusToggleOff,FocusToggleOn,IncrementPreset1ToggleOff,IncrementPreset2ToggleOff,AnglePreset1ToggleOff,AnglePreset2ToggleOff,RotationModeToggleOn,RotationModeToggleOff,Quicktips,GradientFade;
	
	void LoadButtons()
	{
		string path = "Assets/"+installPath+"SnazzyTools/SnazzyGrid/Resources/GUI/";
		

		BackGround = AssetDatabase.LoadAssetAtPath(path+"BG.jpg",typeof(Texture)) as Texture;
		ManualHover = AssetDatabase.LoadAssetAtPath(path+"ManualHover.psd",typeof(Texture)) as Texture;
		ForumHover = AssetDatabase.LoadAssetAtPath(path+"ForumHover.psd",typeof(Texture)) as Texture;
		
		IncrementPreset1ToggleOff = AssetDatabase.LoadAssetAtPath(path+"IncrementPreset1ToggleOff.png",typeof(Texture)) as Texture;
		IncrementPreset2ToggleOff = AssetDatabase.LoadAssetAtPath(path+"IncrementPreset2ToggleOff.png",typeof(Texture)) as Texture;
		AnglePreset1ToggleOff = AssetDatabase.LoadAssetAtPath(path+"AnglePreset1ToggleOff.png",typeof(Texture)) as Texture;
		AnglePreset2ToggleOff = AssetDatabase.LoadAssetAtPath(path+"AnglePreset2ToggleOff.png",typeof(Texture)) as Texture;
		
		AngleLabel = AssetDatabase.LoadAssetAtPath(path+"AngleLabel.png",typeof(Texture)) as Texture;
		AnglePreset1 = AssetDatabase.LoadAssetAtPath(path+"AnglePreset1.png",typeof(Texture)) as Texture;
		AnglePreset2 = AssetDatabase.LoadAssetAtPath(path+"AnglePreset2.png",typeof(Texture)) as Texture;
		CCompToggleOff = AssetDatabase.LoadAssetAtPath(path+"CCompToggleOff.png",typeof(Texture)) as Texture;
		CCompToggleOn = AssetDatabase.LoadAssetAtPath(path+"CCompToggleOn.png",typeof(Texture)) as Texture;
		Duplicate = AssetDatabase.LoadAssetAtPath(path+"Duplicate.png",typeof(Texture)) as Texture;
		GridSizePreset = AssetDatabase.LoadAssetAtPath(path+"GridSizePreset.png",typeof(Texture)) as Texture;
		GridToggleOff = AssetDatabase.LoadAssetAtPath(path+"GridToggleOff.png",typeof(Texture)) as Texture;
		GridToggleOn = AssetDatabase.LoadAssetAtPath(path+"GridToggleOn.png",typeof(Texture)) as Texture;
		HierarchyDown = AssetDatabase.LoadAssetAtPath(path+"HierarchyDown.png",typeof(Texture)) as Texture;
		HierarchyUp = AssetDatabase.LoadAssetAtPath(path+"HierarchyUp.png",typeof(Texture)) as Texture;
		IncrementLabel = AssetDatabase.LoadAssetAtPath(path+"IncrementLabel.png",typeof(Texture)) as Texture;
		IncrementPreset1 = AssetDatabase.LoadAssetAtPath(path+"IncrementPreset1.png",typeof(Texture)) as Texture;
		IncrementPreset2 = AssetDatabase.LoadAssetAtPath(path+"IncrementPreset2.png",typeof(Texture)) as Texture;
		MoveBack = AssetDatabase.LoadAssetAtPath(path+"MoveBack.png",typeof(Texture)) as Texture;
		MoveDown = AssetDatabase.LoadAssetAtPath(path+"MoveDown.png",typeof(Texture)) as Texture;
		MoveForward = AssetDatabase.LoadAssetAtPath(path+"MoveForward.png",typeof(Texture)) as Texture;
		MoveLeft = AssetDatabase.LoadAssetAtPath(path+"MoveLeft.png",typeof(Texture)) as Texture;
		MoveRight = AssetDatabase.LoadAssetAtPath(path+"MoveRight.png",typeof(Texture)) as Texture;
		MoveUp = AssetDatabase.LoadAssetAtPath(path+"MoveUp.png",typeof(Texture)) as Texture;
		Parent = AssetDatabase.LoadAssetAtPath(path+"Parent.png",typeof(Texture)) as Texture;
		PositionToggleOff = AssetDatabase.LoadAssetAtPath(path+"PositionToggleOff.png",typeof(Texture)) as Texture;
		PositionToggleOn = AssetDatabase.LoadAssetAtPath(path+"PositionToggleOn.png",typeof(Texture)) as Texture;
		PositionXToggleOff = AssetDatabase.LoadAssetAtPath(path+"PositionXToggleOff.png",typeof(Texture)) as Texture;
		PositionXToggleOn = AssetDatabase.LoadAssetAtPath(path+"PositionXToggleOn.png",typeof(Texture)) as Texture;
		PositionYToggleOff = AssetDatabase.LoadAssetAtPath(path+"PositionYToggleOff.png",typeof(Texture)) as Texture;
		PositionYToggleOn = AssetDatabase.LoadAssetAtPath(path+"PositionYToggleOn.png",typeof(Texture)) as Texture;
		PositionZToggleOff = AssetDatabase.LoadAssetAtPath(path+"PositionZToggleOff.png",typeof(Texture)) as Texture;
		PositionZToggleOn = AssetDatabase.LoadAssetAtPath(path+"PositionZToggleOn.png",typeof(Texture)) as Texture;
		RotateLeft = AssetDatabase.LoadAssetAtPath(path+"RotateLeft.png",typeof(Texture)) as Texture;
		RotateRight = AssetDatabase.LoadAssetAtPath(path+"RotateRight.png",typeof(Texture)) as Texture;
		RotationToggleOff = AssetDatabase.LoadAssetAtPath(path+"RotationToggleOff.png",typeof(Texture)) as Texture;
		RotationToggleOn = AssetDatabase.LoadAssetAtPath(path+"RotationToggleOn.png",typeof(Texture)) as Texture;
		RotationXToggleOff = AssetDatabase.LoadAssetAtPath(path+"RotationXToggleOff.png",typeof(Texture)) as Texture;
		RotationXToggleOn = AssetDatabase.LoadAssetAtPath(path+"RotationXToggleOn.png",typeof(Texture)) as Texture;
		RotationYToggleOn = AssetDatabase.LoadAssetAtPath(path+"RotationYToggleOn.png",typeof(Texture)) as Texture;
		RotationYToggleOff = AssetDatabase.LoadAssetAtPath(path+"RotationYToggleOff.png",typeof(Texture)) as Texture;
		RotationZToggleOff = AssetDatabase.LoadAssetAtPath(path+"RotationZToggleOff.png",typeof(Texture)) as Texture;
		RotationZToggleOn = AssetDatabase.LoadAssetAtPath(path+"RotationZToggleOn.png",typeof(Texture)) as Texture;
		ScaleToggleOff = AssetDatabase.LoadAssetAtPath(path+"ScaleToggleOff.png",typeof(Texture)) as Texture;
		ScaleToggleOn = AssetDatabase.LoadAssetAtPath(path+"ScaleToggleOn.png",typeof(Texture)) as Texture;
		ScaleXToggleOff = AssetDatabase.LoadAssetAtPath(path+"ScaleXToggleOff.png",typeof(Texture)) as Texture;
		ScaleXToggleOn = AssetDatabase.LoadAssetAtPath(path+"ScaleXToggleOn.png",typeof(Texture)) as Texture;
		ScaleYToggleOff = AssetDatabase.LoadAssetAtPath(path+"ScaleYToggleOff.png",typeof(Texture)) as Texture;
		ScaleYToggleOn = AssetDatabase.LoadAssetAtPath(path+"ScaleYToggleOn.png",typeof(Texture)) as Texture;
		ScaleZToggleOff = AssetDatabase.LoadAssetAtPath(path+"ScaleZToggleOff.png",typeof(Texture)) as Texture;
		ScaleZToggleOn = AssetDatabase.LoadAssetAtPath(path+"ScaleZToggleOn.png",typeof(Texture)) as Texture;
		SettingsToggleOn = AssetDatabase.LoadAssetAtPath(path+"SettingsToggleOn.png",typeof(Texture)) as Texture;
		SettingsToggleOff = AssetDatabase.LoadAssetAtPath(path+"SettingsToggleOff.png",typeof(Texture)) as Texture;
		SnapToggleOff = AssetDatabase.LoadAssetAtPath(path+"SnapToggleOff.png",typeof(Texture)) as Texture;
		SnapToggleOn = AssetDatabase.LoadAssetAtPath(path+"SnapToggleOn.png",typeof(Texture)) as Texture;
		SnazzyTitle = AssetDatabase.LoadAssetAtPath(path+"SnazzyTitle.png",typeof(Texture)) as Texture;
		Spacer = AssetDatabase.LoadAssetAtPath(path+"Spacer.png",typeof(Texture)) as Texture;
		Unparent = AssetDatabase.LoadAssetAtPath(path+"Unparent.png",typeof(Texture)) as Texture;
		FocusToggleOff = AssetDatabase.LoadAssetAtPath(path+"FocusToggleOff.png",typeof(Texture)) as Texture;
		FocusToggleOn = AssetDatabase.LoadAssetAtPath(path+"FocusToggleOn.png",typeof(Texture)) as Texture;
		
		SnapMask = AssetDatabase.LoadAssetAtPath(path+"SnapMask.png",typeof(Texture)) as Texture;
		XYZMask = AssetDatabase.LoadAssetAtPath(path+"XYZMask.png",typeof(Texture)) as Texture;
		RotationModeToggleOn = AssetDatabase.LoadAssetAtPath(path+"RotationModeToggleOn.png",typeof(Texture)) as Texture;
		RotationModeToggleOff = AssetDatabase.LoadAssetAtPath(path+"RotationModeToggleOff.png",typeof(Texture)) as Texture;
		Quicktips = AssetDatabase.LoadAssetAtPath(path+"SnazzyToolsGridQuicktips.jpg",typeof(Texture)) as Texture;
		GradientFade = AssetDatabase.LoadAssetAtPath(path+"GradientFade.png",typeof(Texture)) as Texture;
	}

	static void InitReferences()    
	{
		snazzyGrid = ObjectSearch.Find("##SnazzyGrid##(Clone)");
		camReference = ObjectSearch.Find("##SnazzyCameraReference##(Clone)");
		snazzySettings = GameObject.Find("##SnazzySettings##");
		
		if (snazzyGrid == null)
		{
		  // Debug.Log("instantiage SnazzyGrid");
			snazzyGrid = Instantiate(AssetDatabase.LoadAssetAtPath("Assets/"+installPath+"SnazzyTools/SnazzyGrid/Resources/##SnazzyGrid##.prefab", typeof (GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
		  	snazzyGrid.name = "##SnazzyGrid##(Clone)";
			snazzyGrid.hideFlags = HideFlags.HideAndDontSave;
		}
		
		snazzyGridRenderer = snazzyGrid.GetComponent<MeshRenderer>() as MeshRenderer;
		
		if (camReference == null)
		{
			// Debug.Log("instantiage SnazzyCameraReference");
			camReference = new GameObject();
			camReference.name = "##SnazzyCameraReference##(Clone)";
			camReference.hideFlags = HideFlags.HideAndDontSave;
		}
		if (snazzySettings == null) {
			// Debug.Log("instantiage SnazzySettings");
			snazzySettings = AssetDatabase.LoadAssetAtPath("Assets/"+installPath+"SnazzyTools/SnazzyGrid/Resources/SnazzySettings.prefab",typeof (GameObject)) as GameObject;
			snazzySettings.name = "##SnazzySettings##";
		}
		
		if (snazzySettings != null) {
			snazzy = snazzySettings.GetComponent<SnazzySettings>() as SnazzySettings;
			snazzy.gridIsEnabled = true;
			
			CheckSnazzyGrid();
		}
	}
	
	static void DisableSnazzyGrid()
	{
		DestroyImmediate (snazzyGrid);
		DestroyImmediate (camReference);
		snazzy.gridIsEnabled = false;
	}
	
	static void ClearSnazzyGrid()
	{
		// Debug.Log ("ClearSnazzyGrid");
		snazzyGrid = ObjectSearch.Find("##SnazzyGrid##(Clone)");
		snazzy.gridIsEnabled = false;
		DestroyImmediate (snazzyGrid);
		// DestroyImmediate (camReference);
	}
	
	static void CheckSnazzyGrid()
	{
		if (snazzy != null) {
			if (snazzy.showGrid && !snazzy.gridIsEnabled) {
				ShowGrid();					
			}
		}
	}
	
	static void ShowGrid()
	{
		#if !UNITY_3_4 && !UNITY_3_5
		if (snazzyGrid != null) snazzyGrid.SetActive(true);
		#else
		if (snazzyGrid != null) snazzyGrid.active = true;
		#endif
		snazzy.gridIsEnabled = true;
	}
	
	static void HideGrid()
	{
			
		#if !UNITY_3_4 && !UNITY_3_5
		if (snazzyGrid != null) snazzyGrid.SetActive(false);
		#else
		if (snazzyGrid != null) snazzyGrid.active = false;
		#endif
		snazzy.gridIsEnabled = false;
	}
	
	void OnInspectorUpdate()
	{
		if (Selection.activeTransform != null) {
			if (Selection.activeTransform != snazzy.oldSelectedTransform || Selection.transforms.Length != snazzy.oldSelectedLength) {
				snazzy.oldSelectedTransform = Selection.activeTransform;
				snazzy.oldSelectedLength = Selection.transforms.Length;
			}
		}
		
		if (EditorApplication.isPlaying) HideGrid();
		else CheckSnazzyGrid();
		
		if (AngleLabel == null) {
			LoadButtons ();
			Repaint ();
		}
		
		// Repainting Snazzy Window if Camera X-Axis changes
		if (sceneCamera != null) {
			if (sceneCamera.transform.localEulerAngles.x != cameraXAxis) Repaint ();
			cameraXAxis = sceneCamera.transform.localEulerAngles.x;
		}

		if (snazzy.snapKeyTapped == 1 && !snazzy.snapKeyDown) {
			if (Time.realtimeSinceStartup-snazzy.snapKeyDownStart > 0.7f) {ToggleSnap();snazzy.snapKeyTapped = 0;}
			if (Time.realtimeSinceStartup-snazzy.snapKeyDownStart > 0.5f) snazzy.snapKeyTapped = 0;
			
		}
		if (snazzy.snapPosKeyTapped == 1 && !snazzy.snapPosKeyDown) {
			if (Time.realtimeSinceStartup-snazzy.snapPosKeyDownStart > 0.7f) {ToggleMovSnap();snazzy.snapPosKeyTapped = 0;}
			if (Time.realtimeSinceStartup-snazzy.snapPosKeyDownStart > 0.5f) snazzy.snapPosKeyTapped = 0;
			
		}
		if (snazzy.snapRotKeyTapped == 1 && !snazzy.snapRotKeyDown) {
			if (Time.realtimeSinceStartup-snazzy.snapRotKeyDownStart > 0.7f) {ToggleRotSnap();snazzy.snapRotKeyTapped = 0;}
			if (Time.realtimeSinceStartup-snazzy.snapRotKeyDownStart > 0.5f) snazzy.snapRotKeyTapped = 0;
			
		}
		if (snazzy.snapScaleKeyTapped == 1 && !snazzy.snapScaleKeyDown) {
			if (Time.realtimeSinceStartup-snazzy.snapScaleKeyDownStart > 0.7f) {ToggleScaleSnap();snazzy.snapScaleKeyTapped = 0;}
			if (Time.realtimeSinceStartup-snazzy.snapScaleKeyDownStart > 0.5f) snazzy.snapScaleKeyTapped = 0;
			
		}
	}
	
	static void OnHierarchy (int instanceID, Rect selectionRect)
	{
		Key (Event.current,false);
	}
}

	