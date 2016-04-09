using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;

[System.Serializable]
public class GameData {
	public static GameData current;
	//PolygonGenerator
	public byte[,] blocks;

	//PlayerStats
	[HideInInspector]
	public int level = 1;
	[HideInInspector]
	public float xp = 0;
	public float fuel = 100; 
	public float CargoSpace = 100;
	public float HP = 100;
	public float FuelTimeFactor;
	public float drillPower = 1;
	public float damageReducer = 0;
	public float SpeedModifier = 1;
	private float defaultFuelController = 1.8f;
	[HideInInspector]
	public float FuelPU = 20;
	[HideInInspector]
	public float RepairKitPU = 20;
	public int score;
	public float Money = 50;

	public Hull CurrentHull; 
	public FuelTank CurrentFuelTank;
	public Drill CurrentDrill; 
	public Cargo CurrentCargo; 
	public Rocket CurrentRocket;
	
	public List<PlayerEquip> CurrentEquips, BuyedEquips;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
