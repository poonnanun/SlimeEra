using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MinerController : MonoBehaviour
{
    public int mineStr;
    public float speed;
    private GameManager gameManager;
    private float countdown = 0f;
    private float exp;
    private int level;
    private bool isMax;
    private List<Upgrade> upgrades;
    public string title;
    private float maxExp;
    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        level = 1;
        exp = 0;
        maxExp = 100;
        isMax = false;
        upgrades = new List<Upgrade>();
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
        GainExp(5);
    }
    public void GainExp(float getExp){
        if(!isMax){
            this.exp += getExp;
            if(exp >= maxExp){
                level += 1;
                LevelUp();
                exp = exp - maxExp;  
                maxExp = maxExp * 2;
                if(level == 10){
                    isMax = true;
                    exp = maxExp;
                }
            }
        } 
    }
    public void LevelUp(){
        print("Ding!!");
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
        ui.transform.Find("Name").gameObject.GetComponent<TMP_Text>().text = this.title;
        ui.transform.Find("Level").gameObject.GetComponent<TMP_Text>().text = string.Format("Level {0}",this.level);
        ui.transform.Find("MinerInfo").Find("Str").Find("StrText").gameObject.GetComponent<Text>().text = this.mineStr.ToString();
        ui.transform.Find("MinerInfo").Find("Speed").Find("SpeedText").gameObject.GetComponent<Text>().text = this.speed.ToString();
        ui.transform.Find("Level").Find("ExpBar").gameObject.GetComponent<Slider>().value = (float)((exp/maxExp)*100);
        int i = 1;
        foreach(Upgrade u in upgrades){
            string src = u.GetName();
            ui.transform.Find("PowerUp").Find("Button"+i.ToString()).Find("Power"+i.ToString()).GetComponent<Image>().sprite = u.GetSprite();
            i++;
        }
    }
    
}
