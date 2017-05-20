using UnityEngine;
using System.Collections;

public class FurnitureDropper : MonoBehaviour {
	[SerializeField]
	Rigidbody2D[] furniture;
	[SerializeField]
	float speed = 2;
	[SerializeField]
	GameObject selectWarningText;

	HingeJoint2D hinge;
	Rigidbody2D selectedFurniture;

	bool canDrop = true;
	Rigidbody2D body;
	int itemsDropped = 0;

	void Start() {
		body = GetComponent<Rigidbody2D>();
		hinge = GetComponent<HingeJoint2D>();
		body.velocity = Vector2.left * speed;
	}

	void Update() {
		if ((Input.GetMouseButtonDown(0) || Input.GetButtonDown("Fire1")) && canDrop) {
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
			selectedFurniture.velocity = body.velocity;
			selectedFurniture.GetComponent<SpriteRenderer>().sortingOrder = itemsDropped;
			itemsDropped++;
			hinge.connectedBody = null;
			selectedFurniture = null;
		}
		else {
			selectWarningText.SetActive(true);
			Invoke("TurnOffWarningText", 2);
		}
	}

	void TurnOffWarningText() {
		selectWarningText.SetActive(false);
	}

	//Called via Canvas Buttons, not .cs scripts
	public void PickFurniture(int index) {
		TurnOffWarningText();
		if (selectedFurniture != null) {
			Destroy(selectedFurniture.gameObject);
		}
		Vector2 pos = transform.position;
		pos.y -= 1;
		selectedFurniture = (Rigidbody2D)Instantiate(furniture[index], pos, Quaternion.identity);
		hinge.connectedBody = selectedFurniture;
	}

	public void SetDropEnabled(bool value) {
		canDrop = value;
	}
}