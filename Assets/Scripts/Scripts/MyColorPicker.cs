using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyColorPicker : MonoBehaviour
{

	Texture2D t;
	Texture2D tSlider;
	Texture2D tShowColor;
	Texture2D tToBlack;
	Texture2D tToWhite;
	GUIStyle gshowColor;
	GUIStyle gsAllColor;
	GUIStyle gsToBlackColor;
	GUIStyle gsToWhiteColor;
	Rect PickerBarRect;
	Rect PickerToBlackBarRect;
	Rect PickerToWhiteBarRect;
	//-------------------------------------以下為預覽顏色方塊，你可以修改座標與大小
	int ShowColorX = 10;
	int ShowColorY = 10;
	int ShowColorWidth = 50;
	int ShowColorHeight = 50;
	//-------------------------------------以下為 "彩色" 選取器，你可以修改座標與大小
	int PickerX = 100;
	int PickerY = 10;
	int PickerWidth = 500;
	int PickerHeight = 15;
	//-------------------------------------以下為 "近黑色" 選取器，你可以修改座標與大小
	int PickerToBlackX = 100;
	int PickerToBlackY = 45;
	int PickerToBlackWidth = 500;
	int PickerToBlackHeight = 15;
	//-------------------------------------以下為 "近白色" 選取器，你可以修改座標與大小
	int PickerToWhiteX = 100;
	int PickerToWhiteY = 80;
	int PickerToWhiteWidth = 500;
	int PickerToWhiteHeight = 15;
	//---------------------------------------------------------
	int PercentBar = 0;
	int PercentToBlackBar;
	int PercentToWhiteBar;
	float rgb;
	float[,] ColorRGB; // Index, [R, G, B]

	public static Color ShowColor;
	Color OriginalColor;

	void Start ()
	{
		gshowColor = new GUIStyle ();
		gsAllColor = new GUIStyle ();
		gsToBlackColor = new GUIStyle ();
		gsToWhiteColor = new GUIStyle ();

		tShowColor = new Texture2D (1, 1);

		ColorRGB = new float[PickerWidth, 3];

		PercentToBlackBar = PickerToBlackWidth - 17;

		PickerBarRect = new Rect (PickerX, PickerY - 10, 20, PickerHeight + 20);
		PickerToBlackBarRect = new Rect (PickerToBlackX + PercentToBlackBar, PickerToBlackY - 10, 20, PickerToBlackHeight + 20);
		PickerToWhiteBarRect = new Rect (PickerToWhiteX + PercentToWhiteBar, PickerToWhiteY - 10, 20, PickerToWhiteHeight + 20);

		tSlider = new Texture2D (20, PickerHeight + 20);
		tSlider.Apply ();


		t = new Texture2D (PickerWidth, 1);

		float r = 0;
		float g = 0;
		float b = 0;

		ShowColor = new Color (1, 0, 0);

		for (int x = 0; x < PickerWidth; x++) {

			if (x < PickerWidth / 6 + 1) {
				r = 1;
				g = (float)x * 6 / PickerWidth;
				b = 0;
			} else if (x > PickerWidth / 6 && x < PickerWidth / 6 * 2) {
				r = 1 - (x - ((PickerWidth / 6.0F * 2.0F) - (PickerWidth / 6.0F))) / ((PickerWidth / 6.0F * 2.0F) - (PickerWidth / 6.0F));
				g = 1;
				b = 0;
			} else if (x > PickerWidth / 6 && x < PickerWidth / 6 * 3) {
				r = 0;
				g = 1;
				b = (x - ((PickerWidth / 6.0F * 3.0F) - (PickerWidth / 6.0F))) / ((PickerWidth / 6.0F * 2.0F) - (PickerWidth / 6.0F));
			} else if (x > PickerWidth / 6 && x < PickerWidth / 6 * 4) {
				r = 0;
				g = 1 - (x - ((PickerWidth / 6.0F * 4.0F) - (PickerWidth / 6.0F))) / ((PickerWidth / 6.0F * 2.0F) - (PickerWidth / 6.0F));
				b = 1;
			} else if (x > PickerWidth / 6 && x < PickerWidth / 6 * 5) {
				r = (x - ((PickerWidth / 6.0F * 5.0F) - (PickerWidth / 6.0F))) / ((PickerWidth / 6.0F * 2.0F) - (PickerWidth / 6.0F));
				g = 0;
				b = 1;
			} else if (x > PickerWidth / 6 && x < PickerWidth) {
				r = 1;
				g = 0;
				b = 1 - (x - (PickerWidth - (PickerWidth / 6.0F))) / ((PickerWidth / 6.0F * 2.0F) - (PickerWidth / 6.0F));
			}

			ColorRGB [x, 0] = r;
			ColorRGB [x, 1] = g;
			ColorRGB [x, 2] = b;

			t.SetPixel (x, 0, new Color (r, g, b, 1));
		}

		t.Apply ();
		gsAllColor.normal.background = t;

		//---------------------------------------------------------

		tToBlack = new Texture2D (PickerToBlackWidth, 1);

		//-----

		rgb = 0;

		for (int x=0; x<PickerToBlackWidth; x++) {
			rgb = (float)x / PickerToBlackWidth;
			tToBlack.SetPixel (x, 0, new Color (ShowColor.r * rgb, ShowColor.g * rgb, ShowColor.b * rgb));
		}

		tToBlack.Apply ();
		gsToBlackColor.normal.background = tToBlack;

		//-----
		//---------------------------------------------------------

		tToWhite = new Texture2D (PickerToWhiteWidth, 1);

		//-----

		rgb = 0;

		for (int x=0; x<PickerToWhiteWidth; x++) {
			rgb = (float)x / PickerToWhiteWidth;
			tToWhite.SetPixel (x, 0, new Color (ShowColor.r + rgb, ShowColor.g + rgb, ShowColor.b + rgb));
		}

		tToWhite.Apply ();
		gsToWhiteColor.normal.background = tToWhite;

		//-----

	}

	bool PickerSW = false;
	bool PickerToBlackSW = false;
	bool PickerToWhiteSW = false;
	bool SliderLock = true;

	void Update ()
	{
		ShowColor = new Color (
			ColorRGB [PercentBar, 0] * PercentToBlackBar / PickerToBlackWidth + (PercentToWhiteBar / (float)PickerToWhiteWidth),
			ColorRGB [PercentBar, 1] * PercentToBlackBar / PickerToBlackWidth + (PercentToWhiteBar / (float)PickerToWhiteWidth),
			ColorRGB [PercentBar, 2] * PercentToBlackBar / PickerToBlackWidth + (PercentToWhiteBar / (float)PickerToWhiteWidth));

		tShowColor.SetPixel (0, 0, ShowColor);
		tShowColor.Apply ();
		gshowColor.normal.background = tShowColor;


		if (Input.GetMouseButton (0)) {
			if (SliderLock) {
				if (Input.mousePosition.x > PickerX) {
					if (Input.mousePosition.x < PickerWidth + PickerX) {
						if (Screen.height - Input.mousePosition.y > PickerY) {
							if (Screen.height - Input.mousePosition.y - PickerY < PickerHeight) {
								SliderLock = false;
								PickerSW = true;
								PickerToBlackSW = false;
								PickerToWhiteSW = false;
							}
						}
					}
				}
			}

			//-----------------------------
			if (SliderLock) {
				if (Input.mousePosition.x > PickerToBlackX) {
					if (Input.mousePosition.x < PickerToBlackWidth + PickerToBlackX) {
						if (Screen.height - Input.mousePosition.y > PickerToBlackY) {
							if (Screen.height - Input.mousePosition.y - PickerToBlackY < PickerToBlackHeight) {
								SliderLock = false;
								PickerSW = false;
								PickerToBlackSW = true;
								PickerToWhiteSW = false;
							}
						}
					}
				}
			}
			//-----------------------------
			if (SliderLock) {
				if (Input.mousePosition.x > PickerToWhiteX) {
					if (Input.mousePosition.x < PickerToWhiteWidth + PickerToWhiteX) {
						if (Screen.height - Input.mousePosition.y > PickerToWhiteY) {
							if (Screen.height - Input.mousePosition.y - PickerToWhiteY < PickerToWhiteHeight) {
								SliderLock = false;
								PickerSW = false;
								PickerToBlackSW = false;
								PickerToWhiteSW = true;
							}
						}
					}
				}
			}
			//-----------------------------

		}
		else {
			SliderLock = true;
			PickerSW = false;
			PickerToBlackSW = false;
			PickerToWhiteSW = false;
		}

		if (PickerSW) {
			if (Input.mousePosition.x > PickerX) {
				if (Input.mousePosition.x < PickerWidth + PickerX) {
					PercentBar = (int)(Input.mousePosition.x - PickerX);
					PickerBarRect = new Rect (Input.mousePosition.x - 10, PickerY - 10, 20, PickerHeight + 20);

					OriginalColor = new Color (
						ColorRGB [PercentBar, 0],
						ColorRGB [PercentBar, 1],
						ColorRGB [PercentBar, 2]);

					//-----

					rgb = 0;

					for (int x=0; x<PickerToBlackWidth; x++) {
						rgb = (float)x / PickerToBlackWidth;
						tToBlack.SetPixel (x, 0, new Color (OriginalColor.r * rgb, OriginalColor.g * rgb, OriginalColor.b * rgb));
					}

					tToBlack.Apply ();
					gsToBlackColor.normal.background = tToBlack;

					//-----

					rgb = 0;

					for (int x=0; x<PickerToWhiteWidth; x++) {
						rgb = (float)x / PickerToWhiteWidth;
						tToWhite.SetPixel (x, 0, new Color (OriginalColor.r + rgb, OriginalColor.g + rgb, OriginalColor.b + rgb));
					}

					tToWhite.Apply ();
					gsToWhiteColor.normal.background = tToWhite;

					//-----
				}
			}
		}
		else if (PickerToBlackSW) {
			if (Input.mousePosition.x > PickerToBlackX) {
				if (Input.mousePosition.x < PickerToBlackWidth + PickerToBlackX) {
					PercentToBlackBar = (int)(Input.mousePosition.x - PickerToBlackX);
					PickerToBlackBarRect = new Rect (Input.mousePosition.x - 10, PickerToBlackY - 10, 20, PickerToBlackHeight + 20);
				}
			}
		}
		else if (PickerToWhiteSW) {
			if (Input.mousePosition.x > PickerToWhiteX) {
				if (Input.mousePosition.x < PickerToWhiteWidth + PickerToWhiteX) {
					PercentToWhiteBar = (int)(Input.mousePosition.x - PickerToWhiteX);
					PickerToWhiteBarRect = new Rect (Input.mousePosition.x - 10, PickerToWhiteY - 10, 20, PickerToWhiteHeight + 20);
				}
			}
		}
	}

	void OnGUI ()
	{
		GUI.Label (new Rect (ShowColorX, ShowColorY, ShowColorWidth, ShowColorHeight), "", gshowColor);
		GUI.Label (new Rect (PickerX, PickerY, PickerWidth, PickerHeight), "", gsAllColor);
		GUI.Label (PickerBarRect, tSlider);
		GUI.Label (new Rect (PickerToBlackX, PickerToBlackY, PickerToBlackWidth, PickerToBlackHeight), "", gsToBlackColor);
		GUI.Label (PickerToBlackBarRect, tSlider);
		GUI.Label (new Rect (PickerToWhiteX, PickerToWhiteY, PickerToWhiteWidth, PickerToWhiteHeight), "", gsToWhiteColor);
		GUI.Label (PickerToWhiteBarRect, tSlider);
	}
}
