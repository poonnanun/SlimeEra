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
    private float countdown;
    private float exp;
    private int level;
    private bool isMax;
    private List<Upgrade> upgrades;
    public string title;
    private float maxExp;
    private GameManager gameManager;
    
    private void Awake() {
        level = 1;
        exp = 0;
        isMax = false;
        maxExp = 100;
        upgrades = new List<Upgrade>();
        gameManager = FindObjectOfType<GameManager>();
    }
    private void OnTriggerEnter(Collider other) {
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
        target.GetComponent<MonsterController>().Hit(damage);
        target.GetComponent<MonsterController>().Slow(slow, duration);
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
        int i = 1;
        foreach(Upgrade u in upgrades){
            string src = u.GetName();
            ui.transform.Find("PowerUp").Find("Button"+i.ToString()).Find("Power"+i.ToString()).GetComponent<Image>().sprite = u.GetSprite();
            i++;
        }
        
    }
}
