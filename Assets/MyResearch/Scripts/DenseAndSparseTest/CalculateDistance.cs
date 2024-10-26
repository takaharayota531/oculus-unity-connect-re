using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateDistance : MonoBehaviour
{
    [Header("OVR Input Information")]
    [SerializeField] private GameObject indexFinger;

    [Header("Calculate Sound")]
    [SerializeField] private CalculateSound calculateSound;

    [SerializeField] private List<GameObject> targetSoundObjects;
    private const double requiredLength = 0.015f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bool isSound = false;
        for (int i = 0; i < targetSoundObjects.Count; i++)
        {
            if (IsInSoundTerritory(i))
            {
                float diff = CalculateControllerPositionAndObjectDiff(i);
                calculateSound.SetYDiff(diff);
                isSound = true;
            }
        }
        if (!isSound) calculateSound.SetInitial();
    }

    private float CalculateControllerPositionAndObjectDiff(int index)
    {
        float diff = indexFinger.transform.position.y - targetSoundObjects[index].transform.position.y;
        return diff;

    }
    private bool IsInSoundTerritory(int index)
    {
        Vector3 rightControllerPosition = GetRightIndexFingerPosition();
        Vector3 targetPosition = targetSoundObjects[index].transform.position;
        if ((Mathf.Abs(rightControllerPosition.x - targetPosition.x) < requiredLength) &&
        (Mathf.Abs(rightControllerPosition.y - targetPosition.y) < requiredLength)
    && (Mathf.Abs(rightControllerPosition.z - targetPosition.z) < requiredLength))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private Vector3 GetRightIndexFingerPosition()
    {
        return indexFinger.transform.position;
    }

    public void SetTargetObject(GameObject gameObject)
    {
        targetSoundObjects.Add(gameObject);
    }



}
