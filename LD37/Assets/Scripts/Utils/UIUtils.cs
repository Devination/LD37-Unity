using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

public static class UIUtils {
	public static IEnumerator ScrollText ( Text textComponent, string text ) {
		textComponent.text = "";
		for ( int characterIndex = 0; characterIndex < text.Length; characterIndex++ ) {
			if ( textComponent.text != text ) {
				textComponent.text += text[characterIndex];
				yield return new WaitForSeconds( 0.05f );
			}
		}
	}

	public static IEnumerator ScrollTextWithCallback ( Text textComponent, string text, System.Action callback ) {
		textComponent.text = "";
		for ( int characterIndex = 0; characterIndex < text.Length; characterIndex++ ) {
			if( textComponent.text != text ) {
				textComponent.text += text[characterIndex];
				yield return new WaitForSeconds( 0.05f );
			}
		}
		callback();
	}

	public static string GetTriggerText ( int triggerValue ) {
		switch( triggerValue ) {
			case (int)SaveGame.Results.BEST:
				return "happy";
			case (int)SaveGame.Results.WORST:
				return "sad";
			default:
				return "neutral";
		}
	}
}
