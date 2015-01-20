using UnityEngine;
using System.Collections;

//TODO Rename to AssetPlacmenetGlobals?
public class AssetPlacementKeys {
	//Where the AssetPlacement project is in Assets/
	static public string InstallPath = ""; 

	//Where 'your' assets are in Assets/
	//And this assumes they are in a folder called AssetPlacement
	static public string AssetPathPath = ""; 
	//TODO Maybe add some drag and drop folder selection

	//Where the system will put the icons it renders from 3D assets in Assets/
	static public string IconRenderPath = "/Resources/PlacementIcons/";

	//Keys used internally
	public const string SnapUpdate = "AssetPlacement.doSnapUpdate";
	public const string ShowAll = "AssetPlacement.ShowAll";
	public const string ShowLabels = "AssetPlacement.ShowLabels";

	public const string SelectedTab = "AssetPlacement.SelectedTab";
	public const string SelectedKey = "AssetPlacement.SelectedKey";
	public const string SelectedAssetNumber = "AssetPlacement.SelectedAssetNumber";

	public const string SavedHotkey = "AP.PrefabsKeys.";

	public const string PositionMarker = "AP.PositionMarker";

	public const string CameraRender3D = "AP.CameraRender3D";
	public const string StageRender3D = "AP.StageRender3D";
	public const string LightMainRender3D = "AP.LightMainRender3D";
	public const string LightSubRender3D = "AP.LightSubRender3D";
	public const string LightSunRender3D = "AP.LightSunRender3D";


	//Constants used by AssetPlacementSystem
	public static int HotKeySelectionEnabled = -1;

}
