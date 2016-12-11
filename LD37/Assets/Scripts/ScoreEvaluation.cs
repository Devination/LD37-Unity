using UnityEngine;

/* Scoring Rules
* ?Drop as many objects as you want.
* ?Subtract points per extra object dropped.
* Only calculate points for the closest object to its target.
* Gain points for close distance and straight orientation.
*/

public class ScoreEvaluation : MonoBehaviour {
	public int score = 0;
	Transform[] targets;

	void Start () {
		targets = GetComponentsInChildren<Transform>();
		//Invoke("Evaluate", 10); //Debug to test evaluation in 10 secs
	}

	public void Evaluate() {
		//Collect dropped furniture
		GameObject[] beds = GameObject.FindGameObjectsWithTag("Bed");
		GameObject[] rugs = GameObject.FindGameObjectsWithTag("Rug");

		foreach (Transform target in targets) {
			if (target.CompareTag("Bed")) {
				CalculateDistancesAndOrientation(beds);
            }
			else if (target.CompareTag("Rug")) {
				CalculateDistancesAndOrientation(rugs);
			}
		}
	}

	void CalculateDistancesAndOrientation(GameObject[] furniture) {
		float orientationOffset = -1; //1 = upright, 0 = sideways, -1 = upsidedown
		float closestDist = 4096;
		foreach (GameObject obj in furniture) {
			float dist = Vector2.Distance(obj.transform.position, transform.position);
			if (dist < closestDist) {
				closestDist = dist;
				orientationOffset = Vector2.Dot(obj.transform.up, Vector2.up);
            }
		}
		score += (int)(100 * closestDist) + (int)(100 * (1.5 + orientationOffset));
		Debug.Log(score);
	}
}