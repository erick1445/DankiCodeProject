using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 3;
    public float speed;
    public float jumpForce;

    public GameObject Bow;
    public Transform firepoint;

    private bool isJumping;
    private bool doubleJump;
    private bool isFire;

    private Rigidbody2D rig;
    private Animator anim;

    private float movement;


    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        GameController.instance.UpdateLives(health);
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        BowFire();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        //se nao pressionar o valor e 0, pressionar "direita", valor maximo 1, pressionar "esquerda" valor maximo -1.
        movement = Input.GetAxis("Horizontal");
        
        //adiciona velocidade ao corpo do personagem no eixo"X" e no eixo "Y".
        rig.velocity = new Vector2(movement * speed, rig.velocity.y);

        //andar para a direita
        if (movement > 0)
        {
            if (!isJumping)
            {
                anim.SetInteger("transition", 1);
            }

            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        //andar para a esquerda
        if (movement < 0) 
        {
            if(!isJumping)
            {
                anim.SetInteger("transition", 1);
            }

            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        if (movement == 0 && !isJumping && !isFire)
        {
            anim.SetInteger("transition", 0);
        }

    }

    void Jump()
    {

        if(Input.GetButtonDown("Jump"))
        {
            if(!isJumping)
            {
                anim.SetInteger("transition", 2);
                rig.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                doubleJump = true;
                isJumping = true;
            }
            else
            {
                if(doubleJump)
                {
                    anim.SetInteger("transition", 2);
                    rig.AddForce(new Vector2(0, jumpForce * 1), ForceMode2D.Impulse);
                    doubleJump = false;
                }
            }
        }
    }

    void BowFire()
    {
        StartCoroutine("Fire");
    }

    IEnumerator Fire()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isFire = true;
            anim.SetInteger("transition", 3);
            GameObject bow = Instantiate(Bow, firepoint.position, firepoint.rotation);

            if (transform.rotation.y == 0)
            {
                bow.GetComponent<Bow>().isRight = true;
            }

            if (transform.rotation.y == 180)
            {
                bow.GetComponent<Bow>().isRight = false;
            }

            yield return new WaitForSeconds(0.2f);
            isFire = false;
            anim.SetInteger("transition", 0);
        }
    }

public void Damage(int dmg)
{
    health -= dmg;
    GameController.instance.UpdateLives(health);
    anim.SetTrigger("hit");

    if (transform.rotation.y == 0)
    {
        transform.position += new Vector3(-0.5f, 0, 0);
    }

    if (transform.rotation.y == 180)
    {
        transform.position += new Vector3(0.5f, 0, 0);
    }

    if(health <= 0)
    {
        //chama game over
        GameController.instance.GameOver();
    }
}

public void IncreaseLife(int value)
{
    health += value;
    GameController.instance.UpdateLives(health);
}
    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.layer == 6)
        {
            isJumping = false;
        }

        if(coll.gameObject.layer == 7)
        {
            GameController.instance.GameOver();
        }
    }
}
