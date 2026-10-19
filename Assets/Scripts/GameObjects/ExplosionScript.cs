using UnityEngine;
using F = Functions;

public class ExplosionScript : MonoBehaviour {
    
    // -- IMPORTANT COMPONENTS -- //
    [SerializeField] private bool isHitMeteoriteAnimation;
    private Vector2 position;
    private float speed;
    private float rotationSpeed = 50;

    [SerializeField] private Sprite[] sprites;
    private SpriteRenderer spriteRenderer;
    private int explosionState;

    private UserInterfaceScript userInterface;

    private float cooldownTime;
    private OmegaCooldown cooldown;
    private bool playSoundDisabled = false;

    private void Awake() {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Start() {
        if (F.randomFloat(0,1) > 0.5) {
            rotationSpeed *= (-1);
        }

        cooldown = new OmegaCooldown(cooldownTime);
        spriteRenderer.sprite = sprites[explosionState];
        position = transform.position;

        // Only play explosion sound for actual explosions, not hit animations
        if (!isHitMeteoriteAnimation && !playSoundDisabled) {
            GameObject.Find("AudioManager").GetComponent<AudioManager>().PlayEffect("Explosion");
        }
    }

    public void DisableSound() {
        playSoundDisabled = true;
    }

    private void FixedUpdate() {
        explosionMovement();
        spriteAnimation();
    }

    private void explosionMovement() {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        position = F.direction(position, speed);
        transform.position = position;
    }

    private void spriteAnimation() {
        if (cooldown.coolDownOver()) {
            explosionState += 1;
            if (explosionState > sprites.Length - 1) {
                Destroy(gameObject);
            } else {
                spriteRenderer.sprite = sprites[explosionState];
            }
        }
    }

    public void getComponents(float givenSpeed, float givenCooldownTiem) {
        speed = givenSpeed;
        cooldownTime = givenCooldownTiem;
    }

    public void setDeathAnimation() {
        rotationSpeed = 0;
    }
}
