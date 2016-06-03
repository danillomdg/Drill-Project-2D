using UnityEngine;
using System.Collections;

public class Landmine : Enemy {
	public GameObject Player;
	public GameObject Terrain;

	public bool safe;
	public bool muitolonje;
	public bool sendoUsada;
	private int baseHP = 10;
	
	private PlayerStats StatusPlayer;
	public float pretimer = 0.7f;
	public float timer;
	float blowRadius = 1.1f;
	float damagePowerMulti = 52; 

	private float preTimar = 0.5f; // default: 0.8f
	private float timar;
	public bool dropped = false;
	
	public GameObject Baku;
	

	private PolygonGenerator tScript;
	private BoxCollider collider;
	private Vector2 blowsize = new Vector2(3,3);
	private Color color;
	private Renderer rend;

	
	// Use this for initialization

	void Start () {
		rend = GetComponent<Renderer>();
		color = rend.material.color;
//		color.a = 0;
//		rend.material.color = color;
//		gameObject.renderer.enabled = false;

		collider = gameObject.GetComponent<BoxCollider>();
		tScript = Terrain.GetComponent("PolygonGenerator") as PolygonGenerator; 

	
	}
	
	// Update is called once per frame
	void Update () {

		// MELHORAR OS CALCULOS DE DISTANCIA]
		var distance = Vector3.Distance(Player.transform.position, transform.position);
		
		if (distance >= 4)
		{
			if (sendoUsada == false)
			HideMine();
		}
		else 
		{
			muitolonje = false;
			if (distance <0.5f)
				Bakuhatsu();
		}

		activation();
		
	}

	public void HideMine()
	{
		
		//transform.position = new Vector3(6.51f,-0.9f,-0.1f);
		transform.position = new Vector3(-99f,0,-0.1f);
		gameObject.SetActive(false);
		muitolonje = true;

	}

	void activation()
	{
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, blowRadius);
		foreach (Collider corida in hitColliders)
		{


			if (corida.gameObject.name != ("LandMine(Clone)") && corida.gameObject.name != ("Terrain") && corida.gameObject.name != ("Rock"))
			{
//				print ("CORAIDINGU: "+corida.gameObject.name);
				color.a = 255;
				rend.material.color = color;
				sendoUsada = true;
				StartCoroutine(ItsATrap());

				}
		
		}
	}

	public void Bakuhatsu()
	{
//		color.a = 0;
//		rend.material.color = color;
//		gameObject.renderer.enabled = false;

		for (int i=0; i<blowsize.y; i++)
		{
			for (int j=0; j<blowsize.x; j++)
			{
				int crazyX = -1*Mathf.FloorToInt(blowsize.x/2)+j;
				int crazyY = -1* Mathf.FloorToInt(blowsize.y/2)+i; 
				if (tScript.blocks[Mathf.FloorToInt(transform.position.x)+crazyX,-1*(Mathf.FloorToInt(transform.position.y)-crazyY) -1] != 21)
					tScript.blocks[Mathf.FloorToInt(transform.position.x)+crazyX,-1*(Mathf.FloorToInt(transform.position.y)-crazyY) -1] = 0;
				tScript.update2 = new Vector2(Mathf.RoundToInt(Player.transform.position.x),Mathf.RoundToInt(Player.transform.position.y-tScript.transform.position.y));
				
				
			}
		}
		//tScript.blocks[Mathf.RoundToInt(transform.position.x),-1*Mathf.RoundToInt(transform.position.y)] = 0;
		dropped = false;
		timar = preTimar;
		Baku.transform.position = transform.position;
		//gameObject.SetActive(false);
		Baku.SetActive(true);
	}

	public IEnumerator ItsATrap()
	{

		//gameObject.SetActive(true);
		yield return new WaitForSeconds(preTimar);
		Bakuhatsu();
	}

}
