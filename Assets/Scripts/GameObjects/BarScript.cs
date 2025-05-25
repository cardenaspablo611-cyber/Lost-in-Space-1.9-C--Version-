using UnityEngine;
using F = Functions;

public class BarScript : MonoBehaviour {

    private Vector2 position;

    private float speed;

    private Vector2 scale;

    private float cooldownTime = 1;
    private OmegaCooldown cooldown;

    private float time;

    private float factor = 50f;

    private SpaceParent space;

    private void Start () {

        space = F.GetScriptComponentName<SpaceParent>("Space - Parent");

        scale.x = GameObject.Find("Space - Parent").transform.localScale.x;
        scale.y = 14f;
        transform.localScale = scale;        

        speed = scale.x / time / factor;

        position.y = space.End.y + scale.y / 2;
        transform.position = position;

        cooldownTime = cooldownTime / factor;
        cooldown = new OmegaCooldown(cooldownTime);        
    }

    private void FixedUpdate () {

        if (Functions.checkGameOver()) { Destroy(gameObject); }

        spriteAnimation();
    }

    private void spriteAnimation() {

        if (cooldown.coolDownOver()) {

            scale.x -= speed;

            transform.localScale = scale; 

            if (scale.x <= 0) {

                Destroy(gameObject);
            }
        }
    }

    public void setTimer(float setTime) {

        time = setTime;
    }
}
