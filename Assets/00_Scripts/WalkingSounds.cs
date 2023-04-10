using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingSounds : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] FootStepsGrass;
    [SerializeField]
    private AudioClip[] FootStepsStone;
    [SerializeField]
    private AudioClip[] FootStepsWood;
    [SerializeField]
    private AudioClip[] FootStepsSand;
    [SerializeField]
    private AudioClip[] FootStepsDirt;

    private AudioSource audioSource;
    ThirdPersonMovement movement;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = transform.parent.GetComponent<AudioSource>();
        movement = transform.parent.GetComponent<ThirdPersonMovement>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void Step()
    {
        if (movement.IsStandingOn == "Stone")
        {

            int n = Random.Range(1, FootStepsStone.Length);
            audioSource.clip = FootStepsStone[n];
            //add walkPitch value here
            audioSource.pitch = Random.Range(0.9f, 1.125f);
            audioSource.PlayOneShot(audioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            FootStepsStone[n] = FootStepsStone[0];
            FootStepsStone[0] = audioSource.clip;
        }
        else if (movement.IsStandingOn == "Wood")
        {
            int n = Random.Range(1, FootStepsWood.Length);
            audioSource.clip = FootStepsWood[n];
            //add walkPitch value here
            audioSource.pitch = Random.Range(0.9f, 1.125f);
            audioSource.PlayOneShot(audioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            FootStepsWood[n] = FootStepsWood[0];
            FootStepsWood[0] = audioSource.clip;
        }
        else if (movement.IsStandingOn == "Sand")
        {
            int n = Random.Range(1, FootStepsSand.Length);
            audioSource.clip = FootStepsSand[n];
            //add walkPitch value here
            audioSource.pitch = Random.Range(0.9f, 1.125f);
            audioSource.PlayOneShot(audioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            FootStepsSand[n] = FootStepsSand[0];
            FootStepsSand[0] = audioSource.clip;
        }
        else if (movement.IsStandingOn == "Dirt")
        {
            int n = Random.Range(1, FootStepsDirt.Length);
            audioSource.clip = FootStepsDirt[n];
            //add walkPitch value here
            audioSource.pitch = Random.Range(0.9f, 1.125f);
            audioSource.PlayOneShot(audioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            FootStepsDirt[n] = FootStepsDirt[0];
            FootStepsDirt[0] = audioSource.clip;
        }
        else
        {
            int n = Random.Range(1, FootStepsGrass.Length);
            audioSource.clip = FootStepsGrass[n];
            //add walkPitch value here
            audioSource.pitch = Random.Range(0.9f, 1.125f);
            audioSource.PlayOneShot(audioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            FootStepsGrass[n] = FootStepsGrass[0];
            FootStepsGrass[0] = audioSource.clip;
        }
    }
}
