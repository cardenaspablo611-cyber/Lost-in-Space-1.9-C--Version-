using UnityEngine;

public class Cooldown {

    private float cooldownDuration;
    private float timer;

    public float CooldownDuration { get { return cooldownDuration;}}

    public Cooldown(float cooldownTime) {

        cooldownDuration = cooldownTime;
    }

    public bool coolDownOver() {

        timer += Time.unscaledDeltaTime;

        if (timer >= cooldownDuration) {
            
            timer -= cooldownDuration;

            return true;

        } else { return false; }
    }
}
