using UnityEngine;
using System.Collections;

[System.Serializable]
public class Rocket : PlayerEquip {

	public Rocket(int ID, string Name, float Price, float StatusBonus, string Description) : base(ID,Name,Price,StatusBonus, Description)
	{
		this.ID = ID;
	
		this.Name = Name;
		this.Price = Price;
		this.StatusBonus = StatusBonus;
		this.Description = Description;
	}


}
