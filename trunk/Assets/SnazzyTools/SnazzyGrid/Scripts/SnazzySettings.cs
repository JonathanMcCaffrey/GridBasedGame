using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class SnazzySettings : MonoBehaviour 
{
	public List<float> gridSizePresets = new List<float>();
	public List<float> gridIncrementPresets = new List<float>();
	public List<float> gridMultiPresets = new List<float>();
	public List<float> rotPresets = new List<float>();
	public List<float> rotMultiPresets = new List<float>();

	public bool tooltip = true;

	public float cameraAngleOffset = 0;
	public int gridSizeIndex = 0;
	public int gridIncrementIndex = 0;
	public int gridMultiIndex = 0;
	public int rotIndex = 0;
	public int rotMultiIndex = 0;
	public bool mov2 = false;
	public bool angle2 = false;
	public bool rotationMode = false;
	
	public bool gridIsEnabled = false;
	public float gridIncrement = 1;
	public float gridIncrement2 = 2;
	public float moveAmount;
	public float rotAmount;
	public float gridSize = 1;
	public float objectScale = 50;
	public float snapAreaSize = 0;
	public float rotIncrement = 22.5f;
	public float rotIncrement2 = 45;
	public bool keyDown = false;
	public bool settings = false;
	
	public int rotAxis = 1;
	
	public bool autoSnap = true;
	public bool autoSnapPos = true;
	public bool autoSnapPosX = true;
	public bool autoSnapPosY = true;
	public bool autoSnapPosZ = true;

	public bool autoSnapRot = true;
	public bool autoSnapRotX = true;
	public bool autoSnapRotY = true;
	public bool autoSnapRotZ = true;

	public bool autoSnapScale = true;
	public bool autoSnapScaleX = true;
	public bool autoSnapScaleY = true;
	public bool autoSnapScaleZ = true;

	public bool lockChild = false;
	
	public bool showHelp = true;
	public bool showGrid = false;
	public float viewDependency = 4f;
	
	public bool snapRotCam = false;
	public bool snapRotLight = false;
	public GameObject clone1;
	
	public bool lookAtSelect = false;
	public Transform oldTransform;
	public Transform oldSelectedTransform;
	public int oldSelectedLength;
	public Vector3 oldPosition;
	public Vector3 oldScale;
	public Vector3 oldRotation;

	// Keys
	public KeyCode movToggleKey = KeyCode.None;
	public KeyCode angleToggleKey = KeyCode.None;
	public KeyCode gridKey = KeyCode.G;
	public KeyCode leftKey = KeyCode.Keypad4;
	public KeyCode rightKey = KeyCode.Keypad6;
	public KeyCode upKey = KeyCode.Keypad8;

	public KeyCode downKey = KeyCode.Keypad2;
	public KeyCode frontKey = KeyCode.PageUp;
	public KeyCode backKey = KeyCode.PageDown;
	public KeyCode rotLeftKey = KeyCode.Keypad7;
	public KeyCode rotRightKey = KeyCode.Keypad9;
	public KeyCode snapRotKey = KeyCode.Keypad3;

	public KeyCode rotXYZKey = KeyCode.X;
	
	public KeyCode duplicateKey = KeyCode.KeypadMultiply;
	public KeyCode focusKey = KeyCode.KeypadDivide;
	
	public bool lockKeys = false;
	public bool snapKeyDown = false;
	public float snapKeyDownStart = 0;
	public int snapKeyTapped = 0;
	public bool snapPosKeyDown = false;
	public float snapPosKeyDownStart = 0;
	public int snapPosKeyTapped = 0;
	public bool snapRotKeyDown = false;
	public float snapRotKeyDownStart = 0;
	public int snapRotKeyTapped = 0;
	public bool snapScaleKeyDown = false;
	public float snapScaleKeyDownStart = 0;
	public int snapScaleKeyTapped = 0;
	public KeyCode snapKey = KeyCode.None;
	public KeyCode snapInversionKey = KeyCode.None;
	public KeyCode snapPosKey = KeyCode.None; 
	public KeyCode snapScaleKey = KeyCode.None;
	
	public KeyCode resetTransformKey;
	public KeyCode resetPositionKey;
	public KeyCode resetRotationKey;
	public KeyCode resetScaleKey;
	public KeyCode childCompensationKey;
	public KeyCode parentKey;
	public KeyCode parentToCenterKey;
	public KeyCode centerCurrentParentKey;
	public KeyCode hierarchyUpKey;
	public KeyCode hierarchyDownKey;
	public KeyCode unparentKey;
	public KeyCode unparentDeleteKey;

	public int movToggleKeySpecial = 0;
	public int angleToggleKeySpecial = 0;
	public int gridKeySpecial = 0;
	public int leftKeySpecial = 0;
	public int rightKeySpecial = 0;
	public int upKeySpecial = 0;
	public int downKeySpecial = 0;
	public int frontKeySpecial = 0;
	public int backKeySpecial = 0;
	public int rotLeftKeySpecial = 0;
	public int rotRightKeySpecial = 0;
	public int snapRotKeySpecial = 0;
	public int rotXYZKeySpecial = 0;
	public int duplicateKeySpecial = 0;
	public int focusKeySpecial = 0;
	public int resetTransformKeySpecial = 0;
	public int resetPositionKeySpecial = 0;
	public int resetRotationKeySpecial = 0;
	public int resetScaleKeySpecial = 0;
	public int childCompensationKeySpecial = 0;
	public int parentKeySpecial = 0;
	public int parentToCenterKeySpecial = 0;
	public int hierarchyUpKeySpecial = 0;
	public int hierarchyDownKeySpecial = 0;
	public int unparentKeySpecial = 0;
	
	public int snapKeySpecial = 0;
	public int snapInversionKeySpecial = 0;
	public int snapPosKeySpecial = 0;
	public int snapScaleKeySpecial = 0;

	public int unparentDeleteKeySpecial = 0;
	public int centerCurrentParentKeySpecial = 0;
}
