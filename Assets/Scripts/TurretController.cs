using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TurretController : MonoBehaviour
{
    public Transform target;
    [Header("Attributes")]
    public float range;
    public float fireRate;
    public int damage;
    private float fireCountdown = 0f;
    private GameManager gameManager;
    [Header("Assets")]
    public GameObject bulletPrefabs;
    private int exp;
    private int level;
    private bool isMax;
    private List<Upgrade> upgrades;
    public string title;
    private void Awake() {
        level = 1;
        exp = 0;
        isMax = false;
        upgrades = new List<Upgrade>();
        gameManager = FindObjectOfType<GameManager>();
    }
    void Start()
    {
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
            if(target != null){
                float distToTarget = Vector3.Distance(transform.position, target.position);
                if(distToTarget > range){
                    target = null;
                }
            }
        }
    }
    private void OnMouseDown() {
        if(gameManager.GetState() == 0 || gameManager.GetState() == 2){
            gameManager.SelectUnit(gameObject);
        }
    }
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
        if(target != null){
            GainExp(2);
            GameObject bulletGO = Instantiate(bulletPrefabs, transform.position, transform.rotation);
            bulletGO.GetComponent<BulletScript>().Seek(target, damage);
        }
    }
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
    public void SetDamage(int damage){
        this.damage = damage;
    }
    public int GetDamage(){
        return damage;
    }
    public void SetSpeed(float speed){
        this.fireRate = speed;
    }
    public float GetSpeed(){
        return fireRate;
    }
    public void GainExp(int getExp){
        if(!isMax){
            this.exp += getExp;
            // this.exp += 50;
            if(exp >= 100){
                level += 1;
                LevelUp();
                exp = exp - 100;  
                if(level == 10){
                    isMax = true;
                    exp = 100;
                }
            }
        } 
    }
    public void LevelUp(){
        Upgrade speedUp = new SpeedUp();
        speedUp.Effect(this.gameObject);
        upgrades.Add(speedUp);
        print("Ding!!");
    }
    public void SetInfo(GameObject ui){
        ui.transform.Find("Name").gameObject.GetComponent<TMP_Text>().text = this.title;
        ui.transform.Find("Level").gameObject.GetComponent<TMP_Text>().text = string.Format("Level {0}",this.level);
        ui.transform.Find("GunnerInfo").Find("Damage").Find("DamageText").gameObject.GetComponent<Text>().text = this.damage.ToString();
        ui.transform.Find("GunnerInfo").Find("Range").Find("RangeText").gameObject.GetComponent<Text>().text = this.range.ToString();
        ui.transform.Find("GunnerInfo").Find("Speed").Find("SpeedText").gameObject.GetComponent<Text>().text = Mathf.Round(this.fireRate).ToString();
        ui.transform.Find("Level").Find("ExpBar").gameObject.GetComponent<Slider>().value = exp;
        int i = 1;
        foreach(Upgrade u in upgrades){
            string src = u.GetName();
            ui.transform.Find("PowerUp").Find("Power"+i.ToString()).GetComponent<Image>().sprite = u.GetSprite();
            i++;
        }
    }
}
