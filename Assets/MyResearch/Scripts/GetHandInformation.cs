using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Text;
using TMPro;

public class GetHandInformation : MonoBehaviour
{
    [SerializeField] private Transform TrackingSpace;
    [Header("UI")]
    public TextMeshPro displayText;
    [SerializeField] private Button startButton;
    [SerializeField] private Button endButton;



    private StreamWriter writer;
    private string filePath;
    private bool isMeasuring = true;
    private void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "HandInformation.txt");

        displayText.text = filePath;
    }

    private void Update()
    {

        if (isMeasuring)
        {

            // Nullチェックを追加
            if (TrackingSpace == null)
            {
                Debug.LogError("TrackingSpace is not assigned.");
                return;
            }
            // 右手
            // コントローラーの位置を取得
            Vector3 localPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
            Vector3 rpos = TrackingSpace.TransformPoint(localPos);

            // コントローラーの角度を取得
            Quaternion localRot = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
            Quaternion rrot = TrackingSpace.rotation * localRot;

            // 左手
            // コントローラーの位置を取得
            Vector3 localPosLeft = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
            Vector3 lpos = TrackingSpace.TransformPoint(localPosLeft);

            // コントローラーの角度を取得
            Quaternion localRotLeft = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
            Quaternion lrot = TrackingSpace.rotation * localRotLeft;

            // データをテキストファイルに書き込む
            writer.WriteLine("Right Hand Position: " + rpos);
            writer.WriteLine("Right Hand Rotation: " + rrot.eulerAngles);
            // writer.WriteLine("Left Hand Position: " + lpos);
            // writer.WriteLine("Left Hand Rotation: " + lrot.eulerAngles);
        }
    }

    private void LogHandData(OVRInput.Controller controller, string handName)
    {

    }


    public void StartMeasurement()
    {
        if (!isMeasuring)
        {
            FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
            writer = new StreamWriter(fileStream, Encoding.UTF8);
            isMeasuring = true;

        }

    }

    public void EndMeasurement()
    {
        if (isMeasuring)
        {

            writer.Close();

            writer = null;
        }
        isMeasuring = false;

        Application.Quit();
    }
    private void OnDestroy()
    {
        // writerがnullでないことを確認してからCloseメソッドを呼び出す
        if (writer != null)
        {
            writer.Close();
        }
    }


    public bool IsMeasuring()
    {
        return isMeasuring;
    }
}
