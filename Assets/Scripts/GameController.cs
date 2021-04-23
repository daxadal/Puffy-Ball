using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public BallController ball;
    public GameObject[] spawnPrefabs;
    public int[] chances;
    int totalChance;

    int poolSize = 5;
    GameObject[,] prefabPool;

    const float NormalSpeed = 5f, NormalSpawn = 2f, BoostMultiplier = 3f;

    public float spawnInterval { get { return isBoosted ? NormalSpawn / BoostMultiplier : NormalSpawn; } }
    public float scrollSpeed { get { return isBoosted ? NormalSpeed * BoostMultiplier : NormalSpeed; } }

    float spawnX = 15f, spawnYMin = -2.5f, spawnYMax = 4.5f;
    public float destroyX { get { return -15f; } }

    public bool isGameOver { get; private set; }
    public bool isBoosted;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        prefabPool = new GameObject[spawnPrefabs.Length, poolSize];
        for (int i = 0; i < spawnPrefabs.Length; i++)
            for (int j = 0; j < poolSize; j++)
            {
                prefabPool[i, j] = Instantiate(spawnPrefabs[i], this.transform.position, spawnPrefabs[i].transform.rotation);
                prefabPool[i, j].SetActive(false);
                prefabPool[i, j].transform.parent = this.transform;
            }

        totalChance = 0;
        foreach (int chance in chances)
            totalChance += chance;
    }

    // Start is called before the first frame update
    void Start()
    {
        UIController.instance.SetMainMenu();
        isGameOver = true;
        isBoosted = false;
    }

    IEnumerator SpawnRoutine()
    {
        while (!isGameOver)
        {
            GameObject instance = RandomPrefab();
            if (instance)
            {
                instance.transform.position = RandomPosition();
                instance.SetActive(true);
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private GameObject RandomPrefab()
    {
        int marker = Random.Range(0, totalChance+1);
        Debug.Log($"Marker: {marker}");
        int prefabIndex = 0;
        for (; prefabIndex < chances.Length; prefabIndex++)
        {
            marker -= chances[prefabIndex];
            if (marker <= 0) break;
        }
        Debug.Log($"Index: {prefabIndex}, Prefab: {spawnPrefabs[prefabIndex].name}");

        int j = 0;
        while (j < poolSize && prefabPool[prefabIndex, j].activeSelf)
            j++;

        if (j < poolSize)
            return prefabPool[prefabIndex, j];
        else
            return null;
    }

    private Vector3 RandomPosition()
    {
        return new Vector3(spawnX, Random.Range(spawnYMin, spawnYMax));
    }

    public void GameOver()
    {
        isGameOver = true;
        UIController.instance.SetGameOverMenu();
        StopAllCoroutines();
    }

    public void StartGame()
    {
        UIController.instance.SetGameMenu();
        isGameOver = false;
        ball.ResetBall();

        StartCoroutine(SpawnRoutine());
    }

    public void RestartGame()
    {
        UIController.instance.SetGameMenu();
        isGameOver = false;
        ball.ResetBall();

        StartCoroutine(SpawnRoutine());

        for (int i = 0; i < spawnPrefabs.Length; i++)
            for (int j = 0; j < poolSize; j++)
            {
                prefabPool[i, j].SetActive(false);
            }
    }
}
