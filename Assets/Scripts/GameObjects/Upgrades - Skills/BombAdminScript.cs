using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAdminScript : MonoBehaviour {
    
    [SerializeField] private GameObject bombPrefab;

    private void Start() {

        StartCoroutine(fireBomb());
    }

    private IEnumerator fireBomb() {

        yield return new WaitForSeconds(3);

        while (true) {

            SpaceshipScript spaceship = GetComponent<SpaceshipScript>();
            if (spaceship != null && !spaceship.Deadth) {
                Instantiate(bombPrefab, transform.position, Quaternion.identity);
            }
            
            yield return new WaitForSeconds(35);
        }
    }
}
