using UnityEngine;
using F = Functions;

public class MoneyScript : MonoBehaviour {

    private Vector2 position;

    private float speed;

    private float moneyFrame = 10;

    [SerializeField] private Sprite[] sprites;
    private SpriteRenderer spriteRenderer;
    private int moneyState = 0;

    private PanellAdminScript panell;
    private SpaceParent space;

    private float cooldownTime;
    private OmegaCooldown cooldown;

    [SerializeField] private int price;
    public int Price { get { return price;}}

    private void Awake() {

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        space = F.GetScriptComponentName<SpaceParent>("Space - Parent");
        panell = F.GetScriptComponentName<PanellAdminScript>("Panell Object");  
    }

    private void Start() {

        spriteRenderer.sprite = sprites[moneyState];

        cooldown = new OmegaCooldown(cooldownTime);

        position = transform.position;
    }

    private void FixedUpdate() {


        moneyMovement();

        checkOfLimit();

        spriteAnimation();
    }

    private void OnCollisionEnter2D(Collision2D collision) {

        if (collision.gameObject.CompareTag("Spaceship")) {
            MoneyLogger.Log($"[MoneyScript] Money collected: {price}");
            panell.addMoney(price);
            MoneyLogger.Log($"[MoneyScript] Called panell.addMoney({price})");
            Destroy(gameObject);
        }
    }
    private void moneyMovement() {

        position = F.direction(position, speed);
        transform.position = position;
    }

    private void checkOfLimit() {

        if (position.y < space.End.y - moneyFrame * 2) {
            
            Destroy(gameObject);
        }

    }

    private void spriteAnimation() {

        if (cooldown.coolDownOver()) {

            moneyState += 1;

            if (moneyState > sprites.Length - 1) {
                
                moneyState = 0;
                
            } else {

                spriteRenderer.sprite = sprites[moneyState];
            }
        }
    }

    public void getComponents(float givenSpeed, float givenCooldownTiem) {
        speed = givenSpeed;
        cooldownTime = givenCooldownTiem;
    }
}

