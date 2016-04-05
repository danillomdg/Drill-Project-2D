using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;



public static class SaveLoad  : MonoBehaviour{

	public static List<PolygonGenerator> savedTerrains = new List<PolygonGenerator>();
	public static List<GameManager> savedGames = new List<GameManager>();
	public static List<PlayerStats> savedStats = new List<PlayerStats>();




	public static void Save() {
		savedGames.Add(Game.current);
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/savedGames.gd");
		bf.Serialize(file, SaveLoad.savedGames);
		file.Close();
	}

}


