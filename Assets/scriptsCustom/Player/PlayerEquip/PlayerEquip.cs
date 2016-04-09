using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlayerEquip {
	public int ID = 0;
	//public int type; 
	public float Price, StatusBonus;
	public string Name, Description;
	// Use this for initialization

	
	public PlayerEquip(int ID, string Name, float Price, float StatusBonus, string Description)
	{
		this.ID = ID;

		this.Name = Name;
		this.Price = Price;
		this.StatusBonus = StatusBonus;
		this.Description = Description;
	}

	public void DefineEquip(int ID, string Name, float Price, float StatusBonus, string Description)
	{
		this.ID = ID;
		this.Name = Name;
		this.Price = Price;
		this.StatusBonus = StatusBonus;
		this.Description = Description;
	}

	public void DefineTotalEquip(int ID, string Name, float Price, float StatusBonus, string Description)
	{
		this.ID = ID;

		this.Name = Name;
		this.Price = Price;
		this.StatusBonus = StatusBonus;
		this.Description = Description;
	}

	



}
