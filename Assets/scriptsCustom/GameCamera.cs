using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {

	public GameObject terrain;
	private Transform target;
	private float trackSpeed = 10;
	private Camera cam;
	private float size;
	private Vector2 TerrainSize;

	void Start () 
	{
		cam = gameObject.GetComponent("Camera") as Camera;
		size = cam.orthographicSize * (cam.pixelWidth/cam.pixelHeight);
		terrain.SendMessage("SizeRequested",gameObject);

	}
	
	// Set target
	public void SetTarget(Transform t) {
		target = t;
	}
	
	// Track target
	void LateUpdate() {
		if (target) {
			Vector3 storePos = transform.position;


			float x = IncrementTowards(transform.position.x, target.position.x, trackSpeed);
			float y = IncrementTowards(transform.position.y, target.position.y, trackSpeed);

			if (x - size -1 <=0 || x + size >= TerrainSize.x)
				transform.position = new Vector3(storePos.x ,y, transform.position.z);
			else
			transform.position = new Vector3(x,y, transform.position.z);

		}
	}
	
	// Increase n towards target by speed
	private float IncrementTowards(float n, float target, float a) {
		if (n == target) {
			return n;	
		}
		else {
			float dir = Mathf.Sign(target - n); // must n be increased or decreased to get closer to target
			n += a * Time.deltaTime * dir;
			return (dir == Mathf.Sign(target-n))? n: target; // if n has now passed target then return target, otherwise return n
		}
	}
	public void GetSize(Vector2 saizo)
	{
		TerrainSize = saizo;
		print("saizoooo  "+TerrainSize.x);
	}
}
