using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Script : MonoBehaviour
{
    public float speed, maxSpeed, jumpForce;
    public Canvas Inventory;
    public bool pause;
    public int coins;
    public Manager manager;
    public GameObject start_pos;

    private Vector3 fixed_pos;
    private Animator anim;
    private Camera_script cam;
    private float time_fall;
    private bool facingRight, grounded, jumping, running,started;
    private Rigidbody2D rb;
    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        cam = Camera.main.gameObject.GetComponent<Camera_script>();
        cam.player = this.gameObject;
        cam.offset = transform.position - Camera.main.transform.position;
        Inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Canvas>();
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        if(start_pos!=null && started==false&&!pause)
        {
            transform.position = start_pos.transform.position;
            started = true;
        }
        if (!Inventory.isActiveAndEnabled)
        {
            pause = false;
        }
        else
            pause = true ;

        if (pause)
        {
            fixed_pos = transform.position;
            transform.position = fixed_pos;
            rb.gravityScale = 0f;
            rb.velocity = Vector2.zero;
        }
        else
        {
            rb.gravityScale = 1f;
        }

        anim.SetBool("Running", running);
        anim.SetBool("Jumping", jumping);
    }
    void Movement()
    {
        if (!pause)
        {
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (input.x == 0)
                running = false;
            if (input.x != 0)
            {
                running = true;
                transform.Translate(new Vector3(input.x * speed,0,0));
            }
  
            if (Input.GetAxis("Horizontal") > 0)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
                facingRight = true;
            }

            else if (Input.GetAxis("Horizontal") < 0)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                facingRight = false;
            }
            if (Input.GetKeyDown(KeyCode.Space)||input.y>0)
            {
                if (grounded)
                {
                    jumping = true;
                    rb.AddForce(new Vector2(0, jumpForce*10f));
                }
            }
            if (grounded)
                jumping = false;
            if (rb.velocity.y < -0.1)
            {
                time_fall += Time.deltaTime;
            }
            else
            {
                time_fall = 0;
            }
            if (time_fall > 5)
            {

                if (start_pos != null)
                {
                    time_fall = 0f;
                    transform.position = start_pos.transform.position;
                }
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Coin"))
        {
            coins += 1;
            Destroy(collision.gameObject);
        }
        if(collision.CompareTag("End"))
            {
            if(start_pos!=null)
            transform.position = start_pos.transform.position;
        }
        
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }
    }
}
