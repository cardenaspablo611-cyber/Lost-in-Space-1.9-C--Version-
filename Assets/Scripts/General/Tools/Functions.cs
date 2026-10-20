using System.Collections.Generic;
using UnityRandom = UnityEngine.Random;
using UnityEngine;
using System.Collections;

public class Functions { 

    private static float lateralMovement = 0f;
    private static float linearMovement = 1f;  

    private static float factor = 1f;

    private static SpaceshipInUseScript spaceshipInUse;

    private void Start() {
        factor = 1f;
    }

    public static void setFactor(float givenFactor) {
        factor = givenFactor;
    }

    public static float returnFactor() {
        return factor;
    }
    
    public static void getSpaceshipInUseVariable(SpaceshipInUseScript inUse) {
        spaceshipInUse = inUse;
    }

    // -- MATH -- // 
    public static float randomFloat(float min, float max) {

        System.Random random = new System.Random(); //These makes an object that can returns a random value between 0.0 (inclusive) and 1.0 (exclusive).

        double value = (random.NextDouble() * (max - min) + min); //(max - min) --> calculates the range}

        return (float)value;
    }

    public static float randomFloatFromCero(float max) {

        return randomFloat(0, max);

    }
    
    public static int randomInt(int min, int max) {

        System.Random random = new System.Random();

        int value = random.Next(min, max + 1); 

        return value;

    }

    public static int randomIntFromCero(int max) {

        return randomInt(0, max);

    }

    public static float calculatingHypotenusaSameSides(float hypotenusa) {

        return Mathf.Sqrt(hypotenusa * hypotenusa / 2);

    }

    // -- GET THE SIZE AND CORDS OF A GAMEOBJECT -- //

    public static Vector2 getGameObjectSizeWithScale(string gameObjectName) {

        return GameObject.Find(gameObjectName).transform.localScale;
    }

    public static Vector2 getGameObjectSizeWithRenderer(string gameObjectName) {
       
        return GameObject.Find(gameObjectName).GetComponent<SpriteRenderer>().bounds.size;
    }

    public static Vector2 getGameObjectCords(string gameObjectName) {

        return GameObject.Find(gameObjectName).transform.position;

    }

    // -- CHECK IF GAMEOVER -- //
    
    public static bool checkGameOver() {
        
        Transform parentTransform = GameObject.Find($"{spaceshipInUse.tagOfUsedSpaceship}(Clone)").transform;

        if (parentTransform != null) {
            Transform child = parentTransform.GetChild(0);

            SpaceshipScript spaceshipScriptInstance = child.GetComponent<SpaceshipScript>(); 

            return spaceshipScriptInstance.Deadth;
        } else {
            return false;
        }
        
    }

    // -- SEARCH FOR A SCRIPT IN A GAMEOJBECT -- //
    public static MB GetScriptComponentGameObject<MB>(GameObject triggeredGameObject) where MB : MonoBehaviour {

        MB scriptComponent = triggeredGameObject.GetComponent<MB>();

        return scriptComponent;
    }

    public static MB GetScriptComponentName<MB>(string gameObjectName) where MB : MonoBehaviour {
        GameObject triggeredGameObject = GameObject.Find(gameObjectName);

        MB scriptComponent = GetScriptComponentGameObject<MB>(triggeredGameObject);

        return scriptComponent;
    }

    // -- GAMEOBJECTS NEEDS -- //
    public static Vector2 direction(Vector2 position, float speed) {
        
        UserInterfaceScript userInterface = GetScriptComponentName<UserInterfaceScript>("User Interface Object");

        float currentSpeed;
        
        lateralMovement = 0f;
        linearMovement = 1f;  

        if (userInterface.Up) {
            linearMovement = 1.5f;
            //positivLinear = true;
        }

        if (userInterface.Down) {
            linearMovement = 0.25f;
            //negativLinear = true;
        }

        if (userInterface.Right) {
            lateralMovement = 0.5f;
            //positivLateral = true;
        }

        if (userInterface.Left) {
            lateralMovement = -0.5f;
            //negativLateral = true;
        }

        if (userInterface.Down && userInterface.Left || userInterface.Down && userInterface.Right) {

            currentSpeed = (int)calculatingHypotenusaSameSides(speed) *(-1);

        } else if (userInterface.Up && userInterface.Left || userInterface.Up && userInterface.Right) {

            currentSpeed = (int)calculatingHypotenusaSameSides(speed) *(-1);

        } else {

            currentSpeed = speed *(-1);
        }

        // CRITICAL FIX: Multiply by Time.deltaTime so movement scales with Time.timeScale
        // This makes stars/enemies/objects visibly slow down during death sequence
        position.x += currentSpeed * lateralMovement * factor * Time.deltaTime;
        position.y += currentSpeed * linearMovement * factor * Time.deltaTime;

        return position;
    }

    public static float getLateralMovement() {
        return lateralMovement;
    }

    public static T ChooseRandom<T>(params T[] variables)
    {
        if (variables == null || variables.Length == 0)
        {
            Debug.LogError("No elements provided to choose from.");
            return default;
        }

        int randomIndex = Random.Range(0, variables.Length);
        return variables[randomIndex];
    }
}