using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazePathFinder : MonoBehaviour {
	//Same as maze parameters
	private int xSize = 0;
	private int ySize = 0;
	//Found path
	private List<int> path;
	//System activated?
	private bool canFind = false; 
	//Completed maze
	private Cell[] myMaze;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}
	//You have to activate the path finder atleast once with correct values
	//Sets the parameters and activates the path finder
	public void ActivatePathFinder (int passedSizeX , int passedSizeY,Cell[] passedMaze){
		xSize = passedSizeX;
		ySize = passedSizeY;
		myMaze = passedMaze;
		canFind = true;
		Debug.Log("Maze Path Finder is Ready to use");
	}
	
	//Finds the path and returns as a List<int>
	public List<int> FindPath (int passedStart , int passedEnd){
		if (!canFind) {
			Debug.LogWarning("Path finder is not ready");
			return null;
		}


		if (passedStart < 0 || passedStart >= (xSize * ySize)) {
			Debug.LogWarning("Passed Start cell does not exist");
			return null;
		}

		if (passedEnd < 0 || passedEnd >= (xSize * ySize)) {
			Debug.LogWarning("Passed End cell does not exist");
			return null;
		}

		foreach (Cell currentMaze in myMaze) {
			currentMaze.visited = false;
		}

		path = new List<int> ();
		path.Clear ();

		int currentPoint = passedStart;
		myMaze [currentPoint].visited = true;
		path.Add (currentPoint);

		while (path.Count < (xSize*ySize)){
			currentPoint = path[path.Count-1];
			if (currentPoint == passedEnd){
				break;
			}
			GiveMeNieghbour(currentPoint);
		}

		Debug.Log("Path Found");

		return path;
	}

	//Gives a unvisited and accesible neighbour 
	void GiveMeNieghbour (int currentCell){
		int length = 0;
		int[] neighbour = new int[4];
		int check =0;
		int totalCells = xSize * ySize;
		check = ((currentCell+1) / xSize);
		check -= 1;
		check *= xSize;
		check += xSize;

		//west
		if (!myMaze[currentCell].west && currentCell + 1 < totalCells && (currentCell+1) != check && xSize > 1) {
			//Debug.Log(currentCell + 1);
			if (myMaze[currentCell+1].visited == false){
				neighbour[length] = currentCell+1;
				length++;
			}
		}
		//east
		if (!myMaze[currentCell].east && currentCell - 1 >= 0 && currentCell != check && xSize > 1) {
			//Debug.Log(currentCell - 1);
			if (myMaze[currentCell-1].visited == false){
				neighbour[length] = currentCell-1;
				length++;
			}
		}
		//north
		if (!myMaze[currentCell].north && currentCell + xSize < totalCells && ySize > 1) {
			//Debug.Log(currentCell+xSize);
			if (myMaze[currentCell+xSize].visited == false){
				neighbour[length] = currentCell+xSize;
				length++;
			}
		}
		//south
		if (!myMaze[currentCell].south && currentCell - xSize >= 0 && ySize > 1) {
			//Debug.Log(currentCell-xSize);
			if (myMaze[currentCell-xSize].visited == false){
				neighbour[length] = currentCell-xSize;
				length++;
			}
		}
		if (length != 0){
			int theChosenOne = Random.Range (0, length);
			myMaze[neighbour[theChosenOne]].visited = true;
			path.Add(neighbour[theChosenOne]);
		}
		else {
			if (path.Count > 0){
				path.RemoveAt(path.Count-1);
			}
			
		}
		
	}


}
