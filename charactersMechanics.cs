using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charactersMechanics : MonoBehaviour
{
    //Параметры
    public float speedmove;//Скорость персонажа
    public float jumpPower;//сила прыжка персонажа
    //Геймплей персонажа
    private float gravityForce;//гравитация персонажа
    private Vector3 moveVector;//направелние персонажа

    //ссылки на компоненты
    private CharacterController ch_controller;
    private Animator ch_animator;
    private MobileContreller mContr;



    // Start is called before the first frame update
    void Start()
    {
        ch_controller = GetComponent<CharacterController>();
        ch_animator = GetComponent<Animator>();
        mContr = GameObject.FindGameObjectWithTag("_Joystick").GetComponent<MobileContreller>(); 

    }

    // Update is called once per frame
    void Update()
    {
        CharactersMove();  //метод движения
        GamingGravity();  //метод гравитации


    }
    //метод перемещения персонажа
    private void CharactersMove()
    {
        //премещения по поверхности
        if (ch_controller.isGrounded)
        {
            ch_animator.ResetTrigger("Jump");
            ch_animator.SetBool("Falling", false);
            moveVector = Vector3.zero;
            moveVector.x = mContr.Horizontal() * speedmove;
            moveVector.z = mContr.Vertical() * speedmove;

            //анимация персонажа
            if (moveVector.x != 0 || moveVector.z != 0) ch_animator.SetBool("Move", true);
            else ch_animator.SetBool("Move", false);

            //поворот персонажа
            if (Vector3.Angle(Vector3.forward, moveVector) > 1f || Vector3.Angle(Vector3.forward, moveVector) == 0)
            {
                Vector3 direct = Vector3.RotateTowards(transform.forward, moveVector, speedmove, 0.0f);
                transform.rotation = Quaternion.LookRotation(direct);

            }

        }
        else
        {
            if (gravityForce < -3f) ch_animator.SetBool("Falling", true);
        }
       

        moveVector.y = gravityForce;
        ch_controller.Move(moveVector * Time.deltaTime);//метод передвижения по направлению

    }
        //метод гравитации
        private void GamingGravity()
    {
        if (!ch_controller.isGrounded) gravityForce -= 20f * Time.deltaTime;
        else gravityForce = -1f;
        if (Input.GetKeyDown(KeyCode.Space) && ch_controller.isGrounded)
        {
            gravityForce = jumpPower;
            ch_animator.SetTrigger("Jump");
        }
    }

}