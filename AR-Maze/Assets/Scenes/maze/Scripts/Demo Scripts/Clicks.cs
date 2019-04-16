using UnityEngine;
using System.Collections;

public class Clicks : MonoBehaviour {
	PlayerMovement player;
	bool scriptAttched = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!player) {
			if (GameObject.Find ("FloorHolder") && !scriptAttched) {
				GameObject.Find ("FloorHolder").AddComponent<SpawnPlayer>();
				scriptAttched = true;
			}
			player = GameObject.FindObjectOfType<PlayerMovement>();
		}


		if (Input.GetMouseButtonDown (0)) {
			RaycastHit hit = new RaycastHit();
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray,out hit, 100.0f)){
				if (hit.transform.tag == "CellFloor"){
					hit.transform.GetComponent<Renderer>().material.color = Color.red;
					if (player)
						player.SendMessage("ChangeRoute",hit.transform);
				}
			}
		}
	}
}
