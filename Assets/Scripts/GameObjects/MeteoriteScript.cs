using UnityEngine;
using F = Functions;

public class MeteoriteScript : MonoBehaviour {


    private Vector2 position;

    private float speed;
    private float minSpeed = 9;
    private float maxSpeed = 6;

    [SerializeField] private float width;
    [SerializeField] private float heigth;
    private Collider2D myCollider;

    [SerializeField] private GameObject deathAnimationPrefab;
    [SerializeField] private Sprite[] sprites;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private bool inmortal;
    private int lives;
    private int livesFactor;

    private float rotationSpeed;
    private float minRotationSpeed = 20;
    private float maxRotationSpeed = 175;

    public static int meteoritesDown;
    public static int meteoritesHits;
    private bool resetMeteoritesDataOnPlayer = true;
    private static int lastUsedSegment = -1;

    private SpaceParent space;
    private SpaceshipScript spaceship;
    private AdminScript admin;
    private PanellAdminScript panell;

    private bool destroy = false;
    private bool destroyArtificial = false;

    private int amountPerMeteoriteDown = 10;

    [SerializeField] private bool withSpeed = true;
    [SerializeField] private bool withRotation;
    [SerializeField] private SpaceshipInUseScript spaceshipInUse;    

    private bool launchingItIntoGame = false;

    private void Awake() {

        spriteRenderer = GetComponent<SpriteRenderer>();

        myCollider = GetComponent<Collider2D>();

        if (myCollider is CircleCollider2D circleCollider) {
                
            heigth = circleCollider.radius;
            width = circleCollider.radius;

        } else if (myCollider is CapsuleCollider2D boxCollider) {
            
            Vector3 scale = transform.localScale;

            width = boxCollider.size.x * scale.x; // The size is is like a radius but one axis. So it has to be multipy bu to to get the length or the height
            heigth = boxCollider.size.y * scale.y;
        }

        getNecesaryScripts();
    }

    private void Start() {

        createPropeties();
    }

    private void FixedUpdate() {
        spriteAnimation();
        meteoriteMovement();
        checkOfLimit();

        if (destroy) {
            destroyMeteorite();
        } else if (destroyArtificial) {
            destroyArtificialDeatdh();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!inmortal) {
            if (collision.gameObject.CompareTag("Bullet")) {
                meteoritesHits += 1;

                if (lives != 1 && F.randomFloatFromCero(1) < spaceship.DamageProbability) {
                    lives -= 1;
                    meteoritesHits += 1;
                }

                if (lives == 1) {
                    // Final hit - will trigger explosion sound through ExplosionScript

                    
                    meteoritesDown += 1;
                    panell.addScore(amountPerMeteoriteDown);
                    destroyArtificial = true;
                } else {
                    // Just damage - play hit sound
                    GameObject.Find("AudioManager").GetComponent<AudioManager>().PlayEffect("HitOnEnemy");
                    lives -= 1;   
                }
            }

            if (collision.gameObject.CompareTag("Misil")) {
                hitByUlty();
            }

            if (collision.gameObject.CompareTag("ExplosionBomb")) {
                hitByUlty();
            }

            if (collision.gameObject.CompareTag("Bomb")) {
                hitByUlty();
            }
        }

        if (collision.gameObject.CompareTag("Spaceship") && !spaceship.Immunity) {
            spaceship.gotHitten();
            destroyArtificial = true;
        }
    }

    // -- STARTING COMPONENTS -- // 
    private void getNecesaryScripts() {

        Transform parentTransform = GameObject.Find($"{spaceshipInUse.tagOfUsedSpaceship}(Clone)").transform;
        Transform child = parentTransform.GetChild(0);

        space = F.GetScriptComponentName<SpaceParent>("Space - Parent");
        spaceship = child.GetComponent<SpaceshipScript>(); 
        admin = F.GetScriptComponentName<AdminScript>("Admin");
        panell = F.GetScriptComponentName<PanellAdminScript>("Panell Object");

    }

    private void createPropeties() {
        
        position.x = F.randomFloat(space.Start.x, space.End.x);
        if (launchingItIntoGame) {

            if (resetMeteoritesDataOnPlayer) {
                meteoritesDown = 0;
                meteoritesHits = 0;
                resetMeteoritesDataOnPlayer = false;
            }

            float startingYPosition = space.Start.y + heigth * 2;
            float endingYPosition = space.Start.y * 5 + heigth * 2;

            float segmentHeight = 300f;
            
            int numSegments = Mathf.FloorToInt((endingYPosition - startingYPosition) / segmentHeight);
            
            lastUsedSegment = (lastUsedSegment + 1) % numSegments;
            
            float segmentStart = startingYPosition + (lastUsedSegment * segmentHeight);
            float segmentEnd = segmentStart + segmentHeight;
            
            position.y = F.randomFloat(segmentStart, segmentEnd);

        } else {
            position.y = space.Start.y + heigth * 2;
        }

        transform.position = position;
        speed = F.randomFloat(minSpeed,maxSpeed);

        rotationSpeed = F.randomFloat(minRotationSpeed, maxRotationSpeed);

        if (F.randomFloat(0,1) > 0.5) {
            rotationSpeed *= (-1);
        }

        lives = sprites.Length;
    }

    // -- METEORITES COMPONENTS -- //

    private void meteoriteMovement() {

        if (withRotation) {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

        }
        
        if (withSpeed) {
            position = F.direction(position, speed);
            transform.position = position;
        }
    }

    private void spriteAnimation() {
        
        livesFactor = -1 * lives + sprites.Length;
        spriteRenderer.sprite = sprites[livesFactor];
    }
    
    private void checkOfLimit() {

        if (position.y < space.End.y - heigth) {
            destroy = true;
        } 
        
        if (position.x < space.Start.x - width) {
            lateralReset();
        } else if (position.x > space.End.x + width) {
            lateralReset();
        }
    }

    private void lateralReset() {
        if (position.x > space.End.x) {
            position.x = space.Start.x + width / 2;

        } else if (position.x < space.Start.x) {
            position.x = space.End.x - width / 2;
        }
        transform.position = position;
    }

    private void linearReset() {
        createPropeties();
    }

    // -- CREATE -- //
    private void createExplosionObject() {

        admin.instantiateExplosionObject( new Vector2 (position.x, position.y), deathAnimationPrefab);
        
    }

    private void createMoneyObject() {
        
        if (F.randomFloatFromCero(1) > 0.4) { 

            admin.instantiateMoneyObject( new Vector2 (position.x, position.y) );
        }
    }

    private void destroyMeteorite() {

        GameObject.Find("PlanetsAdmin").GetComponent<PlanetsAdminScript>().regenerateMeteoritesAmount();

        Destroy(gameObject);
    }

    private void destroyArtificialDeatdh() {

        GameObject.Find("PlanetsAdmin").GetComponent<PlanetsAdminScript>().regenerateMeteoritesAmount();

        Destroy(gameObject);

        createExplosionObject();
        
        if (!inmortal) {
            createMoneyObject();
        }
    }

    public void destroyExesMeteorite() {
        destroyArtificialDeatdh();
    }

    public GameObject GetDeathAnimationPrefab() {
        return deathAnimationPrefab;
    }

    private void hitByUlty() {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().PlayEffect("HitOnEnemy");
        meteoritesHits += 1;
        meteoritesDown += 1;

        panell.addScore(amountPerMeteoriteDown);
        destroyArtificial = true;
    }

    public void gameIsBeingExecuted() {
        launchingItIntoGame = true;
    }
}
