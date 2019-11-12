using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using Random = System.Random;

public class GhostCoRoutine : MonoBehaviour
{

	
	public float moveSpeed = 2f;
	public float wanderRange = 3f;

	private Transform _playerTarget;
	public Transform ghostHome;
	private bool isHome = true;
	private Animator _anims;

	public Color IdleColour, ChaseColour, WanderColour, AttackColour, GoHomeColour;
	public SpriteRenderer _spr;

	private State currentState = State.Idle;
	
	// Use this for initialization
	void Start ()
	{
		_playerTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		_anims = GetComponent<Animator>();
		
		StartCoroutine(Idle());

		transform.position = ghostHome.position;
	}

	private float GetDistance()
	{
		return Vector3.Distance(transform.position, _playerTarget.position);
	}

	IEnumerator Idle()
	{
		_spr.color = IdleColour;
		moveSpeed = 1f;
		//Debug.Log("Idling...");
		_anims.SetBool("IsIdling", true);

		while (currentState == State.Idle)
		{
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
			//Debug.Log(Vector2.Distance(transform.position, randDir));
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
			//Vector2.Distance(transform.position, ghostHome.position);
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
