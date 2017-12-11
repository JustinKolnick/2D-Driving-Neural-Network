// Justin Kolnick
// Robotic Systems

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {
	
	public GameObject carPrefab;

	private bool isTraining = false;
	private int populationSize = 10;
	private int generationNumber = 0;
	private int[] layers = new int[] { 5, 4, 3, 2 };
	private List<NeuralNetwork> nets;
	private List<Car> carList = null;

	// The structure of this method was inspired by an online guide. My method for
	// the genetic algorithm, necessary variables, timing, etc. are specific to this project.
	void Update()
	{
		if(isTraining == false)
		{
			// If the generation is 0, this is the first time through the loop
			// CREATE EVERYTHING
			if(generationNumber == 0)
			{
				InitCarNN();
			}
			else
			{
				// Otherwise, sort and breed! (and mutate)!
				nets.Sort();
				Debug.Log("PARENT1: "+ nets[populationSize-1-(0%2)].fitness);
				Debug.Log("PARENT2: "+ nets[populationSize-1-(1%2)].fitness);

				// Create new NN's based on parents. i%2 gives us 0 or 1 here
				// to toggle between selecting the parents
				for (int i = 0; i < populationSize - 1; i++)
				{
					nets[i] = new NeuralNetwork(nets[populationSize-1-(i%2)]);
					nets[i].Mutate();
				}

				// Reset all of the variables for the new NNs
				for (int i = 0; i < populationSize; i++)
				{
					nets[i].SetFitness(0f);
					carList[i].checkpoints = new bool[50];
					carList[i].numCompletedCheckpoints = 0;
				}
            }

            generationNumber++;

            isTraining = true;
           	Invoke("Timer",17f);
            CreateCarBods();
		}
	}

	void InitCarNN()
	{
		nets = new List<NeuralNetwork>();

		for(int i = 0; i < populationSize; i++)
		{
			NeuralNetwork net = new NeuralNetwork(layers);
			net.Mutate();
			nets.Add(net);
		}
	}

	// Creates instances of the cars. If the carList already exists, then 
	// tear em down and rebuild!
	private void CreateCarBods()
	{
		if(carList != null)
		{
			for(int i = 0; i < carList.Count; i++)
			{
				GameObject.Destroy(carList[i].gameObject);
			}
		}

		// Create a new list of cars
		carList = new List<Car>();

		for(int i = 0; i < populationSize; i++)
		{
			Car vroom = ((GameObject)Instantiate(carPrefab, new Vector3(-15, 6, 0), transform.rotation)).GetComponent<Car>();

			// Follow this method to watch the magic unfold of the Car's algorithm
			vroom.Init(nets[i]);
			carList.Add(vroom);
		}

	}

	// Helper for timing each generation.
	void Timer()
    {
        isTraining = false;
    }

}
