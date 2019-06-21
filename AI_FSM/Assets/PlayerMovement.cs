using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour 
{

    // Master Player speed variable
	public float playerSpeed;

	// Initialise our up and down / left and right input variables
	
	private float horiz, vert = 0;


	private Animator _anims;

	private void Awake()
	{
		_anims = GetComponent<Animator>();
	}


	// Update is called once per frame <- Make sure  you understand what this means!!
	void Update ()
	{

		
		// Here we grab the player input, add the player speed value and multiply by delta time
		horiz += Input.GetAxisRaw("Horizontal") * playerSpeed * Time.deltaTime;
		vert += Input.GetAxisRaw("Vertical") * playerSpeed * Time.deltaTime;
		
		
		// here we create a new vector 2 to handle the transformation
		Vector2 movement = new Vector2(horiz, vert);

		
		// This if checks to see if there is any player input and switches the animatin on/off accordingly
		if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") !=0)
		{
			_anims.SetBool("IsWalking", true);
		}
		else
		{
			_anims.SetBool("IsWalking", false);
		}

		// Our new variable is then used to update the player position values
		transform.position = movement;

	}
}
