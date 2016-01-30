using UnityEngine;
using System.Collections;

public class Hull : PlayerEquip {

	// Use this for initialization
	void Start () {
		ID = 0;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static Hull CreateInstance(int ID, string Name, float Price, float StatusBonus, string Description) 
	{
		var data = ScriptableObject.CreateInstance<Hull>();
		data.DefineEquip(ID,Name,Price,StatusBonus,Description) ;
		
		return data;
	}
}
