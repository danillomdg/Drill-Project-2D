using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class GUI_Refinaria : MonoBehaviour {
	
	public GameObject player; 
	public GameObject gameManager; 
	private GameManager ManagerGame;
	private RefineryMenuControl MenuMega;
	public Image ConfBox;
	public Image DetailBox;
	public Image MegaMenu;
	public int teste = 0;
	private Button bataum;

	private new BoxCollider collider;

	// Use this for initialization
	Text Texto;

	void Start () 
		
	{

		ConfBox.gameObject.SetActive(false);
		DetailBox.gameObject.SetActive(false);
		bataum = ConfBox.GetComponent ("Button") as Button;
		MenuMega = MegaMenu.GetComponent ("MegaMenuControl") as RefineryMenuControl;
		ManagerGame = gameManager.GetComponent("GameManager") as GameManager;
	//	bataum.onClick.AddListener(() => { });

	}


	void OnTriggerEnter (Collider col)
	{
	

		if (col.gameObject.name == "DrillFighter")
		{

			ManagerGame.menuSwitcherValue = 1;
			ConfBox.gameObject.SetActive(true);
			DetailBox.gameObject.SetActive(true);
			Texto = ConfBox.GetComponentInChildren<Text>();
			Texto.text = "SELL EVERYTHING";
		} 
		
	}
	
	void OnTriggerExit (Collider col)
	{

		if (col.gameObject.name == "DrillFighter")
		{
			ManagerGame.menuSwitcherValue = 0;
			ConfBox.gameObject.SetActive(false);
			DetailBox.gameObject.SetActive(false);
			
		} 
		
	}





	
}




