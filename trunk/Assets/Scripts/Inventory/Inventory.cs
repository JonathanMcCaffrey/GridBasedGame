using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory {

	public static List<ItemData> player = new List<ItemData>();

	public static void safeAdd(ItemData item) {
		foreach (ItemData checkItem in player) {
			if(item.id == checkItem.id) {
				return;
			}
		}

		player.Add (item);
	}

}
