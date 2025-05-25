using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsBougthScript : MonoBehaviour {

    [SerializeField] private SpecificSpaceshipInformation spaceshipInformation;

    private GameObject child;
    

    private void Start() {

        child = transform.GetChild(0).gameObject;

    }

    private void Update() {


        if (spaceshipInformation.bought) {
            child.SetActive(true);
        } else {
            child.SetActive(false);
        }
    }
}
