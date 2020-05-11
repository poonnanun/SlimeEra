using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MinerController : MonoBehaviour
{
    public int mineStr;
    public float mineRate;
    private GameManager gameManager;
    private float countdown = 0f;
    private int exp;
    private int level;
    private bool isMax;
    private List<Upgrade> upgrades;
    public string title;
    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        mineStr = 3;
        mineRate = 1f;
        level = 1;
        exp = 0;
        isMax = false;
        upgrades = new List<Upgrade>();
    }
    void Update()
    {
        if(gameManager.GetState() == 2){
            if (countdown <= 0){
                Mine();
                countdown = 1f / mineRate;
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
    }
    public void GainExp(int getExp){
        if(!isMax){
            this.exp += 50;
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
        print("Ding!!");
    }
    public void SetInfo(GameObject ui){
        ui.transform.Find("Name").gameObject.GetComponent<TMP_Text>().text = this.title;
        ui.transform.Find("Level").gameObject.GetComponent<TMP_Text>().text = string.Format("Level {0}",this.level);
        ui.transform.Find("MinerInfo").Find("Str").Find("StrText").gameObject.GetComponent<Text>().text = this.mineStr.ToString();
        ui.transform.Find("MinerInfo").Find("Speed").Find("SpeedText").gameObject.GetComponent<Text>().text = this.mineRate.ToString();
    }
    
}
