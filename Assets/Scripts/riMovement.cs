using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class riMovement : MonoBehaviour
{
    public float speed;
    public float height;
    public float pushForce;


    public Camera mainCamera;

    private bool is_left = false;
    private bool is_down = true;
    private float left;

    [SerializeField] private LayerMask layerMaskByBao;
    private Rigidbody2D rb;
    private CapsuleCollider2D bc;
    private Animator anim;

    public LineRenderer lineRenderer;
    public Transform firePoint;
    public GameObject startVFX;
    public GameObject endVFX;


    private float stoneTime = 0.5f;

    public float firingTime = 2.0f;
    private float countFiringTime = 2.0f;

    private float timeScale = 1f;
    private bool firing = false;

    private const float defaultScale = 1f;
    private const float slowScale = 0.2f; 


    private bool stone = false;

    private List<ParticleSystem> particles = new List<ParticleSystem>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        bc = GetComponent<CapsuleCollider2D>();
        mainCamera = Camera.main;
        //_dis = GetComponent<DistanceJoint2D>();
        //_line = GetComponent<LineRenderer>();

        //_dis.enabled = false;
        //_line.enabled = false;

        FillLists();
        DisableLaser();
    }

    // Update is called once per frame
    void Update()
    {
        left = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(left * speed * timeScale, rb.velocity.y * timeScale);

        if (Input.GetKeyDown(KeyCode.W) && isGround())
        {
            rb.AddForce(Vector2.up * height * (is_down ? 1 : -1), ForceMode2D.Impulse);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            Attack();
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            ChangeGravity();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            EnableLaser();
        }
        if (Input.GetButton("Fire1"))
        {
            countFiringTime -= Time.deltaTime; 
            UpdateLaser();
        }

        if (countFiringTime <= 0.0f && firing)
        {
            DisableLaser();
        }
        if (Input.GetButtonUp("Fire1"))
        {
            DisableLaser();
        }
       
        //else if (Input.GetKeyDown(KeyCode.L))
        //{
        //    Debug.Log("HEHE");
        //    //Vector2 wirePos = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);
        //    Vector2 wirePos = new Vector2(-1.5f, 3.49f);
        //    _line.SetPosition(0, wirePos);
        //    _line.SetPosition(1, transform.position);
        //    _dis.connectedAnchor = wirePos;
        //    _dis.enabled = true;
        //    _line.enabled = true;
        //}
        //else if (Input.GetKeyUp(KeyCode.L))
        //{
        //    _dis.enabled = false;
        //    _line.enabled = false;
        //}

        //if (_dis.enabled)
        //{
        //    _line.SetPosition(1, transform.position);
        //}

        Flip();

        anim.SetFloat("move", Mathf.Abs(left));
        
        if(isGround()) {
            anim.SetBool("ground", true);
        }
        else {
            anim.SetBool("ground", false);
            anim.SetFloat("yV", rb.velocity.y);
        }
    }

    void Flip()
    {
        if (is_left && left > 0 || !is_left && left < 0)
        {
            is_left = !is_left;
            Vector3 size = transform.localScale;

            size.x = size.x * -1;
            transform.localScale = size;
        }
    }

    void Attack() 
    {
        anim.SetTrigger("attack");
    }

    void ChangeGravity()
    {
        anim.SetTrigger("changeGravity");
        is_down = !is_down;
        if (is_down)
        {
            transform.position = new Vector3(rb.position.x, 4.4f);
            Physics2D.gravity = new Vector2(0, -9.8f);

            Vector3 size = transform.localScale;

            size.y = size.y * -1;
            transform.localScale = size;
        }
        else
        {
            transform.position = new Vector3(rb.position.x, -1.5f);
            Physics2D.gravity = new Vector2(0, 9.8f);

            Vector3 size = transform.localScale;

            size.y = size.y * -1;
            transform.localScale = size;
        }
    }    

    bool isGround()
    {
        if (is_down)
        {
            RaycastHit2D raycastHit = Physics2D.Raycast(bc.bounds.center, Vector2.down, bc.bounds.extents.y + .1f, layerMaskByBao);
            //Debug.DrawRay(transform.position, raycastHit.point, Color.green);
            //Debug.Log(raycastHit.collider);
            return raycastHit.collider != null;
        }
        else
        {
            RaycastHit2D raycastHit = Physics2D.Raycast(bc.bounds.center, Vector2.down, -bc.bounds.extents.y - .1f, layerMaskByBao);
            //Debug.DrawRay(transform.position, raycastHit.point, Color.green);
            //Debug.Log(raycastHit.collider);
            return raycastHit.collider != null;
        }
    }

    void EnableLaser()
    {
        lineRenderer.enabled = true;
        countFiringTime = firingTime;
        timeScale = slowScale;
        rb.gravityScale = slowScale;
        firing = true;
        stone = false;
        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].Play();
        }
    }

    void UpdateLaser()
    {
        stone = false;
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        lineRenderer.SetPosition(0, firePoint.position);

        lineRenderer.SetPosition(1, mousePos);
        Vector2 direction = mousePos - transform.position;
        startVFX.transform.position = firePoint.position;
        int layerMask = ~LayerMask.GetMask("Player");
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position,direction.normalized,direction.magnitude, layerMask);


        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit2D hit = hits[i];

            if (hit)
            {
                // countFiringTime += firingTime * 0.;
                if (hit.collider.isTrigger && hit.collider.gameObject.tag == "DefaultStone")
                {
                    stoneTime -= Time.deltaTime;
                    if (stoneTime < 0f)
                    {
                        stone = true;
                        stoneTime = 0.5f;
                        Destroy(hit.transform.gameObject);
                    }
                }
                else if (hit.collider.gameObject.tag == "PushStone")
                {
                    if (stone)
                    {
                        Push(direction);
                        Destroy(hit.transform.gameObject);
                        stone = false;
                    }
                    lineRenderer.SetPosition(1, hit.point);

                }
                else if (hit.collider.gameObject.tag == "PushGround")
                {
                    if (stone)
                    {
                        Push(direction);
                        stone = false;
                    }
                    lineRenderer.SetPosition(1, hit.point);
                }
                else{
                    lineRenderer.SetPosition(1, hit.point);
                }
            }
        }

        endVFX.transform.position = lineRenderer.GetPosition(1);

    }

    void DisableLaser()
    {
        timeScale = defaultScale;
        rb.gravityScale = defaultScale;
        lineRenderer.enabled = false;
        firing = false;
        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].Stop();
        }
    }

    void Push(Vector2 direction)
    {
        if (!isGround())
        {
            rb.MovePosition((direction * -1f).normalized * pushForce+ rb.position);
        }
        stone = false;
    }

    void FillLists()
    {
        for (int i = 0; i < startVFX.transform.childCount; i++)
        {
            var ps = startVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if (ps != null)
            {
                particles.Add(ps);
            }
        }

        for (int i = 0; i < endVFX.transform.childCount; i++)
        {
            var ps = endVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if (ps != null)
            {
                particles.Add(ps);
            }
        }
    }

}
