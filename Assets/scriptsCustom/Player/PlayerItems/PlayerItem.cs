using UnityEngine;
using System.Collections;

public class PlayerItem : ScriptableObject {
	public int ID = 0;
	public int type; 
	public float Price, StatusBonus;
	public string Name, Description;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void DefineItem(int ID, string Name, float Price, float StatusBonus, string Description)
	{
		this.ID = ID;
		this.Name = Name;
		this.Price = Price;
		this.StatusBonus = StatusBonus;
		this.Description = Description;
	}
	
	public void DefineTotalItem(int ID, int type, string Name, float Price, float StatusBonus, string Description)
	{
		this.ID = ID;
		this.type = type;
		this.Name = Name;
		this.Price = Price;
		this.StatusBonus = StatusBonus;
		this.Description = Description;
	}
	
	
	
	public static PlayerItem CreateInstance(int ID, string Name, float Price, float StatusBonus, string Description) 
	{
		var data = ScriptableObject.CreateInstance<PlayerItem>();
		data.DefineItem(ID,Name,Price,StatusBonus,Description) ;
		
		return data;
	}
	
}
