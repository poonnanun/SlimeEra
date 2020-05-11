using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface Upgrade{
    void Effect(GameObject tower);
    int GetRarity();
    string GetName();
    int GetId();
    string GetTag();
    string GetDescription();
    Sprite GetSprite();
}

public class PowerUp: Upgrade{
    private int id;
    private string name;
    private int rarity;
    private string tag;
    private string description;
    public Sprite sprite;
    private LoadSprite loadSprite;
    public PowerUp(){
        loadSprite = GameObject.FindObjectOfType<LoadSprite>();
        tag = "Anyone";
        id = 1;
        name = "PowerUp";
        rarity = 1;
        description = "Increase the damage by 1.5x";
        sprite = loadSprite.GetSprite(name);
    }
    public int GetRarity(){
        return rarity;
    }
    public string GetName(){
        return name;
    }
    public int GetId(){
        return id;
    }
    public string GetTag(){
        return tag;
    }
    public string GetDescription(){
        return description;
    }
    public Sprite GetSprite(){
        return sprite;
    }
    public void Effect(GameObject tower){
        if(tower.tag == "Gunner"){
            TurretController sc = tower.GetComponent<TurretController>();
            sc.SetDamage((int)Mathf.Round((float)(sc.GetDamage()*1.5)));
        }else if(tower.tag == "Miner"){
            MinerController sc = tower.GetComponent<MinerController>();
            sc.SetMineStr((int)Mathf.Round((float)(sc.GetMineStr()*1.5)));
        }else if(tower.tag == "Slow"){
            TrapController sc = tower.GetComponent<TrapController>();
            sc.SetSlow((float)(sc.GetSlow()*1.5));
        }else if(tower.tag == "Trap"){
            TrapController sc = tower.GetComponent<TrapController>();
            sc.SetDamage((int)Mathf.Round((float)(sc.GetDamage()*1.5)));
        }
    }
}

public class SpeedUp: Upgrade{
    private int id;
    private string name;
    private int rarity;
    private string tag;
    private string description;
    public Sprite sprite;
    private LoadSprite loadSprite;
    public SpeedUp(){
        loadSprite = GameObject.FindObjectOfType<LoadSprite>();
        tag = "Anyone";
        id = 2;
        name = "SpeedUp";
        rarity = 1;
        description = "Increase the speed by 1.25x";
        sprite = loadSprite.GetSprite(name);
    }
    public int GetRarity(){
        return rarity;
    }
    public string GetName(){
        return name;
    }
    public int GetId(){
        return id;
    }
    public string GetTag(){
        return tag;
    }
    public string GetDescription(){
        return description;
    }
    public Sprite GetSprite(){
        return sprite;
    }
    public void Effect(GameObject tower){
        if(tower.tag == "Gunner"){
            TurretController sc = tower.GetComponent<TurretController>();
            sc.SetSpeed(sc.GetSpeed()*1.25f);
        }else if(tower.tag == "Miner"){
            MinerController sc = tower.GetComponent<MinerController>();
            sc.SetSpeed(sc.GetSpeed()*1.25f);
        }else if(tower.tag == "Slow"){
            TrapController sc = tower.GetComponent<TrapController>();
            sc.SetSpeed(sc.GetSpeed()*1.25f);
        }else if(tower.tag == "Trap"){
            TrapController sc = tower.GetComponent<TrapController>();
            sc.SetSpeed(sc.GetSpeed()*1.25f);
        }
    }
}
public class PowerUp1: Upgrade{
    private int id;
    private string name;
    private int rarity;
    private string tag;
    private string description;
    public Sprite sprite;
    private LoadSprite loadSprite;
    public PowerUp1(){
        loadSprite = GameObject.FindObjectOfType<LoadSprite>();
        tag = "Anyone";
        id = 3;
        name = "PowerUp1";
        rarity = 3;
        description = "Increase the damage by 1";
        sprite = loadSprite.GetSprite(name);
    }
    public int GetRarity(){
        return rarity;
    }
    public string GetName(){
        return name;
    }
    public int GetId(){
        return id;
    }
    public string GetTag(){
        return tag;
    }
    public string GetDescription(){
        return description;
    }
    public Sprite GetSprite(){
        return sprite;
    }
    public void Effect(GameObject tower){
        if(tower.tag == "Gunner"){
            TurretController sc = tower.GetComponent<TurretController>();
            sc.SetDamage((int)Mathf.Round((float)(sc.GetDamage()+1)));
        }else if(tower.tag == "Miner"){
            MinerController sc = tower.GetComponent<MinerController>();
            sc.SetMineStr((int)Mathf.Round((float)(sc.GetMineStr()+1)));
        }else if(tower.tag == "Slow"){
            TrapController sc = tower.GetComponent<TrapController>();
            sc.SetSlow((float)(sc.GetSlow()+1));
        }else if(tower.tag == "Trap"){
            TrapController sc = tower.GetComponent<TrapController>();
            sc.SetDamage((int)Mathf.Round((float)(sc.GetDamage()+1)));
        }
    }
}

public class SpeedUp1: Upgrade{
    private int id;
    private string name;
    private int rarity;
    private string tag;
    private string description;
    public Sprite sprite;
    private LoadSprite loadSprite;
    public SpeedUp1(){
        loadSprite = GameObject.FindObjectOfType<LoadSprite>();
        tag = "Anyone";
        id = 4;
        name = "SpeedUp1";
        rarity = 2;
        description = "Increase the speed by 1";
        sprite = loadSprite.GetSprite(name);
    }
    public int GetRarity(){
        return rarity;
    }
    public string GetName(){
        return name;
    }
    public int GetId(){
        return id;
    }
    public string GetTag(){
        return tag;
    }
    public string GetDescription(){
        return description;
    }
    public Sprite GetSprite(){
        return sprite;
    }
    public void Effect(GameObject tower){
        if(tower.tag == "Gunner"){
            TurretController sc = tower.GetComponent<TurretController>();
            sc.SetSpeed(sc.GetSpeed()+1f);
        }else if(tower.tag == "Miner"){
            MinerController sc = tower.GetComponent<MinerController>();
            sc.SetSpeed(sc.GetSpeed()+1f);
        }else if(tower.tag == "Slow"){
            TrapController sc = tower.GetComponent<TrapController>();
            sc.SetSpeed(sc.GetSpeed()+1f);
        }else if(tower.tag == "Trap"){
            TrapController sc = tower.GetComponent<TrapController>();
            sc.SetSpeed(sc.GetSpeed()+1f);
        }
    }
}

