using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera overHeadCamera;
    public Camera playerCamera;
    public GameObject player;

    private bool isOverheadView = true;

    void Start()
    {
        SwitchToOverheadView();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
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
        Cursor.lockState = CursorLockMode.None;
        overHeadCamera.enabled = true;
        playerCamera.enabled = false;
        isOverheadView = true;
        player.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    void SwitchToFirstPerson()
    {
        Cursor.lockState = CursorLockMode.Locked;
        overHeadCamera.enabled = false;
        playerCamera.enabled = true;
        isOverheadView = false;
    }
}
