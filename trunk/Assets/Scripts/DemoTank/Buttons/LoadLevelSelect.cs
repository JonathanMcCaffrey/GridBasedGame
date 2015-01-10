using UnityEngine;
using System.Collections;

public class LoadLevelSelect : MonoBehaviour {
	public void onSelected() {
		EditorSerialization.instance.LoadLayerFromSlot (LevelHeader.instance.mSlotNumber, true);
	}
}
