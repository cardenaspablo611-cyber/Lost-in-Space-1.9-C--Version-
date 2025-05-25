using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using F = Functions;

public class PlanetScript : MonoBehaviour {
    
    private float speed = 0.35f;
    [SerializeField] private float rotation;

    private float length;

    // -- Components -- //
    private SpriteRenderer  planetSprites;
    [SerializeField] private SpaceParent space;
    private PlanetsAdminScript planetsScript;

    private void Start () {

        planetSprites = GetComponent<SpriteRenderer>();
        space = F.GetScriptComponentName<SpaceParent>("Space - Parent");
        planetsScript = F.GetScriptComponentName<PlanetsAdminScript>("PlanetsAdmin");

        length = planetSprites.bounds.size.y; // Get the length of the image 

        Vector2 position = new Vector2 (transform.position.x, space.Start.y + length / 2);  // Set starting-
        transform.position = position;                                                       //   position
    }

    private void FixedUpdate() {

        //if (F.checkGameOver()) { Destroy(gameObject); }

        if (transform.position.y <  space.End.y - length / 2) { // Gets off limits

            planetsScript.generateAnotherPlanet = true;
            Destroy(gameObject); 
        
        } else {

            if (speed != 0) { 
                transform.position = F.direction(transform.position, speed); //Movement
            }

            if (rotation != 0) { 
                transform.Rotate(Vector3.forward, rotation * Time.deltaTime / 5f); //Rotation
            }
        }
    }
}
