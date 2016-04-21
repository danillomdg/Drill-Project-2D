using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
[System.Serializable]
public class GameEditorManager : MonoBehaviour {

	public string GameName;

	private Vector3 position;

	public GameCamera cam;
	public GameObject terrain;
	private RefineryMenuControl Megalomaniaco;	
	private WorkshopMenuControl WorkshopScript;
	private PolygonGenerator tscripto;
	public Vector2 coordsToUse;
	private int CurrentBValue = 1;
	private int maxBvalue = 14;
	public Image Icon;
	public Texture2D texture;
	public Sprite[] sprites;

	//Not so important
	private bool gameOver;
	private bool restart;
	private int score;
	public Image newsbox;
	public Text newstext;
	public int goldPrice = 400;
	public int silverPrice = 250;
	public int bronzePrice = 150;
	public int ironPrice = 200;
	public int DiamondPrice = 600;
	public float RepairPriceModifier = 1.2f;
	public int menuSwitcherValue;
	float oi = 0;
	public int[] SpawnLevel;
	
	void Start () {
		tscripto = terrain.GetComponent("PolygonGenerator") as PolygonGenerator;
		sprites = Resources.LoadAll<Sprite>(texture.name);
		changeIcon(14);
	}

	void Update () 
	{    
		actions();
	}
	void actions()
	{
		if (Input.GetKeyDown("x"))
			ToggleCurrentBValue(1);
		else if (Input.GetKeyDown("z"))
			ToggleCurrentBValue(-1);
	}


	public void changeBlockValue(Vector2 coords)
	{
		int x = Mathf.FloorToInt(coords.x);
		int y = Mathf.FloorToInt(coords.y);

		switch (CurrentBValue)
		{
		case 0:
			tscripto.blocks[x,y] = 1;
			break;
		case 1:
			tscripto.blocks[x,y] = 0;
			break;
		case 2:
			tscripto.blocks[x,y] = 2;
			break;
		case 3:
			tscripto.blocks[x,y] = 3;
			break;
		case 4:
			tscripto.blocks[x,y] = 4;
			break;
		case 5:
			tscripto.blocks[x,y] = 5;
			break;
		case 6:
			tscripto.blocks[x,y] = 6;
			break;
		case 7:
			tscripto.blocks[x,y] = 7;
			break;
		case 8:
			tscripto.blocks[x,y] = 20;
			break;
		case 9:
			tscripto.blocks[x,y] = 21;
			break;
		case 10:
			tscripto.blocks[x,y] = 22;
			break;
		case 11:
			tscripto.blocks[x,y] = 51;
			break;
		case 12:
			tscripto.blocks[x,y] = 52;
			break;
		case 13:
			print ("to be implemented");
			break;
			
		}
	
		tscripto.EditorUpdateMesh();
	}

	public void changeIcon(int ID)
	{
		Icon.sprite = sprites[ID];
	}


	public void ToggleCurrentBValue(int LR)
	
	{
		if (LR == 1)
		{
			CurrentBValue +=1;
			if (CurrentBValue >= maxBvalue)
				CurrentBValue = 0;
			print ("Currento: "+CurrentBValue);
		}
		else if (LR == -1)
		{
			CurrentBValue -=1;
			if (CurrentBValue < 0)
				CurrentBValue = maxBvalue-1;
		}
		switch (CurrentBValue)
		{
		case 0:
			changeIcon(3);
			break;
		case 1:
			changeIcon(14);
			break;
		case 2:
			changeIcon(11);
			break;
		case 3:
			changeIcon(0);
			break;
		case 4:
			changeIcon(1);
			break;
		case 5:
			changeIcon(2);
			break;
		case 6:
			changeIcon(5);
			break;
		case 7:
			changeIcon(6);
			break;
		case 8:
			changeIcon(4);
			break;
		case 9:
			changeIcon(7);
			break;
		case 10:
			changeIcon(8);
			break;
		case 11:
			changeIcon(9);
			break;
		case 12:
			changeIcon(10);
			break;
		case 13:
			changeIcon(12);
			break;
			
		}

	}
	
	

	public void EndGame(){
		newsbox.gameObject.SetActive(true);
		newstext.text = ("Gameover! Click to Restart!");
		menuSwitcherValue = 10;
		if (Input.GetKeyDown (KeyCode.R))
		{
     	Restarto();
		}	
	}

	
	public void controlTime(){
		oi += 2 * Time.deltaTime;
		if (Mathf.FloorToInt (oi) == 1) {
			oi = 0;
			print (Mathf.FloorToInt (oi));
		}
	}


	
	public void Restarto(){
		Application.LoadLevel (Application.loadedLevel);
	}
	

	



	
}
