using UnityEngine;
using F = Functions;

public class AdvancementsScript : MonoBehaviour {
    
    private MoneyAdministratorScript moneyAdmin;

    private PanellAdminScript panell;

    private void Start() {
        moneyAdmin = FindFirstObjectByType<MoneyAdministratorScript>();
        if (moneyAdmin == null)
        {
            Debug.LogError("MoneyAdministratorScript not found in the scene!");
        }
        panell = F.GetScriptComponentName<PanellAdminScript>("Panell Object");
        
    }

    private void FixedUpdate () {
        if (SpaceshipScript.IsGameOver) return;
        
        var (score, bills, coins, enemiesTakenDown, hitsOnEnemies) = moneyAdmin.GetTemporalInfo();
        // Only update enemies and hits, let PanellAdminScript handle score
        moneyAdmin.AddGameStats(0, MeteoriteScript.meteoritesDown - enemiesTakenDown, MeteoriteScript.meteoritesHits - hitsOnEnemies);
        moneyAdmin.AddBills(panell.Bills - bills);
        moneyAdmin.AddCoins(panell.Coins - coins);
    }
}
