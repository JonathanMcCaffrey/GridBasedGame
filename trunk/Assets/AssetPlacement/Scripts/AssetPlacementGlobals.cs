//For quickly swapping between to projects (personal use)
//TODO Remove later
#define ProjectSelection

using UnityEngine;
using System.Collections;

public class AssetPlacementGlobals {
	//Where the AssetPlacement project is in Assets/
	#if ProjectSelection
	public const string InstallPath = ""; 
	#else
	public const string InstallPath = "Unity Asset Store/"; 
	#endif
	
	//Where 'your' assets are in Assets/
	//And this assumes they are in a folder called AssetPlacement
	#if ProjectSelection
	public const string AssetPathPath = ""; 
	#else
	public const string AssetPathPath = "Resources/"; 
	#endif
	//TODO Maybe add some drag and drop folder selection
	
	//Where the system will put the icons it renders from 3D assets in Assets/
	public const string IconRenderPath = "/Resources/PlacementIcons/";
	
	public const string HotKeysPath = "/" + InstallPath + "AssetPlacement/Editor/AssetPlacementSerializedHotKeys.cs";
	
	//Where all the asset placement hotkeys will go in top menu
	public const string CommandPath = "Window/Asset Placement/";
	
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
	
	public const int HotKeySelectionEnabled = -1; //TODO Delete this key
	
}
