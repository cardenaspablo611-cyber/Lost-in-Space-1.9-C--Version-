using UnityEngine;
using F = Functions;

public class ShopPanellAdminScript : MonoBehaviour {

    [SerializeField] private GameObject billPanell;
    [SerializeField] private GameObject coinPanell;

    [SerializeField] private PlayerData Advancements;

    private TextMesh billText;
    private TextMesh coinText;

    private int bills;
    private int coins;

    private void Awake() {

        billText = billPanell.GetComponentInChildren<TextMesh>();
        coinText = coinPanell.GetComponentInChildren<TextMesh>();
    }

    private void Update() {

        billText.text = Advancements.bills.ToString();
        coinText.text = Advancements.coins.ToString();
        MoneyLogger.Log($"[ShopPanellAdminScript] Update: bills={Advancements.bills}, coins={Advancements.coins}");
    }
    
}

