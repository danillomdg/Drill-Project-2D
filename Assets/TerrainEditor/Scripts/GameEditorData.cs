﻿using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;

[System.Serializable]
public class GameEditorData {
	public static GameEditorData current;

	[XmlAttribute("blocks")]
	public byte[,] blocks;
	[XmlAttribute("Name")]
	public string Name;
	
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
