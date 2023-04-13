using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class click : MonoBehaviour
{
    public int count;
    public GameObject gameObject;
    public GameObject gameObject1;
    public GameObject gameObject2;
    public GameObject gameObject3;
    public GameObject gameObject4;
    public GameObject glove;
    public Text counterText;
    private void Start()
    {
        gameObject.SetActive(true);
        glove.SetActive(false);
        count = 0;

    }
    void Update()
    {
        counterText.text = count.ToString();
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.name == "bigpot") {
                    count++;
                }
            }
        }

        if (count == 5) {
            gameObject.SetActive(false);
            gameObject1.SetActive(true);
        }
        if (count == 10)
        {
            gameObject1.SetActive(false);
            gameObject2.SetActive(true);

        }
        if (count == 15)
        {
            gameObject2.SetActive(false);
            gameObject3.SetActive(true);
            
        }
        if (count == 20)
        {
            gameObject3.SetActive(false);
            gameObject4.SetActive(true);
            glove.SetActive(true);
        }
        if (count == 25)
        {
            glove.SetActive(true);
        }
    }
}