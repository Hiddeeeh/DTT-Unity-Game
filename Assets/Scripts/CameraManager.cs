using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //Define the different cameras I want to switch between
    public Camera overHeadCamera;
    public Camera playerCamera;
    public GameObject player;

    //keep track of which view the camerea should be in
    private bool isOverheadView = true;

    void Start()
    {
        //start in overheadview
        SwitchToOverheadView();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //if the tab key is pressed, switch the view
            if (isOverheadView)
            {
                SwitchToFirstPerson();
            }
            else
            {
                SwitchToOverheadView();
            }
        }
    }

    void SwitchToOverheadView()
    {
        //unlock the mouse (gets locked in first person view)
        Cursor.lockState = CursorLockMode.None;

        //enable and disable the cameras to switch active camera, set the view to overhead
        overHeadCamera.enabled = true;
        playerCamera.enabled = false;
        isOverheadView = true;

        //reset the rotation of the player, so you can move up and down normally in the overhead view
        player.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    void SwitchToFirstPerson()
    {
        //same logic as in "SwitchToOverheadView", but for the first preson camera.
        Cursor.lockState = CursorLockMode.Locked;
        overHeadCamera.enabled = false;
        playerCamera.enabled = true;
        isOverheadView = false;
    }
}
