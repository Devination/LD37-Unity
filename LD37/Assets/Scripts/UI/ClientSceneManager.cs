using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ClientSceneManager : MonoBehaviour {
	public Sprite BlueprintSprite;
	public TextAsset ClientText;

	[SerializeField] GameObject BlueprintImage;
	[SerializeField] GameObject ProceedButton;
	[SerializeField] Animator clientAnim;

	private string[] clientDialogue;
	int currentTextLine;
	private enum ClientSceneState {
		Talking,
		Blueprint,
		Done
	};
	private ClientSceneState sceneState;
	private GameObject dialogueObject;

	public void ScrollNextTextLine() {
		Text textComponent = dialogueObject.GetComponent<Text>();
		if( textComponent.text != clientDialogue[currentTextLine] ) {
			textComponent.text = clientDialogue[currentTextLine];
		}
		else if ( currentTextLine + 1 >= clientDialogue.Length ) {
			sceneState = ClientSceneState.Blueprint;
		}
		else {
			currentTextLine++;
			StartCoroutine( UIUtils.ScrollText( textComponent, clientDialogue[currentTextLine] ) );
		}
	}

	// Use this for initialization
	void Start () {
		sceneState = ClientSceneState.Talking;
		currentTextLine = 0;
		if ( ClientText != null ) {
			clientDialogue = ( ClientText.text.Split( '\n' ) );
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
	}
	
	// Update is called once per frame
	void Update () {
		if ( sceneState == ClientSceneState.Blueprint ) {
			GameObject textBox = GameObject.Find( "DialogueBox" );
			textBox.SetActive( false );
			BlueprintImage.SetActive( true );
			ProceedButton.SetActive( true );
			sceneState = ClientSceneState.Done;
		}
	}
}
