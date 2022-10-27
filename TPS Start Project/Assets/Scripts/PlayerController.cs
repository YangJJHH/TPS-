using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    public AudioClip itemPickupClip;
    public int lifeRemains = 3;
    private AudioSource playerAudioPlayer;
    private PlayerHealth playerHealth;
    private PlayerMovement playerMovement;
    private PlayerShooter playerShooter;

    private void Start()
    {
        Cursor.visible = false;
        animator = GetComponent<Animator>();
        playerAudioPlayer = GetComponent<AudioSource>();
        playerHealth = GetComponent<PlayerHealth>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooter = GetComponent<PlayerShooter>();

        playerHealth.OnDeath += HandleDeath;
        UIManager.Instance.UpdateLifeText(lifeRemains);

    }
    
    private void HandleDeath()
    {
        playerMovement.enabled = false;
        playerShooter.enabled = false;
        Cursor.visible = true;
        Debug.Log("죽음");
        if (lifeRemains > 0)
        {
            lifeRemains--;
            UIManager.Instance.UpdateLifeText(lifeRemains);
            
            Invoke("Respawn",3f);
        }
        else
        {
            GameManager.Instance.EndGame();
        }
    }

    public void Respawn()
    {
        gameObject.SetActive(false);
        transform.position = Utility.GetRandomPointOnNavMesh(transform.position,30f,NavMesh.AllAreas);
        gameObject.SetActive(true);

        playerShooter.gun.ammoRemain = 120;
        playerMovement.enabled = true;
        playerShooter.enabled = true;
        Cursor.visible = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (playerHealth.dead) return;
        var item = other.GetComponent<IItem>();
        if(item != null)
        {
            item.Use(this.gameObject);
            playerAudioPlayer.PlayOneShot(itemPickupClip);
        }
    }
}