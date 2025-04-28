using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIText : MonoBehaviour
{
    public GameObject levelText;  // Ссылка на текстовый объект для отображения уровня
    public GameObject scoreText;  // Ссылка для отображения счета
    public GameObject lineText;   // Ссылка для отображения количества линий

    private int score = 0;  // Текущий счет игрока
    private int level = 0;  // Текущий уровень игры
    private int lines = 0;  // Количество удаленных линий

    private void Update()
    {
        levelText.GetComponent<Text>().text = "Level: " + level;  // Обновление
        scoreText.GetComponent<Text>().text = "Score: " + score;  
        lineText.GetComponent<Text>().text = "Lines: " + lines;   
    }

    public void UpdateScore(int newScore)
    {
        score = newScore;  // Обновление текущего счета
        Update();          // Вызов метода Update для обновления текста на экране
    }

    public void UpdateLevel(int newLevel)
    {
        level = newLevel;  // Обновление текущего уровня
        Update();          
    }

    public void AddScore(int points)
    {
        score += points;  // Добавление указанного количества очков к текущему счету
        Update();         
    }

    public void AddLines(int newLines)
    {
        lines += newLines;  // Добавление указанного количества линий к общему числу удаленных линий
        Update();           
    }

    public int GetLevel()
    {
        return level;  // Возвращает текущий уровень
    }

    public int GetScore()
    {
        return score;  // Возвращает текущий счет
    }
}
