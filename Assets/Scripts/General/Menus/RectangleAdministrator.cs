using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleAdministrator : MonoBehaviour {

    private float totalAnimationDuration = 1f;
    
    [SerializeField] private float startingYPosition;
    [SerializeField] private float endingYPosition;

    private float changeAcelerationPosition;
    private float speed;

    private float YPosition;

    private void Start () {

        changeAcelerationPosition = (startingYPosition + endingYPosition) / 2;

        YPosition = transform.position.y;

        Out();
    }

    public void Inside() {

        StartCoroutine(MoveObjectWithAcceleration(endingYPosition,startingYPosition));
        GameObject.Find("AudioManager").GetComponent<AudioManager>().PlayEffect("Door");
    }

    public void Out() {

        StartCoroutine(MoveObjectWithAcceleration(startingYPosition,endingYPosition));
    }

    private IEnumerator MoveObjectWithAcceleration(float startY, float endY) {
        float elapsedTime = 0f;

        Vector3 start = new Vector3(transform.position.x , startY, transform.position.z);
        Vector3 end = new Vector3(transform.position.x , endY, transform.position.z);

        while (elapsedTime < totalAnimationDuration) {
            // Calculate the time percentage completed
            float t = elapsedTime / totalAnimationDuration;

            // Apply acceleration to the time percentage
            float accelerationFactor = Mathf.Pow(t, 10);

            // Lerp the position based on the acceleration factor
            transform.position = Vector3.Lerp(start, end, accelerationFactor);

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the object reaches the final position
        transform.position = end;
    }
}



