using UnityEngine;
using System.Collections;

public class PlayerDataAdministratorScript : MonoBehaviour {

    [SerializeField] private PlayerData temporalAdvancements;
    [SerializeField] private PlayerData Advancements;

    [SerializeField] private TextMesh shadow;
    [SerializeField] private GameObject shadowParent;

    private Coroutine typingCoroutine;

    [SerializeField] private float typingSpeed = 0.5f;
    [SerializeField] private TextMesh disaplyText;

    private bool isTyping = false;

    private string AdvancementsText;

    private bool checkingForInterfiriences = false;

    private void Awake() {

        int enemiesTakenDown = temporalAdvancements.enemiesTakenDown;
        int hitsOnEnemies = temporalAdvancements.hitsOnEnemies;
        int score = temporalAdvancements.score;
        int bills = temporalAdvancements.bills;
        int coins = temporalAdvancements.coins;

        AdvancementsText = $"Coins: {coins}\nBills: {bills}\nScore: {score}\nEnemies Down: {enemiesTakenDown}\nHits On Enemies: {hitsOnEnemies}";

        Advancements.AddDataToPlayerData(temporalAdvancements);
        temporalAdvancements.resetStaticVariables();
    }   

    private void Start() {

        disaplyText.text = "";

        shadow.text = AdvancementsText;

        shadowParent.SetActive(false);

        typingCoroutine = StartCoroutine(TypeText());
        StartCoroutine(CheckIfTyping());
    }

    private void FixedUpdate() {

        if (checkingForInterfiriences) {

            if (Input.anyKey && isTyping) {

                StopCoroutine(typingCoroutine);

                disaplyText.text = AdvancementsText;

                isTyping = false;

                shadowParent.SetActive(true);
            }
        }
        
    } 

    IEnumerator TypeText() {
        isTyping = true;

        foreach (char c in AdvancementsText) {
        
            disaplyText.text += c;

            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;

        shadowParent.SetActive(true);
    }

    IEnumerator CheckIfTyping() {

        yield return new WaitForSeconds(0.5f);

        checkingForInterfiriences = true;
    }

}
