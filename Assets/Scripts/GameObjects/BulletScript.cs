using UnityEngine;
using F = Functions;

public class BulletScript : MonoBehaviour { 

    private Vector2 position;

    [SerializeField] private float speed = 12f;
    [SerializeField] private bool oneUse = true;
    private float lateralMovement = 0f;

    [SerializeField] private GameObject hitOnEnemyAnimation;

    private float bulletFrame = 10f;

    private SpaceParent space;

    private void Start() {

        space = F.GetScriptComponentName<SpaceParent>("Space - Parent");
        position = transform.position;
    }

    private void FixedUpdate() {

        //if (F.checkGameOver()) { Destroy(gameObject); }

        bulletMovement();
        checkOfLimit();
    }

    private void OnCollisionEnter2D(Collision2D collision) {

        if (collision.gameObject.CompareTag("Meteorite")) {
            
            if (oneUse) {
                Destroy(gameObject);
            }

            GameObject hitAnimation = Instantiate(hitOnEnemyAnimation) as GameObject;
            hitAnimation.transform.position = transform.position;
        }
    }

    private void bulletMovement() {

        if (lateralMovement != 0) {
            position.x += speed * lateralMovement * Time.deltaTime;
        }
        
        position.y += speed * Time.deltaTime;
        transform.position = position;
    }

    private void checkOfLimit() {

        if (position.y > space.Start.y + bulletFrame * 2) {
            Destroy(gameObject);
        } 
    }

    public void setLateralMovement(float movement) {
        lateralMovement = movement;
    }
}
