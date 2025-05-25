using UnityEngine;
using F = Functions;

public class AdvancementsScript : MonoBehaviour {
    
    [SerializeField] private PlayerData temporalAdvancements;

    private PanellAdminScript panell;

    private void Awake() {

        panell = F.GetScriptComponentName<PanellAdminScript>("Panell Object");
    }

    private void FixedUpdate () {

        temporalAdvancements.score = panell.Score;
        temporalAdvancements.bills = panell.Bills;
        temporalAdvancements.coins = panell.Coins;

        temporalAdvancements.enemiesTakenDown = MeteoriteScript.meteoritesDown;
        temporalAdvancements.hitsOnEnemies = MeteoriteScript.meteoritesHits;
    }
}
