using UnityEngine;
using System.Reflection;
using System;

[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData")]
public class PlayerData : ScriptableObject {

    public int score;
    public int bills;
    public int coins;
    public int enemiesTakenDown;
    public int hitsOnEnemies;

    /*public void AddDataTo(PlayerData destination) {

        //GetType() --> Get the Type object representing the type of the current instance (current ScriptableObject).

        FieldInfo[] fields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

        //.GetFields() --> Get an array of FieldInfo objects representing all public instance fields of the type.
        // BindingFlags.Public --> Only consider public fields.
        // BindingFlags.Instance --> Only consider instance (non-static) fields.

        foreach (FieldInfo field in fields) {

            // Get the current value of the field from the source ScriptableObject
            object sourceValue = field.GetValue(this);

            // Get the existing value of the field from the destination ScriptableObject
            object destinationValue = field.GetValue(destination);

            // Use IConvertible for type-safe addition of numeric types
            if (field.FieldType.IsNumeric()) {

                IConvertible convertedDestination = (IConvertible)destinationValue;
                IConvertible convertedSource = (IConvertible)sourceValue;

                TypeCode commonTypeCode = Type.GetTypeCode(field.FieldType);
                object result = Convert.ChangeType(convertedDestination.ToDouble(null) + convertedSource.ToDouble(null), commonTypeCode);

                field.SetValue(destination, result);
            
            } else {
                field.SetValue(destination, sourceValue);
            }
        }

        checkCoins();

    }*/

    public void AddDataToPlayerData(PlayerData temporalData) {
        MoneyLogger.Log($"[PlayerData] AddDataToPlayerData called. Before: bills={bills}, coins={coins}. Adding: bills={temporalData.bills}, coins={temporalData.coins}");
        score += temporalData.score;
        bills += temporalData.bills;
        coins += temporalData.coins;
        enemiesTakenDown += temporalData.enemiesTakenDown;
        hitsOnEnemies += temporalData.hitsOnEnemies;
        checkCoins();
        MoneyLogger.Log($"[PlayerData] AddDataToPlayerData complete. After: bills={bills}, coins={coins}");
    }

    public void resetStaticVariables() {
        MoneyLogger.Log($"[PlayerData] resetStaticVariables called. Before: score={score}, bills={bills}, coins={coins}");
        score = 0;
        bills = 0;
        coins = 0;
        MoneyLogger.Log($"[PlayerData] resetStaticVariables complete. After: score={score}, bills={bills}, coins={coins}");
        //enemiesTakenDown = 0;
        //hitsOnEnemies = 0;
    }

    private void checkCoins() {
        while (coins >= 100) {
            
            coins -= 100;
            bills += 1;

        }
    }
}

// Extension method to check if a type is numeric
public static class TypeExtensions {

    public static bool IsNumeric(this Type type) {

        return type == typeof(int) || type == typeof(float) || type == typeof(double) || type == typeof(long);
    }
}
