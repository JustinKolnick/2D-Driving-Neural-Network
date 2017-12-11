// Justin Kolnick
// Robotic Systems

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class NeuralNetwork : IComparable<NeuralNetwork>
{
	private int[] layers;
	private float[][] neurons;
	private float[][][] weights;
	public float fitness;

	// Initial NN Constructor
	public NeuralNetwork(int[] layers)
	{
		// Make the layers
		this.layers = new int[layers.Length];
		for (int i = 0; i < layers.Length; i++)
			this.layers[i] = layers[i];

		// Create neurons and weights
		InitNeurons();
		InitWeights();
	}

	// Copy Constructor
	public NeuralNetwork(NeuralNetwork copyNetwork)
	{
		// Make the layers
		this.layers = new int[copyNetwork.layers.Length];
		for (int i = 0; i < copyNetwork.layers.Length; i++)
			this.layers[i] = copyNetwork.layers[i];

		// Create neurons and weights, and copy them
		InitNeurons();
		InitWeights();
		CopyWeights(copyNetwork.weights);
	}

	// Simple Copy Weights Method
	private void CopyWeights(float[][][] copyWeights)
	{
		for (int i = 0; i < weights.Length; i++)
			for (int j = 0; j < weights[i].Length; j++)
				for (int k = 0; k < weights[i][j].Length; k++)
					weights[i][j][k] = copyWeights[i][j][k];

	}

	// Initialize Neurons
	private void InitNeurons()
	{
		List<float[]> neuronsList = new List<float[]>();

		for (int i = 0; i < layers.Length; i++)
			neuronsList.Add(new float[layers[i]]);

		neurons = neuronsList.ToArray();
	}

	// Initialize Weights
	private void InitWeights()
	{
		// Create a new list for each dimension, this will end up being a 3D array
		List<float[][]> weightsList = new List<float[][]>();

		for (int i = 1; i < layers.Length; i++)
		{
			List<float[]> layerWeightsList = new List<float[]>();

			int neuronsInPreviousLayer = layers[i - 1]; 

			// Connect neurons from previous layer with new weights
			for (int j = 0; j < neurons[i].Length; j++)
			{
				float[] neuronWeights = new float[neuronsInPreviousLayer];

				// Connect neurons from previous layer with new weights
				for (int k = 0; k < neuronsInPreviousLayer; k++)
				{
					//	Assign a random weight
					neuronWeights[k] = UnityEngine.Random.Range(-0.5f,0.5f);
				}

				layerWeightsList.Add(neuronWeights);
			}

			weightsList.Add(layerWeightsList.ToArray());
		}

		// Convert to 3-Dimensional Array
		weights = weightsList.ToArray();
	}

	// Input -> Output
	public float[] FeedForward(float[] inputs)
	{
		for (int i = 0; i < inputs.Length; i++)
			neurons[0][i] = inputs[i];

		// Dive deep into the NN
		for (int i = 1; i < layers.Length; i++)
		{
			for (int j = 0; j < neurons[i].Length; j++)
			{
				float value = 0f;

				for (int k = 0; k < neurons[i-1].Length; k++)
				{
					// Simple multiplcation of weights and neurons
					value += weights[i - 1][j][k] * neurons[i - 1][k]; 
				}
				//Hyperbolic tangent activation
				neurons[i][j] = (float)Math.Tanh(value);
			}
		}

		// Return 2 neuron output layer
		return neurons[neurons.Length-1];
	}

	// Change weights slightly based on random chance
	public void Mutate()
	{
		for (int i = 0; i < weights.Length; i++)
		{
			for (int j = 0; j < weights[i].Length; j++)
			{
				for (int k = 0; k < weights[i][j].Length; k++)
				{
					float mutation = weights[i][j][k];
					float randomNumber = UnityEngine.Random.Range(0f,100f);
					
					// Flip sign
					if (randomNumber < 2f)
					{
						mutation *= -1f;
					}

					// Chose weight from -0.5 to 0.5
					else if (randomNumber < 4f)
					{
						mutation = UnityEngine.Random.Range(-0.5f, 0.5f);
					}

					// Increase by 0% to 80%
					else if (randomNumber < 6f)
					{
						float factor = UnityEngine.Random.Range(1f, 1.8f);
						mutation *= factor;
					}

					// Decrease by 0% to 80%
					else if (randomNumber < 8f)
					{
						float factor = UnityEngine.Random.Range(0f, 0.8f);
						mutation *= factor;
					}

					weights[i][j][k] = mutation;
				}
			}
		}
	}

	// Used to set fitness every frame of Car
	public void SetFitness(float fit)
	{
		fitness = fit;
	}

	// Used within NN sorting
	public float GetFitness()
	{
		return fitness;
	}

	// NN CompareTo Method in order to sort. Compare using the fitness variable
	public int CompareTo(NeuralNetwork other)
	{
		if (other == null) 
			return 1;

		if (fitness > other.fitness)
			return 1;
		else if (fitness < other.fitness)
			return -1;
		else
			return 0;
	}
}
