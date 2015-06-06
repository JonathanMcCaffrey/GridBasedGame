#if UNITY_EDITOR

using UnityEngine;
using System.Collections;

namespace Utils {
	public class TextureUtils {
		public static Texture2D ConvertSpriteToTexture (Sprite icon) {
			var texture = new Texture2D ((int)icon.textureRect.width, (int)icon.textureRect.height, TextureFormat.RGBA32, false);
			texture.name = icon.name;
			
			/* //TODO Find out
			var pixels = icon.texture.GetPixels ((int)icon.textureRect.x, (int)icon.textureRect.y, (int)icon.textureRect.width, (int)icon.textureRect.height);
			texture.SetPixels (pixels);
			texture.Apply ();
*/
			
			return texture;
		}
	}
}

#endif