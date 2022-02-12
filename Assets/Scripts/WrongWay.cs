using UnityEngine;
using UnityEngine.UI;

public class WrongWay : MonoBehaviour
{
    [SerializeField] private Image wrongWayImage;
    private GameObject _playerCar;
    private float _time;
    
    private void Awake()
    {
        _playerCar = FindObjectOfType<WrongWay>().gameObject;
    }

    private void Update()
    {
        _time += Time.deltaTime * 2;
        wrongWayImage.color =  new Color(1, 1, 1,(Mathf.Sin(_time) + 1) * 0.5f);
    }
}
