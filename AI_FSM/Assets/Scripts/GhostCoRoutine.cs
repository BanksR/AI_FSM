using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using Random = System.Random;


// In this enum we can declare a variety of different
// behaviours; this list can be expanded as required
// enums can be referenced directly (State.Idle) or by their 
// index (0, 1, 2, 3, 4 etc)
public enum State
{
	Idle,
	Wander,
	Chase,
	GoHome,
	Attack,
	Die
}


public class GhostCoRoutine : MonoBehaviour
{

	
	// Basic ghost movement parameters
	public float moveSpeed = 2f;
	public float wanderRange = 3f;
	
	// Here we gather some useful Transforms
	private Transform _playerTarget;
	public Transform ghostHome;
	private bool isHome = true;
	private Animator _anims;

	// Here we create some colour fields to denote the different FSM states
	public Color IdleColour, ChaseColour, WanderColour, AttackColour, GoHomeColour;
	public SpriteRenderer _spr;

	// Here we create a reference to our current state
	private State currentState;
	
	// Use this for initialization
	void Awake ()
	{
		// Find our player object in the scene
		_playerTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		_anims = GetComponent<Animator>();

		// Init our current state to idle
		currentState = State.Idle;
		// Fire off our first Coroutine
		StartCoroutine(Idle());
		// Place the ghost on its home position
		transform.position = ghostHome.position;
	}

	private float GetDistance()
	{
		// This simple helper function returns a float of the distance from ghost to player
		return Vector3.Distance(transform.position, _playerTarget.position);
	}
	
	// This method uses Coroutines to implement the different behaviours
	// These Coroutines are started based on which State we're currently in.
	// Simple logic dictates the switching of states - usually based on distance from the player
	// gameobject, although additional logic could easily be implemented

	IEnumerator Idle()
	{
		_spr.color = IdleColour;
		moveSpeed = 1f;
		//Debug.Log("Idling...");
		_anims.SetBool("IsIdling", true);

		while (currentState == State.Idle)
		{
			Debug.Log("Idling...");
			if (GetDistance() < 5f)
			{
				currentState = State.Chase;
				StartCoroutine(Chase());
				yield return null;
			}
			
			else if (GetDistance() > 5f && !isHome)
			{
				currentState = State.GoHome;
				StartCoroutine(GoHome());
			}

			yield return new WaitForEndOfFrame();
		}
		//Debug.Log("Exiting: Idle");
		_anims.SetBool("IsIdling", false);
		yield return null;
	}

	IEnumerator Chase()
	{
		_spr.color = ChaseColour;
		//Debug.Log("Chasing...");
		moveSpeed = 2.5f;
		
		while (currentState == State.Chase)
		{
			Debug.Log("Chasing...");
			transform.position = Vector3.MoveTowards(transform.position, _playerTarget.position, moveSpeed * Time.deltaTime);
			if (GetDistance() < 1f)
			{
				currentState = State.Attack;
				StartCoroutine(Attack());
				yield return null;
			}

			else if (GetDistance() > 5)
			{
				currentState = State.Wander;
				StartCoroutine(Wander());
				yield return null;
			}


			yield return new WaitForEndOfFrame();
		}

		yield return null;
	}

	IEnumerator Wander()
	{
		_spr.color = WanderColour;
		isHome = false;
		moveSpeed = 0.5f;
		//Debug.Log("Wandering...");
		_anims.SetBool("IsIdling", true);
		Vector2 randDir = new Vector2(transform.position.x + (UnityEngine.Random.insideUnitCircle.x * wanderRange), transform.position.y + (UnityEngine.Random.insideUnitCircle.y * wanderRange));
		
		yield return new WaitForSeconds(1f);


		while (Vector2.Distance(transform.position, randDir) > 0.2f)
			
		{
			Debug.Log("Wnadering...");
			transform.position = Vector3.MoveTowards(transform.position, randDir, Time.deltaTime);
			

			if (GetDistance() < 5)
			{
				break;
			}
			yield return new WaitForEndOfFrame();

		}

		
		currentState = State.GoHome;
		StartCoroutine(GoHome());

		yield return null;
	}

	IEnumerator Attack()
	{
		_spr.color = AttackColour;
		//Debug.Log("Attacking...");

		while (currentState == State.Attack)
		{
			Debug.Log("Attacking...");
			_anims.SetBool("IsAttacking", true);
			
			if (GetDistance() > 1 && GetDistance() < 5)
			{
				_anims.SetBool("IsAttacking", false);
				currentState = State.Chase;
				StartCoroutine(Chase());
				yield return null;
			}
			
			//Debug.Log("ATTACKING!!!");
			yield return new WaitForEndOfFrame();
		}

		yield return null;
	}

	IEnumerator GoHome()
	{
		_spr.color = GoHomeColour;
		moveSpeed = 1f;
		float dist;

		//dist = Vector2.Distance(transform.position, ghostHome.position);

		while (Vector2.Distance(transform.position, ghostHome.position) > 0.1f)
		{
			Debug.Log("Going home...");
			transform.position =
				Vector2.MoveTowards(transform.position, ghostHome.position, moveSpeed * Time.deltaTime);

			if (GetDistance() < 5)
			{
				break;
			}

			yield return new WaitForEndOfFrame();
		
		}

		isHome = true;
		currentState = State.Idle;
		StartCoroutine(Idle());

		yield return null;
	}


}
