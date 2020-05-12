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
    private GameObject upgradePath;
    private GameObject end;
    private float fireCountdown = 0f;
    private GameManager gameManager;
    [Header("Assets")]
    public GameObject bulletPrefabs;
    private float exp;
    private int level;
    private bool isMax;
    private List<Upgrade> upgrades;
    public string title;
    private float maxExp;
    private int skillPoint;
    private List<Upgrade> nextPath;
    private float expRate;
    private LoadSprite loadSprite;
    public Material lvUp;
    public Material normal;
    private Renderer rend;
    private void Awake() {
        level = 1;
        exp = 0;
        isMax = false;
        maxExp = 100;
        expRate = 1;
        rend = GetComponent<Renderer>();
        upgrades = new List<Upgrade>();
        loadSprite = GameObject.FindObjectOfType<LoadSprite>();
        gameManager = FindObjectOfType<GameManager>();
        upgradePath = gameManager.GetUpgradePath();
        end = gameManager.GetEnd();
        nextPath = new List<Upgrade>();
    }
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget(){
        float shortestDist = Mathf.Infinity;
        GameObject nearestMonster = null;
        foreach(GameObject monster in gameManager.GetMonsters()){
            float distToMonster = Vector3.Distance(end.transform.position, monster.transform.position);
            float distToThatMonster = Vector3.Distance(transform.position, monster.transform.position);
            if (distToMonster < shortestDist && distToThatMonster <= range){
                shortestDist = distToMonster;
                nearestMonster = monster;
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
            GainExp(5);
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
    public void SetRange(float range){
        this.range = range;
    }
    public float GetRange(){
        return range;
    }
    public void GainExp(float getExp){
        if(!isMax){
            this.exp += getExp*expRate;;
            if(exp >= maxExp){
                LevelUp();
            }
        } 
    }
    public void LevelUp(){
        level += 1;
        skillPoint += 1;
        if (rend != null){
            rend.material = lvUp;
        }
        if(nextPath.Count == 0){
            nextPath = gameManager.Get3RandomUpgrade(this.gameObject);
        }
        exp = exp - maxExp;  
        maxExp = maxExp * 2f;
        if(level == 10){
            isMax = true;
            exp = maxExp;
        }
    }
    public void BuyLv(){
        GainExp(maxExp-exp);
    }
    public string GetDescription(int number){
        if(upgrades.Count>=number){
            return upgrades[number-1].GetDescription();
        }
        else{
            return "";
        }
    }
    public void SetInfo(GameObject ui){
        int j = 0;
        while(j<10){
            ui.transform.Find("PowerUp").Find("Button"+(j+1).ToString()).Find("Power"+(j+1).ToString()).GetComponent<Image>().sprite = loadSprite.GetNone();
            j++;
        }
        ui.transform.Find("Name").gameObject.GetComponent<TMP_Text>().text = this.title;
        ui.transform.Find("Level").gameObject.GetComponent<TMP_Text>().text = string.Format("Level {0}",this.level);
        ui.transform.Find("Upgrade").Find("Text").gameObject.GetComponent<Text>().text = string.Format("Level: {0}",Mathf.Round((maxExp-exp)*1.5f));
        ui.transform.Find("GunnerInfo").Find("Damage").Find("DamageText").gameObject.GetComponent<Text>().text = this.damage.ToString();
        ui.transform.Find("GunnerInfo").Find("Range").Find("RangeText").gameObject.GetComponent<Text>().text = this.range.ToString();
        ui.transform.Find("GunnerInfo").Find("Speed").Find("SpeedText").gameObject.GetComponent<Text>().text = Mathf.Round(this.fireRate).ToString();
        ui.transform.Find("Level").Find("ExpBar").gameObject.GetComponent<Slider>().value = ((exp/maxExp)*100);
        int i = 1;
        foreach(Upgrade u in upgrades){
            string src = u.GetName();
            ui.transform.Find("PowerUp").Find("Button"+i.ToString()).Find("Power"+i.ToString()).GetComponent<Image>().sprite = u.GetSprite();
            i++;
        }
        if(skillPoint >= 1){
            upgradePath.SetActive(true);
            upgradePath.transform.Find("Upgrade1").Find("Image").gameObject.GetComponent<Image>().sprite = nextPath[0].GetSprite();
            upgradePath.transform.Find("Upgrade2").Find("Image").gameObject.GetComponent<Image>().sprite = nextPath[1].GetSprite();
            upgradePath.transform.Find("Upgrade3").Find("Image").gameObject.GetComponent<Image>().sprite = nextPath[2].GetSprite();
        }
    }
    public int GetExpLeft(){
        return (int)Mathf.Round((maxExp-exp)*1.5f);
    }
     public void UpgradeSkill(int number){
        this.upgrades.Add(nextPath[number-1]);
        nextPath[number-1].Effect(this.gameObject);
        skillPoint -= 1;
        if(skillPoint == 0){
            if (rend != null){
                rend.material = normal;
            }
        }
        nextPath.Clear();
        nextPath = gameManager.Get3RandomUpgrade(this.gameObject);
    }
    public void AddExpRate(float amount){
        this.expRate = this.expRate * amount;
    }
}
