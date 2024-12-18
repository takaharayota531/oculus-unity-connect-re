using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CalculateSound : MonoBehaviour
{
    [SerializeField] private CreateSoundController createSoundController;
    [SerializeField] private DenseSparseExpController denseSparseExpController;
    [SerializeField] private CalculateDistance calculateDistance;



    private float minFrequency = 150f;
    private float maxFrequency = 700f;
    private float amplitudeRange = 16.0f;

    private float minAmplitude = 1 / 16f;
    private float maxAmplitude = 1.0f;
    private double calculatedAmplitude;
    float fixedAmplitude;
    private double calculatedFrequency;
    // float fixedFrequency;
    double fixedFrequency;
    private double calculatedPan;


    private List<double> frequencies = new List<double>();
    private List<double> amplitudes = new List<double>();
    private List<double> pans = new List<double>();

    [SerializeField] private double soundLength = 1.0f;
    [SerializeField]
    private TextMeshProUGUI debugText;

    // private float xDiff = 0;
    // private float yDiff = 0;
    // private float zDiff = 0;
    private double tZ = 0;
    [SerializeField] private float objectLength = 0.01f;
    double frequencyExponent;


    void Start()
    {
        // minAmplitude = maxAmplitude * Mathf.Pow(10.0f, -amplitudeRange / 20.0f);
        Debug.Log("min amplitude:" + minAmplitude);
        // centralFrequency = (minFrequency + maxFrequency) / 2;
        // createSoundController.SetAmplitude(0);
        // CalculateSoundLength();
        // CalculateExponentialFrequency();
        // CalculateExponentialAmplitude();
        // Debug.Log("sound length:" + soundLength);

    }

    void Update()
    {
        CalculateSoundLength();
        debugText.text = "fre:" + fixedFrequency.ToString() + "\n" + "amp:" + fixedAmplitude.ToString("f2")
          + "\n" + "pan:" + calculatedPan.ToString("f2");

    }
    public void SetCoordinateDiff(Vector3 diff)
    {
        // this.yDiff = diff.y;
        // this.zDiff = diff.z;
        // this.xDiff = diff.x;

        calculatedFrequency = CalculateExponentialFrequency(diff.y);
        calculatedAmplitude = CalculateExponentialAmplitude(diff.z);
        calculatedPan = CalculateExponentialPan(diff.x);
        SetAudio();
    }
    // public void SetYDiff(float yDiff)
    // {
    //     this.yDiff = yDiff;
    //     CalculateExponentialFrequency();

    //     SetFrequency();
    // }
    // public void SetZDiff(float zDiff)
    // {
    //     this.zDiff = zDiff;
    //     CalculateExponentialAmplitude();

    // }
    public void SetInitial(bool isSound)
    {
        if (isSound) { createSoundController.EnableAudio(); }
        else
        {
            createSoundController.DisableAudio();
        }

    }

    public void SetCoordinateDiffs(List<Vector3> diffs)
    {
        frequencies.Clear();
        amplitudes.Clear();
        pans.Clear();

        for (int i = 0; i < diffs.Count; i++)
        {
            double frequency = CalculateExponentialFrequency(diffs[i].y);
            double amplitude = CalculateExponentialAmplitude(diffs[i].z);
            double pan = CalculateExponentialPan(diffs[i].x);
            frequencies.Add(frequency);
            amplitudes.Add(amplitude);
            pans.Add(pan);
        }
        SetAudios();
    }
    public void SetObjectLength(float length)
    {
        objectLength = length;
    }


    private double CalculateFrequency(float yDiff)
    {
        // double calculatedFrequency;

        // calculatedFrequency = centralFrequency - yDiff / soundLength * (maxFrequency - minFrequency) / 2;
        calculatedFrequency = minFrequency + (maxFrequency - minFrequency) / (2 * soundLength) * (yDiff + soundLength);
        return calculatedFrequency;
    }
    private double CalculateExponentialFrequency(float yDiff)
    {


        double t = (yDiff + soundLength) / (2 * soundLength);
        frequencyExponent = ConvertToDiscrete(t);
        float frequencyRatio = maxFrequency / (float)minFrequency;
        fixedFrequency = minFrequency * Mathf.Pow(frequencyRatio, (float)frequencyExponent);

        return fixedFrequency;
    }
    private double CalculateExponentialAmplitude(float zDiff)
    {
        tZ = (zDiff + soundLength) / (2 * soundLength);
        double convertedT = ConvertToDiscrete(tZ);
        float amplitudeRatio = maxAmplitude / (float)minAmplitude;
        calculatedAmplitude = minAmplitude * Mathf.Pow(amplitudeRatio, 1 - (float)convertedT);
        fixedAmplitude = CorrectLoudness((float)calculatedAmplitude);
        return fixedAmplitude;

    }
    private float CorrectLoudness(float originalAmplitude)
    {
        float naze = 6;
        float dBReduction = naze + 6;
        if (frequencyExponent == 1) dBReduction += 2;
        else if (frequencyExponent == 0.75) dBReduction += 1;
        else if (frequencyExponent == 0.5) dBReduction += -3;
        else if (frequencyExponent == 0.25) dBReduction += -8;
        else if (frequencyExponent == 0) dBReduction += -12;
        float newAmplitude = originalAmplitude * Mathf.Pow(10, -dBReduction / 20f);
        return newAmplitude;

        // return amplitude;
    }
    private double CalculateExponentialPan(float xDiff)
    {
        // double calculatedPan;

        if (-soundLength < xDiff && xDiff < -objectLength) calculatedPan = -1;
        else if (-objectLength <= xDiff && xDiff <= objectLength) calculatedPan = 0;
        else if (objectLength < xDiff && xDiff < soundLength) calculatedPan = 1;
        else calculatedPan = 0;


        return calculatedPan;

    }

    private double ConvertToDiscrete(double t)
    {
        if (0 <= t && t < 1.0 / 3.0) return 0;
        else if (1.0 / 3.0 <= t && t <= 2.0 / 3.0) return 0.5;
        else if (2.0 / 3.0 < t && t <= 1.0) return 1;
        else return 0;
    }
    private double ConvertToDiscreteAmp(double t)
    {
        if (0 <= t && t < 1.0 / 3.0) return 0;
        else if (1.0 / 3.0 <= t && t <= 2.0 / 3.0) return 0.5;
        else if (2.0 / 3.0 < t && t <= 1.0) return 1;
        else return 0;
    }

    private void SetFrequency()
    {
        createSoundController.SetFrequencySelf((float)calculatedFrequency);
        createSoundController.SetAmplitude(0.5f);

    }
    private void SetAudio()
    {
        createSoundController.SetFrequencySelf((float)calculatedFrequency);
        createSoundController.SetAmplitude((float)calculatedAmplitude);
        createSoundController.SetPan((float)calculatedPan);
    }

    private void SetAudios()
    {
        createSoundController.SetFrequencies(frequencies);
        createSoundController.SetAmplitudes(amplitudes);
        createSoundController.SetPans(pans);
    }

    private void CalculateSoundLength()
    {
        DenseOrSparse denseOrSparse = denseSparseExpController.GetDenseOrSparse();
        if (denseOrSparse == DenseOrSparse.Dense)
        {
            soundLength = calculateDistance.GetCentralRequiredLength();
        }
        else if (denseOrSparse == DenseOrSparse.Sparse)
        {
            soundLength = calculateDistance.GetRequiredLength();
        }
    }
}
