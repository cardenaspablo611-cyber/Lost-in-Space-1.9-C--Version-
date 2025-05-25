using UnityEngine;

public class OmegaCooldown {

    private float cooldownDuration;
    private float timer;

    public float CooldownDuration { get { return cooldownDuration;}}

    public OmegaCooldown(float cooldownTime) {

        cooldownDuration = cooldownTime;
    }

    public bool coolDownOver() {

        timer += Time.deltaTime;

        if (timer >= cooldownDuration) {
            
            timer -= cooldownDuration;

            return true;

        } else { return false; }
    }
}
