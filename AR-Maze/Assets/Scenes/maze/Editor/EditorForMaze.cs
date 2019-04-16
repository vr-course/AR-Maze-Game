using UnityEngine;
using UnityEditor;
using System.Collections;

	[CustomEditor(typeof(Maze))]
	public class EditorForMaze : Editor{
		public override void OnInspectorGUI()
		{
			Maze myMaze = (Maze)target;	
		//Use Enum
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField("Use");
		myMaze.use = (Maze.uses)EditorGUILayout.EnumPopup (myMaze.use);
		EditorGUILayout.EndHorizontal ();


		//Wall variable
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField("Wall");
		myMaze.wall = (GameObject)EditorGUILayout.ObjectField (myMaze.wall,typeof(GameObject),true);
		EditorGUILayout.EndHorizontal ();

		//Floor Enum
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField("Floor Type");
		myMaze.floorType = (Maze.FloorTypes)EditorGUILayout.EnumPopup (myMaze.floorType);
		EditorGUILayout.EndHorizontal ();


		//Plane Floor Variable
		if (myMaze.floorType == Maze.FloorTypes.PlaneFloor) {
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField ("Plane Floor");
			myMaze.planeFloor = (GameObject)EditorGUILayout.ObjectField (myMaze.planeFloor, typeof(GameObject), true);
			EditorGUILayout.EndHorizontal ();
		}

		//Cell Floor Variable
		if (myMaze.floorType == Maze.FloorTypes.SaparateFloorForEachCell) {
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField ("Cell Floor");
			myMaze.cellFloor = (GameObject)EditorGUILayout.ObjectField (myMaze.cellFloor, typeof(GameObject), true);
			EditorGUILayout.EndHorizontal ();
		}
		//XSize
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("X Size");
		myMaze.xSize = EditorGUILayout.IntField (myMaze.xSize);
		EditorGUILayout.EndHorizontal ();

		//YSize
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Y Size");
		myMaze.ySize = EditorGUILayout.IntField (myMaze.ySize);
		EditorGUILayout.EndHorizontal ();


		//Wall Length
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Wall Length");
		myMaze.wallLength = EditorGUILayout.FloatField(myMaze.wallLength);
		EditorGUILayout.EndHorizontal ();

		//Can generate in editor ?
		bool ready = false;

		if (myMaze.use == Maze.uses.GenerateInEditor && myMaze.wall) {
			if (myMaze.floorType == Maze.FloorTypes.PlaneFloor && myMaze.planeFloor){
				ready = true;
			}
			if (myMaze.floorType == Maze.FloorTypes.SaparateFloorForEachCell && myMaze.cellFloor){
				ready = true;
			}
			if (myMaze.floorType == Maze.FloorTypes.None){
				ready = true;
			}
		}
		//Drawing the generate button
		if (ready)
			if (GUILayout.Button ("Generate")) {
				myMaze.Generate();
				DestroyImmediate(myMaze);
				}

		if (!ready && myMaze.use == Maze.uses.GenerateInEditor){
			EditorGUILayout.HelpBox("Please assign all the variables to continue",MessageType.Warning);
			}	
		}

}
