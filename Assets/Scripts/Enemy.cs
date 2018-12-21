using UnityEngine;
using System.Collections;

/* Script Written By Ari Hawlere
 * 
 * Subscribe My Channel For More Video >>> https://goo.gl/HOJxah
 * 
 *  TakeDamage and CheckHit are my own functions, the death condition s my own addition*/


public class Enemy : MonoBehaviour
{
    public Animator animator;
    public int counter= 0;
    public Transform Player;
    public float Attack_range = 2f;
    public float Chase_Range = 100f;
    public float Distance;
    public float RotSpeed = 10; // the target's rotation speed
    public float MoveSpeed = 1; // the target's moving speed
    public int hp = 10;
    public Rigidbody rb;

    private Vector3 moveDirection = Vector3.zero;

    public float gravity = 100.0f;

    // Use this for initialization
    void Start()
    {

        animator = GetComponent<Animator>();
        Player = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (hp > 0)
        {
            // Apply gravity
            moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);
            // The Distance Between Our Player and The Target(Enemy)
            Distance = (Player.transform.position - transform.position).magnitude;

            // when the target gets close to the player
            if (Distance <= Chase_Range && Distance > Attack_range)
            {
                GetComponent<Animator>().SetBool("Walking", true);
                Vector3 Direction = Player.position - transform.position; // the defference of position of these two objects, in order to use in rotation 
                Direction.y = 0; // so the target won't rotate in the y-axis

                // rotate the target toward the player
                transform.rotation = Quaternion.Slerp(transform.rotation,
                                                      Quaternion.LookRotation(Direction), RotSpeed * Time.deltaTime);
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

                // so when the target rotates toward the player then we can move the target to forward direction,
                // Suppose it Chases the player
                transform.position += transform.forward * MoveSpeed * Time.deltaTime;

            }
            else
            {
                GetComponent<Animator>().SetBool("Walking", false);
            }

            // Attack region,
            if (Distance <= Attack_range)
            {
                GetComponent<Animator>().SetBool("Attack", true);

            }
            else
            {
                GetComponent<Animator>().SetBool("Attack", false);
            }
        }
        else if (hp <= 0)
        {
            GetComponent<Animator>().SetBool("isDead", true);
            GetComponent<Animator>().SetBool("Walking", false);
            GetComponent<Animator>().SetBool("Attack", false);
            if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Zombie Death"))
            {
                return;
            }
            else
            {
                HUD.cashval = HUD.cashval + 1;
                Destroy(this.gameObject);
            }
        }
    }
    
    void FixedUpdate()
    {
        rb.AddForce(Vector3.down * gravity * rb.mass);
    }

    void CheckHit()
    {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position, 0.5f, transform.TransformDirection(Vector3.forward), out hit, 2f, 1 << 10))
        {
            HUD enemyHealth = hit.collider.GetComponent<HUD>();
            Debug.Log("Did Hit");
            enemyHealth.TakeDamage(50);
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward), Color.green);
        }
        else
        {
            Debug.Log("Did not Hit");
        }
    }

    public void TakeDamage(int damage)
    {
        GetComponent<Animator>().SetBool("isHit", true);
        hp = hp - damage;
    }
}