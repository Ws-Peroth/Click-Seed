using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class gotopot : MonoBehaviour
{
    public GameObject miniplant;


    public bool click;
    private void Start()
    {
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
                if (hit.collider.name == "miniplant")
                {
                    SceneManager.LoadScene("pot");
                }
            }
        }
    }
}