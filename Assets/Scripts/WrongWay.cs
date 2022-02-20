using UnityEngine;
using UnityEngine.UI;

public class WrongWay : MonoBehaviour
{
    private Image _wrongWayImage;
    private float _time;
    private CarMovementAI _carMovementAI;
    
    private void Awake()
    {
        _carMovementAI = gameObject.GetComponent<CarMovementAI>();
        _wrongWayImage = GameObject.FindWithTag("WrongWayImage").GetComponent<Image>();
        ColorAlfaZero();
    }

    private void Update()
    {
        if (!gameObject || _carMovementAI.nodes.Count == 0) return;
        
        if (Vector3.Dot(_carMovementAI.nodes[_carMovementAI.currentNode].transform.right,
                gameObject.transform.forward) < -0.3f)
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
        _wrongWayImage.color = new Color(1, 1, 1, (Mathf.Sin(_time) + 1) * 0.5f);
    }

    private void ColorAlfaZero()
    {
        _wrongWayImage.color =  new Color(1, 1, 1, 0);
    }
}
