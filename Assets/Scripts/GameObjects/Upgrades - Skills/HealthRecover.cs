using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRecover : MonoBehaviour {

    [SerializeField] private GameObject treeGlowAnimationPrefab;  // New: Pablo's green glow animation on the ship
    [SerializeField] private GameObject greenHeartAnimationPrefab;  // Existing green heart pickup effect

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

                // Play green glow animation on the ship
                glowAnimation = Instantiate(treeGlowAnimationPrefab, transform.position, Quaternion.identity) as GameObject;
                glowAnimation.transform.SetParent(gameObject.transform);

                float time = glowAnimation.GetComponent<AnimationScript>().TotalDurationTime;

                yield return new WaitForSeconds(time);

                // Grant the life
                parentScript.getAHeart();

                // Show green heart effect (reuses existing animation)
                GameObject greenHeart = Instantiate(greenHeartAnimationPrefab, transform.position, Quaternion.identity) as GameObject;
                greenHeart.transform.SetParent(gameObject.transform);
                
            } else {
                yield return null;
            }
        }
    }
}
