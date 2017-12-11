// Justin Kolnick
// Robotic Systems

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

	bool triggered;

	// Use this for initialization
	void Start () {
		triggered = false;
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if(!triggered)
		{
			Car c = collider.gameObject.GetComponent<Car>();
			c.numCompletedCheckpoints++;
			c.timeSinceLastCheckpoint = 0;
		}
		triggered = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
