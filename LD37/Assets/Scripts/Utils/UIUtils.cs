using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

public static class UIUtils {
	public static IEnumerator ScrollText ( Text textComponent, string[] text ) {
		for ( int currentTextLine = 0; currentTextLine < text.Length; currentTextLine++ ) {
			string currentString = text[currentTextLine];
			textComponent.text = "";
			for ( int characterIndex = 0; characterIndex < currentString.Length; characterIndex++ ) {
				textComponent.text += currentString[characterIndex];
				yield return new WaitForSeconds( 0.05f );
			}
		}
	}

	public static IEnumerator ScrollTextWithCallback ( Text textComponent, string[] text, System.Action callback ) {
		for ( int currentTextLine = 0; currentTextLine < text.Length; currentTextLine++ ) {
			string currentString = text[currentTextLine];
			textComponent.text = "";
			for ( int characterIndex = 0; characterIndex < currentString.Length; characterIndex++ ) {
				textComponent.text += currentString[characterIndex];
				yield return new WaitForSeconds( 0.05f );
			}
		}
		callback();
	}
}
