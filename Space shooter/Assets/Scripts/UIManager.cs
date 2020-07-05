using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _bestText;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _liveSprite;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Button _resumeButton;
    [SerializeField]
    private Button _MainMenuButton;

    private GameManager _gameManager;

    public int score;
    public int bestScore;

    // Start is called before the first frame update
    void Start()
    {
        bestScore = PlayerPrefs.GetInt("HighScore", 0);
        _bestText.text = "Best: " + bestScore;
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("GameManager is NULL");
        }
    }

    // Update is called once per frame
    public void UpdateScore(int playerScore)
    {
        score += 10;
        _scoreText.text = "Score: " + playerScore;
    }

    

    public void UpdateLives(int currentLives)
    {
        if (currentLives >= 0)
        {
            _livesImg.sprite = _liveSprite[currentLives];
        }
        
        
        if (currentLives <= 0)
        {
            GameOverSequence();
       
        }

        void GameOverSequence()
        {
            _gameOverText.gameObject.SetActive(true);
            _restartText.gameObject.SetActive(true);
            StartCoroutine(GameOverFlickerRoutine());
        }

        IEnumerator GameOverFlickerRoutine()
        {
            while(true)
            {
                _gameManager.GameOver();
                _gameOverText.text = "GAME OVER";
                yield return new WaitForSeconds(0.5f);
                _gameOverText.text = "";
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
    public void CheckForBestScore()
    {
        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("HighScore", bestScore);
            _bestText.text = "Best: " + bestScore;
        }
    }

    public void ResumePlay()
    {
        _gameManager.ResumeGame();
    }

    public void BacktoMainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
        _gameManager.GameOver();
        _gameManager.ResumeGame();
    }

}
