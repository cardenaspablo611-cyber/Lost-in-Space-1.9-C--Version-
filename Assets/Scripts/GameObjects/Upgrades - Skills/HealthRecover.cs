using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRecover : MonoBehaviour {

    [SerializeField] private GameObject spaceshipAnimationPrefab;
    [SerializeField] private GameObject greenHeartAnimationPrefab;

    private GameObject spaceshipAnimation;

    private SpaceshipScript parentScript;

    private void Awake() {

        parentScript = GetComponent<SpaceshipScript>();
    }


    private void Start() {
        StartCoroutine(startHealthRevocery());
    }

    private void FixedUpdate() {

        if (spaceshipAnimation != null) {
            spaceshipAnimation.transform.position = new Vector3(transform.position.x, transform.position.y + 7.5f, 0f);
        }
    }

    private IEnumerator startHealthRevocery() {
        yield return new WaitForSeconds(3);

        while (true) {
            if (parentScript.Lives < 3) {
                yield return new WaitForSeconds(40);

                spaceshipAnimation = Instantiate(spaceshipAnimationPrefab, transform.position, Quaternion.identity) as GameObject;
                spaceshipAnimation.transform.SetParent(gameObject.transform);

                float time = spaceshipAnimation.GetComponent<AnimationScript>().TotalDurationTime;

                yield return new WaitForSeconds(time);

                parentScript.getAHeart();

                GameObject greenHeart = Instantiate(greenHeartAnimationPrefab, transform.position, Quaternion.identity) as GameObject;
                greenHeart.transform.SetParent(gameObject.transform);
                
            } else {
                yield return null;
            }
        }
    }
}
