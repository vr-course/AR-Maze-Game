using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
/*
 * Please remember the bigger the maze the longer it will take for the system to generate it.
 * 
 * Thank you
 * TheAssetProvider
 * 
 */

//Cell class remembers the doors and the wether it has been visited by the system or not
[System.Serializable]
public class Cell {
	public bool visited;//visited or not
	public GameObject north;//Called as 1
	public GameObject east;//Called as 2
	public GameObject west;//Called as 3
	public GameObject south;//Called as 4
	public Cell (){
		north = null;
		east = null;
		west = null;
		south = null;
	}
}

public class Maze : MonoBehaviour {

	//Selecting GenerateAtStartup will generate a maze when the scene starts
	//Selecting GenerateInEditor will let you generate the maze in the editor
	//Selecting UseInGameTrigger will let you generate a maze by calling the Generate() function

	public enum uses {GenerateAtStartup , GenerateInEditor , UseInGameTrigger};
	public uses use;
	//holds the wall prefab
	public GameObject wall;
	//Floor Type
	public enum FloorTypes {None , PlaneFloor , SaparateFloorForEachCell};
	public FloorTypes floorType;
	//holds the floor prefab
	public GameObject planeFloor;
	//cell based floor
	public GameObject cellFloor;
	//X size of the maze
	public int xSize = 10;
	//Y size of the maze
	public int ySize = 10;
	//wall length on z axis
	public float wallLength = 1.0f;
	//position to start the generation of the grid
	private Vector3 initialPos;
	//holds all the walls
	private GameObject wallHolder;
	//holds all cells
	private Cell[] cells;
	//total number of cells in the maze
	private int totalCells = 0;
	//how many cells have been visited by the script/system
	private int visitedCells = 0;
	//what cell is being processed
	private int currentCell = 0;
	//random neighbour
	private int currentNeighbour = 0;
	//holds a temp instance of the wall
	private GameObject tempWall = null;
	//how many cells have been processed
	private int cellProcess = 0;
	//started building the maze
	private bool startedBuilding = false;
	// which wall to break
	private int wallToBreak = 0;
	//last cell visited
	private List<int> lastCell;
	//backing up in pathway
	private int backingUp = 0;
	//
	private GameObject floorHolder;

	// Use this for initialization
	void Start () {
		if (use == uses.GenerateAtStartup)
			Generate ();
	}


	public void Generate (){
		if (!wall){
			Debug.LogWarning("Please assign wall variable");
			return;
		}
		if (xSize < 1 || ySize < 1) {
			Debug.LogWarning("X and Y values can't be smaller than 1");
			return;
		}
		if (floorType == FloorTypes.PlaneFloor&& !planeFloor) {
			Debug.LogWarning("Please assign PlaneFloor variable");
			return;
		}

		if (floorType == FloorTypes.SaparateFloorForEachCell && !cellFloor) {
			Debug.LogWarning("Please assign CellFloor variable");
			return;
		}
		//Setting the default values
		//currentCell = 0;
		backingUp = 0;
		visitedCells = 0;
		lastCell = new List<int>();
		lastCell.Clear ();
		wallHolder = new GameObject ();
		wallHolder.name = "Maze";
		initialPos = new Vector3 ((-xSize / 2) + wallLength/2, wallLength/2, (-ySize / 2) + wallLength);
		//this will create the grid of walls
		CreateWalls ();
	}

	void CreateWalls () {
		Vector3 myPos = initialPos;
		totalCells = xSize * ySize;
		cells = new Cell[totalCells];
		GameObject floorCell;
		if (floorType == FloorTypes.SaparateFloorForEachCell && cellFloor) {
			floorHolder = new GameObject ();
			floorHolder.name = "FloorHolder";
		}

		//For X
		//Generates walls on the X axis
		for (int j = 0; j < ySize; j++){
			for (int i = 0; i <= xSize; i++) {

				myPos = new Vector3(initialPos.x+(i*wallLength)-wallLength/2,0.5f,initialPos.z+(j*wallLength)-wallLength/2);
				tempWall = Instantiate(wall,myPos,Quaternion.identity) as GameObject;
				tempWall.transform.parent = wallHolder.transform;
				//Generates floor for each cell
				if (j < ySize && i < xSize && floorType == FloorTypes.SaparateFloorForEachCell && cellFloor){
					floorCell = Instantiate(cellFloor,new Vector3(myPos.x +0.5f,0.0f,myPos.z),Quaternion.identity) as GameObject;
					floorCell.transform.parent = floorHolder.transform;
				}
			}
		}
		//For Y
		//Generates walls on the Z axis
		for (int l = 0; l <= ySize; l++){
			for (int k = 0; k < xSize;k++) {
				myPos = new Vector3(initialPos.x+(k*wallLength),0.5f,initialPos.z+(l*wallLength)-wallLength);
				tempWall = Instantiate(wall,myPos,Quaternion.Euler(0.0f,90.0f,0.0f)) as GameObject;
				tempWall.transform.parent = wallHolder.transform;
			}
		}
		//Assignes all the walls for all the cells
		CreateCells ();
	}

	void CreateCells (){
		GameObject[] allWalls;
		int children = wallHolder.transform.childCount;
		int termCount = 0;
		int childProcess = 0;
		int eastWestProcess = 0;


		allWalls = new GameObject[children];
		//Get all the walls in an array
		for (int j = 0; j < children; ++j) {
			allWalls[j] = wallHolder.transform.GetChild (j).gameObject;
		}

		//Assigning all the walls
		for (cellProcess = 0; cellProcess < cells.Length; cellProcess++) {

			if (termCount == xSize){
				eastWestProcess ++;
				termCount = 0;
			}

			cells[cellProcess] = new Cell();
			cells[cellProcess].east = allWalls[eastWestProcess];
			cells[cellProcess].south = allWalls[childProcess+(xSize+1)*ySize];

				eastWestProcess++;

			termCount++;
			childProcess++;
			cells[cellProcess].west = allWalls[eastWestProcess];
			cells[cellProcess].north = allWalls[(childProcess+(xSize+1)*ySize)+xSize-1];

		}
		//Now lets create the maze
		CreateMaze ();
	}

	void CreateMaze (){

		while (visitedCells < totalCells) {
			if (startedBuilding){
				//gives a random neighbour
				GiveMeNieghbour();
				if (cells[currentNeighbour].visited == false && cells[currentCell].visited == true){
					//breaks the wall in betweel currentCell and neighbourCell
					BreakWall();
					cells[currentNeighbour].visited = true;
					visitedCells++;
					lastCell.Add(currentCell);
					currentCell = currentNeighbour;
					if (lastCell.Count > 0)
					backingUp = lastCell.Count-1;
				}
			}

			if (!startedBuilding){
				currentCell = Random.Range(0,cells.Length);
				startedBuilding = true;
				cells[currentCell].visited = true;
				visitedCells++;

			}
			//Invoke("CreateMaze",0.0f);
		}
		//else
		//Generate Plane floor
		if (floorType == FloorTypes.PlaneFloor && planeFloor) {
			GenerateFloor();		
		}
		//Activate Path Finder
		if (transform.GetComponent<MazePathFinder> ()) {
			transform.GetComponent<MazePathFinder> ().ActivatePathFinder(xSize,ySize,cells);
		}
			Debug.Log("Finished");
	}

	void BreakWall(){
		switch (wallToBreak){
		case 1 : DestroyImmediate(cells[currentCell].north); break;
		case 2 : DestroyImmediate(cells[currentCell].east); break;
		case 3 : DestroyImmediate(cells[currentCell].west); break;
		case 4 : DestroyImmediate(cells[currentCell].south); break;
		}
	}

	void GiveMeNieghbour (){
		int length = 0;
		int[] neighbour = new int[4];
		int[] connectingWall = new int[4];
		int check =0;
		check = ((currentCell+1) / xSize);
		check -= 1;
		check *= xSize;
		check += xSize;
		//west
		if (currentCell + 1 < totalCells && (currentCell+1) != check && xSize > 1) {
			//Debug.Log(currentCell + 1);
			if (cells[currentCell+1].visited == false){
				neighbour[length] = currentCell+1;
				connectingWall[length] = 3;
				length++;
			}
		}
		//east
		if (currentCell - 1 >= 0 && currentCell != check && xSize > 1) {
			//Debug.Log(currentCell - 1);
			if (cells[currentCell-1].visited == false){
				neighbour[length] = currentCell-1;
				connectingWall[length] = 2;
				length++;
			}
		}
		//north
		if (currentCell + xSize < totalCells && ySize > 1) {
			//Debug.Log(currentCell+xSize);
			if (cells[currentCell+xSize].visited == false){
				neighbour[length] = currentCell+xSize;
				connectingWall[length] = 1;
				length++;
			}
		}
		//south
		if (currentCell - xSize >= 0 && ySize > 1) {
			//Debug.Log(currentCell-xSize);
			if (cells[currentCell-xSize].visited == false){
				neighbour[length] = currentCell-xSize;
				connectingWall[length] = 4;
				length++;
			}
		}
		if (length != 0){
			int theChosenOne = Random.Range (0, length);
			currentNeighbour = neighbour[theChosenOne];
			wallToBreak = connectingWall[theChosenOne];
		}
		else {

			if (backingUp >= 0){
				currentCell = lastCell[backingUp];
				backingUp--;
			}

		}
	
	}
	
	void GenerateFloor() {
		//Mid point of the maze
		Vector3 midPos = new Vector3 (initialPos.x + (wallLength * xSize/2.0f)-wallLength/2.0f, 0.0f, initialPos.z + (wallLength  * ySize/2.0f)-wallLength);
		//Spawn the floor
		GameObject myFloor = Instantiate (planeFloor, midPos, Quaternion.identity) as GameObject;
		//Scale the floor
		myFloor.transform.localScale = new Vector3 (xSize * wallLength,1.0f,ySize * wallLength);

	}
}


