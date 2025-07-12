using System.Collections;
using System.Collections.Generic;
using F = Functions;
using UnityEngine;
using System;

public class MisilScript : MonoBehaviour {

    private Vector2 position;

    private float initialSpeed = 70f; 
    private float acceleration = 200f; 

    private float misilFrame = 10f;
    private Vector2 startingPositionSpace;
    private float currentSpeed; 

    private void Start() {
        
        try {
            startingPositionSpace = F.GetScriptComponentName<SpaceParent>("Space - Parent").Start;
        } catch (Exception exception) {
            Debug.Log(exception);
            startingPositionSpace = new Vector2 (0,750 / 2);
        }
        position = transform.position;
        currentSpeed = initialSpeed;

        StartCoroutine(MoveWithAcceleration());
    }

    private IEnumerator MoveWithAcceleration() {
        while (true) {
            misilMovement();
            checkOfLimit();

            currentSpeed += acceleration * Time.deltaTime;

            yield return null;
        }
    }

    private void misilMovement() {

        position.y += currentSpeed * Time.fixedDeltaTime;
        transform.position = position;
    }

    private void checkOfLimit() {
        if (position.y > startingPositionSpace.y + misilFrame * 2) {
            Destroy(gameObject);
        }
    }
}
