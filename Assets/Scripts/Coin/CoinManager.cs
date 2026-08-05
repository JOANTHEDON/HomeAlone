using System.Collections;
using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI _textUI;
    [SerializeField]private float _coinSpawnTime = 1f;
    [SerializeField]private bool _startCoinSpawn = false;
    public bool StartCoinSpawn() => _startCoinSpawn;

    private int currentCoinCount = 0;

    public void Start()
    {
        StopAllCoroutines();
        if( _textUI == null)return;
        StartCoroutine(CoinSpawnCoroutine());
    }

     IEnumerator CoinSpawnCoroutine()
    {
        while(_startCoinSpawn == true)
        {
            yield return new WaitForSeconds(_coinSpawnTime);
            currentCoinCount++;
            _textUI.text = currentCoinCount.ToString();
            Debug.Log("coin increased");
            
        }
    }
}
