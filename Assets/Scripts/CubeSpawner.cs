using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private Transform _spawnArea;
    [SerializeField] private Transform _cubeHolder;
    [SerializeField] private float _spawnTime = .1f;

    private ObjectPool<Cube> _cubePool;

    private void Start()
    {
        _cubePool = new ObjectPool<Cube>(
            CreateCube,
            OnGetCube,
            OnReleaseCube,
            OnDestroyCube
        );

        StartCoroutine(SpawnCoroutine());
    }
    
    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            SpawnCube();
            yield return new WaitForSeconds(_spawnTime);
        }
    }

    private Cube CreateCube()
    {
        var cubeObj = Instantiate(_cubePrefab, _cubeHolder);
        return cubeObj.GetComponent<Cube>();
    }

    private void OnGetCube(Cube cube)
    {
        cube.gameObject.SetActive(true);
        Vector3 randomPosition = GetRandomSpawnPosition();
        cube.transform.position = randomPosition;
    }

    private void OnReleaseCube(Cube cube)
    {
        cube.ResetCube();
    }

    private void OnDestroyCube(Cube cube)
    {
        Destroy(cube.gameObject);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float randomX = Random.Range(_spawnArea.position.x - _spawnArea.localScale.x / 2,
            _spawnArea.position.x + _spawnArea.localScale.x / 2);
        float randomZ = Random.Range(_spawnArea.position.z - _spawnArea.localScale.z / 2,
            _spawnArea.position.z + _spawnArea.localScale.z / 2);
        return new Vector3(randomX, _spawnArea.position.y, randomZ);
    }

    private void SpawnCube()
    {
        var cube = _cubePool.Get();
        cube.SetPool(_cubePool);
    }
}