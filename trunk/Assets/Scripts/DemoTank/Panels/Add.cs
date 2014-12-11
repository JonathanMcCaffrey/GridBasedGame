using UnityEngine;
using System.Collections;

public class Add : MonoBehaviour {
	public void onSelected() {
		if(LevelSelect.instance.mDefaultItem) {
			var tempItem = (GameObject.Instantiate(LevelSelect.instance.mDefaultItem) as GameObject).GetComponent<Item>();
			LevelSelect.instance.mItemList.Add(tempItem);
			tempItem.mOrder = LevelSelect.instance.GetHighestOrder + 1;
			tempItem.gameObject.transform.parent =  LevelSelect.instance.mScrollContainer.transform;
			tempItem.gameObject.transform.localScale = Vector3.one;
			LevelSelect.instance.Refresh();
		}
	}
}
