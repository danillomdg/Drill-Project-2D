using UnityEngine;
using System.Collections;

public class CameraEditorControl : MonoBehaviour {
	public GameObject collider;
	public bool StopMessingAround = false;
	[HideInInspector]
	private float Attunement = 0.35f;
	private int profound = 0;
	Vector3 directionVector,directionInVector;
	// Use this for initialization
	void Start () {
		collider.transform.position = new Vector3(transform.position.x,transform.position.y,0);

	}
	
	// Update is called once per frame
	void Update () {
		if (StopMessingAround == false)
		actions();


	
	}

	public void StopMessing()
	{
		StopMessingAround = true;
	}
	public void StartMessing()
	{
		StopMessingAround = false;
	}

	void actions()
	{
		if (Input.GetKey("e"))
			profound = 1;
		else if (Input.GetKey("q"))
			profound = -1;
		else profound = 0;


		directionVector = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), profound);
		directionInVector = new Vector3(Input.GetAxis("Horizontal"), -1 * 0, Input.GetAxis("Vertical"));
		transform.Translate(directionVector * Attunement);
		collider.transform.Translate(directionInVector * Attunement);
	}
}
