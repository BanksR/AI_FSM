using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is an enum, or Enumeration; A list of linked constants.
// Here we use an enum to specify our AI states in our simple Finite State Machine (FSM)

public enum State
{
	Idle,
    Wander,
	Chase,
	GoHome,
    Attack,
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
    public float wanderRadius = 5f;

    private Animator _anims;


	void Awake()
	{
		// Finding our player in the scene to populate our _playerPosition variable
		_playerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        // Grabbing a reference to the Animator Component on the Ghost gameobject
        _anims = GetComponent<Animator>();
		//StartCoroutine(Idle());


	}

    // Remember, the Update() function is called every. single. frame.
    // So we use this to set our FSM state based on some simple logic
	private void Update()
	{
        
        //Debug.Log(dist);

        // This switch statement is the core of our FSM - behaviours are linked to functions
        // selected by changing the currentState variable
        switch (currentState)
        {
            case State.Idle:
                Idle();
                break;
            case State.Wander:
                
                break;
            case State.Chase:
                Chase();
                break;
            case State.Die:
                Die();
                break;
            case State.Attack:
                Attack();
                break;
            default:
                Debug.Log("FSM Unknown State");
                break;

        }
		
	}

    // In this section, we define some functions that describe the behaviours
    // of our FSM - This makes our code easily readable, and allows us to expand
    // upon our FSM with additional behaviours should we need to.

	private void Idle()
	{
        // The idle state checks the distance to the player
        // if the distance drops below 5 it will invoke the Chase state
        if (GetDistance() > 5)
        {
            currentState = State.Idle;
        }
        else
        {
            currentState = State.Chase;
        }
	}

    private IEnumerator Wander()
    {
        currentState = State.Idle;
        

        yield return new WaitForSeconds(.5f);

	    Vector2 wander = Random.insideUnitCircle - (Vector2) transform.position;

        while (Vector3.Distance(transform.position, wander) > .2f)
        {
            transform.position = Vector3.MoveTowards(transform.position, wander * wanderRadius, ghostSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        yield return null;

        
        
    }

	private void Chase()
	{
        // The Chase function moves the ghost closer to the player if the player remains
        // within the GetDistance() threshold
		//currentState = State.Chase;
        if (GetDistance() < 5f && GetDistance() > 1.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, _playerPosition.position, ghostSpeed * Time.deltaTime);
        }

        // However, if the ghost gets too close to the player
        // we change the FSM to play the Attack function
        else if (GetDistance() < 1.5f)
        {
            currentState = State.Attack;
        }
        else if (GetDistance() > 5f)
        {
            
            currentState = State.Idle;
        }

	}

    public void Attack()
    {
        // Here we play our Attack animation and again check the distance
        // to the player. We can transition to Idle if the player leaves the 
        // attack range
        _anims.SetBool("IsAttacking", true);
        if (GetDistance() > 1.5f)
        {
            _anims.SetBool("IsAttacking", false);
            
            currentState = State.Idle;
        }

    }

	public void Die()
	{
        // kill the ghost...if indeed you can kill a ghost?
	}

	private float GetDistance()
	{
        // This function performs a simple check to get the distance
        // from the current ghost to the player
        // it will return a float value
		return Vector3.Distance(transform.position, _playerPosition.position);
		
	}
}
