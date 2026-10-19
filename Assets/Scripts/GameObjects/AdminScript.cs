using UnityEngine;
using F = Functions;

public class AdminScript : MonoBehaviour {
    
    [Header("-- Stars --")]
    [SerializeField] private GameObject[] starsPrefabs;
    [SerializeField] private GameObject starsWave;
    [SerializeField] private int starsAmount;

    [Header("-- Explosion --")]
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject explosionParent;
    
    [Header("-- Taking Damage --")]
    [SerializeField] private GameObject takeDamagePrefab;
    [SerializeField] private GameObject takeDamageParent;

    [Header("-- Money --")]
    [SerializeField] private GameObject billPrefab;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject moneyParent; 

    [Header("-- Extras --")]
    [SerializeField] private GameObject shieldExtraPrefab;
    [SerializeField] private GameObject bulletsExtraPrefab;
    [SerializeField] private GameObject heartsExtraPrefab;
    [SerializeField] private GameObject upSideDownExtraPrefab;
    [SerializeField] private GameObject bulletRushExtraPrefab;
    [SerializeField] private GameObject extrasParent;

    private float extrasCooldownTime = 15f;
    private OmegaCooldown extrasCooldown;

    [Header("-- Money & Explosion - Components --")]
    [SerializeField] private float speed;
    [SerializeField] private float deathAnimationCooldownTime;
    [SerializeField] private float explosionCooldownTime; // The deafault cooldowntime for most of the things in the game

    [SerializeField] private SpaceshipInUseScript spaceshipInUse;


    private SpaceshipScript spaceship;
    private int spaceshipLives;

    private void Start() {

        Transform parentTransform = GameObject.Find($"{spaceshipInUse.tagOfUsedSpaceship}(Clone)").transform;
        Transform child = parentTransform.GetChild(0);


        spaceship = child.GetComponent<SpaceshipScript>(); 
        spaceshipLives = child.GetComponent<SpaceshipScript>().Lives; 


        extrasCooldown = new OmegaCooldown(extrasCooldownTime);

        for (int i = 0; i < starsAmount; i++) {

            int randomIndex = F.randomIntFromCero(starsPrefabs.Length - 1);

            GameObject stars = Instantiate(starsPrefabs[randomIndex]) as GameObject;
            stars.transform.SetParent(starsWave.transform);
        }  
    }

    private void FixedUpdate() {

        if (SpaceshipScript.IsGameOver) return;

        if (extrasCooldown.coolDownOver()) { 

            GameObject extra;

            if (spaceship.Lives == spaceshipLives && spaceship.Shield) {

                extra = Instantiate(F.ChooseRandom(bulletsExtraPrefab, bulletRushExtraPrefab));
                
            } else if (spaceship.Lives == spaceshipLives) {
                
                extra = Instantiate(F.ChooseRandom(shieldExtraPrefab, bulletsExtraPrefab, upSideDownExtraPrefab, bulletRushExtraPrefab));
            
            } else if  (spaceship.Shield){

                extra = Instantiate(F.ChooseRandom(heartsExtraPrefab, bulletsExtraPrefab, upSideDownExtraPrefab, bulletRushExtraPrefab));

            } else {

                extra = Instantiate(F.ChooseRandom(heartsExtraPrefab, bulletsExtraPrefab, shieldExtraPrefab, upSideDownExtraPrefab, bulletRushExtraPrefab));
            }


            extra.transform.SetParent(extrasParent.transform);

            F.GetScriptComponentGameObject<ExtrasScript>(extra).getComponents(speed, explosionCooldownTime);
        }
    }

    // -- Instantiate Objects -- //
    

    public void instantiateExplosionObject(Vector2 newPosition, GameObject deathAnimationPrefab) {

        if (deathAnimationPrefab == null) {

            GameObject explosion = Instantiate(explosionPrefab) as GameObject;
            explosion.transform.SetParent(explosionParent.transform);
            explosion.transform.position = newPosition;

            F.GetScriptComponentGameObject<ExplosionScript>(explosion).getComponents(speed, explosionCooldownTime);
        } else {

            GameObject deathAnimation = Instantiate(deathAnimationPrefab) as GameObject;
            deathAnimation.transform.SetParent(explosionParent.transform);
            deathAnimation.transform.position = newPosition;

            F.GetScriptComponentGameObject<ExplosionScript>(deathAnimation).getComponents(speed, deathAnimationCooldownTime);
            F.GetScriptComponentGameObject<ExplosionScript>(deathAnimation).setDeathAnimation();
        }
    }
    
    public void instantiateMoneyObject(Vector2 newPosition) {

        GameObject moneyPrefab;

        if (F.randomFloatFromCero(1) > 0.9) {
            moneyPrefab = billPrefab;
        } else {
            moneyPrefab = coinPrefab;
        }

        GameObject money = Instantiate(moneyPrefab) as GameObject;
        money.transform.SetParent(moneyParent.transform);
        money.transform.position = newPosition;

        F.GetScriptComponentGameObject<MoneyScript>(money).getComponents(speed, explosionCooldownTime);
    }

    public void instantiateTakeDamageObject() {

        GameObject takeDamage = Instantiate(takeDamagePrefab) as GameObject;
        takeDamage.transform.SetParent(takeDamageParent.transform);
    }
}
