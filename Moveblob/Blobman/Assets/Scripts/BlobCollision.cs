using UnityEngine;
using System.Collections;

public class BlobCollision : MonoBehaviour 
{	
	public GameObject txt;
	
	// Use this for initialization
	void Start () 
	{
	}
	
	public void OnCollisionEnter (Collision collision)
	{   		
		foreach (ContactPoint contact in collision.contacts) 
		{	
			
			txt.guiText.text = MoveBlob.jump.ToString ();
			
			//bounce back to position prior to collision
			if (collision.gameObject.tag == "Tower" && MoveBlob.jump == true)
			{
				MoveBlob.destination = MoveBlob.destination;
			}
			//move as normal
			else
			{
				MoveBlob.destination = MoveBlob.collisionPos;
			}
        }
		
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
}
