using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.Editor.BuildingBlocks;
using UnityEngine;
using UnityEngine.Scripting;

public class HandActionController : MonoBehaviour
{


    // [SerializeField] private GameObject[] paletteObjects;
    private GameObject closestObject;
    private float closestDistance = Mathf.Infinity;
    // private GameObject closestObject;

    private bool isGrabbed = false;




    // Update is called once per frame
    void Update()
    {
        if (isGrabbed)
        {
            MoveGrabbedObject();
        }
    }



    private void OnTriggerStay(Collider otherObject)
    {

        if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
        {
            float tmpDistance = Vector3.Distance(otherObject.transform.position, this.transform.position);
            if (tmpDistance < closestDistance)
            {
                closestObject = otherObject.gameObject;
                closestDistance = tmpDistance;

            }
            else
            {
                return;
            }

            Renderer objectRenderer = otherObject.gameObject.GetComponent<Renderer>();
            if (objectRenderer != null)
            {
                objectRenderer.material.color = Color.green;
                isGrabbed = true;
            }

        }
        else
        {

            isGrabbed = false;

        }
    }
    private void OnTriggerExit(Collider otherObject)

    {
        PaletteObjectController paletteObjectController =
        otherObject.GetComponent<PaletteObjectController>();
        if (paletteObjectController != null)
        {
            paletteObjectController.SetDefaultPosition();
            paletteObjectController.SetDefaultColor();

        }
        if (otherObject.gameObject == closestObject)
        {
            closestObject = null;
            closestDistance = Mathf.Infinity;

        }
    }

    // 物体を追従させる
    private void MoveGrabbedObject()
    {
        if (closestObject != null)
        {
            closestObject.transform.position = this.gameObject.transform.position;
        }
    }





}
