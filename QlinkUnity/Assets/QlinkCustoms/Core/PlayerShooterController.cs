using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.Rendering;

public class PlayerShooterController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera = null;
    [SerializeField] private Canvas crosshair = null;
    [SerializeField] private float normalSensitivity = 0f;
    [SerializeField] private float aimSensitivity = 0f;
    [SerializeField] private LayerMask aimColliderMask = 0;
    [SerializeField] private Transform missVFX = null;
    [SerializeField] private Transform hitVFX = null;
    //[SerializeField] private Transform debugTransform;



    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs input;

    // Start is called before the first frame update
    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        input = GetComponent<StarterAssetsInputs>();

        normalSensitivity = Mathf.Max(normalSensitivity, 0.1f);
        aimSensitivity = Mathf.Max(aimSensitivity, 0.1f);
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;

        Vector2 ScreenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(ScreenCenterPoint);
        Transform hitTransform = null;
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderMask))
        {
            //debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
            hitTransform = raycastHit.transform;
        }

        if (input.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            crosshair.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);

            Vector3 worldAimInput = mouseWorldPosition;
            worldAimInput.y = transform.position.y;
            Vector3 aimDirection = (worldAimInput - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
            if (input.shoot)
            {
                if (hitTransform != null)
                {
                    if (hitTransform.GetComponent<EnemyTurret>() != null)
                    {
                        Instantiate(hitVFX, transform.position, Quaternion.identity);
                        print("hit");
                    }
                    else
                    {
                        {
                            Instantiate(missVFX, transform.position, Quaternion.identity);
                            print("miss");
                        }
                    }
                }
                input.shoot = false;
            }
        }
        else
        {
            {
                aimVirtualCamera.gameObject.SetActive(false);
                crosshair.gameObject.SetActive(false);
                thirdPersonController.SetSensitivity(normalSensitivity);
                thirdPersonController.SetRotateOnMove(true);
            }
        }
    }
}
