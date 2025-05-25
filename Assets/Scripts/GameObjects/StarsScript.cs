using UnityEngine;
using F = Functions;

public class StarsScript : MonoBehaviour { 

    private Vector2 position;

    private float speed;

    private float starsFrame = 10;

    private float max = 7;
    private float min = 1;

    private SpaceParent space;

    private void Awake() {
        space = F.GetScriptComponentName<SpaceParent>("Space - Parent");
    }

    private void Start() {

        float radius = F.randomFloat(min,max);
        transform.localScale = new Vector2 (radius,radius); 

        speed = F.randomFloat(min,max);

        position.x = F.randomFloat(space.Start.x - starsFrame,space.End.x + starsFrame);
        position.y = F.randomFloat(space.Start.y + starsFrame,space.End.y - starsFrame);
        
        transform.position = position;
    }

    private void FixedUpdate() {

        //if (F.checkGameOver()) { Destroy(gameObject); }

        starsMovement();

        checkOfLimit();
    }

    private void starsMovement() {

        position = F.direction(position, speed);
        transform.position = position;
    }

    private void checkOfLimit() {

        if (transform.position.y < space.End.y - starsFrame * 2) {
            linearReset(); 
        } 
        
        if (transform.position.x < space.Start.x - starsFrame * 2) {
            lateralReset();
        } else if (transform.position.x > space.End.x + starsFrame * 2) {
            lateralReset();
        }
    }

    private void linearReset() {   

        float radius = F.randomFloat(min,max);
        transform.localScale = new Vector2 (radius,radius); 

        speed = F.randomFloat(min,max);

        position.x = F.randomFloat(space.Start.x - starsFrame,space.End.x + starsFrame);
        position.y = space.Start.y + starsFrame;
        
        transform.position = position;
    }

    private void lateralReset() {

        position.x *= -1;
        transform.position = position;
    }
}