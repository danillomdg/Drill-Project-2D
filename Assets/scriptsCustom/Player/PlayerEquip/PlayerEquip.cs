using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlayerEquip : ScriptableObject {
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

	public void DefineEquip(int ID, string Name, float Price, float StatusBonus, string Description)
	{
		this.ID = ID;
		this.Name = Name;
		this.Price = Price;
		this.StatusBonus = StatusBonus;
		this.Description = Description;
	}

	public void DefineTotalEquip(int ID, int type, string Name, float Price, float StatusBonus, string Description)
	{
		this.ID = ID;
		this.type = type;
		this.Name = Name;
		this.Price = Price;
		this.StatusBonus = StatusBonus;
		this.Description = Description;
	}

	

	public static PlayerEquip CreateInstance(int ID, string Name, float Price, float StatusBonus, string Description) 
	{
		var data = ScriptableObject.CreateInstance<PlayerEquip>();
		data.DefineEquip(ID,Name,Price,StatusBonus,Description) ;
		
		return data;
	}

}
