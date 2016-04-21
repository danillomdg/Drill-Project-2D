using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;

[System.Serializable]
public class GameEditorData {
	public static GameEditorData current;
	//PolygonGenerator
	public byte[,] blocks;
	public string Name;
	
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
