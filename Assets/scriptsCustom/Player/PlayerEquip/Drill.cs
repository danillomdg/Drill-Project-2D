using UnityEngine;
using System.Collections;

public class Drill : PlayerEquip {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public static Drill CreateInstance(int ID, string Name, float Price, float StatusBonus, string Description) 
	{
		var data = ScriptableObject.CreateInstance<Drill>();
		data.DefineEquip(ID,Name,Price,StatusBonus,Description) ;
		
		return data;
	}
}
