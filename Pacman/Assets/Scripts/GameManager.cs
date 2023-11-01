using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private Maze mazePrefab;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject playButton;
    [SerializeField] private TextMeshProUGUI lifes;
    [SerializeField] private TextMeshProUGUI score;
    private Maze maze;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (maze != null)
        {
            lifes.text = "LIFES: " + maze.GetLifes();
            score.text = "SCORE: " + maze.GetScore();
        }
    }

    public void LoadMaze()
    {
        playButton.SetActive(false);
        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);
        maze = Instantiate(mazePrefab);
        maze.GenPlayer();
    }

    public void GameOver()
    {
        lifes.text = "LIFES: " + maze.GetLifes();
        score.text = "SCORE: " + maze.GetScore();
        Destroy(maze.gameObject);
        gameOverPanel.SetActive(true);
        playButton.SetActive(true);
    }

    public void Victory()
    {
        lifes.text = "LIFES: " + maze.GetLifes();
        score.text = "SCORE: " + maze.GetScore();
        Destroy(maze.gameObject);
        victoryPanel.SetActive(true);
        playButton.SetActive(true);
    }
}
