using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public float offset;
    public float offsetSmoothing;
    private Vector3 playerPosition;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = new Vector3(player.transform.position.x, player.transform.position.y + 1, transform.position.z);

        if (player.transform.localScale.x > 0f)
        {
            playerPosition = new Vector3(playerPosition.x + offset, playerPosition.y, -10);
        }
        else
        {
            playerPosition = new Vector3(playerPosition.x - offset, playerPosition.y, -10);
        }

        transform.position = Vector3.Lerp(transform.position, playerPosition, offsetSmoothing * Time.deltaTime);
    }
}