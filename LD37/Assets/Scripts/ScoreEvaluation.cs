using UnityEngine;
using System;

/* Scoring Rules
* ?Drop as many objects as you want.
* ?Subtract points per extra object dropped.
* Only calculate points for the closest object to its target.
* Gain points for close distance and straight orientation.
*/

public class ScoreEvaluation : MonoBehaviour {
	public int totalScore = 0;
	public int orientationScore = 0;
	public int distanceScore = 0;
	public int objectCountScore = 0;

	public string[] furnitureNames;
	public int[] expectedCounts;
	GameObject[][] furnitureItems;
	Transform[] targets;
	
	void Start () {
		targets = GetComponentsInChildren<Transform>();
		//Invoke("Evaluate", 10); //Debug to test evaluation in 10 secs
	}

	int GetCountScore( GameObject[] objects, int expectedCount ) {
		Debug.Assert( expectedCount > 0 );
		float stepValue = 100 / expectedCount;
		// The targets have the same tag as the game objects, subtract the expectedCount to figure out our actual count.
		int objectCount = objects.Length - expectedCount;
		int stepCount = Math.Abs( expectedCount - objectCount );
		return ( int )Math.Ceiling( Math.Max( 100 - ( stepValue * stepCount ), 0 ) );
	}

	public void Evaluate() {
		//Collect dropped furniture
		int objectCountTotal = 0;
		furnitureItems = new GameObject[furnitureNames.Length][];
		Debug.Assert( furnitureNames.Length == expectedCounts.Length );
		for ( int furnitureIndex = 0; furnitureIndex < furnitureNames.Length; furnitureIndex++ ) {
			furnitureItems[furnitureIndex] = GameObject.FindGameObjectsWithTag( furnitureNames[furnitureIndex] );
			int itemScore = GetCountScore( furnitureItems[furnitureIndex], expectedCounts[furnitureIndex] );
			objectCountTotal += itemScore;
		}
		objectCountScore = ( int )Math.Ceiling( ( float )( objectCountTotal / furnitureNames.Length ) );

		int numFurnitureCalculated = 0;
		foreach ( Transform target in targets ) {
			for ( int furnitureIndex = 0; furnitureIndex < furnitureNames.Length; furnitureIndex++ ) {
				if ( target.CompareTag( furnitureNames[furnitureIndex] ) ) {
					numFurnitureCalculated += 1;
					CalculateDistancesAndOrientation( target, furnitureItems[furnitureIndex] );
				}
			}
		}
		distanceScore = distanceScore / numFurnitureCalculated;
		orientationScore /= numFurnitureCalculated;
		totalScore = ( int )Math.Ceiling( ( float )( ( objectCountScore + distanceScore + orientationScore ) / 3 ) );
	}

	void CalculateDistancesAndOrientation( Transform target, GameObject[] furniture ) {
		float orientationOffset = -1; //1 = upright, 0 = sideways, -1 = upsidedown
		float closestDist = 4096;
		foreach ( GameObject obj in furniture ) {
			if ( obj.name.IndexOf( "Target" ) != -1 ) {
				continue;
			}
			float dist = Vector2.Distance( obj.transform.position, target.position );
			if ( dist < closestDist ) {
				closestDist = dist;
				orientationOffset = Vector2.Dot( obj.transform.up, Vector2.up );
            }
		}
		// HACK: Assuming 0.3 is the perfect score, add 0.7 to closestDist before calculating.
		closestDist += 0.7f;
		distanceScore += Math.Min( 100, ( int )( 100 / closestDist ) );
		orientationScore += Math.Min( 100, ( int )( 50 * ( 1.1 + orientationOffset ) ) );
	}
}