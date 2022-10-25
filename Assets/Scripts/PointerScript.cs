using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PointerScript : MonoBehaviour
{
    [SerializeField] private Camera uiCamera;
    private Vector3 targetPosition;
    private RectTransform pointerRectTransform;
    [SerializeField] private DeliveryController dc;

    // Start is called before the first frame update
    void Start()
    {
        dc = FindObjectOfType<DeliveryController>();
        pointerRectTransform = transform.Find("Arrow").GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dc.IfHaveLocation())
        {
            targetPosition = dc.LocationOfPlace();
        }

        Vector3 fromPosition = Camera.main.transform.position;
        //for making sure weet z axises
        fromPosition.z = targetPosition.z;

        //Direction vector in units (via normalized)
        Vector3 dir = (targetPosition - fromPosition).normalized;

        //new vector3(1,0,0) 1 because we want to mesaure an angle
        //in a unit circle so we start at 0 degree which is 1,0
        float angle = Vector3.Angle(new Vector3(1, 0, 0), dir);

        //if y unit vector is negative we substract the angle from
        //360 because apperantly Vector3.Angle takes the smallest angle
        //so if the angle is 210 Angle method will return 150 which is wrong
        if (dir.y < 0)
            angle = 360 - angle;

        pointerRectTransform.eulerAngles = new Vector3(0, 0, angle);


        //since this arrow is an ui element we need to 
        //change its position to screen position first
        //and do the calculations there
        //lastly we put the arrow in the real game world
        //by making that screen position to world position

        //now we put pointer right positon in the screen
        Vector3 targetPositionScreenPos =
            Camera.main.WorldToScreenPoint(targetPosition);
        float border = 100f;
        bool isOffScreen = targetPositionScreenPos.x < border ||
                           targetPositionScreenPos.y < border ||
                           targetPositionScreenPos.x > Screen.width - border ||
                           targetPositionScreenPos.y > Screen.height - border;


        if (isOffScreen)
        {
            Image img = transform.Find("Arrow").GetComponent<Image>();
            if (img.enabled == false)
                img.enabled = !img.enabled;

            //    Debug.Log("dıaşrdaaa");
            Vector3 cappedTargetScreenPosition = targetPositionScreenPos;
            if (cappedTargetScreenPosition.x <= border)
                cappedTargetScreenPosition.x = border;
            if (cappedTargetScreenPosition.x >= Screen.width - border)
                cappedTargetScreenPosition.x = Screen.width - border;
            if (cappedTargetScreenPosition.y <= border)
                cappedTargetScreenPosition.y = border;
            if (cappedTargetScreenPosition.y >= Screen.height - border)
                cappedTargetScreenPosition.y = Screen.height - border;

            //DEBUG THIS PARTT
            //we need to put an object into the game's world
            //so we need it's world position
            Vector3 pointerWorldPosition =
                uiCamera.ScreenToWorldPoint(cappedTargetScreenPosition);
            pointerRectTransform.position =
                pointerWorldPosition;
        }

        else
        {
            Image img = transform.Find("Arrow").GetComponent<Image>();
            //make a enable function for this and use it in driver controller
            if (img.enabled == true)
                img.enabled = !img.enabled;
        }
    }
}