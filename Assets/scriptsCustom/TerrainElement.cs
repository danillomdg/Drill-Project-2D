using UnityEngine;
using System.Collections;

public class TerrainElement : ScriptableObject {

	public int ID;
	public string name;
	public float rareness; 
	public float value;
	public int quantity;

	// Update is called once per frame


	// Use this for initialization
	public void Init (int ID,string name, float rareness,float value, int quantity ) 
	{
		this.ID = ID;
		this.name = name;
		this.rareness = rareness;
		this.value = value;
		this.quantity = quantity;

	}
	


	public static TerrainElement CreateInstance(int ID,string name, float rareness,float value, int quantity ) 
	{
		var data = ScriptableObject.CreateInstance<TerrainElement>();
		data.Init(ID,name,rareness,value,quantity) ;

		return data;
	}
}
