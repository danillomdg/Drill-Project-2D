using UnityEngine;
using System.Collections;

[System.Serializable]
public class FuelTank : PlayerEquip {
	
	// Use this for initialization
	void Start () {
		ID = 0;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public static FuelTank CreateInstance(int ID, string Name, float Price, float StatusBonus, string Description) 
	{
		var data = ScriptableObject.CreateInstance<FuelTank>();
		data.DefineEquip(ID,Name,Price,StatusBonus,Description) ;
		
		return data;
	}
}