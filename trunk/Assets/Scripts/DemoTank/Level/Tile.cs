using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Xml;

public class Tile : MonoBehaviour {
	
	public enum EditorType { Tile, Button };
	public enum CollidableType { Floor, Wall, Damage, Bullet, Enemy, Player, Breakable };
	
	public EditorType mEditorType = EditorType.Button;
	public CollidableType mCollidableType = CollidableType.Floor;
	
	float mGridSize = 50.0f;
	

	public KeyCode mHotKey = KeyCode.None;

	void TileToGrid () {
		mGridSize = GetComponent<BoxCollider2D> ().size.x;
		if (mEditorType == EditorType.Tile) {
			Vector2 gridPosition = new Vector2 ((float)(((int)(this.transform.position.x / mGridSize)) * mGridSize) + mGridSize * 0.5f, (float)(((int)(this.transform.position.y / mGridSize)) * mGridSize) + mGridSize * 0.5f);
			this.transform.position = gridPosition;
		}
	}
	
	public void Start() {
		TileToGrid ();
	}
	
	public void OnDrawGizmos () {
		TileToGrid ();
		
	}
	
	void Update () {
		
	}
	
	public string Save() {

		System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder ();

		using (XmlWriter writer = XmlWriter.Create(stringBuilder)) {
			
			writer.WriteElementString ("Position", this.gameObject.transform.position.ToString ());
			writer.WriteElementString ("Scale", this.gameObject.transform.localScale.ToString());
			writer.WriteElementString ("FileName", GetComponent<SpriteRenderer> ().sprite.texture.name);
		
		}

		return stringBuilder.ToString();

	}
	
	public void Load(XmlReader data) {
	/*	using(XmlReader xmlReader = XmlReader.Create(data)) {
			string position = xmlReader.ReadElementString("Position");
			string scale = xmlReader.ReadElementString("Scale");
			string fileName = xmlReader.ReadElementString("FileName");


		}*/
	}
}
