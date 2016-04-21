using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlayerItem {
	public int ID = 0;
	public int type; 
	public float Price, Quantity;
	public string Name, Description;
	// Use this for initialization
	public PlayerItem (int ID, int type, string Name, float Price, float Quantity, string Description)
	{
		this.ID = ID;
		this.type = type;
		this.Name = Name;
		this.Price = Price;
		this.Quantity = Quantity;
		this.Description = Description;
	}
	
	public void DefineItem(int ID, string Name, float Price, float Quantity, string Description)
	{
		this.ID = ID;
		this.Name = Name;
		this.Price = Price;
		this.Quantity = Quantity;
		this.Description = Description;
	}
	

	
	

	
}
