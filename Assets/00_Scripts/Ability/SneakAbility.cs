using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SneakAbility : MonoBehaviour
{
    // Start is called before the first frame update
    private ThirdPersonMovement tpm;
    public bool isSneaking = false;
    private float duration = 5f;
    private float cooldown = 20f;
    private bool isCooldown = false;
    private float nextTimeAvailable = 0f;
    [SerializeField] private Material sneakingMaterial;
    private Material playerMaterial;

    void Start()
    {
        tpm = GetComponent<ThirdPersonMovement>();
        playerMaterial = GetComponentInChildren<Renderer>().material;
    }

   public void Update()
    {
        // Check if the cooldown has passed to activate the sneak ability again
        if (Time.time > nextTimeAvailable)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                isSneaking = true;
                nextTimeAvailable = Time.time + cooldown;
                StartCoroutine(ActivateSneakAbility());
            }
        }
    }

    IEnumerator ActivateSneakAbility()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.material = sneakingMaterial;
        }

        tpm.SetSneakSpeed();
        tpm.SetCrouch(true);
        Debug.Log("Sneak ability has been activated for " + duration + " seconds.");
        yield return new WaitForSeconds(duration);

        tpm.SetCrouch(false); // Set player to standing position
        tpm.ResetSpeed(); // Reset speed to avoid movement bugs

        isSneaking = false;
        Debug.Log("Sneak ability is off.");
        // Revert player's material to the original material
        foreach (Renderer renderer in renderers)
        {
            renderer.material = playerMaterial;
        }

        yield return new WaitForSeconds(0.5f); // Wait for a short delay to avoid any movement bugs

        tpm.ResetSpeed();
    }
}
