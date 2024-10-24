using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Cube : MonoBehaviour
{
    private const string PlatformTag = "Platform";

    [SerializeField] private float _minLifetime = 2f;
    [SerializeField] private float _maxLifetime = 5f;
    [SerializeField] private float _timeToLive;
    
    private bool _hasCollided = false;
    private Renderer _cubeRenderer;
    private IObjectPool<Cube> _pool;

    private void Awake()
    {
        _cubeRenderer = GetComponent<Renderer>();
        ResetCube();
    }

    public void SetPool(IObjectPool<Cube> objectPool)
    {
        _pool = objectPool;
    }

    public void ResetCube()
    {
        _hasCollided = false;
        _cubeRenderer.material.color = Color.white;
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_hasCollided && collision.gameObject.CompareTag(PlatformTag))
        {
            _hasCollided = true;
            _cubeRenderer.material.color = GetRandomColor();
            _timeToLive = Random.Range(_minLifetime, _maxLifetime);
            StartCoroutine(LifetimeCoroutine());
        }
    }

    private IEnumerator LifetimeCoroutine()
    {
        yield return new WaitForSeconds(_timeToLive);
        _pool.Release(this);
    }

    private Color GetRandomColor()
    {
        return new Color(Random.value, Random.value, Random.value);
    }
}