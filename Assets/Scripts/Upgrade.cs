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
        TurretController sc = tower.GetComponent<TurretController>();
        sc.SetDamage((int)Mathf.Round((float)(sc.GetDamage()*1.5)));
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
        id = 1;
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
        TurretController sc = tower.GetComponent<TurretController>();
        sc.SetSpeed(sc.GetSpeed()*1.25f);
    }
}