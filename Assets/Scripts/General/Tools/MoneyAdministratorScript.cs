using System.Collections.Generic;
using UnityEngine;
using System.IO;
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
    private string temporalPath;
    private string generalPath;

    [SerializeField] private GameObject scoreTextPrefab;
    [HideInInspector] public GameObject ScoreTextPrefab { get { return scoreTextPrefab;}}


    private void Awake() {

        temporalPath = Path.Combine(System.Environment.CurrentDirectory, "Temporal.json");
        generalPath = Path.Combine(System.Environment.CurrentDirectory, "General.json");
        InitializeFiles();
    }

    private void InitializeFiles() {
        if (!File.Exists(temporalPath)) {
            GameData temporalData = new GameData();
            SaveGameData(temporalData, temporalPath);
        }
        if (!File.Exists(generalPath)) {
            GameData generalData = new GameData();
            SaveGameData(generalData, generalPath);
        }
    }

    private void SaveGameData(GameData data, string filePath) {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
    }

    private GameData LoadGameData(string filePath) {
        if (File.Exists(filePath)) {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<GameData>(json);
        }
        return new GameData();
    }

    public void AddCoins(int amount) { // For General and Temporal Money Data
        GameData temporalData = LoadGameData(temporalPath);
        temporalData.Coins += amount;
        SaveGameData(temporalData, temporalPath);

        GameData generalData = LoadGameData(generalPath);
        generalData.Coins += amount;
        SaveGameData(generalData, generalPath);
    }

    public void AddBills(int amount) { // For General and Temporal Money Data
        GameData temporalData = LoadGameData(temporalPath);
        temporalData.Bills += amount;
        SaveGameData(temporalData, temporalPath);
        
        GameData generalData = LoadGameData(generalPath);
        generalData.Bills += amount;
        SaveGameData(generalData, generalPath);
    }

    public void SubtractCoins(int amount) { // Only for the General Money Data
    
        GameData generalData = LoadGameData(generalPath);
        if (generalData.Coins <= 0) return;
        generalData.Coins = Mathf.Max(0, generalData.Coins - amount);
        SaveGameData(generalData, generalPath);
    }

    public void SubtractBills(int amount) { // Only for the General Money Data

        GameData generalData = LoadGameData(generalPath);
        if (generalData.Bills <= 0) return;
        generalData.Bills = Mathf.Max(0, generalData.Bills - amount);
        SaveGameData(generalData, generalPath);
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

        GameData temporalData = LoadGameData(temporalPath);
        temporalData.Score += score;
        temporalData.EnemiesTakenDown += enemiesTakenDown;
        temporalData.HitsOnEnemies += hitsOnEnemies;
        SaveGameData(temporalData, temporalPath);
        
        GameData generalData = LoadGameData(generalPath);
        generalData.Score += score;
        generalData.EnemiesTakenDown += enemiesTakenDown;
        generalData.HitsOnEnemies += hitsOnEnemies;
        SaveGameData(generalData, generalPath);
    }
    
    public void ResetTemporal() {
        GameData data = new GameData();
        SaveGameData(data, temporalPath);
    }
    
    public void ResetGeneral() {
        GameData data = new GameData();
        SaveGameData(data, generalPath);
    }
    
    public (int Score, int Bills, int Coins, int EnemiesTakenDown, int HitsOnEnemies) GetTemporalInfo() {
        GameData data = LoadGameData(temporalPath);
        return (data.Score, data.Bills, data.Coins, data.EnemiesTakenDown, data.HitsOnEnemies);
    }
    
    public (int Score, int Bills, int Coins, int EnemiesTakenDown, int HitsOnEnemies) GetGeneralInfo() {
        GameData data = LoadGameData(generalPath);
        return (data.Score, data.Bills, data.Coins, data.EnemiesTakenDown, data.HitsOnEnemies);
    }

    
}