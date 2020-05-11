using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Upgrade{
    void Effect(GameObject tower);
    int GetRarity();
    string GetName();
    int GetId();
    string GetTag();
}

public class DamageUp: Upgrade{
    private int id;
    private string name;
    private int rarity;
    private string tag;
    public DamageUp(){
        tag = "Gunner";
        id = 1;
        name = "DamageUp";
        rarity = 1;
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
    public void Effect(GameObject tower){
        TurretController sc = tower.GetComponent<TurretController>();
        sc.SetDamage((int)Mathf.Round((float)(sc.GetDamage()*1.5)));
    }
}