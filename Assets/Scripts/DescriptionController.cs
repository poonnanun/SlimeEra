using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionController : MonoBehaviour
{
    public int number;
    private GameManager gameManager;

    private void Start() {
        gameManager = FindObjectOfType<GameManager>();
    }
    private void OnMouseEnter() {
        gameManager.Describe(number);
    }
}
