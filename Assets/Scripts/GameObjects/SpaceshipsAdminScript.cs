using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipsAdminScript : MonoBehaviour {

    [SerializeField] private SpaceshipInUseScript spaceshipInUse;
    private string tag;
    [SerializeField] private GameObject[] spaceshipsList;

    private void Awake() {

        tag = spaceshipInUse.tagOfUsedSpaceship;

        foreach (GameObject spaceship in spaceshipsList) {

            if (spaceship.tag == tag) {

                GameObject instantiatedSpaceship = Instantiate(spaceship, transform.position, Quaternion.identity) as GameObject;
            }
        }
    }
}
