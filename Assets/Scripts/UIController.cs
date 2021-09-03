using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Text _scoreText;

    [SerializeField] private Image _livesImg;

    [SerializeField] private Sprite[] _livesSprites;

    [SerializeField] private Text _gameOverText;

    [SerializeField] private Text _PressRKeyText;

    private GameManager _gameManager;
    
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _PressRKeyText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_manager").GetComponent<GameManager>();

        if (_gameManager==null)
        {
            Debug.Log("GameManager is NULL");
        }
    }

    public void UpdatePlayerScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdatePlayerLives(int currentLife)
    {
        _livesImg.sprite = _livesSprites[currentLife];

        if(currentLife == 0)
        {
            GameOverSequence();
        }
    }

    private void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        StartCoroutine(FlickerGameOverText());
        _PressRKeyText.gameObject.SetActive(true);
        StartCoroutine(EnableRText());
    }

    private IEnumerator FlickerGameOverText()
    {
        while (true)
        {
            _gameOverText.text = "Game Over, try again";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator EnableRText()
    {
        while (true)
        {
            _PressRKeyText.text = "Press the 'R' key to restart";
            yield return new WaitForSeconds(0.5f);
            _PressRKeyText.text = "";
            yield return new WaitForSeconds(0.5f);
        }

    }
}
