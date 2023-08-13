using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class riMovement : MonoBehaviour
{
    public float speed;
    public float height;

    public Camera mainCamera;

    private bool is_left = false;
    private bool is_down = true;
    private float left;

    [SerializeField] private LayerMask layerMaskByBao;
    private Rigidbody2D rb;
    private CapsuleCollider2D bc;
    private Animator anim;

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
    }

    // Update is called once per frame
    void Update()
    {
        left = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(left * speed, rb.velocity.y);

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

}
