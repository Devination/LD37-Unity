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
	private enum ClientSceneState {
		Talking,
		Blueprint,
		Done
	};
	private ClientSceneState sceneState;
	private GameObject dialogueObject;

	void UpdateSceneState() {
		sceneState = ClientSceneState.Blueprint;
	}

	// Use this for initialization
	void Start () {
		sceneState = ClientSceneState.Talking;
		if ( ClientText != null ) {
			clientDialogue = ( ClientText.text.Split( '\n' ) );
			dialogueObject = GameObject.Find( "ClientDialogue" );
			Text textComponent = dialogueObject.GetComponent<Text>();
			Action callback = () => UpdateSceneState();
			StartCoroutine( UIUtils.ScrollTextWithCallback( textComponent, clientDialogue, callback ) );
			clientAnim.SetTrigger(0);
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
			GameObject textBox = GameObject.Find( "TextBox" );
			textBox.SetActive( false );
			BlueprintImage.SetActive( true );
			ProceedButton.SetActive( true );
			sceneState = ClientSceneState.Done;
		}
	}
}
