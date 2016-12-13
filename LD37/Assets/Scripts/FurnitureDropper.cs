using UnityEngine;
using System.Collections;

public class FurnitureDropper : MonoBehaviour {
	[SerializeField]
	Rigidbody2D[] furniture;
	[SerializeField]
	float speed = 2;
	[SerializeField]
	GameObject selectWarningText;

	[HideInInspector]
	public Rigidbody2D selectedFurniture; //Assigned via FurnitureSelector.cs

	bool canDrop = true;
	Rigidbody2D body;
	int itemsDropped = 0;

	void Start() {
		body = GetComponent<Rigidbody2D>();
		body.velocity = Vector2.left * speed;
	}

	void Update() {
		if (Input.GetMouseButtonDown(0) && canDrop) {
			Drop();
		}

		//Move back and forth
		float posOnScreen = Camera.main.WorldToScreenPoint(transform.position).x;
        if ((posOnScreen < 0 && body.velocity.x < 0) //On the left and moving left
			|| (posOnScreen > Screen.width && body.velocity.x > 0)) { //On the right and moving right
			body.velocity *= -1; //Reverse direction
        }
	}

	void Drop() {
		if (selectedFurniture != null) {
			Rigidbody2D obj = (Rigidbody2D)Instantiate(selectedFurniture, transform.position, Quaternion.identity);
			obj.velocity = body.velocity;
			selectedFurniture = null;
			obj.GetComponent<SpriteRenderer>().sortingOrder = itemsDropped;
			itemsDropped++;
        }
		else {
			selectWarningText.SetActive(true);
			Invoke("TurnOffWarningText", 2);
        }
	}

	void TurnOffWarningText() {
		selectWarningText.SetActive(false);
	}

	public void PickFurniture(int index) {
		TurnOffWarningText();
        selectedFurniture = furniture[index];
    }

	public void SetDropEnabled(bool value) {
		canDrop = value;
    }
}