using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour
{
    public Transform[,] grid;// Матрица, представляющая игровое поле как двумерный массив трансформов (объектов)
    public int width, height;// Ширина и высота игрового поля
    public UIText uiText; // Ссылка на компонент UIText для обновления интерфейса

    void Start()
    {
        grid = new Transform[width, height];  // Инициализация игрового поля при запуске игры
    }

    // Обновляет игровое поле на основе текущего положения тетромино
    public void UpdateGrid(Transform tetromino)
    {
        // Очищаем ячейки, которые занимает текущее тетромино
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y] != null && grid[x, y].parent == tetromino)
                {
                    grid[x, y] = null;
                }
            }
        }

        // Заполняем новыми значениями ячейки, которые занимает тетромино
        foreach (Transform mino in tetromino)
        {
            Vector2 pos = Round(mino.position);  // Округляем позицию блока тетромино до целых чисел
            if (pos.y < height)
            {
                grid[(int)pos.x, (int)pos.y] = mino;
            }
        }
    }

    // Округляет вектор до ближайшего целого числа
    public static Vector2 Round(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }

    
    // Проверяет, находится ли позиция внутри границ игрового поля
    public bool IsInsideBorder(Vector2 pos)
    {
        return (int)pos.x >= 0 && (int)pos.x < width && (int)pos.y >= 0 && (int)pos.y < height;
    }

    // Возвращает трансформ объекта в указанной позиции на игровом поле
    public Transform GetTransformAtGridPosition(Vector2 pos)
    {
        if (pos.y > height - 1)
        {
            return null;
        }
        return grid[(int)pos.x, (int)pos.y];
    }

    // Проверяет, является ли текущее положение тетромино допустимым
    public bool IsValidPosition(Transform tetromino)
    {
        foreach (Transform mino in tetromino)
        {
            Vector2 pos = Round(mino.position);  // Округляем позицию блока тетромино
            if (!IsInsideBorder(pos))  // Проверяем, что позиция внутри границ поля
            {
                return false;
            }

            // Проверяем, что на позиции нет другого тетромино или части другого тетромино
            if (GetTransformAtGridPosition(pos) != null && GetTransformAtGridPosition(pos).parent != tetromino)
            {
                return false;
            }
        }
        return true;
    }

    // Проверяет игровое поле на наличие заполненных линий и удаляет их
    public void CheckForLines()
    {
        int linesCleared = 0;  // Счетчик удаленных линий
        for (int y = 0; y < height; y++)
        {
            if (LineIsFull(y))  // Если текущая строка полностью заполнена
            {
                DeleteLine(y);  // Удаляем эту строку
                DecreaseRowsAbove(y + 1);  // Сдвигаем строки выше на одну вниз
                y--;  // Уменьшаем счетчик строк, так как следующая строка сдвинется в текущую
                linesCleared++;  // Увеличиваем счетчик удаленных линий
            }
        }

        // Если удалены какие-то линии, обновляем интерфейс счета и уровня
        if (linesCleared > 0)
        {
            uiText.AddLines(linesCleared);  // Добавляем количество удаленных линий в интерфейс
            uiText.AddScore(linesCleared * 100);  // Добавляем очки за удаленные линии в интерфейс

            int currentScore = uiText.GetScore();  // Получаем текущий счет

            // Если текущий счет больше или равен 1000, рассчитываем новый уровень
            if (currentScore >= 1000)
            {
                int newLevel = currentScore / 1000;

                // Если новый уровень выше текущего, обновляем уровень в интерфейсе
                if (newLevel > uiText.GetLevel())
                {
                    uiText.UpdateLevel(newLevel);
                }

                // Вычисляем количество очков для вычитания для перехода на новый уровень
                int pointsToSubtract = (newLevel - uiText.GetLevel()) * 1000;
                uiText.AddScore(-pointsToSubtract);  // Вычитаем очки из счета
            }
        }
    }

    // Проверяет, является ли указанная строка полностью заполненной
    bool LineIsFull(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }
        return true;
    }

    // Удаляет указанную строку из игрового поля
    void DeleteLine(int y)
    {
        for (int x = 0; x < width; x++)
        {
            Destroy(grid[x, y].gameObject);  // Уничтожаем игровой объект в указанной позиции
            grid[x, y] = null;  // Освобождаем ячейку в матрице игрового поля
        }
    }

    // Сдвигает все строки выше указанной строки на одну вниз
    void DecreaseRowsAbove(int startRow)
    {
        for (int y = startRow; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y] != null)
                {
                    grid[x, y - 1] = grid[x, y];  // Сдвигаем трансформ объекта на одну строку вниз
                    grid[x, y] = null;  // Освобождаем текущую ячейку
                    grid[x, y - 1].position += Vector3.down;  // Сдвигаем позицию объекта вниз
                }
            }
        }
    }
}
