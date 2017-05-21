using UnityEngine;
using System.Collections;

[System.Serializable]
public class SaveGame {
	public enum Results {
		NONE,
		WORST,
		INTERMEDIATE,
		BEST
	}

	public static SaveGame CurrentSaveGame;
	public int TerryResults;

	public SaveGame () {
		TerryResults = (int)Results.NONE;
	}
}