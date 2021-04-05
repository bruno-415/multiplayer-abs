using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player2 : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator anim;

    public InputMaster controls;

    public float runSpeed = 40f;
    public float direction;

    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;

    public Transform crouchAttackPoint;
    public Transform normalAttackPoint;
    public Transform jumpAttackPoint;
    public LayerMask enemyLayers;

    public float normalAttackRange = 0.5f;
    public int normalAttackDamage = 20;
    public float normalAttackRate = 1.5f;
    float nextNormalAttackTime = 0f;

    public float crouchAttackRange = 0.5f;
    public int crouchAttackDamage = 15;
    public float crouchAttackRate = 1.75f;
    float nextCrouchAttackTime = 0f;

    public float jumpAttackRange = 0.75f;
    public int jumpAttackDamage = 20;
    public float jumpAttackRate = 1.5f;
    float nextJumpAttackTime = 0f;

    public int maxHealth = 100;
    int currentHealth;

    public Image healthBar;


    private void Awake()
    {
        controls = new InputMaster();

        controls.Player2.Attack.performed += ctx => Attack();
        controls.Player2.Move.performed += ctx => direction = ctx.ReadValue<float>();
        controls.Player2.Jump.performed += ctx => Jump();
        controls.Player2.Crouch.performed += ctx => Crouch(true);
        controls.Player2.Crouch.canceled += ctx => Crouch(false);

    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        horizontalMove = direction * runSpeed;

        anim.SetFloat("Speed", Mathf.Abs(horizontalMove));
    }

    public void OnLanding()
    {
        anim.SetBool("IsJumping", false);
        jump = false;
    }

    public void OnCrouching(bool isCrouching)
    {
        anim.SetBool("IsCrouching", isCrouching);
    }

    private void FixedUpdate()
    {
        //Move character
        controller.Move(horizontalMove * Time.deltaTime, crouch, jump);
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Jump()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("NormalAttack"))
        {
            anim.SetBool("IsJumping", true);
            jump = true;
        }
    }

    private void Crouch(bool crouched)
    {
        if (crouched == true)
        {
            crouch = true;
        }
        if (crouched == false)
        {
            crouch = false;
        }
    }

    //Combat

    public void Attack()
    {
        if ((crouch == false) && (jump == false) && (Time.time >= nextNormalAttackTime))
        {
            anim.SetTrigger("Attack");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(normalAttackPoint.position, normalAttackRange, enemyLayers);
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Player1>().TakeDamage(normalAttackDamage);
                break;
            }

            nextNormalAttackTime = Time.time + 1f / normalAttackRate;
        }
        else if ((crouch == true) && (Time.time >= nextCrouchAttackTime))
        {
            anim.SetTrigger("Attack");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(crouchAttackPoint.position, crouchAttackRange, enemyLayers);
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Player1>().TakeDamage(crouchAttackDamage);
                break;
            }

            nextCrouchAttackTime = Time.time + 1f / crouchAttackRate;
        }
        else if ((jump == true) && (Time.time >= nextJumpAttackTime))
        {
            anim.SetTrigger("Attack");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(jumpAttackPoint.position, jumpAttackRange, enemyLayers);
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Player2>().TakeDamage(jumpAttackDamage);
                break;
            }

            nextJumpAttackTime = Time.time + 1f / jumpAttackRate;
        }
        else return;
    }

    private void OnDrawGizmosSelected()
    {
        if (normalAttackPoint == null)
            return;

        Gizmos.DrawWireSphere(normalAttackPoint.position, normalAttackRange);

        if (crouchAttackPoint == null)
            return;

        Gizmos.DrawWireSphere(crouchAttackPoint.position, crouchAttackRange);

        if (jumpAttackPoint == null)
            return;

        Gizmos.DrawWireSphere(jumpAttackPoint.position, jumpAttackRange);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player 2 took " + damage + " damage.");
        healthBar.fillAmount -= damage / 100f;

        anim.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player 2 died!");

        anim.SetBool("IsDead", true);

        transform.Rotate(0f, 0f, -90f);
        anim.enabled = false;

        GetComponent<Rigidbody2D>().simulated = false;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}
