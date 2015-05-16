using UnityEngine;
using System.Collections;

public class ItemData {
	public string name;
	public string id;
	public string title;

	public ItemData() {
		name = "";
		id = "";
		title = "";
	}

	public ItemData(string name, string id, string title) {
		this.name = name;
		this.id = id;
		this.title = title;
	}
}
