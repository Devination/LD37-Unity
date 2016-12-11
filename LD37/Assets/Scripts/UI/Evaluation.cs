using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evaluation : MonoBehaviour {
	public Sprite ClientSprite;

	private string[] clientDialogue;
	private ClientSceneState sceneState;
	private GameObject dialogueObject;

	void UpdateSceneState () {
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
	}
}
