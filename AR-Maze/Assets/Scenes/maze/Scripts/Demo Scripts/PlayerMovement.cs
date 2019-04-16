using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour {
	[HideInInspector]
	public int currentTarget = 0;
	int lastTarget = 0;
	List<int> path;
	int currentPoint = 0;
	GameObject floorHolder;
	[HideInInspector]
	public int startPont = 0;
	[HideInInspector]
	public Transform currentFloor;
	// Use this for initialization
	void Start () {
		path = GameObject.FindObjectOfType<MazePathFinder> ().FindPath (startPont, currentTarget);
	}
	
	// Update is called once per frame
	void Update () {
		if (!floorHolder){
			floorHolder = GameObject.Find ("FloorHolder");
			return;
		}

		if (lastTarget != currentTarget) {
			path = new List<int>();
			path.Clear ();
			path = GameObject.FindObjectOfType<MazePathFinder> ().FindPath (startPont, currentTarget);
			lastTarget = currentTarget;
			currentPoint = 0;
		} else {
			if (currentPoint < path.Count){
				transform.position = Vector3.MoveTowards(transform.position,floorHolder.transform.GetChild(path[currentPoint]).transform.position, 0.05f);
				if ((transform.position-floorHolder.transform.GetChild(path[currentPoint]).transform.position).magnitude <= 0.25f){
					currentPoint++;
				}
			}
		}
	}

	void ChangeRoute (Transform end){
		//Get StartIndex
		for (int i = 0; i < floorHolder.transform.childCount; i++) {
			if (currentFloor == floorHolder.transform.GetChild(i).transform){
				startPont = i;
				break;
			}
		}
		//Get EndIndex
		for (int i = 0; i < floorHolder.transform.childCount; i++) {
			if (end == floorHolder.transform.GetChild(i).transform){
				currentTarget = i;
				break;
			}
		}
	}

	void OnCollisionStay (Collision other){
		if (other.collider.tag == "CellFloor") {
			currentFloor = other.collider.transform;
		}
	}
}
