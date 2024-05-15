using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class StartMenu : MonoBehaviour
{

    [SerializeField] TMP_InputField passwordInputField;

    public void StartGame(){
        if(passwordInputField.text !=""){
            PlayerPrefs.SetString("username", passwordInputField.text);
            SceneManager.LoadScene("main");
        }
        
    }
}
