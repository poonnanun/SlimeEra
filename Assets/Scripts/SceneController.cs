using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public Sprite soundOn;
    public Sprite soundOff;
    public Sprite musicOn;
    public Sprite musicOff;
    public Button musicB;
    public Button soundB;
    public bool sound;
    public bool music;
    private void Start() {
        sound = true;
        music = true;
    }
    public void Credits(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("Credits", LoadSceneMode.Single);
    }
    public void Play(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
    public void SoundSwitch(){
        if(sound){
            sound = false;
            soundB.gameObject.GetComponent<Image>().sprite = soundOff;
        }else{
            sound = true;
            soundB.gameObject.GetComponent<Image>().sprite = soundOn;
        }
    }
    public void MusicSwitch(){
        if(music){
            music = false;
            musicB.gameObject.GetComponent<Image>().sprite = musicOff;
        }else{
            music = true;
            musicB.gameObject.GetComponent<Image>().sprite = musicOn;
        }
    }
}
