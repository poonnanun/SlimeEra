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
    private int exp;
    private int level;
    private bool isMax;
    private List<Upgrade> upgrades;
    public string title;
    private GameManager gameManager;
    
    private void Awake() {
        level = 1;
        exp = 0;
        isMax = false;
        upgrades = new List<Upgrade>();
        gameManager = FindObjectOfType<GameManager>();
    }
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Monster"){
            Hit(other.gameObject);
        }
    }
    private void OnMouseDown() {
        if(gameManager.GetState() == 0 || gameManager.GetState() == 2){
            gameManager.SelectUnit(gameObject);
        }
    }
    void Update()
    {
        
    }
    private void Hit(GameObject target){
        target.GetComponent<MonsterController>().Hit(damage);
        target.GetComponent<MonsterController>().Slow(slow, duration);
    }
    public void GainExp(int getExp){
        if(!isMax){
            this.exp += getExp;
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
        if(title == "Slow"){
            ui.transform.Find("Name").gameObject.GetComponent<TMP_Text>().text = this.title;
            ui.transform.Find("Level").gameObject.GetComponent<TMP_Text>().text = string.Format("Level {0}",this.level);
            ui.transform.Find("SlowInfo").Find("Str").Find("SlowText").gameObject.GetComponent<Text>().text = this.slow.ToString();
            ui.transform.Find("SlowInfo").Find("Duration").Find("DurationText").gameObject.GetComponent<Text>().text = this.duration.ToString();
        }else if(title == "Trap"){
            ui.transform.Find("Name").gameObject.GetComponent<TMP_Text>().text = this.title;
            ui.transform.Find("Level").gameObject.GetComponent<TMP_Text>().text = string.Format("Level {0}",this.level);
            ui.transform.Find("TrapInfo").Find("Str").Find("StrText").gameObject.GetComponent<Text>().text = this.damage.ToString();
        }
        
    }
}
