using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class popuppot : MonoBehaviour
{
    public GameObject potpopup;

    public bool click;
    private void Start()
    {
        potpopup.SetActive(false); ;
        click = false;

    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.name == "pot")
                {
                    potpopup.SetActive(true);
                }
                if (hit.collider.name == "popuppot")
                {
                    potpopup.SetActive(false);
                }
            }
        }


    }
}