using UnityEngine;
using System.Collections;

public class FurnitureDropper : MonoBehaviour {
	[SerializeField]
	Rigidbody2D[] furniture;
	[SerializeField]
	float speed = 2;
	[SerializeField]
	GameObject selectWarningText;
	SpriteRenderer art;

	int selectedIndex = -1;
	HingeJoint2D hinge;
	Rigidbody2D selectedFurniture;
	LineRenderer[] ropes;

	bool canDrop = true;
	Rigidbody2D body;
	int itemsDropped = 0;

	void Start() {
		body = GetComponent<Rigidbody2D>();
		hinge = GetComponent<HingeJoint2D>();
		art = GetComponentInChildren<SpriteRenderer>();
		body.velocity = Vector2.left * speed;
		ropes = GetComponentsInChildren<LineRenderer>();
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
			art.flipX = !art.flipX;
		}
	}

	void Drop() {
		if (selectedFurniture != null) {
			selectedFurniture.velocity = body.velocity;
			selectedFurniture.GetComponent<SpriteRenderer>().sortingOrder = itemsDropped;
			itemsDropped++;
			hinge.connectedBody = null;
			selectedFurniture = null;
			selectedIndex = -1;
		}
		else {
			selectWarningText.SetActive(true);
			Invoke("TurnOffWarningText", 2);
		}
	}

	void TurnOffWarningText() {
		selectWarningText.SetActive(false);
	}

	void DetachRopes() {
		for (int i = 0; i < ropes.Length; ++i) {
			ropes[i].SetPosition(1, Vector3.zero);
		}
	}

	IEnumerator AttachRopes(Transform parent) {
		DetachRopes();
		yield return new WaitForSeconds(0.1f);
		while (selectedIndex > -1) {
			for (int i=0; i < ropes.Length && i < parent.childCount; ++i) {
				ropes[i].SetPosition(1, parent.GetChild(i).position - transform.position);
			}
			yield return new WaitForEndOfFrame();
		}
		DetachRopes();
	}

	//Called via Canvas Buttons, not .cs scripts
	public void PickFurniture(int index) {
		TurnOffWarningText();
		if (selectedIndex == index) {
			return;
		}
		if (selectedFurniture != null) {
			StopAllCoroutines();
			Destroy(selectedFurniture.gameObject);
		}
		selectedIndex = index;
		Vector2 pos = transform.position;
		pos.y -= 1;
		selectedFurniture = (Rigidbody2D)Instantiate(furniture[index], pos, Quaternion.identity);
		hinge.connectedBody = selectedFurniture;
		StartCoroutine(AttachRopes(selectedFurniture.transform));
	}

	public void SetDropEnabled(bool value) {
		canDrop = value;
	}
}