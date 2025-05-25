using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using F = Functions;

public class BombSript : MonoBehaviour {
    
    [SerializeField] private GameObject ExplosionBombPrefab;
    private SpaceParent space;
    
    private void Awake() {
        space = F.GetScriptComponentName<SpaceParent>("Space - Parent");
    }

    private void FixedUpdate() {



        bombMovement();

        checkOfLimit();
    }

    private void bombMovement() {

        float speed = 1.5f;

        Vector2 position = F.direction(transform.position, speed);
        transform.position = position;
    }

    private void checkOfLimit() {

        if (transform.position.y < space.End.y - 10 * 2) {
            Destroy(gameObject);
        } 
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
