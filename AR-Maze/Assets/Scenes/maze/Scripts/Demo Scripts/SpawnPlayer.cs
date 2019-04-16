using UnityEngine;
using System.Collections;

public class SpawnPlayer : MonoBehaviour {

	[HideInInspector]
	public int spawnInCell = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.childCount > 0) {
			Vector3 pos = new Vector3 (transform.GetChild(spawnInCell).transform.position.x,transform.GetChild(spawnInCell).transform.position.y+0.5f,transform.GetChild(spawnInCell).transform.position.z);
			Instantiate(Resources.Load("Player"),pos,Quaternion.identity);
			Destroy(transform.GetComponent<SpawnPlayer>());
		}
	}
}
