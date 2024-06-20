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


    private float fireReady;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        print("fireRate: " + fireReady);
        float playerDistance = Vector3.Distance(playerTargetTransform.position, gameObject.transform.position);
        if (fireReady > fireRate)
        {
            if (playerDistance < 300f)
            { 
                fireReady += Time.deltaTime; 
                Vector3 aimDirection = (playerTargetTransform.position - gameObject.transform.position).normalized;
                Ray playerCheck = new Ray(bulletSpawn.position, aimDirection);
                RaycastHit hitInfo;

                if (Physics.Raycast(playerCheck, out hitInfo, 999f))
                {
                    if (hitInfo.collider.gameObject.GetComponent<PlayerShooterController>() != null)
                    {
                        Instantiate(pfBulletProjectile, bulletSpawn.position, Quaternion.LookRotation(aimDirection, Vector3.up));
                    }
                }
            }
        }
        fireReady = 0;
    }
}
