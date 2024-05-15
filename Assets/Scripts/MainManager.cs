using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System;
using TMPro;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;

    [SerializeField] TextMeshProUGUI bestScore;

    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;
    private string username;

    
    // Start is called before the first frame update
    void Start()
    {
        username = PlayerPrefs.GetString("username");
        ScoreText.text = username;
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        LoadScore();
        WriteBestScore();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = UnityEngine.Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"User: {username} --> Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        string bestScoreString = PlayerPrefs.GetString("bestScore");
        Debug.Log(bestScoreString);
        int bestScore = Int32.Parse(bestScoreString);
        if(m_Points > bestScore){
            SaveScore();
            WriteBestScore();
        }
        GameOverText.SetActive(true);
    }

    [System.Serializable]
    private class UserScore
    {
        public string username;
        public int score;
    }

    public void SaveScore()
    {
        UserScore data = new UserScore();
        data.username = username;
        data.score = m_Points;

        string json = JsonUtility.ToJson(data);
    
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            UserScore data = JsonUtility.FromJson<UserScore>(json);
            PlayerPrefs.SetString("bestUser", data.username);
            PlayerPrefs.SetString("bestScore", data.score.ToString());
        }
    }

    private void WriteBestScore(){
        string bestUsernameText = PlayerPrefs.GetString("bestUser");
        string bestScoreText = PlayerPrefs.GetString("bestScore");
        if(bestUsernameText == "" || bestUsernameText == null){
            PlayerPrefs.SetString("bestUser", "");
            PlayerPrefs.SetString("bestScore", "0");
        }
        bestScore.text = $"Best score: {bestUsernameText} --> {bestScoreText}";
    }
}
