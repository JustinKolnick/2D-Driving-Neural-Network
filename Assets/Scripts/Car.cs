// Justin Kolnick
// Robotic Systems

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {
	
	public bool initialized = false;
	private NeuralNetwork net;
	private Rigidbody2D rb;
	private Sensor[] sensors;
	public float[] sensorReadings = new float[5];
	public float[] output = new float[2];
	public int numCompletedCheckpoints = 0;
	public float timeSinceLastCheckpoint = 0;
	public bool[] checkpoints = new bool[50];

	private const float MAX_VEL = 20f;
	private const float ACCELERATION = 8f;
	private const float VEL_FRICT = 2f;
	private const float TURN_SPEED = 80;
	private const float MAX_CHECKPOINT_WAIT = 7;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
        sensors = GetComponentsInChildren<Sensor>();
	}
	
	// FixedUpdate is called once per physics frame
	void FixedUpdate () {
		if(initialized == true)
		{
			// Get inputs
			for(int i = 0; i < sensors.Length; i++)
			{
				sensorReadings[i] = sensors[i].Output;
			}

			// Feed forward
			output = net.FeedForward(sensorReadings);

			// Update velocity
			rb.AddForce((output[0] + 1) * ACCELERATION * transform.up);
			rb.AddTorque(output[1] * TURN_SPEED);

			rb.velocity = ForwardVelocity();

			// Set Fitness 
			net.SetFitness(numCompletedCheckpoints);

			// If the car hasn't been through a checkpoint, in a while, stop.
			if(timeSinceLastCheckpoint > MAX_CHECKPOINT_WAIT)
			{
				initialized = false;
			}
		}
		else
		{
			rb.velocity = new Vector2(0,0);
		}
	}

	public void Update()
	{
		timeSinceLastCheckpoint += Time.deltaTime;
	}

	public void Init(NeuralNetwork net)
	{
		this.net = net;
		initialized = true;
	}

	// If the car collides with something,
	void OnTriggerEnter2D(Collider2D collider)
	{
		// If its a wall, stop the car.
		if(collider.tag == "WALL")
		{
			initialized = false;
		}

		// If its a checkpoint,
		if(collider.tag == "CHECKPOINT")
		{
			// ...and its not already visisted, 
			if(checkpoints[numCompletedCheckpoints] == false)
			{
				// Mark it as visited and add to the fitness counter numCompleted
				checkpoints[numCompletedCheckpoints] = true;
				Debug.Log("CHECKPOINT" + numCompletedCheckpoints);
				timeSinceLastCheckpoint = 0;
				numCompletedCheckpoints++;

			}

		}
	}

	// Helper for Velocity
	Vector2 ForwardVelocity()
	{
		return transform.up * Vector2.Dot(rb.velocity, transform.up);
	}

}
