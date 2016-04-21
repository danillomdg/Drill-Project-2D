using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Teleport : PlayerItem {
	private List<Vector2> Locations;

	public Teleport (int ID, int type, string Name, float Price, float Quantity, string Description) : base (ID,type,Name,Price,Quantity,Description)
	{
		this.ID = ID;
		this.type = type;
		this.Name = Name;
		this.Price = Price;
		this.Quantity = Quantity;
		this.Description = Description;
	}
}
