using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.Body.Input;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DenseSparseExpController : MonoBehaviour
{
    [SerializeField] private GameObject baseObject;
    [SerializeField] private Vector3 startCoordinate;
    [SerializeField] private float interval = 0.05f;
    [Header("Setting")]
    [SerializeField] private CalculateDistance calculateDistance;
    [SerializeField] private DisplayTargetPlaceColorController displayTargetPlaceColorController;
    [SerializeField] private HandController handController;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI restCountText;

    [Header("Visualizer")]
    [SerializeField] private FrequencyRangeVisualizer frequencyRangeVisualizer;

    [SerializeField] private DenseOrSparse denseOrSparse;

    private List<Vector3> targetCoordinates = new List<Vector3>();
    private List<GameObject> targetObjects = new List<GameObject>();

    private int objectCount = 5;
    private int score = 0;
    private bool isGame = false;
    private int restCount = 10;

    private int targetCorrectIndex = 0;
    private float previousInterval;

    // Start is called before the first frame update
    void Start()
    {
        previousInterval = interval;
        CreateTargetObjects();
    }

    // Update is called once per frame
    void Update()
    {
        if (interval != previousInterval)
        {
            UpdateObjectPositions();
            previousInterval = interval;
        }
        if (restCount <= 0)
        {
            isGame = false;
            restCountText.text = "game finished";
        }

        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            isGame = true;

            SetNextTarget();
        }
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            int tmpIndex = handController.GetIndex();
            SetRejoinedIndex(tmpIndex);
        }
        if (isGame)
        {
            scoreText.text = "score:" + score.ToString();
            restCountText.text = "rest:" + restCount.ToString();
        }
    }

    public void SetNextTarget()
    {
        restCount -= 1;
        DecideTargetIndex();
        ChangeDisplayColor();
    }

    private void CreateTargetObjects()
    {
        for (int i = 0; i < objectCount; i++)
        {
            float positionOffset = 0f;
            if (objectCount % 2 == 1)
            {
                // Odd number of objects
                int midIndex = objectCount / 2;
                positionOffset = -(i - midIndex) * interval; // Reversed the sign
            }
            else
            {
                // Even number of objects
                int midIndex = objectCount / 2;
                positionOffset = -(i - midIndex + 0.5f) * interval; // Reversed the sign
            }

            Vector3 newPosition = new Vector3(startCoordinate.x, startCoordinate.y + positionOffset, startCoordinate.z);
            GameObject gameObject = Instantiate(baseObject, newPosition, Quaternion.identity);
            targetObjects.Add(gameObject);
            targetCoordinates.Add(newPosition);

            // Additional setup
            TextMeshPro text = gameObject.GetComponentInChildren<TextMeshPro>();
            if (text != null)
            {
                text.text = (i + 1).ToString();
            }
            else
            {
                Debug.Log("text null");
            }

            PaletteObjectController paletteObjectController = gameObject.GetComponent<PaletteObjectController>();
            paletteObjectController.SetIndex(i);

            calculateDistance.SetTargetObject(gameObject);
            if (i == objectCount / 2)
            {
                calculateDistance.SetCentralObject(gameObject);
            }
        }
    }

    private void UpdateObjectPositions()
    {
        for (int i = 0; i < targetObjects.Count; i++)
        {
            float positionOffset = 0f;
            if (objectCount % 2 == 1)
            {
                // Odd number of objects
                int midIndex = objectCount / 2;
                positionOffset = -(i - midIndex) * interval; // Reversed the sign
            }
            else
            {
                // Even number of objects
                int midIndex = objectCount / 2;
                positionOffset = -(i - midIndex + 0.5f) * interval; // Reversed the sign
            }

            Vector3 newPosition = new Vector3(startCoordinate.x, startCoordinate.y + positionOffset, startCoordinate.z);
            targetObjects[i].transform.position = newPosition;
            targetCoordinates[i] = newPosition;
        }
    }

    public DenseOrSparse GetDenseOrSparse()
    {
        return denseOrSparse;
    }

    public bool GetIsGame()
    {
        return isGame;
    }

    private void DecideTargetIndex()
    {
        targetCorrectIndex = Random.Range(0, objectCount);
    }

    public void SetRejoinedIndex(int rejoinedIndex)
    {
        CheckCorrectAnswer(rejoinedIndex);
        DecideTargetIndex();
        SetNextTarget();
    }

    private void CheckCorrectAnswer(int rejoinedIndex)
    {
        if (targetCorrectIndex == rejoinedIndex)
        {
            score += 1;
        }
    }

    public void ChangeDisplayColor()
    {
        displayTargetPlaceColorController.ChangeIndexAndReflect(targetCorrectIndex);
    }

    public Vector3 GetStartCoordinate()
    {
        return startCoordinate;
    }

    public List<Vector3> GetTargetCoordinates()
    {
        return targetCoordinates;
    }

    public float GetInterval()
    {
        return interval;
    }
    public int GetObjectCount()
    {
        return objectCount;
    }
}

public enum DenseOrSparse
{
    Dense,
    Sparse
}
