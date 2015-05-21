using UnityEngine;
using System.Collections.Generic;

namespace Facebook {
	
	public class Item {
		public List<string> itemName;
		public string sender;
		public List<string> itemTitle;
		public string id;
		
		public Item() {
			itemName = new List<string>();
			sender = "";
			itemTitle = new List<string>();
			id = "";
		}
		
		public Item(List<string> itemName, string sender, List<string> itemTitle, string id) {
			this.sender = sender;
			this.itemName = itemName;
			
			this.itemTitle = itemTitle;
			this.id = id;
		}
	}
}

