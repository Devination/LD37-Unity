using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitApplier : MonoBehaviour {
	public string Character;
	Image portrait;
	// Use this for initialization
	void Start () {
		portrait = GetComponent<Image>();
		int result = 0;
		if( Character == "terry" ) {
			result = SaveGame.CurrentSaveGame.TerryResults;
		}

		string sprite;
		switch( result ) {
			case ( (int)SaveGame.Results.NONE ):
			case ( (int)SaveGame.Results.WORST ):
				sprite = Character + "_face_sad";
				break;
			case ( ( int )SaveGame.Results.INTERMEDIATE ):
				sprite = Character + "_face_neutral";
				break;
			default:
				sprite = Character + "_face_happy";
				break;
		}
		portrait.sprite = Resources.Load<Sprite>( sprite );
	}
}
