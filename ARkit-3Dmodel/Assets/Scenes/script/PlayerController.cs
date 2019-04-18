using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Player player;
    Animator animator;
    public Transform OriginTransform;
    bool Die;
    bool Walking;
    bool PreviousState;
    float angle;
    float ForwardForce, TurnForce;
    GameController gameController;
    void OnEnable()
    {
        EasyJoystick.On_JoystickMove += OnJoystickMove;
        EasyJoystick.On_JoystickMoveEnd += OnJoystickMoveEnd;
    }

    void OnDisable()
    {
        EasyJoystick.On_JoystickMove -= OnJoystickMove;
        EasyJoystick.On_JoystickMoveEnd -= OnJoystickMoveEnd;
    }

    void OnDestroy()
    {
        EasyJoystick.On_JoystickMove -= OnJoystickMove;
        EasyJoystick.On_JoystickMoveEnd -= OnJoystickMoveEnd;
    }
    void OnJoystickMoveEnd(MovingJoystick move)
    {
        Walking = false;
        animator.SetBool("Walk", Walking);
    }
    void OnJoystickMove(MovingJoystick move)
    {
        if (move.joystickName != "PlayerJoystick")       //  在这里的名字new joystick 就是上面所说的很重要的名字，在上面图片中joystickName的你修改了什么名字，这里就要写你修改的好的名字（不然脚本不起作用）。
        {
            return;
        }

        float PositionX = move.joystickAxis.x;       //   获取摇杆偏移摇杆中心的x坐标
        float PositionY = move.joystickAxis.y;      //    获取摇杆偏移摇杆中心的y坐标


        Walking = true;
        animator.SetBool("Walk", Walking);
        angle = move.Axis2Angle(true);
        transform.localEulerAngles = new Vector3(0, angle, 0);
        transform.localPosition += new Vector3(PositionX * Time.deltaTime * player.Speed, 0, PositionY * Time.deltaTime * player.Speed);

    }
    void Start()
    {
        animator = GetComponent<Animator>();
        OriginPlayerSetting();
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    void OriginPlayerSetting()
    {
        transform.position = OriginTransform.position;
        transform.rotation = OriginTransform.rotation;
        Die = false;
        Walking = false;
        animator.SetBool("Walk", Walking);
        player = new Player();

    }
    // Update is called once per frame
    void Update()
    {
       // player.Speed = speed;
       // player.TurnRate = turnforce;
        if (PreviousState == true && gameController.Playing == false)
            OriginPlayerSetting();
        PreviousState = gameController.Playing;
    }

    private void Walk()
    { 
        ForwardForce = Input.GetAxis("Vertical");
        TurnForce = Input.GetAxis("Horizontal");  //input
        if (IsEqual(ForwardForce, 0))
            Walking = false;
        else
            Walking = true;
        transform.position += transform.forward * player.Speed * Time.deltaTime * ForwardForce;
        transform.Rotate(0, player.TurnRate * TurnForce, 0);
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
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "coin")//子弹碰击事件
        {
            player.Money++;
            Debug.Log(player.Money);
            other.gameObject.SetActive(false);
        }
    }

}
