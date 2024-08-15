using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : MonoBehaviour
{

    public GameObject Player, Ball;
    public Transform FirePos,Spine,BallCon;
    public float FireTime;
    private bool FireReady = true;
    private GameController gameController;
    private void Start()
    {
        gameController = FindAnyObjectByType<GameController>();
    }

    void Update()
    {
        if (gameController.gameIsOn)
        {
            transform.LookAt(Player.transform.position);
            //Spine.position=transform.position;
            Spine.rotation = transform.rotation;
            if (FireReady)
            {
                Instantiate(Ball, FirePos.position, FirePos.rotation, BallCon);
                StartCoroutine(FireDelay());
            }
        }
    }

    IEnumerator FireDelay()
    {
        FireTime = Random.RandomRange(1, FireTime);
        FireReady = false;
        yield return new WaitForSeconds(FireTime);
        //MuzelFlash.SetActive(false);
        FireReady = true;

    }
}
