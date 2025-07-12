using UnityEngine;

public class AnimationScript : MonoBehaviour {

    [SerializeField] private Sprite[] sprites;
    private SpriteRenderer spriteRenderer;

    private int animationState;

    [SerializeField] private float cooldownTime;
    private float originalCooldown;
    private Cooldown cooldown;

    [SerializeField] private bool loop;
    [SerializeField] private bool inShopMenu;
    [SerializeField] private bool autoDestroy;
    private float totalDurationTime; 
    public float TotalDurationTime {get {return totalDurationTime;}}

    [SerializeField] private bool realTime;

    private SpaceshipsShopAnimationScript SpaceshipAnimationInstance;

    private void Awake() {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[animationState];


        cooldown = new Cooldown(cooldownTime);

        if (inShopMenu) {

            SpaceshipAnimationInstance = GameObject.Find("Spaceship Animations").GetComponent<SpaceshipsShopAnimationScript>();
        }

        totalDurationTime = cooldownTime * (sprites.Length - 1);
    }

    private void Start() {
        originalCooldown = cooldownTime;
    }

    private void FixedUpdate() {

        spriteAnimation();

        if (cooldownTime != originalCooldown) {
            originalCooldown = cooldownTime;
            cooldown = new Cooldown(cooldownTime);
        }
    }

    private void spriteAnimation() {

        if (GetComponent<Animation>()) {
            if (inShopMenu) {
                SpaceshipAnimationInstance.changeFalse();
            }
            if (cooldown.coolDownOver()) {

                
                if (animationState < sprites.Length - 1) {
                    
                    animationState += 1;
                } else {
                    
                    if (!loop) {

                        animationState = sprites.Length - 1;

                        

                    } else {

                        animationState = 0;
                    }
                }

                spriteRenderer.sprite = sprites[animationState];
            }
        } else if (inShopMenu) {
            SpaceshipAnimationInstance.changeTrue();
            
        } else if (autoDestroy) {
            Destroy(gameObject);
        }

        if (loop) {
        } 
    }

    public void restartAnimation() {
        animationState = 0;
    }
}
