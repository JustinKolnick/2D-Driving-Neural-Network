// Justin Kolnick
// Robotic Systems

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
	// The layer this sensor will be reacting to.
	[SerializeField]
	private LayerMask LayerToSense;

	//The crosshair of the sensor.
	[SerializeField]
	private SpriteRenderer marker;

	private const float MAX_DISTANCE = 10f;
	private const float MIN_DISTANCE = 0.01f;
	
	// Output
	public float Output
	{
		get;
		private set;
	}

	// Use this for initialization
	void Start () 
	{
		marker.gameObject.SetActive(true);
	}

	// Determine distance using RayCast. This method is from another source online
	// as determining this function was more difficult than anticipated. 
	void FixedUpdate () 
	{
		Vector2 direction = marker.transform.position - this.transform.position;
		direction.Normalize();	

		RaycastHit2D hit =  Physics2D.Raycast(this.transform.position, direction, MAX_DISTANCE, LayerToSense);

		if (hit.collider == null)
			hit.distance = MAX_DISTANCE;
		else if (hit.distance < MIN_DISTANCE)
			hit.distance = MIN_DISTANCE;

		this.Output = hit.distance;
		marker.transform.position = (Vector2) this.transform.position + direction * hit.distance;
	}

}
