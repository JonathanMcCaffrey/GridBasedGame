using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {
	//TODO Load all systems from some sort of base function
	
	
	private static Inventory instance = null;
	void Awake() {
		if (instance) {
			Destroy (gameObject);
		} else {
			DontDestroyOnLoad(gameObject);
			instance = this;
		}
	}
	
	
	public List<ItemData> player = new List<ItemData>();
	
	public static void safeAdd(ItemData item) {
		foreach (ItemData checkItem in instance.player) {
			if(item.id == checkItem.id) {
				return;
			}
		}
		
		instance.player.Add (item);
	}
	
	public bool foundSpecialItem = false;
	public static void FoundSpecialItem() {
		instance.foundSpecialItem = true;
	}
	public static void ClearSpecialItem() {
		instance.foundSpecialItem = false;
	}
	public static bool DidFindSpecialItem() {
		return instance.foundSpecialItem;
	}
}
