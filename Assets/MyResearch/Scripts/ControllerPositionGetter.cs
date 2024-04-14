using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Text;
using TMPro;
using System;
public class ControllerPositionGetter : MonoBehaviour
{



    [Header("audio source setting")]
    [SerializeField] AudioSource audioSource;

    [SerializeField] CreateSoundController soundController;
    [SerializeField] float spatialBlend = 1.0f;


    [Header("UI Setting")]
    [SerializeField] private Text text;
    [SerializeField] private Slider amplitudeSlider;
    [SerializeField] private Slider pitchSlider;
    [SerializeField] private Slider panSlider;

    private StreamWriter writer;
    private string filePath;
    private bool isMeasuring = false;



    private Vector3
     rightControllerPosition;

    private Vector3
    previousRightControllerPosition;
    private float movementSpeed;
    private Vector3 acceleration;

    [SerializeField] private float amplitudeCoefficient;
    private float amplitude;
    [SerializeField] private float frequencyCoefficient;
    private float frequency;
    [SerializeField] private float panCoefficient;
    private float pan;

    [Header("Debug")]
    [SerializeField] private bool isDebug = false;

    [Header("Debug UI Setting")]

    [SerializeField] private Text debugText;
    [SerializeField] private Slider debugAmplitudeSlider;
    [SerializeField] private Slider debugPitchSlider;
    [SerializeField] private Slider debugPanSlider;


    void Start()
    {
        Initialize();
        audioSource.loop = true;
        audioSource.Play();
        ChangeSpatialBlend();

    }

    void Initialize()
    {
        string dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
        filePath = Path.Combine(Application.persistentDataPath, $"OpenBrushData_{dateTime}.txt");

        FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
        writer = new StreamWriter(fileStream, Encoding.UTF8);
        isMeasuring = true;
        previousRightControllerPosition = GetRightControllerPosition();
        acceleration = OVRInput.GetLocalControllerAcceleration(OVRInput.Controller.RTouch);
        Debug.Log("acceleration" + acceleration);

    }

    void Update()
    {
        DebugSetting();

        previousRightControllerPosition = rightControllerPosition;
        acceleration = OVRInput.GetLocalControllerAcceleration(OVRInput.Controller.RTouch);
        // Oculus Touchの右コントローラーのスティック入力を取得
        Vector2 stickInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);

        // スティックのY軸入力（上下）を使ってスライダーの値を調整
        float newValue = amplitudeSlider.value + (stickInput.y * 0.2f);
        amplitudeSlider.value = Mathf.Clamp(newValue, amplitudeSlider.minValue, amplitudeSlider.maxValue);


        WriteInformation();
        DisplayOnUI();
    }

    private void DebugSetting()
    {
        if (isDebug)
        {
            rightControllerPosition = GetPointerPosition();
            AudioSettingForDebug();
            DebugAudioSettingFromUI();

        }
        else
        {

            rightControllerPosition = GetRightControllerPosition();
            AudioSetting();
            AudioSettingFromUI();
        }
    }

    private void FrequencyModulation()
    {
        audioSource.pitch = Mathf.Clamp(movementSpeed / 10, 0.5f, 2.0f);
        audioSource.volume = Mathf.Clamp(1 - Mathf.Abs(rightControllerPosition.z) / 10, 0.1f, 1.0f);
    }

    private void WriteInformation()
    {
        writer.WriteLine("right hand position: " + rightControllerPosition);
        writer.WriteLine("amplitude: " + audioSource.volume);
        writer.WriteLine("pitch: " + audioSource.pitch);
        writer.WriteLine("pan: " + audioSource.panStereo);
        writer.WriteLine("acceleration: " + acceleration);
    }

    private Vector3 GetRightControllerPosition()
    {
        Vector3 controllerPosition = InputTracking.GetLocalPosition(XRNode.RightHand);
        return controllerPosition;

    }
    private Vector3 GetPointerPosition()
    {
        return Input.mousePosition;
    }

    private void AudioSetting()
    {
        amplitude = Mathf.Abs(rightControllerPosition.z);
        frequency = rightControllerPosition.y;
        pan = rightControllerPosition.x;


        audioSource.volume = amplitude * amplitudeCoefficient;
        audioSource.pitch = frequency * frequencyCoefficient;
        audioSource.panStereo = pan * panCoefficient;

        soundController.gainCoefficient = amplitude * amplitudeCoefficient;
        soundController.frequencyCoefficient = frequency * frequencyCoefficient;
        soundController.panCoefficient = pan * panCoefficient;

    }

    private void DebugAudioSettingFromUI()
    {
        amplitudeCoefficient = debugAmplitudeSlider.value;
        frequencyCoefficient = debugPitchSlider.value;
        panCoefficient = debugPanSlider.value;





    }

    private void AudioSettingFromUI()
    {
        amplitudeCoefficient = amplitudeSlider.value;
        frequencyCoefficient = pitchSlider.value;
        panCoefficient = panSlider.value;


    }

    private void AudioSettingForDebug()
    {
        amplitude = Mathf.Abs(rightControllerPosition.y) / 200;
        frequency = rightControllerPosition.y / 200;
        pan = rightControllerPosition.x / 200;
        audioSource.volume = amplitude * amplitudeCoefficient;
        audioSource.pitch = frequency * frequencyCoefficient;
        audioSource.panStereo = pan * panCoefficient;

        soundController.gainCoefficient = amplitude * amplitudeCoefficient;
        soundController.frequencyCoefficient = frequency * frequencyCoefficient;
        soundController.panCoefficient = pan * panCoefficient;



    }


    private void ChangeSpatialBlend()
    {

        audioSource.spatialBlend = spatialBlend;
    }

    private void DisplayOnUI()
    {
        if (isDebug)
        {
            debugText.text = acceleration.ToString();
        }
        else
        {
            text.text = acceleration.ToString();

        }
    }

}
