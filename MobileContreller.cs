using UnityEngine;
using UnityEngine.UI;           //библиотека на интерфейс
using UnityEngine.EventSystems;//билблиотека на события

public class MobileContreller : MonoBehaviour,IDragHandler,IPointerUpHandler,IPointerDownHandler//виртуальные библиотеки
{
    private Image _JoystickBG;
    [SerializeField]            //проверка в инспекторе
    private Image _Joystick;
    private Vector2 inputVector;// получение координатов джойстика

    private void Start()
    {
        _JoystickBG = GetComponent<Image>();

       _Joystick = transform.GetChild(0).GetComponent<Image>();
         
    }
    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }
    public virtual void OnPointerUp(PointerEventData ped)
    {
        inputVector = Vector2.zero;

        _Joystick.rectTransform.anchoredPosition = Vector2.zero;
    }
    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(_JoystickBG.rectTransform,ped.position,ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / _JoystickBG.rectTransform.sizeDelta.x); //полученик координат позиции касания на джойстик
            pos.y = (pos.y / _JoystickBG.rectTransform.sizeDelta.y); // получение координат позиции касания на джойстик по y

            inputVector = new Vector2(pos.x * 2 - 1, pos.y * 2 - 1); //установка точных координат из касания
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            _Joystick.rectTransform.anchoredPosition = new Vector2(inputVector.x * (_JoystickBG.rectTransform.sizeDelta.x / 2), inputVector.y * (_JoystickBG.rectTransform.sizeDelta.y / 2));
             
        }
    }
    public float Horizontal()
    {
        if (inputVector.x != 0) return inputVector.x;
        else return Input.GetAxis("Horizontal");

    
    }
    public float Vertical()
        {
        if (inputVector.y != 0) return inputVector.y;
        else return Input.GetAxis("Vertical");

    }
}
