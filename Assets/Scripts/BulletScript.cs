using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Transform target;
    public GameObject bulletParticle;
    public float speed = 10f;
    private int damage;
    public void Seek(Transform target, int damage){
        this.target = target;
        this.damage = damage;
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null){
            Destroy(gameObject);
            return;
        }
        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;
        if(dir.magnitude <= distanceThisFrame){
            HitTarget();
            return;
        }
        transform.Translate( dir.normalized * distanceThisFrame, Space.World );
    }
    void HitTarget(){
        GameObject effect = Instantiate(bulletParticle, transform.position, transform.rotation);
        target.gameObject.GetComponent<MonsterController>().Hit(damage);
        Destroy(effect, 1.5f);
        Destroy(this.gameObject);
    }
}
