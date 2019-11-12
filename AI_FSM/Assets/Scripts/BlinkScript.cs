using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkScript : MonoBehaviour
{
	public Vector2 blinkTimeRange = new Vector2(3, 7);

	private Animator _anims;

	// Use this for initialization
	void Start ()
	{
		_anims = GetComponent<Animator>();
		PickRandomRange();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	IEnumerator Blinker(float randomTime)
	{
		
		float t = 0;

		while (t < 0.25f)
		{
			_anims.SetLayerWeight(1, 1);
			t += Time.deltaTime;
			yield return new WaitForEndOfFrame();

		}
		_anims.SetLayerWeight(1, 0);


		while (t < randomTime)
		{
			t += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		





		PickRandomRange();
		yield return null;
	}

	private void PickRandomRange()
	{
		float randTime = Random.Range(blinkTimeRange.x, blinkTimeRange.y);

		StartCoroutine(Blinker(randTime));
	}
}
