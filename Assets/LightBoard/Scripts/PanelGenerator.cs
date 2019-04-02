using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PanelGenerator : MonoBehaviour {

	public enum MotionStyle
	{
		Static,
		Pages,
		RightToLeft,
		LeftToRight,
		LetterDecal,
		Blink
	}

	// Number of led unit in height
	public int				NumberUnitHeight = 10;
	// Number of led unit in width
	public int 				NumberUnitWidth = 20;
	// Size of spaces between two led
	public float 			Spaces = 0.2f;
	// Led GameObject with "LedUnit" script attached.
	public GameObject		PrefabUnit;
	// GameObject representing background.
	public GameObject		PrefabBackground;
	// Path to a font file in a Resources folder
	[Header("Eg : Fonts/7x5")]
	public string			FontToUse = "Fonts/7x5";
	// String drawn on the panel
	public string			StringToDraw = "X";
	// Type of transition
	public MotionStyle		TextMotionStyle = MotionStyle.Static;
	// Time between two states in second
	public float			RefreshTime = 0.5f;

	private List<List<LedUnit>>	ledUnits;
	private GameObject			background;
	private List<int[,]>		lettersList;
	private int					actPos = 0;
	private float				actTime = 0;
	private int					stringToDrawStart = 0;
	private bool				IsBlinked = false;
	private int 				stringPixelSize = 0;
	private FontLoader			fontLoader;
  public Font font;
	private string				copyString;
  [HideInInspector]
  public GameObject bgToDelete;

  private void Awake() {
    bgToDelete = transform.GetChild(0).gameObject;
    Destroy(bgToDelete);
  }

  // Use this for initialization
  void Start () {
    //bgToDelete = transform.GetChild(0).gameObject;
    
    if (FontToUse.Equals ("")) {
			FontToUse = "Fonts/7x5";
		}

		fontLoader = new FontLoader ();
    fontLoader.FontName = FontToUse;

		ledUnits = new List<List<LedUnit>> ();
		fontLoader.SetSize(NumberUnitHeight);

		float startX = -1 * ((NumberUnitWidth + Spaces * (NumberUnitWidth - 1)) / 2 - 0.5f);
		float startY = (NumberUnitHeight + Spaces * (NumberUnitHeight - 1)) / 2 - 0.5f;
		for (int j = 0; j < NumberUnitWidth; j++) {
			List<LedUnit> lList = new List<LedUnit>();
			for (int i = 0; i < (int)NumberUnitHeight; i++) {
				GameObject ledUnit = Instantiate(PrefabUnit);
				ledUnit.transform.SetParent(this.transform);
				ledUnit.transform.localPosition = new Vector3(startX + j * Spaces + j, startY - i * Spaces - i, 0);
				ledUnit.transform.localScale = new Vector3(1, 1, 1);
				lList.Add(ledUnit.GetComponent<LedUnit>());
			}
			ledUnits.Add (lList);
		}

    background = Instantiate (PrefabBackground);
    //background.GetComponent<Collider>().enabled = false;
		background.transform.SetParent (this.transform);
		background.transform.localPosition = new Vector3 (0, 0, 0.5f);
		background.transform.localScale = new Vector3 (NumberUnitWidth + Spaces * (NumberUnitWidth - 1) + 1, NumberUnitHeight + Spaces * (NumberUnitHeight - 1) + 1, 1);
		background.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

		GenerateLetterList ();

		actTime = RefreshTime;
  }
	
	/**
	 * Generate the liste off pixel letter, can be called at runtime if the text change
	*/
	private void GenerateLetterList() {
		lettersList = new List<int[,]> ();
		foreach (char c in StringToDraw) {
			int[,] letter = fontLoader.GetLetter (c);
			if (letter != null) {
				lettersList.Add (letter);
				if (c == ' ') {
					lettersList.Add (letter);
					lettersList.Add (letter);
					lettersList.Add (letter);
				}
			}
		}
		foreach (int[,] letter in lettersList)
			stringPixelSize += letter.GetLength (0) + 1;
		
		if (TextMotionStyle == MotionStyle.LeftToRight)
			stringToDrawStart = stringPixelSize - NumberUnitWidth;

		copyString = StringToDraw;
	}
	
	/**
	 * Update the LightBoard.
	 * Switch on the transition type to call the selected update function
	*/
	private void Update() {

		actTime += Time.deltaTime;
		if (actTime >= RefreshTime) {
			if (!copyString.Equals(StringToDraw))
				GenerateLetterList ();
			switch (TextMotionStyle) {
			case MotionStyle.Static:
				if (!IsBlinked)
					DrawBoardStatic ();
				IsBlinked = true;
				break;
			case MotionStyle.Pages:
				CleanBoard();
				DrawBoardPage ();
				if (stringToDrawStart >= lettersList.Count - 1)
					stringToDrawStart = 0;
				break;
			case MotionStyle.LetterDecal:
				CleanBoard();
				DrawBoardLetterDecal ();
				stringToDrawStart++;
				if (StringToDraw.ToCharArray().Length > stringToDrawStart && StringToDraw.ToCharArray()[stringToDrawStart] == ' ')
					stringToDrawStart++;
				if (stringToDrawStart >= lettersList.Count)
					stringToDrawStart = 0;
				break;
			case MotionStyle.RightToLeft:
				CleanBoard();
				DrawBoardRigthToLeft ();
				stringToDrawStart++;
				if (actPos <= stringToDrawStart)
					stringToDrawStart = -NumberUnitWidth;
				break;
			case MotionStyle.LeftToRight:
				CleanBoard();
				DrawBoardRigthToLeft ();
				stringToDrawStart--;
				if (stringToDrawStart <= -NumberUnitWidth)
					stringToDrawStart = 0 + stringPixelSize;
				break;
			case MotionStyle.Blink:
				CleanBoard();
				if (IsBlinked)
					DrawBoardStatic ();
				IsBlinked = !IsBlinked;
				break;
			}
			actPos = 0;
			actTime = 0;
		}
	}
	
	/**
	 * This function is used to reset all led
	*/
	private void CleanBoard() {
		if (ledUnits != null) {
			foreach (List<LedUnit> lList in ledUnits) {
				foreach (LedUnit lUnit in lList)
					lUnit.IsLightOn = false;
			}
		}
	}
	
	/**
	 * This function is called to draw the text once. 
	*/
	private void DrawBoardStatic () {
		if (lettersList.Count > 0) {
			for (int letterPos = 0; letterPos < lettersList.Count; ++letterPos) {
				int[,] letter = lettersList[letterPos];
				int width = letter.GetLength (0);
				int height = letter.GetLength (1);
				
				int startH = (NumberUnitHeight - height);
				for (int i = startH; i < height + startH; i++) {
					for (int j = 0; j < width; j++) {
						if (j + actPos - stringToDrawStart < NumberUnitWidth && j + actPos >= stringToDrawStart) {
							ledUnits [j + actPos - stringToDrawStart] [i].IsLightOn = letter [j, i - startH] > 0;
						}
					}
				}
				actPos += width + 1;
			}
		}
	}
	
	/**
	 * This function is called to update the lightScreen by moving the text form right to left (decal of one line each steps) 
	*/
	private void DrawBoardRigthToLeft () {
		if (lettersList.Count > 0) {
			for (int letterPos = 0; letterPos < lettersList.Count; ++letterPos) {
				int[,] letter = lettersList[letterPos];
				int width = letter.GetLength (0);
				int height = letter.GetLength (1);
				
				int startH = (NumberUnitHeight - height);
				for (int i = startH; i < height + startH; i++) {
					for (int j = 0; j < width; j++) {
						if (j + actPos - stringToDrawStart < NumberUnitWidth && j + actPos >= stringToDrawStart) {
							ledUnits [j + actPos - stringToDrawStart] [i].IsLightOn = letter [j, i - startH] > 0;
						}
					}
				}
				actPos += width + 1;
			}
		}
	}
	
	/**
	 * This function is called to update the lightScreen by moving all the screen by the letter size. 
	*/
	private void DrawBoardLetterDecal () {
		if (lettersList.Count > 0) {
			for (int letterPos = stringToDrawStart; letterPos < lettersList.Count; ++letterPos) {
				int[,] letter = lettersList[letterPos];
				int width = letter.GetLength (0);
				int height = letter.GetLength (1);

				int startH = (NumberUnitHeight - height);
				if (actPos < NumberUnitWidth) {
					for (int i = startH; i < letter.Length; i++) {
						if (i % width + actPos < NumberUnitWidth)
							ledUnits [i % width + actPos] [i / width + startH].IsLightOn = letter [i % width, i / width] > 0;
					}
					actPos += width + 1; 
					if (actPos >= NumberUnitWidth) {
						actPos = NumberUnitWidth;
					}
				}
			}
		}
	}

	/**
	 * This function is called to update the lightScreen by pages 
	 * 
	*/
	private void DrawBoardPage () {
		if (lettersList.Count > 0) {
			for (int letterPos = stringToDrawStart; letterPos < lettersList.Count; ++letterPos) {
				int[,] letter = lettersList[letterPos];
				int width = letter.GetLength (0);
				int height = letter.GetLength (1);

				int startH = (NumberUnitHeight - height);
				if (actPos < NumberUnitWidth) {
					for (int i = startH; i < letter.Length; i++) {
						if (i % width + actPos < NumberUnitWidth)
							ledUnits [i % width + actPos] [i / width + startH].IsLightOn = letter [i % width, i / width] > 0;
					}
					actPos += width + 1; 
					if (actPos >= NumberUnitWidth) {
						actPos = NumberUnitWidth;
						stringToDrawStart = letterPos + 1;
					}
				}
			}
			if (actPos < NumberUnitWidth) {
				stringToDrawStart = lettersList.Count;
			}
		}
	}
}
