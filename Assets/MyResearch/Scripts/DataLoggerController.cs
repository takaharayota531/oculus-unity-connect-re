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
using Microsoft.Win32.SafeHandles;
public class DataLoggerController : MonoBehaviour
{


    private StreamWriter writer;



    public void Initialize(string filePath)
    {
        FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
        writer = new StreamWriter(fileStream, Encoding.UTF8);
    }

    public void WriteInformation(Vector3 controllerPosition)
    {

        writer.WriteLine($"{controllerPosition.x},{controllerPosition.y},{controllerPosition.z}");

    }

    public void CountAdd(int count)
    {
        writer.WriteLine("count:" + count);
    }

    public void Close()
    {
        writer.Close();
    }
}