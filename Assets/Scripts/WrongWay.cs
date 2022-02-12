using UnityEngine;
using UnityEngine.UI;

public class WrongWay : MonoBehaviour
{
    [SerializeField] private Image wrongWayImage;
    private GameObject _playerCar;
    private float _time;
    private CarMovementAI _carMovementAI;
    
    private void Awake()
    {
        _playerCar = FindObjectOfType<WrongWay>().gameObject;
        _carMovementAI = _playerCar.GetComponent<CarMovementAI>();
        ColorAlfaZero();
    }

    private void Update()
    {
        if (Vector3.Dot(_carMovementAI.nodes[_carMovementAI.currentNode].transform.right, gameObject.transform.forward) < -0.3)
        {
            FadeInOut();
        }
        else
        {
            ColorAlfaZero();
        }
    }

    private void FadeInOut()
    {
        _time += Time.deltaTime * 2;
        wrongWayImage.color = new Color(1, 1, 1, (Mathf.Sin(_time) + 1) * 0.5f);
    }

    private void ColorAlfaZero()
    {
        wrongWayImage.color =  new Color(1, 1, 1, 0);
    }
}
