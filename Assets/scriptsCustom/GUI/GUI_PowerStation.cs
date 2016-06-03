using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUI_PowerStation : MonoBehaviour {
	
	public GameObject player;
	public GameObject gameManager; 
	private GameManager ManagerGame;

	private PlayerStats StatusPlayer; 
	private RefineryMenuControl MenuMega;
	public Image ConfBox;
	public Image MegaMenu;
	public int teste = 0;
	private Button bataum;
	private Color cor;
	
	private new BoxCollider collider;
	
	// Use this for initialization
	Text Texto;

	void Start () 
		
	{

		ConfBox.gameObject.SetActive(false);
		StatusPlayer = player.GetComponent("PlayerStats") as PlayerStats;
		bataum = ConfBox.GetComponent ("Button") as Button;
		MenuMega = MegaMenu.GetComponent ("MegaMenuControl") as RefineryMenuControl;
		ManagerGame = gameManager.GetComponent("GameManager") as GameManager;
		//	bataum.onClick.AddListener(() => { });
		cor = bataum.image.color;
		
	}
	
	
	
	void OnTriggerEnter (Collider col)
	{
		
		
		if (col.gameObject.name == "DrillFighter")
		{


			ConfBox.gameObject.SetActive(true);
			Texto = ConfBox.gameObject.GetComponentInChildren<Text>();
			if ((StatusPlayer.Money - 50) >= 0){
				ManagerGame.menuSwitcherValue = 2;
				Texto.text = "RECHARGE POWER";
			}
			else {Texto.text = "NO CASH";
				bataum.image.color = Color.red;
			}


		} 
		
	}
	
	void OnTriggerExit (Collider col)
	{
		
		if (col.gameObject.name == "DrillFighter")
		{
			ManagerGame.menuSwitcherValue = 0;
			ConfBox.gameObject.SetActive(false);
			bataum.image.color = cor;
			
		} 
		
	}
	
	
	
	
}


