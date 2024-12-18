using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class SurgeryExpController : MonoBehaviour
{
    [Header("Controller Setting")]
    [SerializeField] private CalculateDistance calculateDistance;

    [SerializeField] private List<GameObject> targetObjects = new List<GameObject>();

    [Header("Hand")]
    [SerializeField] private HandController handController;
    [SerializeField] private GameObject targetHand;

    [Header("UI")]

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI targetDisplayText;
    [Header("Setting")]
    [SerializeField] private bool isSound = true;
    [SerializeField] private DenseDataLoggerController dataLoggerController;
    [SerializeField] private TimeController timeController;
    [SerializeField] private float requiredLength;

    private int targetIndex = 0;
    private int gameTime = 120;

    private bool isGame = false;

    private int score = 0;
    private int mistake = 0;

    // private 

    void Start()
    {
        if (!isSound)
        {
            calculateDistance.SetNoSound();
        }
        for (int i = 0; i < targetObjects.Count; i++)
        {
            calculateDistance.SetTargetObject(targetObjects[i]);
        }

        dataLoggerController.InitializeAsSurgeryExp(0.04f, DenseOrSparse.Sparse);
        // int centralIndex = (int)targetObjects.Count / 2;
        // calculateDistance.SetCentralObject(targetObjects[centralIndex]);
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Three) && !isGame)
        {
            // isGame = true;
            timeController.StartGameCountDown();
            timeController.SetRemainingTime(gameTime);
            SetNextTarget();
            Reset();
        }

        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            int tmpIndex = handController.GetIndex();
            if (isGame)
            {
                dataLoggerController.ReflectPlaceChange(targetIndex);
                dataLoggerController.WriteInformation(GetRightIndexFingerPosition());
            }
            // if (tmpIndex == -1) return;
            // targetIndex = tmpIndex;
            CheckCorrectAnswer(tmpIndex);
            SetNextTarget();
        }


    }


    private void Reset()
    {
        score = 0;
        mistake = 0;
        scoreText.text = "score:" + score.ToString() + "\n" + "mistake:" + mistake.ToString();
    }


    private void CheckCorrectAnswer(int tmpIndex)
    {
        if (targetIndex == tmpIndex)
        {
            score += 1;
        }
        else
        {
            mistake += 1;
        }

        scoreText.text = "score:" + score.ToString() + "\n" + "mistake:" + mistake.ToString();
    }
    public void CallGameStart()
    {
        isGame = true;
    }

    private void SetNextTarget()
    {
        int nextIndex;
        do
        {
            nextIndex = Random.Range(0, targetObjects.Count);
        } while (nextIndex == targetIndex); // 前回と同じ値であれば再生成
        targetIndex = nextIndex;
        // if (isGame)
        // {

        // }
        targetDisplayText.text = "target:" + targetIndex.ToString();
        SetTargetColor();
    }



    private void SetTargetColor()
    {
        for (int i = 0; i < targetObjects.Count; i++)
        {
            if (i != targetIndex)
            {
                targetObjects[i].GetComponent<PaletteObjectController>().SetColor(Color.white);
            }
            else if (i == targetIndex)
            {
                targetObjects[i].GetComponent<PaletteObjectController>().SetColor(Color.green);
            }
        }
    }
    private Vector3 GetRightIndexFingerPosition()
    {
        return targetHand.transform.position;
    }

    public float GetRequiredLength()
    {
        return this.requiredLength;
    }
}
