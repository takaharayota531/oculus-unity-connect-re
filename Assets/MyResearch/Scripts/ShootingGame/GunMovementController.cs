using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMovementController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += new Vector3(Input.GetAxis("Horizontal") * Time.deltaTime, 0f, 0f);
        this.transform.position += new Vector3(0f, Input.GetAxis("Vertical") * Time.deltaTime, 0f);

    }
}