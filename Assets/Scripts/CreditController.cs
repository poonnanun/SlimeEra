using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditController : MonoBehaviour
{
    public void BackToMain(){
        print(1);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
