using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class CameraShakeController : MonobehaviourSingleton<CameraShakeController>
{
    public float ShakeDuration = 0.3f;          // Time the Camera Shake effect will last
    public float ShakeAmplitude = 1.2f;         // Cinemachine Noise Profile Parameter
    public float ShakeFrequency = 2.0f;         // Cinemachine Noise Profile Parameter

    private Player player;
    private bool shake = false;

    private float ShakeElapsedTime = 0f;

    enum CameraRigs
    {
        TopRig,
        MiddleRig,
        BottomRig
    }

    // Cinemachine Shake
    public CinemachineFreeLook freeLookCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;


    public override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        player = Player.Get();

        // Get Virtual Camera Noise Profile
        if (freeLookCamera != null)
            virtualCameraNoise = freeLookCamera.GetRig((int)CameraRigs.MiddleRig).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
   
    }

    void Update()
    {
        // TODO: Replace with your trigger
        /*if ((Input.GetMouseButton(0) && player.machineGunIsActive) || shake)
        {
            ShakeElapsedTime = ShakeDuration;
        }*/

        // If the Cinemachine componet is not set, avoid update
        if (freeLookCamera != null && virtualCameraNoise != null)
        {
            // If Camera Shake effect is still playing
            if (ShakeElapsedTime > 0)
            {
                // Set Cinemachine Camera Noise parameters
                virtualCameraNoise.m_AmplitudeGain = ShakeAmplitude;
                virtualCameraNoise.m_FrequencyGain = ShakeFrequency;

                // Update Shake Timer
                ShakeElapsedTime -= Time.deltaTime;
            }
            else
            {
                // If Camera Shake effect is over, reset variables
                virtualCameraNoise.m_AmplitudeGain = 0f;
                virtualCameraNoise.m_FrequencyGain = 0f;
                ShakeElapsedTime = 0f;
            }
        }
    }

    IEnumerator Shake()
    {
        shake = true;
        yield return new WaitForSecondsRealtime(ShakeDuration);
        shake = false;
        // If Camera Shake effect is over, reset variables
        virtualCameraNoise.m_AmplitudeGain = 0f;
        virtualCameraNoise.m_FrequencyGain = 0f;
        ShakeElapsedTime = 0f;
    }

    public void ActiveScreenShake()
    {
        StartCoroutine(Shake());
    }

}
