using UnityEngine;
using System.Collections;

public class SaveLevelSelect : MonoBehaviour {
	public void onSelected() {
		EditorSerialization.instance.Save (LevelHeader.instance.mSlotNumber);
	}
}
