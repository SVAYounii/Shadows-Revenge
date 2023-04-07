using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Windows;
using Random = UnityEngine.Random;


public class FootSteps : MonoBehaviour
{
    public float speed;
    private ThirdPersonMovement m_thirdPersonMovement;
    [SerializeField][Range(0f, 1f)] private float m_RunstepLenghten;
    [SerializeField] private AudioClip[] m_FootstepSounds;
    [SerializeField] private float m_StepInterval;
    private AudioSource m_AudioSource;
    private float m_walkPitch;

    float NextStep;
    bool isWalking;
    private float m_StepCycle;
    private float m_NextStep;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        m_thirdPersonMovement = GetComponent<ThirdPersonMovement>();

        //m_StepCycle = 0f;
        //m_NextStep = m_StepCycle / 2f;
        m_AudioSource = GetComponent<AudioSource>();
        m_walkPitch = Random.Range(0.5f, 1.5f);
        //add the random calculation of footstep. jump and landing pitch
    }
    void Update()
    {
        calcutatespeed();
    }

    private void calcutatespeed()
    {

        if (m_thirdPersonMovement.sprinting)
        {
            speed = 0.5f;
        }
        else if (m_thirdPersonMovement._isCrouching)
        {
            speed = 1f;
        }
        else
        {
            speed = 2f;
        }
        isWalking = (m_thirdPersonMovement.trueSpeed == 0 ? false : true);

        ProgressStepCycle(isWalking, speed);
    }


    private void ProgressStepCycle(bool iswalking, float speed)
    {
        if (!iswalking)
        {
            return;
        }

        if (Time.time > NextStep)
        {
            NextStep = Time.time + speed;

            PlayFootStepAudio();
        }
    }

    private void PlayFootStepAudio()
    {
        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        int n = Random.Range(1, m_FootstepSounds.Length);
        m_AudioSource.clip = m_FootstepSounds[n];
        //add walkPitch value here
        m_AudioSource.pitch = m_walkPitch;
        m_AudioSource.PlayOneShot(m_AudioSource.clip);
        m_walkPitch = Random.Range(0.5f, 1.5f);
        // move picked sound to index 0 so it's not picked next time
        m_FootstepSounds[n] = m_FootstepSounds[0];
        m_FootstepSounds[0] = m_AudioSource.clip;
    }
}
