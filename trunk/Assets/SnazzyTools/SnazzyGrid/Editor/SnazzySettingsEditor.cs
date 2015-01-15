using UnityEngine;
using UnityEditor;
using System.Collections;

public class SnazzySettingsEditor : EditorWindow {

	public GameObject snazzySettings;
	public GameObject snazzyGrid;
	public SnazzySettings snazzy;
	public int keyIndex = -1;
	public Texture SnazzySettingsBGImage;
	public Vector2 scrollPosition;
	public Texture2D background;
	public bool unitySkin = EditorGUIUtility.isProSkin;
	public MeshRenderer snazzyGridRenderer;
	void OnDestroy()
	{
		if (SnazzyToolsEditor.snazzy != null) SnazzyToolsEditor.snazzy.settings = false;
	}

	public void InitSnazzySettings()
	{
		LoadText();
		snazzySettings = ObjectSearch.Find("##SnazzySettings##");

		if (snazzySettings == null) {
			snazzySettings = AssetDatabase.LoadAssetAtPath("Assets/"+SnazzyToolsEditor.installPath+"SnazzyTools/SnazzyGrid/Scripts/SnazzySettings.prefab",typeof (GameObject)) as GameObject;
			snazzySettings.name = "##SnazzySettings##";
		}

		if (snazzySettings != null) {
			snazzy = snazzySettings.GetComponent<SnazzySettings>() as SnazzySettings;
		}
		snazzyGrid = ObjectSearch.Find("##SnazzyGrid##(Clone)");
		if (snazzyGrid != null) snazzyGridRenderer = snazzyGrid.GetComponent<MeshRenderer>() as MeshRenderer;
	}

	Vector2 globalOffset = new Vector2(13,113);
	Vector2 offset1 = new Vector2(2,0);
	Vector2 offset2 = new Vector2(156,0);
	Vector2 offset3 = new Vector2(255,-1);
	Vector2 offset4 = new Vector2(312,-1);
	Vector3 offset5 = new Vector2(2,0);
	Vector3 offset6 = new Vector2(255,-1);
	Vector2 size1 = new Vector2(150,19);
	Vector2 size2 = new Vector2(100,19);
	Vector2 size3 = new Vector2(15,19);
	Vector2 size4 = new Vector2(55,17);
	Vector2 size5 = new Vector2(150,19);
	Vector2 size6 = new Vector2(80,19);

	void KeyField(Vector2 pos,string text,ref KeyCode key,ref int specialKey,int index)
	{
		GUI.Label (new Rect(pos.x+globalOffset.x+offset1.x,pos.y+globalOffset.y+offset1.y,size1.x,size1.y),text);  
		GUI.Label (new Rect(pos.x+globalOffset.x+offset2.x,pos.y+globalOffset.y+offset2.y,size2.x,size2.y),key.ToString());
		SelectKey (pos,ref key,index,ref specialKey);	
	}
	
	Rect GetRect5(Vector2 pos)
	{
		return new Rect(pos.x+globalOffset.x+offset5.x,pos.y+globalOffset.y+offset5.y,size5.x,size5.y);
	}
	
	Rect GetRect6(Vector2 pos)
	{
		return new Rect(pos.x+globalOffset.x+offset6.x,pos.y+globalOffset.y+offset6.y,size6.x,size6.y);
	}
	
	void OnGUI()
	{
		if (snazzy == null) {Close();}
		
		SnazzyToolsEditor.Key(Event.current,false);
		// Debug.Log (EditorGUIUtility.isProSkin);
		if (unitySkin != EditorGUIUtility.isProSkin) {LoadText();unitySkin = EditorGUIUtility.isProSkin;}
		if (!background) {
			background = new Texture2D(1,1);
			background.SetPixel (0,0,new Color(40f/255f,40f/255f,40f/255f,1));
			background.Apply ();
		}
		EditorGUI.DrawPreviewTexture(new Rect(0,0,Screen.width,Screen.height),background);	
		
		scrollPosition = GUI.BeginScrollView(new Rect(0,0,Screen.width,Screen.height), scrollPosition, new Rect(0,0,394+6,1135+6+21));
		EditorGUI.DrawPreviewTexture (new Rect(0,-1,428,1155),SnazzySettingsBGImage);		

		Vector2 pos = new Vector2(10,10+21);		

		KeyField(pos,"Grid toggle",ref snazzy.gridKey,ref snazzy.gridKeySpecial,0);
		pos.y += 36;
		KeyField(pos,"Move increment toggle",ref snazzy.movToggleKey,ref snazzy.movToggleKeySpecial,100);
		pos.y += 36;
		KeyField(pos,"Rotation angle toggle",ref snazzy.angleToggleKey,ref snazzy.angleToggleKeySpecial,101);
		pos.y += 41;
		KeyField(pos,"Rotation axis",ref snazzy.rotXYZKey,ref snazzy.rotXYZKeySpecial,1);
		pos.y += 20;
		KeyField(pos,"Rotate left",ref snazzy.rotLeftKey,ref snazzy.rotLeftKeySpecial,2);
		pos.y += 20;
		KeyField (pos,"Rotate right",ref snazzy.rotRightKey,ref snazzy.rotRightKeySpecial,3);
		pos.y += 25;
		KeyField (pos,"Move left",ref snazzy.leftKey,ref snazzy.leftKeySpecial,4);
		pos.y += 20;
		KeyField (pos,"Move right",ref snazzy.rightKey,ref snazzy.rightKeySpecial,5);
		pos.y += 20;
		KeyField (pos,"Move up",ref snazzy.upKey,ref snazzy.upKeySpecial,6);
		pos.y += 20;
		KeyField (pos,"Move down",ref snazzy.downKey,ref snazzy.downKeySpecial,7);
		pos.y += 20;
		KeyField (pos,"Move forward",ref snazzy.frontKey,ref snazzy.frontKeySpecial,8);
		pos.y += 20;
		KeyField (pos,"Move back",ref snazzy.backKey,ref snazzy.backKeySpecial,9);
		pos.y += 25;
		KeyField (pos,"Duplicate",ref snazzy.duplicateKey,ref snazzy.duplicateKeySpecial,10);
		pos.y += 36;
		KeyField (pos,"Focus",ref snazzy.focusKey,ref snazzy.focusKeySpecial,11);
		pos.y += 41;
		KeyField (pos,"Snap",ref snazzy.snapKey,ref snazzy.snapKeySpecial,12);
		pos.y += 20;
		KeyField (pos,"Reset transform",ref snazzy.resetTransformKey,ref snazzy.resetTransformKeySpecial,13);
		pos.y += 36;
		KeyField (pos,"Snap position",ref snazzy.snapPosKey,ref snazzy.snapPosKeySpecial,14);
		pos.y += 20;
		KeyField (pos,"Reset position",ref snazzy.resetPositionKey,ref snazzy.resetPositionKeySpecial,15);
		pos.y += 36;
		KeyField (pos,"Snap rotation",ref snazzy.snapRotKey,ref snazzy.snapRotKeySpecial,16);
		pos.y += 20;
		KeyField (pos,"Reset rotation",ref snazzy.resetRotationKey,ref snazzy.resetRotationKeySpecial,17);
		pos.y += 36;
		KeyField (pos,"Snap scale",ref snazzy.snapScaleKey,ref snazzy.snapScaleKeySpecial,18);
		pos.y += 20;
		KeyField (pos,"Reset scale",ref snazzy.resetScaleKey,ref snazzy.resetScaleKeySpecial,19);
		pos.y += 41;
		KeyField (pos,"Child compensation",ref snazzy.childCompensationKey,ref snazzy.childCompensationKeySpecial,20);
		pos.y += 36;
		KeyField (pos,"New parent",ref snazzy.parentKey,ref snazzy.parentKeySpecial,21);
		pos.y += 20;
		KeyField (pos,"New parent to center",ref snazzy.parentToCenterKey,ref snazzy.parentToCenterKeySpecial,22);
		pos.y += 20;
		KeyField (pos,"Current parent to center",ref snazzy.centerCurrentParentKey,ref snazzy.centerCurrentParentKeySpecial,23);	
		pos.y += 20;
		KeyField (pos,"Hierarchy up",ref snazzy.hierarchyUpKey,ref snazzy.hierarchyUpKeySpecial,24);
		pos.y += 20;
		KeyField (pos,"Hierarchy down",ref snazzy.hierarchyDownKey,ref snazzy.hierarchyDownKeySpecial,25);
		pos.y += 20;
		KeyField (pos,"Unparent",ref snazzy.unparentKey,ref snazzy.unparentKeySpecial,26);
		pos.y += 20;
//		KeyField (pos,"Unparent & delete",ref snazzy.unparentDeleteKey,ref snazzy.unparentDeleteKeySpecial,27);
		
		// Settings
		if (snazzyGridRenderer != null) {
			pos.y += 66;
			
			GUI.Label(GetRect5(pos),"Tooltip text");
			snazzy.tooltip = GUI.Toggle(GetRect6(pos),snazzy.tooltip,"");
			pos.y += 20;

			GUI.Label (GetRect5(pos),"Grid size");
			GUI.changed = false;
			snazzy.objectScale = EditorGUI.IntField(GetRect6(pos),(int)snazzy.objectScale);
			if (GUI.changed) {
				if (snazzyGrid != null) snazzyGrid.transform.localScale = new Vector3(snazzy.objectScale*snazzy.gridSize,snazzy.objectScale*snazzy.gridSize,snazzy.objectScale*snazzy.gridSize);
			}
			pos.y += 20;
			
			GUI.Label (GetRect5(pos),"Grid color");
			snazzyGridRenderer.sharedMaterial.SetColor("_GridColor",EditorGUI.ColorField(GetRect6(pos),snazzyGridRenderer.sharedMaterial.GetColor("_GridColor")));
			pos.y += 20;
			GUI.Label (GetRect5(pos),"Streak color");
			snazzyGridRenderer.sharedMaterial.SetColor("_StreakColor",EditorGUI.ColorField(GetRect6(pos),snazzyGridRenderer.sharedMaterial.GetColor("_StreakColor")));
			pos.y += 20;

			GUI.Label (GetRect5(pos),"Streak scale");
			float slider = snazzyGridRenderer.sharedMaterial.GetFloat("_StreakScale");
			slider = GUI.HorizontalSlider (GetRect6(pos),slider,20f,7f);
			snazzyGridRenderer.sharedMaterial.SetFloat("_StreakScale",slider);
			
			pos.y += 20;
			GUI.Label (GetRect5(pos),"Move indicaction transp.");
			slider = snazzyGridRenderer.sharedMaterial.GetFloat("_SnapTransparency");
			slider = GUI.HorizontalSlider (GetRect6(pos),slider,0,2);
			snazzyGridRenderer.sharedMaterial.SetFloat("_SnapTransparency",slider);
			pos.y += 20;
			
			GUI.Label (GetRect5(pos),"Grid offset");
			float snapOffset = snazzyGridRenderer.sharedMaterial.GetFloat("_SnapOffset");
			bool toggle;
			if (snapOffset == 0) toggle = false; else toggle = true;
			toggle = GUI.Toggle (GetRect6(pos),toggle,"");
			if (toggle) snapOffset = 0f; else snapOffset = 0.5f;
			snazzyGridRenderer.sharedMaterial.SetFloat("_SnapOffset",snapOffset);
			snazzyGridRenderer.sharedMaterial.SetFloat("_GridOffset",snapOffset);

			pos.y += 20;
			GUI.Label (GetRect5(pos),"Vertex push");
			slider = snazzyGridRenderer.sharedMaterial.GetFloat("_VertexPush");
			slider = GUI.HorizontalSlider (GetRect6(pos),slider,0.0001f,0.01f);
			snazzyGridRenderer.sharedMaterial.SetFloat("_VertexPush",slider);
			pos.y += 20;
		}	
		GUI.EndScrollView();
	}

	void SelectKey(Vector2 pos,ref KeyCode key,int _keyIndex,ref int specialKey)
	{
		SpecialKey(pos,ref specialKey);
		
		if (keyIndex == _keyIndex) GUI.color = Color.green;
		if (GUI.Button (new Rect(pos.x+globalOffset.x+offset4.x,pos.y+globalOffset.y+offset4.y,size4.x,size4.y),"Select")) {
			if (Event.current.button == 1) {
				key = KeyCode.None;
				keyIndex = -1;
			}
			else if (keyIndex != _keyIndex) keyIndex = _keyIndex; else keyIndex = -1;
		}
		if (keyIndex == _keyIndex) {
			if (Event.current.type == EventType.keyDown) {
				key = Event.current.keyCode;
				keyIndex = -1;
				Repaint();
			}
		}
		GUI.color = Color.white;
	}

	void SpecialKey(Vector2 pos,ref int specialKey)
	{
		bool t = false;
		if ((specialKey & 1) != 0) t = true;
		t = GUI.Toggle(new Rect(pos.x+globalOffset.x+offset3.x,pos.y+globalOffset.y+offset3.y,size3.x,size3.y),t,"");
		if (t) specialKey |= 1; else specialKey &= 6;
		
		if ((specialKey & 2) != 0) t = true; else t = false;
		t = GUI.Toggle(new Rect(pos.x+globalOffset.x+offset3.x+19,pos.y+globalOffset.y+offset3.y,size3.x,size3.y),t,"");
		if (t) specialKey |= 2; else specialKey &= 5;

		if ((specialKey & 4) != 0) t = true; else t = false;
		t = GUI.Toggle(new Rect(pos.x+globalOffset.x+offset3.x+38,pos.y+globalOffset.y+offset3.y,size3.x,size3.y),t,"");
		if (t) specialKey |= 4; else specialKey &= 3;
	}

	void LoadText()
	{
		string path = "Assets/"+SnazzyToolsEditor.installPath+"SnazzyTools/SnazzyGrid/Resources/GUI/";
		
		if (EditorGUIUtility.isProSkin)
			SnazzySettingsBGImage = AssetDatabase.LoadAssetAtPath(path+"SnazzySettingsBGImage.jpg",typeof(Texture)) as Texture;
		else 
			SnazzySettingsBGImage = AssetDatabase.LoadAssetAtPath(path+"SnazzySettingsBGImageBright.jpg",typeof(Texture)) as Texture;
	}
}
