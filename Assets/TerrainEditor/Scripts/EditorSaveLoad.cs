using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic; 
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;


[System.Serializable]
public class EditorSaveLoad  : MonoBehaviour{
	public GameObject terrain;
	public GameObject Player;
	public GameObject LoadMenu;
	public Text inPutin;
	private PolygonGenerator Porigon;
	private EditorLoadScript RodoSucripto;
	public string fileLocation;
	
	public static List<PolygonGenerator> savedTerrains = new List<PolygonGenerator>();
	public static List<GameEditorData> savedPatterns = new List<GameEditorData>();
	
	
	void Start()
	{

		Porigon=terrain.GetComponent("PolygonGenerator") as PolygonGenerator;
		RodoSucripto = LoadMenu.GetComponent("EditorLoadScript") as EditorLoadScript;
		
		
	}









 

	public static void setDefaultFileLocation() {
		 fileLocation = Application.dataPath + "\\Data";
		Debug.Log("fileLocation : " + fileLocation);
	}
	

	
	public void Save() {
		

		BinaryFormatter bf0 = new BinaryFormatter();
		FileStream file0 = File.Open(Application.persistentDataPath + "/savedPatterns.gd", FileMode.Open);
		savedPatterns = (List<GameEditorData>)bf0.Deserialize(file0);
		file0.Close();

		GameEditorData saving = new GameEditorData();
		saving.blocks = Porigon.blocks;

		int numba = savedPatterns.Count+1;
		if (inPutin.text == "")
			saving.Name = "Pattern 0"+numba;
		else
			saving.Name = inPutin.text;


		savedPatterns.Add(saving);
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/savedPatterns.gd");
		bf.Serialize(file, savedPatterns);
		file.Close();

		//string path = "Assets/Resources/savedPatterns.byte";
		BinaryFormatter bf2 = new BinaryFormatter();
		FileStream file2 = File.Create (Application.streamingAssetsPath + "/savedPatterns.gd");
		bf2.Serialize(file2, savedPatterns);
		file2.Close();



		print("All safe and sound! Pattern Name: "+saving.Name+ " | games saved: "+savedPatterns.Count);


	}

	public void ClearInputin()
	{
		inPutin.text.Remove(0);
		print ("fui");
	}



	public void ClearSaved()
	{


		savedPatterns.Clear();

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/savedPatterns.gd");
		bf.Serialize(file, savedPatterns);
		file.Close();

		BinaryFormatter bf1 = new BinaryFormatter();
		FileStream file1 = File.Create (Application.streamingAssetsPath + "/savedPatterns.gd");
		bf1.Serialize(file1, savedPatterns);
		file1.Close();

		print("All Clear!");
		RodoSucripto.ClearSlotList();
	}

	public void PatternsSize()
	{
		//return savedPatterns.Count;
		//return 1;
		print ("oe");
	}
	
	public void Load() {
//		if(File.Exists(Application.persistentDataPath + "/savedPatterns.gd")) {	
//			BinaryFormatter bf = new BinaryFormatter();
//			FileStream file = File.Open(Application.persistentDataPath + "/savedPatterns.gd", FileMode.Open);
//			savedPatterns = (List<GameEditorData>)bf.Deserialize(file);
//			file.Close();
//			
//			GameEditorData loading = new GameEditorData();
//			loading = savedPatterns[savedPatterns.Count-1];
//			Porigon.blocks = loading.blocks;
//
//			Porigon.EditorUpdateMesh();
//		
//			print("Done! games saved: "+savedPatterns.Count);
//		}


		RodoSucripto.OpenLoaded();
	}
	
}


