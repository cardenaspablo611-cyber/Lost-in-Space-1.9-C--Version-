using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipsAdminScript : MonoBehaviour {

    [SerializeField] private SpaceshipInUseScript spaceshipInUse;
    private string spaceshipTag;
    [SerializeField] private GameObject[] spaceshipsList;

    private void Awake() {

        spaceshipTag = spaceshipInUse.tagOfUsedSpaceship;

        foreach (GameObject spaceship in spaceshipsList) {

            if (spaceship.tag == spaceshipTag) {

                GameObject instantiatedSpaceship = Instantiate(spaceship, transform.position, Quaternion.identity) as GameObject;
            }
        }
    }
}
