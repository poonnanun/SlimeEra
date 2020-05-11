using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MinerController : MonoBehaviour
{
    public int mineStr;
    public float speed;
    private GameObject upgradePath;
    private GameManager gameManager;
    private float countdown = 0f;
    private float exp;
    private int level;
    private bool isMax;
    private List<Upgrade> upgrades;
    public string title;
    private float maxExp;
    private int skillPoint;
    private float expRate;
    private List<Upgrade> nextPath;
    private LoadSprite loadSprite;
    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        loadSprite = GameObject.FindObjectOfType<LoadSprite>();
        upgradePath = gameManager.GetUpgradePath();
        level = 1;
        exp = 0;
        expRate = 1;
        maxExp = 100;
        isMax = false;
        upgrades = new List<Upgrade>();
        nextPath = new List<Upgrade>();
        skillPoint = 0;
    }
    void Update()
    {
        if(gameManager.GetState() == 2){
            if (countdown <= 0){
                Mine();
                countdown = 1f / speed;
            }
            countdown -= Time.deltaTime;
        }
    }
    private void OnMouseDown() {
        if(gameManager.GetState() == 0 || gameManager.GetState() == 2){
            gameManager.SelectUnit(gameObject);
        }
    }
    private void Mine(){
        gameManager.AddCurrency(mineStr);
        GainExp(50);
    }
    public void GainExp(float getExp){
        if(!isMax){
            this.exp += getExp*expRate;
            if(exp >= maxExp){
                LevelUp();
                
            }
        } 
    }
    public void LevelUp(){
        level += 1;
        skillPoint += 1;
        if(nextPath.Count == 0){
            nextPath = gameManager.Get3RandomUpgrade(this.gameObject);
        }
        exp = exp - maxExp;  
        maxExp = maxExp * 2;
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
    public int GetMineStr(){
        return mineStr;
    }
    public void SetMineStr(int mineStr){
        this.mineStr = mineStr;
    }
    public float GetSpeed(){
        return speed;
    }
    public void SetSpeed(float speed){
        this.speed = speed;
    }
    public void SetInfo(GameObject ui){
        int j = 0;
        while(j<10){
            ui.transform.Find("PowerUp").Find("Button"+(j+1).ToString()).Find("Power"+(j+1).ToString()).GetComponent<Image>().sprite = loadSprite.GetNone();
            j++;
        }
        ui.transform.Find("Name").gameObject.GetComponent<TMP_Text>().text = this.title;
        ui.transform.Find("Level").gameObject.GetComponent<TMP_Text>().text = string.Format("Level {0}",this.level);
        ui.transform.Find("Upgrade").Find("Text").gameObject.GetComponent<Text>().text = string.Format("Level: {0}",(maxExp-exp)*1.5);
        ui.transform.Find("MinerInfo").Find("Str").Find("StrText").gameObject.GetComponent<Text>().text = this.mineStr.ToString();
        ui.transform.Find("MinerInfo").Find("Speed").Find("SpeedText").gameObject.GetComponent<Text>().text = this.speed.ToString();
        ui.transform.Find("Level").Find("ExpBar").gameObject.GetComponent<Slider>().value = (float)((exp/maxExp)*100);
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
        nextPath.Clear();
        nextPath = gameManager.Get3RandomUpgrade(this.gameObject);
    }
    public void AddExpRate(float amount){
        this.expRate = this.expRate * amount;
    }
}
