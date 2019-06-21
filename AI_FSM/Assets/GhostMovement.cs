using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is an enum, or Enumeration; A list of linked constants.
// Here we use an enum to specify our AI states in our simple Finite State Machine (FSM)

public enum State
{
	Idle,
	Chase,
	Die
}

// *****
// Notice how I declared the enum OUTSIDE of the main body of the class - this makes the enum available 
// to other classes in the project.


public class GhostMovement : MonoBehaviour
{

	
	// Here I declare a new variable of type State to keep track of this ghost's current AI state
	public State currentState = State.Idle;
	
	// Creating a reference to our players position
	private Transform _playerPosition;

	public float ghostSpeed = 2f;


	void Awake()
	{
		// Finding our player in the scene to populate our _playerPosition variable
		_playerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

		
	}

	private void Update()
	{
		
		float dist = GetDistance();
		Debug.Log(dist);

		if (dist < 10f)
		{
			Chase();
		}
		else
		{
			Idle();
		}
	}

	private void Idle()
	{
		// Do nothing
		currentState = State.Idle;
	}

	private void Chase()
	{
		currentState = State.Chase;
		transform.position = Vector3.MoveTowards(transform.position, _playerPosition.position, ghostSpeed * Time.deltaTime );

	}

	public void Die()
	{
	}

	private float GetDistance()
	{
		return Vector3.Distance(transform.position, _playerPosition.position);
		
	}
}
