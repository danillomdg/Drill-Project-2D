using UnityEngine;
using System.Collections;


[RequireComponent (typeof(BoxCollider))]
public class PlayerPhysics3D : MonoBehaviour {
	
	public LayerMask collisionMask;
	//public GameObject Player;

	private PlayerControl movimento;

	private new BoxCollider collider;
	private Vector3 s;
	private Vector3 c;

	private float skin = .02f;
	public int ColmaskRedux =5;
	[HideInInspector]
	public bool grounded;
	[HideInInspector]
	public bool movementStoppedX;
	public bool movementStoppedY;



	Ray ray;
	RaycastHit hit;
	
	void Start() {
		collider = GetComponent<BoxCollider>();
		s = collider.size;
		c = collider.center;
		movimento = GetComponent<PlayerControl>();
	}
	
	public void Move(Vector2 moveAmount) {
		
		float deltaY = moveAmount.y;
		float deltaX = moveAmount.x;
		Vector2 p = transform.position;
		
		// Check collisions above and below
		grounded = false;


		for (int i = 0; i<3; i ++) {
			float dir = Mathf.Sign(deltaY);
			float x = (p.x + c.x - s.x/ColmaskRedux) + s.x/ColmaskRedux * i; // Left, centre and then rightmost point of collider
			float y = p.y + c.y + s.y/ColmaskRedux * dir; // Bottom of collider
			
			ray = new Ray(new Vector2(x,y), new Vector2(0,dir));
			Debug.DrawRay(ray.origin,ray.direction);
			
			if (Physics.Raycast(ray,out hit,Mathf.Abs(deltaY) + skin,collisionMask) 

			    //&& movimento.digdown == false

			    ) {
				// Get Distance between player and ground
				float dst = Vector3.Distance (ray.origin, hit.point);
				
				// Stop player's downwards movement after coming within skin width of a collider
				if (dst > skin) {
					deltaY = dst * dir - skin * dir;
				}
				else {
					deltaY = 0;
				}
				if (dir != 1)
				grounded = true;
				
				break;
				
			}
		}

				
		// Check collisions left and right
		movementStoppedX = false;
		for (int i = 0; i<3; i ++) {
			float dir = Mathf.Sign(deltaX);
			float x = p.x + c.x + s.x/ColmaskRedux * dir;
			float y = p.y + c.y - s.y/ColmaskRedux + s.y/ColmaskRedux * i+0.03f;
			
			ray = new Ray(new Vector2(x,y), new Vector2(dir,0));
			Debug.DrawRay(ray.origin,ray.direction);
			
			if (Physics.Raycast(ray,out hit,Mathf.Abs(deltaX) + skin,collisionMask) 
			    //&& movimento.digLR == false  
			    ) {
				// Get Distance between player and block
				float dst = Vector3.Distance (ray.origin, hit.point);
				
				// Stop player's downwards movement after coming within skin width of a collider
				if (dst > skin) {
					deltaX = dst * dir - skin * dir;
				}
				else {
					deltaX = 0;
				}
				
				movementStoppedX = true;
				break;
				
			}
		}

		// cria um raycast giratorio mutante
		if (!grounded && !movementStoppedX) {
			Vector3 playerDir = new Vector3(deltaX,deltaY);
			Vector3 o = new Vector3(p.x + c.x + s.x/ColmaskRedux * Mathf.Sign(deltaX),p.y + c.y + s.y/ColmaskRedux * Mathf.Sign(deltaY));
			ray = new Ray(o,playerDir.normalized);
			
			if (Physics.Raycast(ray,Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY),collisionMask) && movimento.digdown == false ) {
				grounded = true;
				deltaY = 0;
			}
		}


		Vector2 finalTransform = new Vector2(deltaX,deltaY);		
		transform.Translate(finalTransform,Space.World);
	}
	
}
