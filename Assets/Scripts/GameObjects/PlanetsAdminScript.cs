using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using F = Functions;
using System.Linq;

public class PlanetsAdminScript : MonoBehaviour {

    // -- Generating the Planet -- //
    [SerializeField] private GameObject[] planets; 
    public bool generateAnotherPlanet; 
    // -- The Order for the display of the planets: Destroied, Dessert, Tech, Electric, Octopus
    private int order = 0;

    // -- Generate acording Meteorites -- //
    private string tagOfSpawnedPlanet;

    [SerializeField] private GameObject[] desertPlanetEnemies;
    [SerializeField] private GameObject[] destroyedPlanetEnemies;
    [SerializeField] private GameObject[] octopusPlanetEnemies;
    [SerializeField] private GameObject[] technoPlanetEnemies;
    [SerializeField] private GameObject[] electricPlanetEnemies;

    private int startingMeteoritesAmount = 5;
    private GameObject[] meteorites; // All the in game Meteorites saved in this array
    private bool generateAnotherMeteorite; // This changes with the time. To give come difficulty to the game.

    private bool isStartingGame = true;

    // -- Game built-in Functions -- //

    private void Start() {
        
        generateAnotherPlanet = false;

        generateAllMatterPlanets(); // Generate just one a plante. Mostly to get the tag and to start the game!
        for (int i = 0; i <= startingMeteoritesAmount - 1; i++) { 

            regenerateMeteoritesAmount(); // With the tag we can spawn the meteorites
        } 

        isStartingGame = false;

        StartCoroutine(gainDificulty());
    }

    private void FixedUpdate() {

        if (SpaceshipScript.IsGameOver) return;

        generatePlanet();
        checkForDestroyedMeteorites();
    }

    // -- Generate Meteorites acording to the planet -- //

    private void checkForDestroyedMeteorites() {
        
        if (generateAnotherMeteorite) {

            regenerateMeteoritesAmount();
        }
    }

    public void regenerateMeteoritesAmount() {

        if (tagOfSpawnedPlanet == "Desert Planet") {

            instantiateMeteoriteObject(desertPlanetEnemies);

        } else if (tagOfSpawnedPlanet == "Destroyed Planet") {

            instantiateMeteoriteObject(destroyedPlanetEnemies);

        } else if (tagOfSpawnedPlanet == "Octopus Planet") {

            instantiateMeteoriteObject(octopusPlanetEnemies);

        } else if (tagOfSpawnedPlanet == "Techno Planet") {

            instantiateMeteoriteObject(technoPlanetEnemies);
        } else if (tagOfSpawnedPlanet == "Electric Planet") {

            if (meteorites.Length > 4) {

                int excessCount = meteorites.Length - 4; // Calculate how many need to be destroyed

                for (int i = 0; i < excessCount; i++) {
                    
                    meteorites[i].GetComponent<MeteoriteScript>().destroyExesMeteorite();
                }
            } else {
                instantiateMeteoriteObject(electricPlanetEnemies);
            }
        }

    }

    private void instantiateMeteoriteObject(GameObject[] enemies) {


        if (enemies != null) {

            int randomIndex = F.randomIntFromCero(enemies.Length - 1);

            GameObject meteorite = Instantiate(enemies[randomIndex]) as GameObject;
            meteorite.transform.SetParent(gameObject.transform);

            if (isStartingGame) {
                meteorite.GetComponent<MeteoriteScript>().gameIsBeingExecuted();
                
            }

            List<GameObject> meteoriteList = new List<GameObject>(); // Creating an list of all the meteorite objects
            meteoriteList.Add(meteorite); // Adding the new meteorite to the list
            meteorites = meteoriteList.ToArray(); // Converting the list into the meteorites array 
        }
    }

    // -- Generate Planet -- //

    private void generatePlanet() {
        if (generateAnotherPlanet) {

            generateAnotherPlanet = false;  
            StartCoroutine(InstantiatePlanets());
        }
    }

    IEnumerator  InstantiatePlanets() {

        yield return new WaitForSeconds(1f); // Wait for 1 second

        generateAllMatterPlanets();
    }

    private void generateAllMatterPlanets() {

        Instantiate(planets[order]); // Generate the object based on the number

        tagOfSpawnedPlanet = planets[order].tag;

        StarsScript.SetPlanetIndex(order);

        order += 1;

        if (order == planets.Length) {
            order = 0;
        }
    }

    private IEnumerator gainDificulty() { // DIFICULTY CONTROLLER

        F.setFactor(1.0f);

        while (true) {
            yield return new WaitForSeconds(16f); // The time it takes to add a enemy in the form of a meteorite

            if (order % 2 == 0) {

                float meteoriteAmount = 1;
                for (int i = 0; i <= meteoriteAmount - 1; i++) { 

                    regenerateMeteoritesAmount();
                } 
            } else {
                StartCoroutine(gainingSpeed());

            }
        }
    }

    private IEnumerator gainingSpeed() {

        float startFactor = F.returnFactor();
        float endFactor = startFactor * 1.15f;
        float elapsedTime = 0f;
        float duration = 9f;

        while (elapsedTime < duration) {
            elapsedTime += Time.unscaledDeltaTime;

            float currentFactor = Mathf.Lerp(startFactor, endFactor, elapsedTime / duration);
            F.setFactor(currentFactor);
            
            yield return null;
        }
        F.setFactor(endFactor);
    }
}
