using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ItemHandler : MonoBehaviour {
	private PlayerStats StatusPlayer;

	public Texture2D texture;
	public Sprite[] sprites;
	public Image Icon;
	public Text quantityText;
	public GameObject bomb;
	[HideInInspector]
	public bomb bombie;
	// Use this for initialization
	void Start () {
		StatusPlayer = GetComponent<PlayerStats>();
		bombie = bomb.GetComponent("bomb") as bomb;

		sprites = Resources.LoadAll<Sprite>(texture.name);

		
	}
	
	// Update is called once per frame
	void Update () {
	
	}



	public void ItemQuantity()
	{
		if (StatusPlayer.CurrentItens.Count != 0)
		quantityText.text = StatusPlayer.CurrentItens[StatusPlayer.SelectedItem].Quantity.ToString();
	}

	public void changeIcon(int ID)
	{
		if (sprites.Length > 0)
		{
		if (ID == 1001)
		Icon.sprite = sprites[0];
		else if (ID == 1009)
		Icon.sprite = sprites[1];
		}
		else 
			print ("algo errado");
	}

	
	public void UseItem()
	{
	if (StatusPlayer.CurrentItens[StatusPlayer.SelectedItem].Quantity > 0)
		{
			if (StatusPlayer.SelectedItemID == 1001)
			{
			bool Condition = DropIt();
				if (Condition == true )
				{
					StatusPlayer.CurrentItens[StatusPlayer.SelectedItem].Quantity -= 1;
					ItemQuantity();
				}
			}
			else if (StatusPlayer.SelectedItemID == 1009)
			{
			Teleport();
				StatusPlayer.CurrentItens[StatusPlayer.SelectedItem].Quantity -= 1;
				ItemQuantity();
			}




	
		}
	}
	public bool DropIt()
	{
//		print ("Trigged");
		return bombie.DropIt();
	}

	public void Teleport()
	{
		print ("tereporuto");
		Vector3 consertarIssae = new Vector3(StatusPlayer.PlacesToGo[0].x,StatusPlayer.PlacesToGo[0].y,0);
		StatusPlayer.ChangePosition(consertarIssae);

	}
}
