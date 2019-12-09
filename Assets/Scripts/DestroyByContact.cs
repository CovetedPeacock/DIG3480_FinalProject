using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour
{
    public GameObject explosion;
    public GameObject playerExplosion;
    public int scoreValue;
    private GameController gameController;

    private void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameControllerObject == null)
        {
            Debug.Log("Cannot find 'GameController' script.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Boundary" || other.CompareTag("Enemy"))
        {
            return;
        }
        if (explosion != null && this.tag != "PowerUp")
        {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        /*if (explosion != null && this.tag == "PowerUp")
        {
            return;
        }*/
        if (this.tag != "PowerUp" && other.tag == "Player")
        {
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            gameController.GameOver();
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        if (explosion == null && playerExplosion == null && other.tag == "Player")
        {
            gameController.PowerUpCollected();
            Destroy(gameObject);
        }
        gameController.AddScore(scoreValue);
    }
}
