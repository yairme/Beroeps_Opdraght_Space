using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_AI : MonoBehaviour
{

    public float startSpeed = 10f;

    private WaveSpawner WS;
    private WayPoints WP;
    private PlayerStats ST;

    [HideInInspector]
    public float speed;

    private int startHealth;
    public int health;

    public int value;


    [Header("Unity stuff")]
    public Image healthBar;

    protected Transform target;
    protected int wavepointIndex = 0;

    private void Start()
    {
        WS = GameObject.Find("GameMaster").GetComponent<WaveSpawner>();
        WP = GameObject.Find("waypoint").GetComponent<WayPoints>();
        ST = GameObject.Find("GameMaster").GetComponent<PlayerStats>();

        speed = startSpeed;
        target = WP.wpoints[0];
    }

    public void TakeDamage (int amount)
    {
        startHealth -= amount;


        healthBar.fillAmount = startHealth / health;
        
        if (startHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        ST.Money += value;
        WS.EnemiesAlive--;
        Destroy(gameObject);
    }

    private void Update()
    {
        Vector2 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);


        if (Vector2.Distance(transform.position, target.position) <= 0.2f) { 
            GetNextWaypoint();
        }
        Vector3 dirL = WP.wpoints[wavepointIndex].position - transform.position;
        float angle = Mathf.Atan2(dirL.y, dirL.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void GetNextWaypoint()
    {
        if (wavepointIndex >= WP.wpoints.Length - 1) { 
            EndPath();
            return;
        }

        wavepointIndex++;
        target = WP.wpoints[wavepointIndex];
    }

    void EndPath()
    {
        ST.Lives--;
        WS.EnemiesAlive--;
        Destroy(gameObject);
    }
}

