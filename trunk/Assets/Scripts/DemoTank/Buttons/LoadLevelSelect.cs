using UnityEngine;
using System.Collections;

public class LoadLevelSelect : MonoBehaviour {
	public void onSelected() {
		EditorSave.instance.LoadLayerFromSlot (LevelHeader.instance.mSlotNumber, true);
	}
}
