using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class bomb : MonoBehaviour {

	//COLISION VARIABLES

	public LayerMask collisionMask;
	private new BoxCollider collider;
	private Vector3 s;
	private Vector3 c;
	private float skin = .005f;
	public int ColmaskRedux =5;

	public GameObject gameManager;
	public GameObject Terrain;
	public GameObject Player;

	private PolygonGenerator tScript;
	private GameManager ManagerGame;
	public GameObject Baku;

	//[HideInInspector]
	private float preTimar = 0.6f; // default: 0.8f
	private float timar;
	public bool dropped = false;

	private Vector2 blowsize = new Vector2(3,3);

	public int quantidade;
	public List<Vector2> UnityPosition = new List<Vector2>();
	// Use this for initialization
	void Start () {
		tScript = Terrain.GetComponent("PolygonGenerator") as PolygonGenerator; 
		ManagerGame = gameManager.GetComponent("GameManager") as GameManager;

		collider = GetComponent<BoxCollider>();
		s = collider.size;
		c = collider.center;

		timar = preTimar;
	}
	
	// Update is called once per frame
	void Update () {
		if (dropped == true)
		{

		timar -= Time.deltaTime;
		if (timar <= 0)
			Bakuhatsu ();
		}
	}

	public void Bakuhatsu()
	{

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
		//StartCoroutine(ManagerGame.CameraShake());
		gameObject.SetActive(false);
		Baku.SetActive(true);

	}

	public bool DropIt()
	{
		
		if (dropped == false)
		{
			if (-1 * Player.transform.position.y > 2)
			{
				Vector3 semiposition = Player.transform.position;
					semiposition.z = -0.6f;
					transform.position = semiposition;
				gameObject.SetActive(true);
				dropped = true;
				return true;
			}
			return false;

		}
		return false;
	}
}
