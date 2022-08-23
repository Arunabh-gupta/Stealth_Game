using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UiManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject gameWinUI;
    public GameObject gameLoseUI;
    bool gameOver = false;
    
    public void showGameWinUI(){
        gameWinUI.SetActive(true);
        gameOver = true;
    }
    public void showGameLoseUI(){
        gameLoseUI.SetActive(true);
        gameOver = true;
    }
    public void newScene(){
        if(Input.GetKeyDown(KeyCode.Space)){
            SceneManager.LoadScene(0);
        }
    }
}
