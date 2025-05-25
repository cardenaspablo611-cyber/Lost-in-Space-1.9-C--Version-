using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBombScript : MonoBehaviour {

    [SerializeField] private int[] sizeOrder;
    [SerializeField] private string nameOfExplosion;
    private int order = 0;

    private float cooldown = 0.1f;

    [SerializeField] private Sprite[] bombAnimationSprites;
    private SpriteRenderer spriteRenderer;

    private CircleCollider2D circleCollider;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Start() {
        StartCoroutine(AnimateSpriteWithCooldown());
    }

    private IEnumerator AnimateSpriteWithCooldown() {

        GameObject.Find("AudioManager").GetComponent<AudioManager>().PlayEffect(nameOfExplosion);

        foreach (Sprite currentState in bombAnimationSprites) {

            spriteRenderer.sprite = currentState;
            yield return new WaitForSeconds(cooldown); 

            if (order < sizeOrder.Length - 1) {
                order += 1;
            }

            circleCollider.radius = sizeOrder[order];
        }
        Destroy(gameObject);
    }

}
