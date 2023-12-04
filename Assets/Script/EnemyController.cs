using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Rigidbody rb;
    InfoManager im;
    [SerializeField]GameObject player;
    [SerializeField] GameObject destroyer;
    [SerializeField] private float speed;
    [SerializeField] private float centerSpeed;
    [SerializeField] private float power;
    [SerializeField] ParticleSystem sparkParticle;
    public bool isGrounded;
    Vector3 centripetalForce;
    Vector3 centerPos;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        im = InfoManager.Instance;
        centerPos = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        centripetalForce = centerPos - transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector3 dir = (player.transform.position -
                transform.position).normalized;
            dir.y = 0; 
            transform.forward = dir;
            if (isGrounded == true && im.isGame == true)
            {
                rb.AddForce(dir * speed);
                rb.AddForce(centripetalForce * centerSpeed / 2, ForceMode.Force);
            }
        }
        if (im.isReset)Restart(); 
    }
    

    private void OnCollisionEnter(Collision collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.tag == "Ground")
        {
            isGrounded = true;
        }
        else
        if (obj.tag == "Enemy")
        {
            Rigidbody enemyrb = obj.GetComponent<Rigidbody>();
            Vector3 dir = (obj.transform.position -
                this.transform.position);
            enemyrb.AddForce(dir * power, ForceMode.Impulse);
            rb.AddForce(-dir * power, ForceMode.Impulse);
        }
        else
        if (obj.tag == "Out" && isGrounded == false)
        {
            EnemyCurb();
        }
        sparkParticle.Play();
    }
    private void OnCollisionExit(Collision collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.tag == "Ground")
        {
            isGrounded = false;
        }
    }

    public void EnemyCurb() 
    {
        destroyer.SetActive(true);
        Destroy(this);
    }

    public void Restart()
    {
        Destroy(this.gameObject);
    }
}