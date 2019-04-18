using UnityEngine;
using System.Collections;

//This is just a demo script to show how this system works

public class Demo : MonoBehaviour {
	public UnityEngine.UI.Slider xSize;
	public UnityEngine.UI.Slider ySize;
	public UnityEngine.UI.Slider cam; 
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (cam)
		transform.position = new Vector3 (transform.position.x, cam.value, transform.position.z);
	}

	void Create(){
		Maze myScript = transform.GetComponent <Maze>();
		if (xSize && ySize && myScript){
			myScript.xSize = (int)xSize.value;
			myScript.ySize = (int)ySize.value;
			myScript.Generate ();
		}
	}

	void Reset (){
		Application.LoadLevel (Application.loadedLevel);
	}
}
