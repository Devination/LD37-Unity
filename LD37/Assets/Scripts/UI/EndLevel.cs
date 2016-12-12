using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EndLevel : MonoBehaviour {
	public Sprite BlueprintSprite;
	public TextAsset ClientFirstText;
	public TextAsset ClientSecondText;

	[SerializeField]
	GameObject DialogueObjects;
	[SerializeField]
	GameObject BlueprintObjects;
	[SerializeField]
	GameObject Blur;
	[SerializeField]
	GameObject FinishLevelButton;
	[SerializeField]
	Animator clientAnim;

	string[] clientDialogue;
	int currentTextLine;
	GameObject dialogueObject;

	enum EndLevelState {
		FirstDialogue,
		SecondDialogue,
	};
	EndLevelState currentEndLevelState;

	void ShowResults () {
		Blur.SetActive( false );
		DialogueObjects.SetActive( false );
		FinishLevelButton.SetActive( true );
	}

	void ShowBlueprint () {
		DialogueObjects.SetActive( false );
		BlueprintObjects.SetActive( true );
	}

	public void ScrollNextTextLine() {
		Text textComponent = dialogueObject.GetComponent<Text>();
		if ( textComponent.text != clientDialogue[currentTextLine] ) {
			textComponent.text = clientDialogue[currentTextLine];
		}
		else if ( currentTextLine + 1 >= clientDialogue.Length ) {
			switch ( currentEndLevelState ) {
				case EndLevelState.SecondDialogue:
					ShowResults();
					break;
				case EndLevelState.FirstDialogue:
					ShowBlueprint();
					break;
			}
		}
		else {
			currentTextLine++;
			StartCoroutine( UIUtils.ScrollText( textComponent, clientDialogue[currentTextLine] ) );
		}
	}

	public void SecondDialogue() {
		BlueprintObjects.SetActive( false );
		DialogueObjects.SetActive( true );
		if ( ClientSecondText != null ) {
			currentTextLine = 0;
			currentEndLevelState = EndLevelState.SecondDialogue;
			clientDialogue = ( ClientSecondText.text.Split( '\n' ) );
			Text textComponent = dialogueObject.GetComponent<Text>();
			StartCoroutine( UIUtils.ScrollText( textComponent, clientDialogue[currentTextLine] ) );
		}
	}

	// Use this for initialization
	public void StartEndLevel () {
		GameObject helicopter = GameObject.Find( "Helicopter" );
		Destroy( helicopter );
		GameObject furniturePanel = GameObject.Find( "FurniturePanel" );
		furniturePanel.SetActive( false );
		DialogueObjects.SetActive( true );
		Blur.SetActive( true );
		if ( ClientFirstText != null ) {
			currentTextLine = 0;
			currentEndLevelState = EndLevelState.FirstDialogue;
			clientDialogue = ( ClientFirstText.text.Split( '\n' ) );
			dialogueObject = GameObject.Find( "ClientDialogue" );
			Text textComponent = dialogueObject.GetComponent<Text>();
			StartCoroutine( UIUtils.ScrollText( textComponent, clientDialogue[currentTextLine] ) );
			clientAnim.SetTrigger( UIUtils.GetTriggerText( 0 ) );
		}

		if ( BlueprintSprite != null ) {
			GameObject blueprint = GameObject.Find( "Blueprint" );
			Image blueprintImage = blueprint.GetComponent<Image>();
			blueprintImage.sprite = BlueprintSprite;
		}

		ScoreEvaluation scoreEvaluation = FindObjectOfType<ScoreEvaluation>();
		scoreEvaluation.Evaluate();
		PlayerPrefs.SetInt( "Total Score", scoreEvaluation.totalScore );
		PlayerPrefs.SetInt( "Distance Score", scoreEvaluation.distanceScore );
		PlayerPrefs.SetInt( "Count Score", scoreEvaluation.objectCountScore );
		PlayerPrefs.SetInt( "Orientation Score", scoreEvaluation.orientationScore );
	}
}
