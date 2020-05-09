using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    public Transform target;
    [Header("Attributes")]
    public float range = 2f;
    public float fireRate = 1f;
    private float fireCountdown = 0f;
    private GameManager gameManager;
    [Header("Assets")]
    public GameObject bulletPrefabs;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget(){
        float shortestDist = Mathf.Infinity;
        GameObject nearestMonster = null;
        foreach(GameObject monster in gameManager.GetMonsters()){
            float distToMonster = Vector3.Distance(transform.position, monster.transform.position);
            if (distToMonster < shortestDist){
                shortestDist = distToMonster;
                nearestMonster = monster;
            }
            if(nearestMonster != null && shortestDist <= range){
                target = nearestMonster.transform;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if( target == null){
            return;
        }
        if (fireCountdown <= 0){
            Shoot();
            fireCountdown = 1f / fireRate;
        }
        fireCountdown -= Time.deltaTime;
    }

    void Shoot(){
        GameObject bulletGO = Instantiate(bulletPrefabs, transform.position, transform.rotation);
        bulletGO.GetComponent<BulletScript>().Seek(target);
    }
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
