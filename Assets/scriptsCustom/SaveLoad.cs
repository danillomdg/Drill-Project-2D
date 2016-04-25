using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic; 
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;


[System.Serializable]
public class SaveLoad  : MonoBehaviour{

	public GameObject terrain;
	public GameObject Player;

	//Saving UI:
	public Text inPutin;
	//loading UI:
	public Canvas rosto;
	public RectTransform myPanel;
	public GameObject SlotElement;
	List<GameObject> SlotList = new List<GameObject>();


	private PolygonGenerator Porigon;
	private GameManager ManagerGame;
	private PlayerStats StatusPlayer;
	private PlayerControl Contororu;
	private PlayerPhysics3D Physx;

//	public static List<PolygonGenerator> savedTerrains = new List<PolygonGenerator>();
//	public static List<PlayerStats> savedStats = new List<PlayerStats>();

	public static List<GameData> savedGames = new List<GameData>();


	void Start()
	{
		StatusPlayer = Player.GetComponent("PlayerStats") as PlayerStats;
		Contororu = Player.GetComponent("PlayerControl") as PlayerControl;
		Physx = Player.GetComponent("PlayerPhysics3D") as PlayerPhysics3D;
		Porigon=terrain.GetComponent("PolygonGenerator") as PolygonGenerator;


	}

	public void Save() {
		
		//REMOVE/COMMENT THIS IF YOU WANT MULTIPLE SAVEFILES
		//savedGames.Clear();
		BinaryFormatter bf0 = new BinaryFormatter();
		FileStream file0 = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
		savedGames = (List<GameData>)bf0.Deserialize(file0);
		file0.Close();



		GameData saving = new GameData();

		//Name
		int numba = savedGames.Count+1;
		if (inPutin.text == "")
			saving.Name = "Game 0"+numba;
		else
			saving.Name = inPutin.text;
		//terrain
		saving.blocks = Porigon.blocks;
		//PlayerStats

		saving.level = StatusPlayer.level;
		saving.xp  = StatusPlayer.xp ;
		saving.fuel  = StatusPlayer.fuel ;
		saving.CargoSpace = StatusPlayer.CargoSpace;
		saving.HP = StatusPlayer.HP ;
		saving.FuelTimeFactor = StatusPlayer.FuelTimeFactor;
		saving.drillPower  = StatusPlayer.drillPower ;
		saving.damageReducer  = StatusPlayer.damageReducer ;
		saving.SpeedModifier  = StatusPlayer.SpeedModifier ;
		//saving.defaultFuelController  = StatusPlayer.defaultFuelController;
		saving.FuelPU  = StatusPlayer.FuelPU ;
		saving.RepairKitPU  = StatusPlayer.RepairKitPU ;
		saving.Money  = StatusPlayer.Money ;

		saving.CurrentHull = StatusPlayer.CurrentHull  ;
		saving.CurrentFuelTank  = StatusPlayer.CurrentFuelTank ;
		saving.CurrentDrill  = StatusPlayer.CurrentDrill ;
		saving.CurrentCargo  = StatusPlayer.CurrentCargo ;
		saving.CurrentRocket  = StatusPlayer.CurrentRocket ;

		saving.CurrentEquips  = StatusPlayer.CurrentEquips ;
		saving.BuyedEquips  = StatusPlayer.BuyedEquips ;

		savedGames.Add(saving);
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/savedGames.gd");
		bf.Serialize(file, savedGames);
		file.Close();
		print("All safe and sound! games saved: "+savedGames.Count);

		inPutin.text = "";
	}

	public void OpenLoaded()
	{
		print ("Toaqui");
		
		if(File.Exists(Application.persistentDataPath + "/savedGames.gd")) {
			
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
			savedGames = (List<GameData>)bf.Deserialize(file);
			file.Close();
			
			
			print("Done! games saved: "+savedGames.Count);
			//SlotList.Clear();
			if (savedGames.Count > 0)
			{
				
				
				for (int i = 0; i<savedGames.Count;i++)
				{
					Vector3 vectoru = SlotElement.transform.position;
					GameObject TempSlot = Instantiate(SlotElement,vectoru , Quaternion.identity) as GameObject;
					Image mag = TempSlot.GetComponentInChildren<Image>();
					float slider = i * mag.rectTransform.rect.height * mag.rectTransform.localScale.y * -1;		
					SlotList.Add(TempSlot);
					SlotList[i].transform.SetParent(myPanel,false);
					
					Text textaum;
					Button battaum;
					Button butaum;
					
					
					textaum = SlotList[i].GetComponentInChildren<Text>();
					battaum = SlotList[i].GetComponentsInChildren<Button>()[0];
					//battaum.onClick.AddListener (delegate { FinallyLoad(savedPatterns,i); });
					textaum.text = savedGames[i].Name;
					
					int CurrentI = i;
					textaum = SlotList[i].GetComponentsInChildren<Button>()[1].GetComponentInChildren<Text>();
					butaum = SlotList[i].GetComponentsInChildren<Button>()[1];
					textaum.text = "Load";
					butaum.onClick.AddListener (delegate { FinallyLoad(savedGames,CurrentI); });
					
				}
			}
			else 
			{
				print ("ops");
				//SlotList.Clear();
			}			
		}	
	}
	
	public void FinallyLoad(List<GameData> Risto, int i)
	{


		GameData loading = new GameData();
		loading = Risto[i];

		//ManagerGame.LoadedName = Risto[i].Name;
		Porigon.blocks = loading.blocks;
		
		StatusPlayer.level = loading.level ;
		StatusPlayer.xp = loading.xp ;
		StatusPlayer.fuel = loading.fuel;
		StatusPlayer.CargoSpace = loading.CargoSpace ;
		StatusPlayer.HP = loading.HP;
		StatusPlayer.FuelTimeFactor = loading.FuelTimeFactor;
		StatusPlayer.drillPower = loading.drillPower ;
		StatusPlayer.damageReducer = loading.damageReducer  ;
		StatusPlayer.SpeedModifier = loading.SpeedModifier  ;
		//saving.defaultFuelController  = StatusPlayer.defaultFuelController;
		StatusPlayer.FuelPU = loading.FuelPU  ;
		StatusPlayer.RepairKitPU = loading.RepairKitPU  ;
		StatusPlayer.Money = loading.Money  ;
		
		StatusPlayer.CurrentHull  = loading.CurrentHull ;
		StatusPlayer.CurrentFuelTank = loading.CurrentFuelTank  ;
		StatusPlayer.CurrentDrill = loading.CurrentDrill  ;
		StatusPlayer.CurrentCargo = loading.CurrentCargo  ;
		StatusPlayer.CurrentRocket = loading.CurrentRocket  ;
		
		StatusPlayer.CurrentEquips = loading.CurrentEquips  ;
		StatusPlayer.BuyedEquips = loading.BuyedEquips  ;
		
		StatusPlayer.ChangePosition(new Vector3(9.5f,0.5f,0));
		Porigon.update2.x = 0;
		
		
		
		
		print("Done! games saved: "+savedGames.Count);
	}
	
	public void ClearSlotList()
	{

		int deixa = savedGames.Count;

		for (int i = savedGames.Count -1; i>=0;i--)
		{
			Destroy(SlotList[i]);
			SlotList.Remove(SlotList[i]);
		}
		
	}

	public void ClearSaved()
	{
		ClearSlotList();
		savedGames.Clear();
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/savedGames.gd");
		bf.Serialize(file, savedGames);
		file.Close();
		print("All Clear!");
	

	}

	public void Load() {
		if(File.Exists(Application.persistentDataPath + "/savedGames.gd")) {



			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
			savedGames = (List<GameData>)bf.Deserialize(file);
			file.Close();

			GameData loading = new GameData();
			loading = savedGames[savedGames.Count-1];
			Porigon.blocks = loading.blocks;

			StatusPlayer.level = loading.level ;
			StatusPlayer.xp = loading.xp ;
			StatusPlayer.fuel = loading.fuel;
			StatusPlayer.CargoSpace = loading.CargoSpace ;
			StatusPlayer.HP = loading.HP;
			StatusPlayer.FuelTimeFactor = loading.FuelTimeFactor;
			StatusPlayer.drillPower = loading.drillPower ;
			StatusPlayer.damageReducer = loading.damageReducer  ;
			StatusPlayer.SpeedModifier = loading.SpeedModifier  ;
			//saving.defaultFuelController  = StatusPlayer.defaultFuelController;
			StatusPlayer.FuelPU = loading.FuelPU  ;
			StatusPlayer.RepairKitPU = loading.RepairKitPU  ;
			StatusPlayer.Money = loading.Money  ;
			
			StatusPlayer.CurrentHull  = loading.CurrentHull ;
			StatusPlayer.CurrentFuelTank = loading.CurrentFuelTank  ;
			StatusPlayer.CurrentDrill = loading.CurrentDrill  ;
			StatusPlayer.CurrentCargo = loading.CurrentCargo  ;
			StatusPlayer.CurrentRocket = loading.CurrentRocket  ;
			
			StatusPlayer.CurrentEquips = loading.CurrentEquips  ;
			StatusPlayer.BuyedEquips = loading.BuyedEquips  ;

			StatusPlayer.ChangePosition(new Vector3(9.5f,0.5f,0));
			Porigon.update2.x = 0;



		
			print("Done! games saved: "+savedGames.Count);
		}
	}

}


