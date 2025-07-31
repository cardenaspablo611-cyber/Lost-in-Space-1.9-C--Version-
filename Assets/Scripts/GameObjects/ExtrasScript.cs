using UnityEngine;
using F = Functions;

public class ExtrasScript : MonoBehaviour {

    private Vector2 position;

    private float speed;

    private float extrasFrame = 20;

    [SerializeField] private Sprite[] sprites;
    private SpriteRenderer spriteRenderer;
    private int extrasState = 0;

    [SerializeField] private new string name;

    private SpaceshipScript spaceship;
    private SpaceParent space;

    private float cooldownTime;
    private OmegaCooldown cooldown;

    [SerializeField] private SpaceshipInUseScript spaceshipInUse;

    private void Awake() {

        Transform parentTransform = GameObject.Find($"{spaceshipInUse.tagOfUsedSpaceship}(Clone)").transform;
        Transform child = parentTransform.GetChild(0);

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        space = F.GetScriptComponentName<SpaceParent>("Space - Parent");
        spaceship = child.GetComponent<SpaceshipScript>();
    }

    private void Start() {

        spriteRenderer.sprite = sprites[extrasState];

        cooldown = new OmegaCooldown(cooldownTime);

        position.x = F.randomFloat(space.Start.x + extrasFrame * 3,space.End.x - extrasFrame * 3);
        position.y = space.Start.y + extrasFrame;

        transform.position = position;
    }

    private void FixedUpdate() {



        extrasMovement();

        checkOfLimit();

        spriteAnimation();
    }

    private void OnCollisionEnter2D(Collision2D collision) {

        if (collision.gameObject.CompareTag("Spaceship")) {


            GameObject.Find("AudioManager").GetComponent<AudioManager>().PlayEffect("Collision Power Ups");
            
            spaceship.getExtras(name);

            Destroy(gameObject);
        }
    }
    private void extrasMovement() {

        position = F.direction(position, speed);
        transform.position = position;
    }

    private void checkOfLimit() {

        if (position.y < space.End.y - extrasFrame * 2) {
            
            Destroy(gameObject);
        }

    }

    private void spriteAnimation() {

        if (cooldown.coolDownOver()) {

            extrasState += 1;

            if (extrasState > sprites.Length - 1) {
                
                extrasState = 0;
                
            } else {

                spriteRenderer.sprite = sprites[extrasState];
            }
        }
    }

    public void getComponents(float givenSpeed, float givenCooldownTiem) {
        speed = givenSpeed;
        cooldownTime = givenCooldownTiem;
    }
}