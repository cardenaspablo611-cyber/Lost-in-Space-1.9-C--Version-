using UnityEngine;
using F = Functions;

public class ShopPanellAdminScript : MonoBehaviour {

    [SerializeField] private GameObject billPanell;
    [SerializeField] private GameObject coinPanell;

    private MoneyAdministratorScript moneyAdmin;

    private TextMesh billText;
    private TextMesh coinText;

    private int bills;
    private int coins;

    private void Awake() {

        billText = billPanell.GetComponentInChildren<TextMesh>();
        coinText = coinPanell.GetComponentInChildren<TextMesh>();
        moneyAdmin = FindFirstObjectByType<MoneyAdministratorScript>();
        if (moneyAdmin == null)
        {
            Debug.LogError("MoneyAdministratorScript not found in the scene!");
        }
    }

    private void Update() {
        var (_, bills, coins, _, _) = moneyAdmin.GetGeneralInfo();
        billText.text = bills.ToString();
        coinText.text = coins.ToString();
        MoneyLogger.Log($"[ShopPanellAdminScript] Update: bills={bills}, coins={coins}");
    }
    
}

