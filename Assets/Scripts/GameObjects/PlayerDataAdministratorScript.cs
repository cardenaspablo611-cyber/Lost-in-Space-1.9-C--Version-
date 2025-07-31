using UnityEngine;
using System.Collections;

public class PlayerDataAdministratorScript : MonoBehaviour {

    [SerializeField] private TextMesh shadow;
    [SerializeField] private GameObject shadowParent;

    private Coroutine typingCoroutine;

    [SerializeField] private float typingSpeed = 0.5f;
    [SerializeField] private TextMesh disaplyText;

    private bool isTyping = false;

    private string AdvancementsText;

    private bool checkingForInterfiriences = false;

    private MoneyAdministratorScript moneyAdmin;

    private void Awake() {
        moneyAdmin = FindFirstObjectByType<MoneyAdministratorScript>();
        if (moneyAdmin == null)
        {
            Debug.LogError("MoneyAdministratorScript not found in the scene!");
        }

        var (score, bills, coins, enemiesTakenDown, hitsOnEnemies) = moneyAdmin.GetTemporalInfo();
        MoneyLogger.Log($"[PlayerDataAdministratorScript] Awake: temporalAdvancements: score={score}, bills={bills}, coins={coins}");

        AdvancementsText = $"Coins: {coins}\nBills: {bills}\nScore: {score}\nEnemies Down: {enemiesTakenDown}\nHits On Enemies: {hitsOnEnemies}";

        moneyAdmin.ResetTemporal();
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
