using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Evaluation : MonoBehaviour {
	public Sprite ClientSprite;

	GameObject dialogueObject;
	[SerializeField]
	Animator clientAnim;
	string currentText;

	private enum EvaluationState {
		Distance,
		Orientation,
		Count,
		Overall,
	};
	EvaluationState currentEvaluationState;

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

	int GetResultOrientation ( int score ) {
		Results Result;
		if ( score <= 60 ) {
			Result = Results.Worst;
		}
		else if ( score <= 85 ) {
			Result = Results.Intermediate;
		}
		else {
			Result = Results.Best;
		}
		return ( int )Result;
	}

	int GetResultCount ( int score ) {
		Results Result;
		if ( score >= 100 ) {
			Result = Results.Best;
		}
		else if ( score >= 60 ) {
			Result = Results.Intermediate;
		}
		else {
			Result = Results.Worst;
		}
		return ( int )Result;
	}

	void SetHeaderText ( string newText ) {
		GameObject header = GameObject.Find( "Header" );
		Text headerText = header.GetComponent<Text>();
		headerText.text = newText;
	}

	void HandleTotalResults () {
		currentEvaluationState = EvaluationState.Overall;
		int totalScore = PlayerPrefs.GetInt( "Total Score" );
		int result = GetResult( totalScore );
		clientAnim.SetTrigger( UIUtils.GetTriggerText( result ) );
		SetHeaderText( "Final Evaluation" );
		currentText = Overall[result];
		Text textComponent = dialogueObject.GetComponent<Text>();
		StartCoroutine( UIUtils.ScrollText( textComponent, currentText ) );
	}

	void HandleCountResults() {
		currentEvaluationState = EvaluationState.Count;
		int countScore = PlayerPrefs.GetInt( "Count Score" );
		int result = GetResultCount( countScore );
		clientAnim.SetTrigger( UIUtils.GetTriggerText( result ) );
		SetHeaderText( "Furniture Count" );
		currentText = Count[result];
		Text textComponent = dialogueObject.GetComponent<Text>();
		StartCoroutine( UIUtils.ScrollText( textComponent, currentText ) );
	}

	void HandleOrientationResults() {
		currentEvaluationState = EvaluationState.Orientation;
		int orientationScore = PlayerPrefs.GetInt( "Orientation Score" );
		int result = GetResultOrientation( orientationScore );
		clientAnim.SetTrigger( UIUtils.GetTriggerText( result ) );
		SetHeaderText( "Furniture Orientation" );
		currentText = Orientation[result];
		Text textComponent = dialogueObject.GetComponent<Text>();
		StartCoroutine( UIUtils.ScrollText( textComponent, currentText ) );
	}

	public void ScrollNextTextLine () {
		Text textComponent = dialogueObject.GetComponent<Text>();
		if ( textComponent.text == currentText ) {
			switch ( currentEvaluationState ) {
				case EvaluationState.Distance:
					HandleOrientationResults();
					break;
				case EvaluationState.Orientation:
					HandleCountResults();
					break;
				case EvaluationState.Count:
					HandleTotalResults();
					break;
				default:
					break;
			}
		}
		else {
			textComponent.text = currentText;
		}
	}

	// Use this for initialization
	void Start () {
		if ( ClientSprite != null ) {
			GameObject client = GameObject.Find( "Client" );
			Image clientImage = client.GetComponent<Image>();
			clientImage.sprite = ClientSprite;
		}

		currentEvaluationState = EvaluationState.Distance;
		int distanceScore = PlayerPrefs.GetInt( "Distance Score" );
		int result = GetResult( distanceScore );
		clientAnim.SetTrigger( UIUtils.GetTriggerText( result ) );
		dialogueObject = GameObject.Find( "ClientDialogue" );
		currentText = Distance[result];
		Text textComponent = dialogueObject.GetComponent<Text>();
		StartCoroutine( UIUtils.ScrollText( textComponent, currentText ) );
	}
}
