using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisilAdminScript : MonoBehaviour {

    [SerializeField] private GameObject misilPrefab;

    private int bulletStartingPointX = 11;
    private int bulletStartingPointY = 25;

    private void Start() {

        StartCoroutine(FireMisile());
    }

    private IEnumerator FireMisile() {

        yield return new WaitForSeconds(3);

        while (true) {

            generateMisile(bulletStartingPointX,bulletStartingPointY);
            generateMisile(-bulletStartingPointX,bulletStartingPointY);
            
            yield return new WaitForSeconds(20);
        }
    }

    private void generateMisile(float x, float y) {

        GameObject bullet = Instantiate(misilPrefab, transform.position, Quaternion.identity) as GameObject;
        bullet.transform.position = new Vector2 (transform.position.x + x * 2, transform.position.y + y * 2) ;
    }
    
}
