//Used to unfreeze the rotation after the furniture scaleIn anim. Unity's anim system couldn't keyframe this for some reason.
using UnityEngine;

public class RotationUnfreezer : MonoBehaviour {
	Rigidbody2D body;

	private void Start() {
		body = GetComponent<Rigidbody2D>();
	}

	public void UnfreezeRotation() {
		body.freezeRotation = false;
	}
}
