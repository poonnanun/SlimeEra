using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class SceneController : MonoBehaviour
{
    public Sprite musicOn;
    public Sprite musicOff;
    public Button musicB;
    public bool music;
    public AudioMixer mixer;
    private void Start() {
        music = true;
    }
    public void Credits(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("Credits", LoadSceneMode.Single);
    }
    public void Play(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
    public void MusicSwitch(){
        if(music){
            music = false;
            musicB.gameObject.GetComponent<Image>().sprite = musicOff;
            mixer.SetFloat("Bg", -80f);
        }else{
            music = true;
            musicB.gameObject.GetComponent<Image>().sprite = musicOn;
            mixer.SetFloat("Bg", -10f);
        }
    }
}
