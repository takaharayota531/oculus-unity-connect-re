using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [Header("Controller Setting")]
    [SerializeField] private DrawExperienceController drawExperienceController;

    [SerializeField] private const float GAME_TIME = 90.0f;
    private const float countTime = 5.0f;

    [Header("UI Setting")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField]
    private TextMeshProUGUI descriptionText;
    [SerializeField] private GameObject countDownPanel;
    [SerializeField]
    private TextMeshProUGUI countDownText;
    [SerializeField]
    private GameObject gameFinishDiplayCanvas;

    private float remainingTime;
    private float countDownTime;
    private float consumedTime = 0;
    private bool isCount = false;
    private bool isGame = false;


    // Start is called before the first frame update
    void Start()
    {
        Initialize();

    }
    void Initialize()
    {
        remainingTime = GAME_TIME;
        countDownTime = countTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCount)
        {
            countDownTime -= Time.deltaTime;
            countDownText.text = "count " + countDownTime.ToString("f0");
            if (countDownTime <= 0)
            {
                StartGame();
                countDownPanel.SetActive(false);
                isCount = false;
            }

        }


        if (isGame)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0)
            {
                EndGame();

            }
            consumedTime += Time.deltaTime;

        }
        timerText.text = "Time:" + remainingTime.ToString("f1");
        // timerText.text = "Time:" + consumedTime.ToString("f1");
    }

    public void StartGameCountDown()
    {
        isCount = true;
        countDownPanel.SetActive(true);

    }

    public void StartGame()
    {
        isGame = true;
        Initialize();
        drawExperienceController.CallGameStart();

    }

    public void EndGame()
    {
        isGame = false;
        gameFinishDiplayCanvas.SetActive(true);
        descriptionText.text = "";
        drawExperienceController.EndGame();
    }
    // public void EndGame1()
    // {

    //     isGame = false;
    //     gameFinishDiplayCanvas.SetActive(true);
    //     descriptionText.text = "";

    // }
    public float GetConsumedTime()
    {
        return consumedTime;
    }
}
