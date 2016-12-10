using UnityEngine;
using System.Collections;

public class FurnitureDropper : MonoBehaviour {
	[SerializeField]
	Rigidbody2D[] furniture;
	[SerializeField]
	float speed = 2;

	public Rigidbody2D selectedFurniture; //Assigned via FurnitureSelector.cs

	Rigidbody2D body;

	void Start() {
		body = GetComponent<Rigidbody2D>();
		body.velocity = Vector2.left * speed;
	}

	void Update() {
		if (Input.anyKeyDown) {
			Drop();
		}

		//Move back and forth
		float posOnScreen = Camera.main.WorldToScreenPoint(transform.position).x;
        if (posOnScreen < 0 || posOnScreen > Screen.width) {
			body.velocity *= -1;
        }
	}

	void Drop() {
		if (selectedFurniture != null) {
			Instantiate(selectedFurniture);
		}
		else {
			//TODO: Should expose this in the on screen UI
			Debug.Log("Select the furniture you want to drop first!");
		}
	}
}
