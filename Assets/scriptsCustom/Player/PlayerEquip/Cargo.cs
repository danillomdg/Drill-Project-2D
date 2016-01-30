using UnityEngine;
using System.Collections;

public class Cargo : PlayerEquip {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static Cargo CreateInstance(int ID, string Name, float Price, float StatusBonus, string Description) 
	{
		var data = ScriptableObject.CreateInstance<Cargo>();
		data.DefineEquip(ID,Name,Price,StatusBonus,Description) ;
		
		return data;
	}
}
