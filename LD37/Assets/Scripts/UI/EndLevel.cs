using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EndLevel : MonoBehaviour {
	public Sprite ClientSprite;
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

	private string[] clientDialogue;
	private GameObject dialogueObject;

	void ShowResults() {
		Blur.SetActive( false );
		DialogueObjects.SetActive( false );
		FinishLevelButton.SetActive( true );
	}

	public void SecondDialogue() {
		BlueprintObjects.SetActive( false );
		DialogueObjects.SetActive( true );
		if ( ClientSecondText != null ) {
			clientDialogue = ( ClientSecondText.text.Split( '\n' ) );
			dialogueObject = GameObject.Find( "ClientDialogue" );
			Text textComponent = dialogueObject.GetComponent<Text>();
			Action callback = () => ShowResults();
			StartCoroutine( UIUtils.ScrollText( textComponent, clientDialogue, callback ) );
		}
	}

	public void ShowBlueprint () {
		DialogueObjects.SetActive( false );
		BlueprintObjects.SetActive( true );
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
			clientDialogue = ( ClientFirstText.text.Split( '\n' ) );
			dialogueObject = GameObject.Find( "ClientDialogue" );
			Text textComponent = dialogueObject.GetComponent<Text>();
			Action callback = () => ShowBlueprint();
			StartCoroutine( UIUtils.ScrollText( textComponent, clientDialogue, callback ) );
		}

		if ( ClientSprite != null ) {
			GameObject client = GameObject.Find( "Client" );
			Image clientImage = client.GetComponent<Image>();
			clientImage.sprite = ClientSprite;
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
