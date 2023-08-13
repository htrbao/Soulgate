using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public Camera cam;
    public LineRenderer lineRenderer;
    public Transform firePoint;
    public GameObject startVFX;
    public GameObject endVFX;
    public GameObject character;
    public float targetTime = 3.0f;
    private Quaternion rotation;
    private List<ParticleSystem> particles = new List<ParticleSystem>();
    
    // Start is called before the first frame update
    void Start()
    {
        FillLists();
        DisableLaser();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            EnableLaser();
            
        }
        if (Input.GetButton("Fire1"))
        {
            targetTime -= Time.deltaTime; 
            UpdateLaser();
        }

        if (targetTime <= 0.0f)
        {
            DisableLaser();
        }
        if (Input.GetButtonUp("Fire1"))
        {
            DisableLaser();
        }
        // RotateToMouse();
    }

    void EnableLaser()
    {
        lineRenderer.enabled = true;
        targetTime = 3.0f;
        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].Play();
        }
    }

    void UpdateLaser()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
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

            if (hit && hit.collider.isTrigger == false)
            {
                lineRenderer.SetPosition(1, hit.point);
            }
        }

        endVFX.transform.position = lineRenderer.GetPosition(1);

    }

    void DisableLaser()
    {
        lineRenderer.enabled = false;
        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].Stop();
        }
    }

    // void RotateToMouse()
    // {
    //     Vector2 direction = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    //     float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //     rotation.eulerAngles = new Vector3(0,0,angle);
    //     transform.rotation = rotation;
    // }

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
