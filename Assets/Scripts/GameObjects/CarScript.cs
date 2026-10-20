using UnityEngine;
using F = Functions;

public class CarScript : MonoBehaviour {

    private Vector2 position;

    private float speed;
    private bool inMotion = true;
    private bool createCooldownBool;

    private float length;

    private SpriteRenderer sprite;

    private SpaceParent space;

    private OmegaCooldown cooldown;
    
    private void Awake() {
        sprite = GetComponent<SpriteRenderer>();

        space = F.GetScriptComponentName<SpaceParent>("Space - Parent");
    }

    private void Start () {
        Vector3 spriteSize = sprite.bounds.size;
        length = spriteSize.x;

        position = transform.position;
        speed = F.randomFloat(3,8);
    }

    private void FixedUpdate() {

        carMovement();
        checkOfLimit();
    }

    private void carMovement() {

        if (inMotion) {
            position.x += speed * Time.deltaTime;
            transform.position = position;
        }        
    }
    
    private void checkOfLimit() {

        if (transform.position.x > space.End.x + length / 2) {

            inMotion = false;
            createCooldownBool = true;

            position.x = space.Start.x - length / 2;
            transform.position = position;
        } 

        if (!inMotion) {
            if (createCooldownBool) {
                
                float cooldownTime = F.randomFloat(1,7);
                speed = F.randomFloat(3,8);

                cooldown = new OmegaCooldown(cooldownTime);
                createCooldownBool = false;
            }

            if (cooldown.coolDownOver()) {
                inMotion = true;
            }
        }
    }
}
