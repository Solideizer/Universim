# **Universim**
Universim is our senior design project. Our advisor is [Assoc.Prof. Zeynep Orman](https://profil.istanbulc.edu.tr/tr/p/ormanz)

![screenshot](https://github.com/Solideizer/Universim/blob/master/Frame001_0105.jpg)
## Summary
 
#### Simulating the fundamental interactions of Carnivore – Herbivore AI Agents in a procedurally generated environment with the help of Unity Engine

There are 2 kinds of artificial intelligence agents in the simulation: Carnivore and Herbivore agents. Fundamental urges such as thirst and hunger is simulated for every artificial intelligence agent. 

Carnivore agents are hunting and feeding on herbivore agents. Herbivore agents on the other hand are trying to find plants for food. Both agent types are searching for water on a procedurally generated environment.

Perlin Noise is used to create procedural terrain heights. Objects such as plants,trees and ai agents are also procedurally placed on the terrain.

Agents can reproduce if the requirements are met. After a pregnancy period, a new agent is deployed on to simulation area. This new agents genetic features (movement speed, vision radius, fertility etc) are determined based on its parents. 

Object Pooling is used in situations like deploying a new agent or handling the initialization of vfx objects.

Certain metrics such as population numbers and gender statistics are obtained while the simulation is running. A statistics screen is created to visualize those metrics as bar/line charts. This system can be improved to show how natural selection is affecting ai agents.

## **Implemented Features** 
*  Event Based Decision Maker
*  Procedurally Generated Terrain (Perlin Noise) 
*  Procedurally Placed Objects 
*  Hunger / Searching Food
*  Thirst / Searching Water
*  Reproduction System
*  Genetic Features
*  Natural Selection
*  Particle Swarm Optimization
*  Statistics System 

![screenshot](https://github.com/Solideizer/Universim/blob/master/tiev3-9lr9f.png)
