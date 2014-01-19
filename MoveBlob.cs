using UnityEngine;
using System.Collections;

public class MoveBlob : MonoBehaviour 
{			
	//public declarations
	static public	Vector3 startPos;
	static public	Vector3 destination;
	static public	Vector3 collisionPos;
	static public	bool jump = false;
	
	OTAnimatingSprite blobman;
	
	public 	Vector3 moveDest;
	public	float startTime;
	public	float jumpTime;
	public	float beginPos;
	public	float endPos;
	public	float duration;
	public	float absX;
	public	float absY;
	public	float distSqr;
	
	public GameObject text;
	
	public 	enum move
	{
		MoveRight,
		MoveLeft,
		MoveUp,
		MoveDown,
		Jump
	}
	
	public move direction;
	public move prevDir = move.MoveLeft;
	
	
	// Use this for initialization
	void Start () 
	{
		//Get Reference to animating sprite
		blobman = OT.ObjectByName ("Blobman") as OTAnimatingSprite;
		
		//set the public variables
		startPos = destination = transform.position;
		startTime = Time.time;
		duration = 1.0f;
	}

	
	//Move functions, moving a square in the respective direction
	
	void Move (Vector3 _destination)
	{
		//update startPos with current position
		startPos = transform.position;
		
		destination = _destination;
		startTime = Time.time;
	}
	
	void Jump ()
	{
		ChooseDirection (prevDir);
		direction = prevDir;
	}
	
	void ChooseDirection (move _direction)
	{
		switch (_direction)
		{							
			case move.MoveRight:
				if (jump == true)
					blobman.PlayOnce("jumpR");
				else
					blobman.PlayOnceBackward("moveRight");
				moveDest.x += 2;
				break;
			case move.MoveLeft:
				blobman.PlayOnceBackward("moveLeft");
				moveDest.x += -2;
				break;
			case move.MoveDown:
				blobman.PlayOnceBackward("moveDown");
				moveDest.y += -2;
				break;
			case move.MoveUp:
				blobman.PlayOnceBackward("moveUp");
				moveDest.y += 2;
				break;
			case move.Jump:
				Jump();
				jump = true;
				break;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		//make sure that the startPos contains the blobs current position
		startPos = transform.position;
		
		moveDest = transform.position;
			
		if (Time.time - jumpTime < 0.38f) return;
		
		//move the position of the blob between startPos and destination using lerp
		transform.position = Vector3.Lerp (startPos, destination, Mathf.Clamp ((Time.time-startTime)/duration, 0, 1));
		
		//if the blob has reached the destination find out swipe direction and begin next move function
		if (destination != startPos) return;
		
		foreach (Touch touch in Input.touches)
		{
			absX = Mathf.Abs (touch.deltaPosition.x);
			absY = Mathf.Abs (touch.deltaPosition.y);				
			
			if (touch.phase == TouchPhase.Began)
				beginPos = (touch.position.x*touch.position.x) + (touch.position.y*touch.position.y);
						
			//obtain and store the direction of the swipe
		
			if (absX > absY)
			{
				if (touch.deltaPosition.x > touch.deltaPosition.y)
					direction = move.MoveRight;
				else if (touch.deltaPosition.x < touch.deltaPosition.y)
					direction = move.MoveLeft;
			}
			else if (absX < absY)
			{
				if (touch.deltaPosition.x > touch.deltaPosition.y)
					direction = move.MoveDown;
				else if (touch.deltaPosition.x < touch.deltaPosition.y)
					direction = move.MoveUp;
			}		
			
			jump = false;

			//if statement to cause blob to move after finishing the swipe
			if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
			{	
				//calculate the endPos value, find the difference and make it positive value
				endPos = (touch.position.x*touch.position.x) + (touch.position.y*touch.position.y);
				distSqr = beginPos - endPos;
				distSqr = Mathf.Abs (distSqr);
				
				text.guiText.text = distSqr.ToString ();
				
				//distSqr = distance the finger has moved
				if (distSqr < 10000)
				{
					direction = move.Jump;
					jump = true;
					jumpTime = Time.time;
					duration = 2.0f;
				}
				
				ChooseDirection (direction);
				
				Move (moveDest);
				
				prevDir = direction;
				
				collisionPos = transform.position;
			}
		}
			
//			if (Input.GetMouseButtonDown(0) == true)
//			{
//				float deltaX = Input.GetAxis ("Mouse X");
//				float deltaY = Input.GetAxis ("Mouse Y");
//				
//				absX = Mathf.Abs (deltaX);
//				absY = Mathf.Abs (deltaY);
//				
//				distSqr = deltaX*deltaX + deltaY*deltaY;
//				
//				if (distSqr > 0f)
//				{
//					//obtain and store the direction of the swipe
//					if (absX > absY)
//					{
//						if (deltaX > deltaY)
//							direction = move.MoveRight;
//						else if (deltaX < deltaY)
//							direction = move.MoveLeft;
//					}
//					else if (absX < absY)
//					{
//						if (deltaX > deltaY)
//							direction = move.MoveDown;
//						else if (deltaX < deltaY)
//							direction = move.MoveUp;
//					}
//					jump = false;
//				}
//				else
//				{
//					direction = move.Jump;
//					jump = true;
//				}
//			}	
//				//included extra if statement to cause blob to move after finishing the swipe
//			if (Input.GetMouseButtonUp (0) == true)
//			{					
//				ChooseDirection (direction);
//				
//				Move (moveDest);
//				
//				prevDir = direction;
//				
//				collisionPos = transform.position;
//			}
			
		
	}

}