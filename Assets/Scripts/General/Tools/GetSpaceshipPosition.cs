using UnityEngine;

public class GetSpaceshipPosition : MonoBehaviour {


    private void Start() {
        GameObject spaceship = GameObject.FindWithTag("Spaceship");

        transform.position = spaceship.transform.position + new Vector3(0f, 60f, 0f);
        StartCoroutine(AutoDestroyAfterDelay());
    }

    private System.Collections.IEnumerator AutoDestroyAfterDelay() {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}