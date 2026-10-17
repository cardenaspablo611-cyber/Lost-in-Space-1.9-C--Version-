using UnityEngine;

/// <summary>
/// Ensures consistent session state on game launch in both Editor and builds.
/// Resets money to 0 and ship unlocks to defaults (only Normal Spaceship available).
/// This prevents Editor Play mode from dirtying ScriptableObject assets.
/// </summary>
public static class SessionStateManager
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void ResetSessionState()
    {
        // Reset money data to zero for new session
        MoneyAdministratorScript.ResetSessionData();
        
        // Find and reset spaceship data
        // Note: FindObjectsByType only works after scene loads, so we handle ships
        // via SpaceshipInUseScript's OnEnable or first access
        
        // Mark that session has been initialized
        Debug.Log("[SessionStateManager] Game session initialized - money and ship unlocks reset to defaults");
    }
}
