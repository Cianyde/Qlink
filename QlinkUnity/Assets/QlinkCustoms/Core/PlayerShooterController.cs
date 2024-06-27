using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class PlayerShooterController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera = null;
    [SerializeField] private Canvas crosshair = null;
    [SerializeField] private float normalSensitivity = 0f;
    [SerializeField] private float aimSensitivity = 0f;
    [SerializeField] private LayerMask aimColliderMask = 0;
    [SerializeField] private Transform missVFX = null;
    [SerializeField] private Transform hitVFX = null;
    [SerializeField] private Transform parryVFX = null;
    [SerializeField] private Transform blockVFX = null;
    [SerializeField] private Transform takeDamageVFX = null;
    [SerializeField] private int chargeMax = 100;
    [SerializeField] private int chargeAmount = 20;
    [SerializeField] private int blockReduction = 10;


    [SerializeField] private float parryTime = 1f;

    //[SerializeField] private Transform debugTransform;

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs input;
    private Animator animator;

    //parry and projectile condition
    private float parryIncrement;
    private bool canParry = true;
    private bool parried = false;
    private bool bIsProjectile;
    private Vector3 impactPoint = Vector3.zero;

    //charging
    private int chargeProgress;
    private bool canShoot = false;

    // Start is called before the first frame update
    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        input = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();

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
            //can't parry while aiming, changing cameraObject and setting new sensitivity
            canParry = false;
            aimVirtualCamera.gameObject.SetActive(true);
            crosshair.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));


            //calculating aimDirection using mouse position while using player character's transform
            Vector3 worldAimInput = mouseWorldPosition;
            worldAimInput.y = transform.position.y;
            Vector3 aimDirection = (worldAimInput - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
            if (input.shoot)
            {
                Debug.Log("can't shoot lmao");

                if (checkCharge())
                {
                    if (hitTransform != null)
                    {
                        if (hitTransform.GetComponent<EnemyTurret>() != null)
                        {
                            Instantiate(hitVFX, raycastHit.point, Quaternion.identity);
                        }
                        else
                        { 
                            Instantiate(missVFX, raycastHit.point, Quaternion.identity);
                        }
                        chargeProgress = 0;
                    }
                    canShoot = false;
                    input.shoot = false;
                    canParry = true;
                }

            }
        }
        else
        {
            {
                aimVirtualCamera.gameObject.SetActive(false);
                crosshair.gameObject.SetActive(false);
                thirdPersonController.SetSensitivity(normalSensitivity);
                thirdPersonController.SetRotateOnMove(true);
                animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));


                canParry = true;
            }
        }

        if (canParry && bIsProjectile)
        {
            parryIncrement += Time.deltaTime;
            Parry();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BulletProjectile>() != null)
        {
            bIsProjectile = true;
            impactPoint = other.ClosestPointOnBounds(other.transform.position);
            other.gameObject.SetActive(false);
        }
    }

    private void Parry()
    {
            //parry
            if (input.parry && parryIncrement < parryTime)
            {
                Debug.Log("parried!!!!");
                Instantiate(parryVFX, impactPoint, Quaternion.identity);
                parryIncrement = 0f;
                input.parry = false;
                parried = true;
                updateCharge();
            }
            //block
            else if (Input.GetKey(KeyCode.E))
            {
                Debug.Log("blocked!!!!");
                Instantiate(blockVFX, impactPoint, Quaternion.identity);
                parried = false;
                updateCharge();
            }
            //fuck around n find out
            else
            {
                Debug.Log("lol u succ!!!!");
                Instantiate(takeDamageVFX, impactPoint, Quaternion.identity);
            }
            bIsProjectile = false;
    }

    private void updateCharge()
    {
        int chargeVar = 0;
        if (parried)
        {
            chargeVar = chargeAmount;
        }
        else
        {
            chargeVar = -blockReduction;
        }

        chargeProgress += chargeVar;
        if (chargeProgress > chargeMax)
        {
            chargeProgress = chargeMax;
        }
        if (chargeProgress < 0)
        {
            chargeProgress = 0;
        }

        Debug.Log("progress: " + chargeProgress);
        Debug.Log("parried: " + parried);
    }

    private bool checkCharge()
    {
        if (chargeProgress >= chargeMax)
        {
            canShoot = true;
        }
        return canShoot;
    }
}
