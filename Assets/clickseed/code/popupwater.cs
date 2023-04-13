using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class popupwater : MonoBehaviour
{
    public GameObject waterpopup;

    public bool click;
    private void Start()
    {
        waterpopup.SetActive(false); ;
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
                if (hit.collider.name == "Water")
                {
                    waterpopup.SetActive(true);
                }
                if (hit.collider.name == "popupwater") {
                    waterpopup.SetActive(false);
                }
            }
        }

      
    }
}