using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject game;
    public GameObject enemyGenerator;
    public AudioClip jumpClip;
    public AudioClip dieClip;
    public AudioClip pointClip;
    public ParticleSystem particulas;
    private Animator animator;

    private AudioSource audioPlayer;
    private float startY;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioPlayer = GetComponent<AudioSource>();
        startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        bool isGround = transform.position.y == startY;
        bool gamePlaying = game.GetComponent<GameController>().gameState == GameController.GameState.Playing;
        if(isGround && gamePlaying && (Input.GetKeyDown("up") || Input.GetMouseButtonDown(0))){
            UpdateState("PlayerJump");
            audioPlayer.clip = jumpClip;
            audioPlayer.Play();
        }
    }

    public void UpdateState(string state = null)
    {
         if(state != null){
             animator.Play(state);
         }
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.tag == "Enemy")
        {
            UpdateState("PlayeDie");
            game.GetComponent<GameController>().gameState = GameController.GameState.Ended;
            enemyGenerator.SendMessage("CancelGenerator", true);
            game.SendMessage("ResetTimeScale", 0.5f);

            game.GetComponent<AudioSource>().Stop();
            audioPlayer.clip = dieClip;
            audioPlayer.Play();

            PartStop();
        }
        else if (other.gameObject.tag == "Point")
        {
            game.SendMessage("IncreasePoints");
            audioPlayer.clip = pointClip;
            audioPlayer.Play();
        }
            
    }

    void GameReady()
    {
        game.GetComponent<GameController>().gameState = GameController.GameState.Ready;
    }

    void PartPlay()
    {
        particulas.Play();
    }

    void PartStop()
    {
        particulas.Stop();
    }
}
