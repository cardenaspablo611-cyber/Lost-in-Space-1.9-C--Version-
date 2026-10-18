using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRecover : MonoBehaviour {

    [SerializeField] private GameObject spaceshipAnimationPrefab;
    [SerializeField] private GameObject greenHeartAnimationPrefab;

    private GameObject glowAnimation;

    private SpaceshipScript parentScript;

    private void Awake() {

        parentScript = GetComponent<SpaceshipScript>();
    }


    private void Start() {
        StartCoroutine(startHealthRevocery());
    }

    private void FixedUpdate() {

        if (glowAnimation != null) {
            glowAnimation.transform.position = transform.position;
        }
    }

    private IEnumerator startHealthRevocery() {
        yield return new WaitForSeconds(3);

        while (true) {
            if (parentScript.Lives < 3) {
                yield return new WaitForSeconds(40);

                if (spaceshipAnimationPrefab != null) {
                    glowAnimation = Instantiate(spaceshipAnimationPrefab, transform.position, Quaternion.identity) as GameObject;
                    glowAnimation.transform.SetParent(gameObject.transform);

                    float time = glowAnimation.GetComponent<AnimationScript>().TotalDurationTime;

                    yield return new WaitForSeconds(time);
                } else {
                    Debug.LogWarning("[HealthRecover] spaceshipAnimationPrefab is null on Tree Spaceship");
                    yield return new WaitForSeconds(3);
                }

                parentScript.getAHeart();

                if (greenHeartAnimationPrefab != null) {
                    GameObject greenHeart = Instantiate(greenHeartAnimationPrefab, transform.position, Quaternion.identity) as GameObject;
                    greenHeart.transform.SetParent(gameObject.transform);
                } else {
                    Debug.LogWarning("[HealthRecover] greenHeartAnimationPrefab is null on Tree Spaceship");
                }
                
            } else {
                yield return null;
            }
        }
    }
}
