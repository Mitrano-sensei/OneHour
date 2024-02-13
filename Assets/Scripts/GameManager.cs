using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private TargetScript _targetPrefab;
    [SerializeField] private Transform _bottomLeft;
    [SerializeField] private Transform _topRight;

    [SerializeField] private Transform _targetParent;

    [SerializeField][Range(1, 10)] private int _numberOfTarget = 2;

    [SerializeField] private EnemyScript _enemyScript;
    [SerializeField][Range(1, 10)] int _numberOfEnemy = 1;
    [SerializeField] Transform _enemyParent;

    public UnityEvent OnLevelFinished = new();

    private List<EnemyScript> _enemies = new List<EnemyScript>();
    private List<TargetScript> _targetList;

    private bool _hasLost = false;

    void Start()
    {
        GenerateLevel();
        OnLevelFinished.AddListener(() => Debug.Log("Level Finished !"));
        OnLevelFinished.AddListener(() => {
            ScoreManager.Instance.AddScore(_hasLost ? 0 : 10);
            _hasLost = false;
            });
    }

    private void Update()
    {
        if (CheckCondition())
        {
            FinishLevel();
        }
    }

    private void GenerateLevel()
    {
        ClearLevel();

        for(int i = 0 ; i < _numberOfTarget; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(_bottomLeft.position.x, _topRight.position.x), 1, Random.Range(_bottomLeft.position.z, _topRight.position.z));
            _targetList.Add(Instantiate(_targetPrefab, randomPosition, Quaternion.identity, _targetParent));
        }

        for (int i = 0; i < _numberOfEnemy; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(_bottomLeft.position.x, _topRight.position.x), 1, Random.Range(_bottomLeft.position.z, _topRight.position.z));
            var enemy = Instantiate(_enemyScript, randomPosition, Quaternion.identity, _enemyParent);

            var placedCorrectly = false;

            while (!placedCorrectly)
            {
                placedCorrectly = true;
                foreach (var target in _targetList)
                {
                    if (Vector3.Distance(enemy.transform.position, target.transform.position) < 2)
                    {
                        placedCorrectly = false;
                        enemy.transform.position = new Vector3(Random.Range(_bottomLeft.position.x, _topRight.position.x), 1, Random.Range(_bottomLeft.position.z, _topRight.position.z));
                        continue;
                    }
                }
            }

            _enemies.Add(enemy);
        }

    }

    private bool CheckCondition()
    {
        foreach (var target in _targetList)
        {
            if (!target.IsActivated)
            {
                return false;
            }
        }
        return true;
    }

    private void FinishLevel()
    {
        GenerateLevel();

        OnLevelFinished.Invoke();
    }

    private void ClearLevel()
    {
        if (_targetList == null) _targetList = new List<TargetScript>();   
        if (_enemies == null) _enemies = new List<EnemyScript>();

        _targetList.Clear();
        _enemies.ForEach(e => { e.gameObject.SetActive(false); });
        _enemies.Clear();
    }

    public void Loose()
    {
        _hasLost = true;
        Debug.Log("You Loose !");

    }
}
