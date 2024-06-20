using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyTurret : MonoBehaviour
{
    [SerializeField] private Transform playerTargetTransform;
    [SerializeField] private Transform pfBulletProjectile;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private LayerMask layer;
    [SerializeField] private float targetDistance = 300f;
    


    private float fireReady;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float playerDistance = Vector3.Distance(playerTargetTransform.position, bulletSpawn.transform.position);
        if (fireReady > fireRate)
        {
            fireReady = 0;
            if (playerDistance < targetDistance)
            {
                //Debug.Log("within Distance");
                Vector3 aimDirection = (playerTargetTransform.position - bulletSpawn.transform.position).normalized;
                Ray playerCheck = new Ray(bulletSpawn.position, aimDirection);
                RaycastHit hitInfo;
                Debug.DrawRay(bulletSpawn.position, aimDirection * targetDistance, Color.red, 2f);
                if (Physics.Raycast(playerCheck, out hitInfo, targetDistance + 5f))
                {
                    if (hitInfo.collider.gameObject.GetComponent<PlayerShooterController>() != null)
                    {
                        Instantiate(pfBulletProjectile, bulletSpawn.position, Quaternion.LookRotation(aimDirection, Vector3.up));
                    }
                    else
                    {
                        Debug.Log("hitInfo: " + hitInfo.collider.gameObject.name);
                    }
                }
            }
        }
        else
        {
            fireReady += Time.deltaTime;
        }
    }
}
