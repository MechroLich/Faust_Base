using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//ComboStarter and ComboCheck are by GrimoireHex, modfied to fit my own animator otherwise it is completely identical
public class BasicController : MonoBehaviour
{

    public Animator animator;
    private CharacterController characterController;
    public int attack = 0;

    Ray shootRay;                              
    RaycastHit shootHit;

    public float inputX;
    public float InputZ;
    public Vector3 DesiredMoveDirection;
    private static bool playerRotLock;
    public float desiredRotSpeed;
    public float Speed;
    public float allowPlayerRot;
    public Camera cam;
    public bool isgrounded;

    private Vector3 moveDirection = Vector3.zero;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    public float InputX
    {
        get
        {
            return inputX;
        }

        set
        {
            inputX = value;
        }
    }

    public static bool PlayerRotLock
    {
        get
        {
            return playerRotLock;
        }

        set
        {
            playerRotLock = value;
        }
    }

    int noOfClicks; //Determines Which Animation Will Play
    bool canClick; //Locks ability to click during animation event

    void Start()
    {
        //Initialize appropriate components
        noOfClicks = 0;
        canClick = true;

        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        cam = Camera.main;
    }

    void Update()
    {
        InputMagnitude();
        if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Fire1")) { ComboStarter(); }

        if (characterController.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        // Apply gravity
        moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
    }


    public void MoveAndRot()
         {
                InputX = Input.GetAxis("Horizontal");
                InputZ = Input.GetAxis("Vertical");

                var camera = Camera.main;
                var forward = cam.transform.forward;
                var right = cam.transform.right;

                forward.y = 0f;
                right.y = 0f;

                forward.Normalize();
                right.Normalize();

                DesiredMoveDirection = forward * InputZ + right * InputX;

                if (PlayerRotLock == false)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(DesiredMoveDirection), desiredRotSpeed);
                }
            }

    public void InputMagnitude()
            {
                //calc input vect
                InputX = Input.GetAxis("Horizontal");
                InputZ = Input.GetAxis("Vertical");

                animator.SetFloat("InputZ", InputZ, 0.0f, Time.deltaTime * 2f);
                animator.SetFloat("InputX", InputX, 0.0f, Time.deltaTime * 2f);

                //calc input magnitude
                Speed = new Vector2(InputX, InputZ).sqrMagnitude;

                //physmove player
                if (Speed > allowPlayerRot)
                {
                    animator.SetFloat("InputMagnitude", Speed, 0.0f, Time.deltaTime);
                    MoveAndRot();
                }
                else if (Speed < allowPlayerRot)
                {
                    animator.SetFloat("InputMagnitude", Speed, 0.0f, Time.deltaTime);
                }
            }

    void ComboStarter()
    {
        if (canClick)
        {
            noOfClicks++;
        }

        if (noOfClicks == 1)
        {
            animator.SetInteger("Attack", 1);
            CheckHit();
        }
    }

    public void ComboCheck()
    {

        canClick = false;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack01") && noOfClicks == 1)
        {//If the first animation is still playing and only 1 click has happened, return to idle
            animator.SetInteger("Attack", 0);
            canClick = true;
            noOfClicks = 0;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack01") && noOfClicks >= 2)
        {//If the first animation is still playing and at least 2 clicks have happened, continue the combo          
            animator.SetInteger("Attack", 2);
            CheckHit();
            canClick = true;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack02") && noOfClicks == 2)
        {  //If the second animation is still playing and only 2 clicks have happened, return to idle         
            animator.SetInteger("Attack", 0);
            canClick = true;
            noOfClicks = 0;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack02") && noOfClicks >= 3)
        {  //If the second animation is still playing and at least 3 clicks have happened, continue the combo         
            animator.SetInteger("Attack", 3);
            CheckHit();
            canClick = true;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
        { //Since this is the third and last animation, return to idle          
            animator.SetInteger("Attack", 0);
            canClick = true;
            noOfClicks = 0;
        }
    }

    void CheckHit()
    {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position, 0.25f , transform.TransformDirection(Vector3.forward), out hit, 2f, 1 << 9))
        {
            Enemy enemyHealth = hit.collider.GetComponent<Enemy>();
            Debug.Log("Did Hit");
            enemyHealth.TakeDamage(5);
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward), Color.green);
        }
        else
        {
            Debug.Log("Did not Hit");
        }
    }
}
