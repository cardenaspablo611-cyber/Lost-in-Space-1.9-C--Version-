using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShotingBombScript : MonoBehaviour {

    [SerializeField] private GameObject ExplosionBombPrefab;

    private float time;
    private float speed;

    private float yLimit;
    private float bombRadius = 88f;

    private float targetY;
    private bool opositeDirection = false;

    private void Awake() {

    
        try {
            yLimit = GameObject.Find("Spaceship").GetComponent<SpaceshipScript>().Space.Start.y;
        } catch (Exception exception) {
            _ = exception;
            yLimit = 375f;

        }
    }

    private void Start() {

        targetY = yLimit - bombRadius * 1.5f;

        StartCoroutine(DecelerateBasedOnDistanceAndTime());

        if (transform.position.y > targetY) {
            opositeDirection = true;
        }
    }

    IEnumerator DecelerateBasedOnDistanceAndTime() {

        Vector3 startPosition = transform.position;
        float totalDistance = Mathf.Abs(targetY - startPosition.y);
        float time = 4f;  
        float k = 400f / totalDistance;
        float initialSpeed = totalDistance * k / (1 - Mathf.Exp(-k * time));

        float elapsedTime = 0f;

        while (elapsedTime < time) {

            elapsedTime += Time.deltaTime;

            float currentSpeed = initialSpeed * Mathf.Exp(-k * elapsedTime);

            if (!opositeDirection) {
                transform.position += new Vector3(0, currentSpeed * Time.deltaTime, 0);
            } else {
                transform.position -= new Vector3(0, currentSpeed * Time.deltaTime, 0);
            }

            yield return null;
        }
        
        yield return new WaitForSeconds(1f); 

        explote();

    }

    private void OnCollisionEnter2D(Collision2D collision) {

        if (collision.gameObject.CompareTag("Meteorite")) {
    
            explote();
        }
    }

    private void explote() {

        GameObject explotion = Instantiate(ExplosionBombPrefab) as GameObject;
        explotion.transform.position = transform.position;

        Destroy(gameObject);
    }
}
