using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Player))]

public class PlayerController : MonoBehaviour
{   
    [HideInInspector] public Player player;
    //Seta animator
    public Animator playerAnimator; 
    //Para andar
    float input_x = 0;
    float input_y = 0;
    //Velocidade do personagem
    
    //Seta o personagem para andando ou não
    bool isWalking = false;

    Rigidbody2D rb2d;
    Vector2 movement = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        //Primeira animação é Idle
        isWalking = false;
        rb2d = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        //Pega dados do teclado
        input_x = Input.GetAxisRaw("Horizontal");
        input_y = Input.GetAxisRaw("Vertical");
        //Seta variavel se está andando quando uma das variáveis é diferente de 0
        isWalking = (input_x != 0 || input_y != 0);
        movement = new Vector2(input_x, input_y);

        if (isWalking)
        {
            //Move o Objeto(Personagem)
            playerAnimator.SetFloat("input_x", input_x);
            playerAnimator.SetFloat("input_y", input_y);
        }

        //Seta o player como andando
        playerAnimator.SetBool("isWalking", isWalking);

        if( player.entity.attackTimer < 0 ){
            player.entity.attackTimer = 0;
        }
        else{
            player.entity.attackTimer -= Time.deltaTime;
        }

        if( player.entity.attackTimer == 0 && !isWalking){
            //Attack do player
            //                      Mouse
            if (Input.GetButtonDown("Fire1")){
                playerAnimator.SetTrigger("attack");
                player.entity.attackTimer = player.entity.cooldown;
                Attack();
            }
        }
    }

    private void FixedUpdate(){
        //Posicao atual, onde se move, velocidade, tempo relativo
        rb2d.MovePosition(rb2d.position + movement * player.entity.speed * Time.fixedDeltaTime);
    }

    void OnTriggerStay2D(Collider2D collider){
        if( collider.transform.tag == "Enemy" ){
            player.entity.target = collider.transform.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D collider){
        if( collider.transform.tag == "Enemy" ){
            player.entity.target = null;
        }
    }

    void Attack(){
        if(player.entity.target == null){
            return;
        }
        
        Monster monster = player.entity.target.GetComponent<Monster>();
        
        if(monster.entity.dead){
            player.entity.target = null;
            return;
        }

        float distance = Vector2.Distance(transform.position, player.entity.target.transform.position);

        if(distance <= player.entity.attackDistance){
            int dmg = player.manager.CalculateDamage( player.entity, player.entity.damage );
            int enemyDef = player.manager.CalculateDefense( monster.entity, monster.entity.defense );

            int result = dmg - enemyDef;
            if( result < 0){
                result = 0;
            }

            Debug.Log("Player dmg:" + result.ToString());
            monster.entity.currentHealth -= result;
            monster.entity.target = this.gameObject;

        }
    }
}