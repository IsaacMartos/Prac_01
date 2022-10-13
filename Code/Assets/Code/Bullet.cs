using UnityEngine;

public class Bullet: MonoBehaviour
{
    public float m_Speed = 5.0f;
    public float lifeDuration = 3f;
    float lifeTimer;

    private void Start()
    {
        lifeTimer = lifeDuration;
    }

    private void Update()
    {
        lifeTimer -= Time.deltaTime;
        if(lifeTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward * m_Speed * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "EDiana")
        {
            collision.gameObject.SetActive(false);
            GameController.m_GameController.HitEasyDiana();
            gameObject.transform.SetParent(collision.transform);
        }
        if (collision.gameObject.tag == "NDiana")
        {
            collision.gameObject.SetActive(false);
            GameController.m_GameController.HitNormalDiana();
            gameObject.transform.SetParent(collision.transform);
        }
        if (collision.gameObject.tag == "DDiana")
        {
            collision.gameObject.SetActive(false);
            GameController.m_GameController.HitHardDiana();
            gameObject.transform.SetParent(collision.transform);
        }
    }

}
