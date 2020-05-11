using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadSprite : MonoBehaviour
{
    public Sprite[] sprites;

    public Sprite GetSprite(string name){
        foreach(Sprite s in sprites){
            if(s.name == name){
                return s;
            }
        }
        return null;
    }
}
