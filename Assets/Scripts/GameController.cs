using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject[] hazards;
    public GameObject background;
    public GameObject starfieldFront;
    public GameObject starfieldBack;
    public float fSliderValue;
    public float bSliderValue;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;
    public AudioClip[] music;
    public int powerUps;

    public Text scoreText;
    public Text restartText;
    public Text gameOverText;
    public Text shotText;

    private int score;
    private bool timeAttack;
    private bool gameOver;
    private bool restart;
    private bool timerOver;
    private ParticleSystem front;
    private ParticleSystem back;
    private int backgroundSpeed;
    private Component speed;
    private AudioSource audioSource;

    private void Start()
    {
        timerOver = false;
        gameOver = false;
        restart = false;
        gameOverText.text = "";
        shotText.text = "";
        restartText.text = "Press 'Q' for Time Attack mode!";
        score = 0;
        powerUps = 0;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = music[0];
        audioSource.loop = true;
        audioSource.Play();
        UpdateScore();
        StartCoroutine(SpawnWaves());
        front = starfieldFront.GetComponent<ParticleSystem>();
        back = starfieldBack.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        var mainFront = front.main; //starfield front
        mainFront.simulationSpeed = fSliderValue;
        var mainBack = back.main;   //starfield back
        mainBack.simulationSpeed = bSliderValue;

        if (restart)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.LoadScene("Main");
            }
        }
        if (Input.GetKey("escape"))
            Application.Quit();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (gameOver == false)
            {
                timeAttackStart();
            }
        }
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);
            if (gameOver)
            {
                restartText.text = "Press 'E' to Restart!";
                restart = true;
                break;
            }
        }
    }

    public void AddScore(int newScoreValue)
    {
        if (gameOver == false)
        {
            score += newScoreValue;
        }
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Points: " + score;
        if (score >= 100 && timeAttack == false)
        {
            gameOverText.text = "You win! Game created by Connor Peacock.";
            gameOver = true;
            background.GetComponent<BGScroller>().scrollSpeed = -1.25F;    //NOTE: IF I HAVE TIME, FIND A WAY TO SMOOTH THIS OUT
            fSliderValue = 10.0F;
            bSliderValue = 10.0F;
            audioSource.clip = music[2];
            audioSource.loop = false;
            audioSource.Play();
        }
    }

    public void GameOver()
    {
        if (timeAttack == false)
        {
            gameOverText.text = "Game Over! Game created by Connor Peacock.";
            audioSource.clip = music[1];
            audioSource.loop = false;
            audioSource.Play();
            background.GetComponent<BGScroller>().scrollSpeed = 0.0F;    //NOTE: IF I HAVE TIME, FIND A WAY TO SMOOTH THIS OUT
            fSliderValue = 0.0F;
            bSliderValue = 0.0F;
        } else if (timeAttack == true)
        {
            gameOverText.text = "You scored " + score + " points!" + "\n" + "Game created by Connor Peacock.";
            if (timerOver == true)
                audioSource.clip = music[2];
            else if (timerOver == false)
                audioSource.clip = music[1];
            audioSource.loop = false;
            audioSource.Play();
            background.GetComponent<BGScroller>().scrollSpeed = -1.25F;    //NOTE: IF I HAVE TIME, FIND A WAY TO SMOOTH THIS OUT
            fSliderValue = 10.0F;
            bSliderValue = 10.0F;
        }
        gameOver = true;
        restartText.text = "";
    }

    public void PowerUpCollected()
    {
        powerUps += 1;
        if (powerUps >= 1 && powerUps < 5)
            shotText.text = "Attack Speed Up x" + powerUps;
        if (powerUps >= 5)
            shotText.text = "Attack Speed Max!";
    }

    public void timeAttackStart()
    {
        score = 0;
        UpdateScore();
        timeAttack = true;
        StartCoroutine(TimerCountdown());
    }

    IEnumerator TimerCountdown()
    {
        for (int i = 30; i >= 0; i--)
        {
            if (gameOver == false)
            {
                restartText.text = "Time: " + i;
            }
            yield return new WaitForSeconds(1);
        }
        timerOver = true;
        GameOver();
    }
}
