using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Evaluation : MonoBehaviour {
	public Sprite ClientSprite;

	private string[] clientDialogue;
	private GameObject dialogueObject;
	[SerializeField]
	Animator clientAnim;

	private enum Results {
		Intermediate,
		Best,
		Worst,
	};
	private string[] Distance = {
		"They're c-close! Good job.",
		"Amazing! You dropped that stuff right where I needed it. I see why you're the pro.",
		"U-uh... Those things aren't where I wanted them.",
	};
	private string[] Orientation = {
		"I think this will do.",
		"Woah, everything is so straight!",
		"Hmm, I don't think everything is the right way up.",
	};
	private string[] Count = {
		"This is almost exactly what I needed!",
		"Uwaaaa, all my items are here!",
		"M-math is hard I guess.",
	};
	private string[] Overall = {
		"OK: Thank you for your hard work.",
		"GREAT: You've blown me away! Are we best friends now?",
		"TERRIBLE: M-maybe you can try a bit harder next time.",
	};

	int GetResult ( int score ) {
		Results Result;
		if ( score <= 60 ) {
			Result = Results.Worst;
		} else if ( score <= 90 ) {
			Result = Results.Intermediate;
		} else {
			Result = Results.Best;
		}
		return ( int )Result;
	}

	void SetHeaderText ( string newText ) {
		GameObject header = GameObject.Find( "Header" );
		Text headerText = header.GetComponent<Text>();
		headerText.text = newText;
	}

	void HandleTotalResults () {
		int totalScore = PlayerPrefs.GetInt( "Total Score" );
		int result = GetResult( totalScore );
		clientAnim.SetTrigger( UIUtils.GetTriggerText( result ) );
		SetHeaderText( "Final Evaluation" );
		clientDialogue[0] = Overall[result];
		dialogueObject = GameObject.Find( "ClientDialogue" );
		Text textComponent = dialogueObject.GetComponent<Text>();
		StartCoroutine( UIUtils.ScrollText( textComponent, clientDialogue ) );
	}

	void HandleCountResults() {
		int countScore = PlayerPrefs.GetInt( "Count Score" );
		int result;
		if ( countScore >= 100 ) {
			result = ( int )Results.Best;
		} else if ( countScore >= 90 ) {
			result = ( int )Results.Intermediate;
		} else {
			result = GetResult( countScore );
		}
		clientAnim.SetTrigger( UIUtils.GetTriggerText( result ) );
		SetHeaderText( "Furniture Count" );
		clientDialogue[0] = Count[result];
		dialogueObject = GameObject.Find( "ClientDialogue" );
		Text textComponent = dialogueObject.GetComponent<Text>();
		Action callback = () => HandleTotalResults();
		StartCoroutine( UIUtils.ScrollTextWithCallback( textComponent, clientDialogue, callback ) );
	}

	void HandleOrientationResults() {
		int orientationScore = PlayerPrefs.GetInt( "Orientation Score" );
		int result = GetResult( orientationScore );
		clientAnim.SetTrigger( UIUtils.GetTriggerText( result ) );
		SetHeaderText( "Furniture Orientation" );
		clientDialogue[0] = Orientation[result];
		dialogueObject = GameObject.Find( "ClientDialogue" );
		Text textComponent = dialogueObject.GetComponent<Text>();
		Action callback = () => HandleCountResults();
		StartCoroutine( UIUtils.ScrollTextWithCallback( textComponent, clientDialogue, callback ) );
	}

	// Use this for initialization
	void Start () {
		if ( ClientSprite != null ) {
			GameObject client = GameObject.Find( "Client" );
			Image clientImage = client.GetComponent<Image>();
			clientImage.sprite = ClientSprite;
		}

		// HACK: Size 1 because the dialogue function takes a string array.
		clientDialogue = new string[1];
		int distanceScore = PlayerPrefs.GetInt( "Distance Score" );
		int result = GetResult( distanceScore );
		clientAnim.SetTrigger( UIUtils.GetTriggerText( result ) );
		clientDialogue[0] = Distance[result];
		dialogueObject = GameObject.Find( "ClientDialogue" );
		Text textComponent = dialogueObject.GetComponent<Text>();
		Action callback = () => HandleOrientationResults();
		StartCoroutine( UIUtils.ScrollTextWithCallback( textComponent, clientDialogue, callback ) );
	}
}
