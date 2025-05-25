using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using F = Functions;


public class extrasBehaviorSpacehip : MonoBehaviour {
     
    [SerializeField] private Sprite bulletExtraTexture;
    [SerializeField] private Sprite upSideDownExtraTexture;
    [SerializeField] private Sprite bulletRushExtraTexture;

    [SerializeField] private GameObject barPrefab;
    private float totalTime;

    private SpriteRenderer spriteRenderer;
    private SpaceshipScript spaceshipScript;


    private SpaceParent space;

    private void Awake() {

        spriteRenderer = GetComponent<SpriteRenderer>();
        spaceshipScript = GetComponent<SpaceshipScript>();
        space = F.GetScriptComponentName<SpaceParent>("Space - Parent");
    }

    private void Start() {
        totalTime = spaceshipScript.ExtrasCooldown;
    }

    private void FixedUpdate() {

        if (F.checkGameOver()) { spriteRenderer.sprite = null; }
        
        if (spaceshipScript.bulletUpgrade) {

            spriteRenderer.sprite = bulletExtraTexture;

            if (spaceshipScript.RecentlyGain) {

                spaceshipScript.resetRecentlyGain();
                StartCoroutine(ScaleDownBar(Color.yellow));
            }
        } else if (spaceshipScript.UpSideDown) {

            spriteRenderer.sprite = upSideDownExtraTexture;

            if (spaceshipScript.RecentlyGain) {

                spaceshipScript.resetRecentlyGain();
                StartCoroutine(ScaleDownBar(Color.green));
            }
        } else if (spaceshipScript.BulletRush) {

            spriteRenderer.sprite = bulletRushExtraTexture;

            if (spaceshipScript.RecentlyGain) {

                spaceshipScript.resetRecentlyGain();
                StartCoroutine(ScaleDownBar(Color.magenta));
            }
        } else {
            spriteRenderer.sprite = null;
        }
    }

    private IEnumerator ScaleDownBar(Color colorGiven) {

        GameObject barInstance = Instantiate(barPrefab, transform.position, Quaternion.identity) as GameObject;
        Vector3 scale = new Vector3 (GameObject.Find("Space - Parent").transform.localScale.x, 14f, 0f);

        barInstance.transform.localScale = scale;        
        barInstance.transform.position = new Vector3 (0f, space.End.y + scale.y * 1.5f, 0f);
        colorGiven.a = 0.5f;
        barInstance.GetComponent<SpriteRenderer>().color = colorGiven;

        float initialScaleX = scale.x;
        float elapsedTime = 0f;

        while (elapsedTime < totalTime) {
            elapsedTime += Time.deltaTime;
            scale.x = Mathf.Lerp(initialScaleX, 0, elapsedTime / totalTime);
            barInstance.transform.localScale = scale;
            yield return null;
        }

        Destroy(barInstance);
    }
}
