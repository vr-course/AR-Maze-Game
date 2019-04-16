using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    Player player;
    public Transform OriginTransform;
    bool Die;
    bool Walking;
    bool PreviousState;
    float angle;
    GameController gameController;
    void OnEnable()
    {
        EasyJoystick.On_JoystickMove += OnJoystickMove;
    }

    void OnJoystickMoveEnd(MovingJoystick move)
    {
       // Walking = false;
        //idle animation
    }
    void OnJoystickMove(MovingJoystick move)
    {
        if (move.joystickName != "PlayerJoystick")       //  在这里的名字new joystick 就是上面所说的很重要的名字，在上面图片中joystickName的你修改了什么名字，这里就要写你修改的好的名字（不然脚本不起作用）。
        {
            return;
        }
        float PositionX = move.joystickAxis.x;       //   获取摇杆偏移摇杆中心的x坐标
        float PositionY = move.joystickAxis.y;      //    获取摇杆偏移摇杆中心的y坐标
        angle = Mathf.Acos(PositionY / (Mathf.Sqrt(PositionX * PositionX + PositionY * PositionY)));

      //  transform.localEulerAngles = new Vector3(0, angle, 0);
      //  transform.Translate(transform.forward * player.Speed * Time.deltaTime);
        if (PositionY > 0)
        {
            
            //forward animation
            if (PositionX > 0)
                transform.Rotate(0, angle, 0);
            else
                transform.Rotate(0, -angle, 0);
            transform.position += transform.forward * player.Speed * Time.deltaTime * (Mathf.Sqrt(PositionX * PositionX + PositionY * PositionY));

        }
        else
        {
            //backward animation
            if (PositionX < 0)
                transform.Rotate(0, angle, 0);
            else
                transform.Rotate(0, -angle, 0);
            transform.position -= transform.forward * player.Speed * Time.deltaTime * (Mathf.Sqrt(PositionX * PositionX + PositionY * PositionY));

        }

    } 
    void Start()
    {
        OriginPlayerSetting();
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();

    }

    void OriginPlayerSetting()
    {
        transform.position = OriginTransform.position;
        transform.rotation = OriginTransform.rotation;
        Die = false;
        Walking = false;
        player = new Player();

    }
    // Update is called once per frame
    void Update()
    {
        if (PreviousState == true && gameController.Playing == false)
            OriginPlayerSetting();
        PreviousState = gameController.Playing;
    }


    bool IsEqual(float a, float b)
    {
        if (a >= b - Mathf.Epsilon && a <= b + Mathf.Epsilon)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
