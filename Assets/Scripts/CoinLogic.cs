using UnityEngine;

public class CoinLogic : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;
    bool wasCollected = false;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !wasCollected)
        {
            wasCollected = true;
            AudioSource.PlayClipAtPoint(coinPickupSFX, transform.position);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
