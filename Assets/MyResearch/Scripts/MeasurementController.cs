using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MeasurementController : MonoBehaviour
{

    [SerializeField]
    private FingerPaint fingerPaint;

    [SerializeField]
    private ExperienceController experienceController;
    [SerializeField] private ProgressController progressController;



    [SerializeField] private bool isMeasuring;
    [SerializeField] private int count = 0;

    private bool isTimeMeasuring;
    // private double measuringTime = 0;
    private float pressStartTime;


    public void Initialize()
    {
        count = 0;

    }


    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Three))
        {

            if (!isMeasuring)
            {

                Debug.Log("Measurement and drawing started");
                count++;

                experienceController.StartMeasurement(count);
                isMeasuring = true;


            }
        }
        else if (OVRInput.GetUp(OVRInput.Button.Three))
        {
            isMeasuring = false;

            Debug.Log("Measurement and drawing stopped");
            Debug.Log("count:" + count);

            experienceController.EndMeasurement();
            progressController.SubtractCount();

            // fingerPaint.StopDrawing(); // Stop drawing

        }


        if (OVRInput.GetDown(OVRInput.Button.Four))
        {

            if (!isTimeMeasuring)
            {
                Debug.Log("time measuring start");
                // measuringTime = 0;
                isTimeMeasuring = true;
                pressStartTime = Time.time;
            }
            // measuringTime += Time.deltaTime;


        }
        else if (OVRInput.GetUp(OVRInput.Button.Four))
        {
            isTimeMeasuring = false;


            Debug.Log("time Measurement and drawing stopped");
            float pressDuration = Time.time - pressStartTime;
            experienceController.WriteTimeInformation(pressDuration);
            // measuringTime = 0;

        }
    }

}
