using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SneakAbility : MonoBehaviour
{
    // Start is called before the first frame update
    private ThirdPersonMovement tpm;
    public bool isSneaking = false;
    public bool IsAbleToSneak = false;
    private float duration = 7f;
    private float cooldown = 30f;
    private bool isCooldown = false;
    private float nextTimeAvailable = 0f;
    private float nextTimeAbility = 0f;
    public GameObject NotAvailbleImage;
    [SerializeField] private Material sneakingMaterial;
    private Material playerMaterial;

    void Start()
    {
        tpm = GetComponent<ThirdPersonMovement>();
        playerMaterial = GetComponentInChildren<Renderer>().material;
    }

    public void Update()
    {
        if (!IsAbleToSneak)
            return;

        // Check if the cooldown has passed to activate the sneak ability again
        if (Time.time > nextTimeAvailable)
        {
            if (NotAvailbleImage != null)
                NotAvailbleImage.SetActive(false);
            if (Input.GetKeyDown(KeyCode.Q))
            {
                isSneaking = true;
                nextTimeAvailable = Time.time + cooldown;
                nextTimeAbility = Time.time + duration;
                StartCoroutine(ActivateSneakAbility());
            }
        }
        else
        {
            if (NotAvailbleImage != null)
                NotAvailbleImage.SetActive(true);

        }



        if (Time.time > nextTimeAbility + 0.5f && isSneaking)
        {
            Renderer[] renderers = GetComponentsInChildren<Renderer>();

            tpm.SetCrouch(false); // Set player to standing position
            tpm.ResetSpeed(); // Reset speed to avoid movement bugs

            isSneaking = false;
            Debug.Log("Sneak ability is off.");
            // Revert player's material to the original material
            foreach (Renderer renderer in renderers)
            {
                renderer.material = playerMaterial;
            }
            tpm.ResetSpeed();

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
