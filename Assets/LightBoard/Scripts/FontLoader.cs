using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/*
 * Classe used to get pixel letter from font file.
 * 
 */
public class FontLoader {

	public string		FontName = "Fonts/7x5";
	public int			FontSize = 16;

	private int			nbUnitHeight = 6;
	private Hashtable	letterMap = null;

	public FontLoader() {
		init ();
	}

	// Use this for initialization
	private void init () {
		letterMap = new Hashtable();
	}

	public void SetSize(int numberUnitHeight) {
		if (letterMap == null)
			init ();
		nbUnitHeight = numberUnitHeight;
		FontSize = numberUnitHeight * 2;
		if (FontSize == null)
			FontSize = 8;
	}

	public int[,] GetLetter(char charToSearch) {
		if (letterMap [charToSearch] == null) {
			if (loadLetter (charToSearch))
				return (int[,])letterMap [charToSearch];
			return null;
		}
		return (int[,])letterMap [charToSearch];
	}

	/**
	 * This function is used to load extra character not loaded yet
	 * charToLoad : the character to load
	**/
	private bool loadLetter(char charToLoad) {
		Font tmpFont = Resources.Load<Font>(FontName);
		if (tmpFont != null && tmpFont.HasCharacter (charToLoad)) {
			CharacterInfo ci = new CharacterInfo ();
			tmpFont.RequestCharactersInTexture (charToLoad.ToString (), FontSize);
			tmpFont.GetCharacterInfo (charToLoad, out ci, FontSize);

      // Get the font texture where the letter was printed
			Texture2D letterTexture = (Texture2D)tmpFont.material.mainTexture;

			// Copy it into a readable texture
			byte[] bytes = letterTexture.GetRawTextureData();
			Texture2D tex = new Texture2D(letterTexture.width, letterTexture.height, letterTexture.format, letterTexture.mipmapCount > 1);
			tex.LoadRawTextureData(bytes);
			tex.Apply ();

			// Get all character pixel into one array
			Color[] letterColor = tex.GetPixels (0, 0, ci.glyphWidth, ci.glyphHeight);

			int[,] result = new int[ci.glyphWidth,ci.glyphHeight];

			for (int i = 0; i < letterColor.Length; i++) {
				if (letterColor[i].a > 0.1f)
					result[i % ci.glyphWidth, i / ci.glyphWidth] = 1;
				else
					result[i % ci.glyphWidth, i / ci.glyphWidth] += 0;
			}
			if (result.GetLength(1) > nbUnitHeight && FontSize > 4) {
				FontSize--;
				Resources.UnloadAsset(tmpFont);
				loadLetter(charToLoad);
				return true;
			} else if (result.GetLength(1) > nbUnitHeight) {
				Resources.UnloadAsset(tmpFont);
				return false;
			}
			letterMap[charToLoad] = result;
			Resources.UnloadAsset(tmpFont);
			return true;
		}
		return false;
	}

	private static void logCharcterInfo(CharacterInfo ci) {
		string log = "";

		log += "ASCII CODE : " + ci.index + "\n";
		log += "Width : " + ci.glyphHeight + "\n";
		log += "Height : " + ci.glyphHeight + "\n";
		log += "Advance : " + ci.advance + "\n";
		log += "Bearing : " + ci.bearing + "\n";
		log += "UVTopLeft : " + ci.uvTopLeft + " - UVTopRight : " + ci.uvTopRight + " - UVBottomLeft : " + ci.uvBottomLeft + " - UVBottomRight : " + ci.uvBottomRight + "\n";
		log += "minX : " + ci.minX + " - maxX : " + ci.maxX + " - minY : " + ci.minY + " - maxY : " + ci.maxY + "\n";

		Debug.Log (log);
	}
}
