using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TrapController : MonoBehaviour
{
    public int damage;
    public float slow;
    public float duration;
    public float speed;
    private GameObject upgradePath;
    private float countdown;
    private float exp;
    private int level;
    private bool isMax;
    private List<Upgrade> upgrades;
    public string title;
    private float maxExp;
    private GameManager gameManager;
    private int skillPoint;
    private List<Upgrade> nextPath;
    private float expRate;
    private LoadSprite loadSprite;
    public GameObject bulletParticle;
    public GameObject slowParticle;
    public GameObject trapParticle;
    public Material lvUp;
    public Material trapM;
    public Material slowM;
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
        nextPath = new List<Upgrade>();
    }
    private void OnTriggerStay(Collider other) {
        if (countdown <= 0){
            if(other.gameObject.tag == "Monster"){
                Hit(other.gameObject);
            }
            countdown = 2f / speed;
        }
        
    }
    private void OnMouseDown() {
        if(gameManager.GetState() == 0 || gameManager.GetState() == 2){
            gameManager.SelectUnit(gameObject);
        }
    }
    void Update()
    {
        countdown -= Time.deltaTime;
    }
    private void Hit(GameObject target){
        GainExp(5);
        if(damage != 0){
            GameObject effect = Instantiate(trapParticle, transform.position, trapParticle.transform.rotation);
            Destroy(effect, 1.5f);
        }else{
            GameObject effect = Instantiate(slowParticle, transform.position, slowParticle.transform.rotation);
            Destroy(effect, 1.5f);
        }
        target.GetComponent<MonsterController>().Hit(damage);
        target.GetComponent<MonsterController>().Slow(slow, duration);
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
        return upgrades[number-1].GetDescription();
    }
    public int GetDamage(){
        return damage;
    }
    public float GetSlow(){
        return slow;
    }
    public void SetDamage(int damage){
        this.damage = damage;
    }
    public void SetSlow(float slow){
        this.slow = slow;
    }
    public float GetSpeed(){
        return speed;
    }
    public void SetSpeed(float speed){
        this.speed = speed;
    }
    public float GetDuration(){
        return duration;
    }
    public void SetDuration(float duration){
        this.duration = duration;
    }
    public void SetInfo(GameObject ui){
        int j = 0;
        while(j<10){
            ui.transform.Find("PowerUp").Find("Button"+(j+1).ToString()).Find("Power"+(j+1).ToString()).GetComponent<Image>().sprite = loadSprite.GetNone();
            j++;
        }
        if(title == "Slow"){
            ui.transform.Find("Name").gameObject.GetComponent<TMP_Text>().text = this.title;
            ui.transform.Find("Level").gameObject.GetComponent<TMP_Text>().text = string.Format("Level {0}",this.level);
            ui.transform.Find("SlowInfo").Find("Str").Find("SlowText").gameObject.GetComponent<Text>().text = this.slow.ToString();
            ui.transform.Find("SlowInfo").Find("Duration").Find("DurationText").gameObject.GetComponent<Text>().text = this.duration.ToString();
            ui.transform.Find("Level").Find("ExpBar").gameObject.GetComponent<Slider>().value = (int)Mathf.Round((exp/maxExp)*100);
        }else if(title == "Trap"){
            ui.transform.Find("Name").gameObject.GetComponent<TMP_Text>().text = this.title;
            ui.transform.Find("Level").gameObject.GetComponent<TMP_Text>().text = string.Format("Level {0}",this.level);
            ui.transform.Find("TrapInfo").Find("Str").Find("StrText").gameObject.GetComponent<Text>().text = this.damage.ToString();
            ui.transform.Find("Level").Find("ExpBar").gameObject.GetComponent<Slider>().value = ((exp/maxExp)*100);
        }
        ui.transform.Find("Upgrade").Find("Text").gameObject.GetComponent<Text>().text = string.Format("Level: {0}",Mathf.Round((maxExp-exp)*1.5f));
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
                if(damage == 0){
                    rend.material = slowM;
                }else{
                    rend.material = trapM;
                }
                
            }
        }
        nextPath.Clear();
        nextPath = gameManager.Get3RandomUpgrade(this.gameObject);
    }
    public void AddExpRate(float amount){
        this.expRate = this.expRate * amount;
    }
}
