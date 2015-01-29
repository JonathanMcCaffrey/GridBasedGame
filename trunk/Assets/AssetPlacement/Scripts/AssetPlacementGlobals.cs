//For quickly swapping between to projects (personal use)
//TODO Remove later
#define ProjectSelection

using UnityEngine;
using System.Collections;

public class AssetPlacementGlobals {
	//Where the AssetPlacement project is in Assets/
	#if ProjectSelection
	static public string InstallPath = ""; 
	#else
	static public string InstallPath = ""; 
	#endif
	
	//Where 'your' assets are in Assets/
	//And this assumes they are in a folder called AssetPlacement
	#if ProjectSelection
	static public string AssetPathPath = ""; 
	#else
	static public string AssetPathPath = "Resources/"; 
	#endif
	//TODO Maybe add some drag and drop folder selection
	
	//Where the system will put the icons it renders from 3D assets in Assets/
	static public string IconRenderPath = "/Resources/PlacementIcons/";
	
	static public string HotKeysPath = InstallPath + "/AssetPlacement/Editor/AssetPlacementSerializedHotKeys.cs";
	
	//Keys used internally
	public const string SnapUpdate = "AssetPlacement.doSnapUpdate";
	public const string ShowAll = "AssetPlacement.ShowAll";
	public const string ShowLabels = "AssetPlacement.ShowLabels";
	
	public const string SelectedTab = "AssetPlacement.SelectedTab";
	public const string SelectedKey = "AssetPlacement.SelectedKey";
	public const string SelectedAssetNumber = "AssetPlacement.SelectedAssetNumber";
	
	public const string SavedHotkey = "AP.SavedHotkey.";
	public const string ShouldRefreshHotkeys = "AP.ShouldRefreshHotkeys.";
	
	
	public const string PositionMarker = "AP.PositionMarker";
	
	public const string CameraRender3D = "AP.CameraRender3D";
	public const string StageRender3D = "AP.StageRender3D";
	public const string LightMainRender3D = "AP.LightMainRender3D";
	public const string LightSubRender3D = "AP.LightSubRender3D";
	public const string LightSunRender3D = "AP.LightSunRender3D";
	
	
	//Constants used by AssetPlacementSystem
	public static int HotKeySelectionEnabled = -1; //TODO Delete this
	
}
