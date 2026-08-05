using System.Collections;
using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI _textUI;
    [SerializeField]private float _coinSpawnTime = 1f;
    private bool _startCoinSpawn = false;
    public bool StartCoinSpawn 
    { 
        get => _startCoinSpawn;
        set
        {
            _startCoinSpawn = value;
        }
    }
    

    private int currentCoinCount = 0;

    public void Start()
    {
        StopAllCoroutines();
        if( _textUI == null)return;
        StartCoroutine(CoinSpawnCoroutine());
    }

     IEnumerator CoinSpawnCoroutine()
    {
        while(true)
        {
            if (_startCoinSpawn)
            {
                yield return new WaitForSeconds(_coinSpawnTime);
                currentCoinCount++;
                _textUI.text = currentCoinCount.ToString();
                Debug.Log("coin increased");
            }
            else
            {
                yield return null;
            }
            
            
        }
    }
}
