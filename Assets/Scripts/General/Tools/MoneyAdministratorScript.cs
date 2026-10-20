using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class GameData {
    public int Score;
    public int Bills;
    public int Coins;
    public int EnemiesTakenDown;
    public int HitsOnEnemies;
}

public class MoneyAdministratorScript : MonoBehaviour {
    // Session-only storage (resets when game is closed/restarted)
    // This replaces File IO to support WebGL and ensures data only persists during play session
    private static GameData temporalData = new GameData();
    private static GameData generalData = new GameData();

    [SerializeField] private GameObject scoreTextPrefab;
    [HideInInspector] public GameObject ScoreTextPrefab { get { return scoreTextPrefab;}}

    /// <summary>
    /// Called by SessionStateManager at game launch to reset all money data.
    /// Ensures Editor and builds start with clean state.
    /// </summary>
    public static void ResetSessionData() {
        temporalData = new GameData();
        generalData = new GameData();
    }

    private void SaveGameData(GameData data, bool isTemporal) {
        // In-memory save - no file IO needed for session persistence
        if (isTemporal) {
            temporalData = data;
        } else {
            generalData = data;
        }
    }

    private GameData LoadGameData(bool isTemporal) {
        if (isTemporal) {
            return temporalData;
        } else {
            return generalData;
        }
    }

    public void AddCoins(int amount) { // For General and Temporal Money Data
        GameData tempData = LoadGameData(true);
        tempData.Coins += amount;
        SaveGameData(tempData, true);

        GameData genData = LoadGameData(false);
        genData.Coins += amount;
        SaveGameData(genData, false);
    }

    public void AddBills(int amount) { // For General and Temporal Money Data
        GameData tempData = LoadGameData(true);
        tempData.Bills += amount;
        SaveGameData(tempData, true);
        
        GameData genData = LoadGameData(false);
        genData.Bills += amount;
        SaveGameData(genData, false);
    }

    public void SubtractCoins(int amount) { // Only for the General Money Data
        GameData genData = LoadGameData(false);
        if (genData.Coins <= 0) return;
        genData.Coins = Mathf.Max(0, genData.Coins - amount);
        SaveGameData(genData, false);
    }

    public void SubtractBills(int amount) { // Only for the General Money Data
        GameData genData = LoadGameData(false);
        if (genData.Bills <= 0) return;
        genData.Bills = Mathf.Max(0, genData.Bills - amount);
        SaveGameData(genData, false);
    }
    
    public void AddGameStats(int score, int enemiesTakenDown, int hitsOnEnemies) {

        if (score != 0) {
            GameObject spaceship = GameObject.FindWithTag("Spaceship");
            MoneyAdministratorScript moneyAdmin = spaceship.GetComponent<MoneyAdministratorScript>();
            var spaceshipScript = spaceship.GetComponent<SpaceshipScript>();
            int lives = spaceshipScript != null ? spaceshipScript.Lives : 1;
            int maxLives = spaceshipScript != null ? spaceshipScript.MaxLives : 4;

            float lifeRatio = Mathf.Clamp01((float)lives / maxLives);

            Color baseColor = score < 0 ? Color.red : Color.green;
            Color textColor = Color.Lerp(Color.black, baseColor, lifeRatio);

            GameObject scoreText = Instantiate(moneyAdmin.ScoreTextPrefab, Vector3.zero, Quaternion.identity);
            TextMesh textMesh = scoreText.GetComponentInChildren<TextMesh>();

            textMesh.text = score < 0 ? score.ToString() : "+" + score.ToString();
            textMesh.color = textColor;
        }

        GameData tempData = LoadGameData(true);
        tempData.Score += score;
        tempData.EnemiesTakenDown += enemiesTakenDown;
        tempData.HitsOnEnemies += hitsOnEnemies;
        SaveGameData(tempData, true);
        
        GameData genData = LoadGameData(false);
        genData.Score += score;
        genData.EnemiesTakenDown += enemiesTakenDown;
        genData.HitsOnEnemies += hitsOnEnemies;
        SaveGameData(genData, false);
    }
    
    public void ResetTemporal() {
        GameData data = new GameData();
        SaveGameData(data, true);
    }
    
    public void ResetGeneral() {
        GameData data = new GameData();
        SaveGameData(data, false);
    }
    
    public (int Score, int Bills, int Coins, int EnemiesTakenDown, int HitsOnEnemies) GetTemporalInfo() {
        GameData data = LoadGameData(true);
        return (data.Score, data.Bills, data.Coins, data.EnemiesTakenDown, data.HitsOnEnemies);
    }
    
    public (int Score, int Bills, int Coins, int EnemiesTakenDown, int HitsOnEnemies) GetGeneralInfo() {
        GameData data = LoadGameData(false);
        return (data.Score, data.Bills, data.Coins, data.EnemiesTakenDown, data.HitsOnEnemies);
    }

    
}