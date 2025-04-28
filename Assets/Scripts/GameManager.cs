using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] Tetrominos;  // Массив префабов тетромино для случайного выбора
    public float movementFrequency = 0.8f;  // Частота движения тетромино вниз
    private float passedTime = 0;  // Время, прошедшее с последнего движения тетромино
    private GameObject currentTetromino;  // Текущее тетромино на игровом поле
    private GameObject nextTetromino;  // Следующее тетромино для отображения
    public UIText uiText;  // Ссылка на компонент UIText для обновления интерфейса
    public Transform nextTetrominoDisplay;  // Позиция для отображения следующего тетромино

    void Start()
    {
        SpawnNextTetromino();  // Создаем первое следующее тетромино
        SpawnTetromino();  // Создаем текущее тетромино
    }

    void Update()
    {
        passedTime += Time.deltaTime;  // Обновляем прошедшее время
        if (passedTime >= movementFrequency)
        {
            passedTime -= movementFrequency;
            MoveTetromino(Vector3.down);  // Двигаем текущее тетромино вниз по истечении времени
        }
        UserInput();  // Обрабатываем пользовательский ввод

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();  // Выход из игры при нажатии Escape
        }
    }

    void QuitGame()
    {
        Application.Quit();  // Выход из приложения
    }

    void UserInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveTetromino(Vector3.left);  // Двигаем текущее тетромино влево
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveTetromino(Vector3.right);  // Двигаем текущее тетромино вправо
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            RotateTetromino();  // Поворачиваем текущее тетромино по часовой стрелке
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            movementFrequency = 0.2f;  // Ускоряем движение тетромино вниз при удержании клавиши
        }
        else
        {
            movementFrequency = 0.8f;  // Возвращаем стандартную частоту движения тетромино вниз
        }
    }

    void RotateTetromino()
    {
        if (currentTetromino == null) return;

        currentTetromino.transform.Rotate(0, 0, 90);  // Поворот текущего тетромино на 90 градусов по часовой стрелке
        if (!IsValidPosition())
        {
            currentTetromino.transform.Rotate(0, 0, -90);  // Возврат в исходное положение при невозможности поворота
        }
    }

    void SpawnTetromino()
    {
        if (nextTetromino == null)
        {
            SpawnNextTetromino();  // Создаем следующее тетромино, если оно отсутствует
        }
        currentTetromino = Instantiate(nextTetromino, new Vector3(5, 18, 0), Quaternion.identity);  // Создаем текущее тетромино
        SpawnNextTetromino();  // Создаем новое следующее тетромино
    }

    void SpawnNextTetromino()
    {
        if (nextTetromino != null)
        {
            Destroy(nextTetromino);  // Уничтожаем предыдущее следующее тетромино
        }
        int index = Random.Range(0, Tetrominos.Length);  // Случайным образом выбираем индекс тетромино из массива
        nextTetromino = Instantiate(Tetrominos[index], nextTetrominoDisplay.position, Quaternion.identity);  // Создаем новое следующее тетромино

        Collider2D collider = nextTetromino.GetComponent<Collider2D>();  // Получаем компонент коллайдера следующего тетромино
        if (collider != null)
        {
            collider.enabled = false;  // Отключаем коллайдер для корректного отображения следующего тетромино
        }

        Rigidbody2D rb = nextTetromino.GetComponent<Rigidbody2D>();  // Получаем компонент Rigidbody2D следующего тетромино
        if (rb != null)
        {
            rb.isKinematic = true;  // Делаем Rigidbody2D следующего тетромино кинематическим для статического позиционирования
        }
    }

    void MoveTetromino(Vector3 direction)
    {
        if (currentTetromino == null) return;  // Проверяем наличие текущего тетромино

        currentTetromino.transform.position += direction;  // Двигаем текущее тетромино в заданном направлении
        if (!IsValidPosition())  // Проверяем допустимость новой позиции тетромино
        {
            currentTetromino.transform.position -= direction;  // Возвращаем тетромино на предыдущую позицию
            if (direction == Vector3.down)  // Если двигаем вниз и позиция недопустима
            {
                GetComponent<GridScript>().UpdateGrid(currentTetromino.transform);  // Обновляем игровое поле
                CheckForLines();  // Проверяем и удаляем заполненные линии
                SpawnTetromino();  // Создаем новое текущее тетромино
            }
        }
    }

    bool IsValidPosition()
    {
        return GetComponent<GridScript>().IsValidPosition(currentTetromino.transform);  // Проверяем допустимость позиции текущего тетромино на игровом поле
    }

    void CheckForLines()
    {
        GetComponent<GridScript>().CheckForLines();  // Проверяем игровое поле на наличие заполненных линий и удаляем их
    }
}
