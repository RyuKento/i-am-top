using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    InfoManager im;
    [SerializeField] private float moveSpeed = 30;
    [SerializeField] private float centerSpeed = 1.5f;
    [SerializeField] private float rushSpeed = 5;
    [SerializeField] private float power = 5;
    [SerializeField] private float rigor = 5;
    [SerializeField] ParticleSystem sparkParticle;
    [SerializeField] AudioSource playerAudio;
    [SerializeField] AudioClip collideSound;
    [SerializeField] Text lifeText;
    private ParticleSystem usedParticle;
    public bool isUseRush = false;
    public bool isGrounded;
    Vector3 centerPos;
    Vector3 dir;
    Vector3 centripetalForce;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 100;
        im = InfoManager.Instance;
        centerPos = Vector3.zero;
        im.isHeal = true;
    }

    // Update is called once per frame
    void Update()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        dir = Vector3.forward * v + Vector3.right * h;
        dir.y = 0;

        if (dir != Vector3.zero) this.transform.forward = dir;

        dir = dir.normalized * moveSpeed;

        if (Input.GetKeyDown(KeyCode.Space) && isUseRush == false && isGrounded && im.isPlay)
            Rush();
        if (im.isReset) Restart();
        centripetalForce = centerPos - this.transform.position;
    }

    private void FixedUpdate()
    {
        if (isGrounded && im.isGame)
        {
            rb.AddForce(dir, ForceMode.Force);
            rb.AddForce(centripetalForce * centerSpeed, ForceMode.Force);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.tag == "Enemy")
        {
            Rigidbody enemyrb = obj.GetComponent<Rigidbody>();
            Vector3 dir = (obj.transform.position -
                this.transform.position);
            enemyrb.AddForce(dir * power, ForceMode.Impulse);
            rb.AddForce(-dir * power, ForceMode.Impulse);

            usedParticle = Instantiate(sparkParticle, collision.transform.position,
            transform.rotation);
            usedParticle.Play();
            playerAudio.PlayOneShot(collideSound);
        }
        else
        if (obj.tag == "Ground")
        {
            isGrounded = true;
            im.isPlay = true;
            Debug.Log(im.isPlay);
        }
        else if (obj.tag == "Out" && this.tag == "Player")
            PlayerCurb();
    }
    private void OnCollisionExit(Collision collision)
    {
        GameObject obj = collision.gameObject;
        if(obj.tag == "Ground")
        {
            isGrounded = false;
        }
    }
    public void PlayerCurb()
    {
        Destroy(this.gameObject,3);
        im.isOut = true;
        im.isPlay = false;
    }
    public void Restart()
    {
        Destroy(this.gameObject);
    }
    private void Rush()
    {
        rb.AddForce(transform.forward.normalized * rushSpeed,
            ForceMode.Impulse);
        isUseRush = true;
        StartCoroutine(RushCoroutine(rigor));
    }
    IEnumerator RushCoroutine(float sec)
    {
        yield return new WaitForSeconds(sec);
        isUseRush = false;
    }
}
