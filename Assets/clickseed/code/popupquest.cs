using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class popupquest : MonoBehaviour
{
    public GameObject  questpopup;
    public GameObject quest2;


    public bool click;
    private void Start()
    {
        questpopup.SetActive(false); ;
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
                if (hit.collider.name == "quest1"|| hit.collider.name == "quest2")
                {
                    questpopup.SetActive(true);
                }
                if (hit.collider.name == "questpop")
                {
                    questpopup.SetActive(false);
                    quest2.SetActive(false);
                }
            }
        }


    }
}