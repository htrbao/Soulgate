using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Text : MonoBehaviour
{
    [SerializeField] public TMP_Text[] texts;
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < texts.Length; i++)
        {
            SetText(i, false);
        }
        SetText(0, true);
    }

    void SetText(int index, bool show)
    {
        texts[index].enabled = show;

        texts[index].gameObject.SetActive(show);
    }
    // Update is called once per frame
    void Update()
    {
        if (rb.position.x >= -4 && rb.position.x < 3)
        {
            SetText(1, true);
            SetText(0, false);
        }
        else if (rb.position.x >= 4)
        {
            SetText(2, true);
            SetText(1, false);
        }
        else{
            SetText(2, false);
        }
    }
}
