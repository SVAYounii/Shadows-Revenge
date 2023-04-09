using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingSounds : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] FootSteps;

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = transform.parent.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void Step()
    {
        int n = Random.Range(1, FootSteps.Length);
        audioSource.clip = FootSteps[n];
        //add walkPitch value here
        audioSource.pitch = Random.Range(0.9f, 1.125f);
        audioSource.PlayOneShot(audioSource.clip);
        // move picked sound to index 0 so it's not picked next time
        FootSteps[n] = FootSteps[0];
        FootSteps[0] = audioSource.clip;
    }
}
