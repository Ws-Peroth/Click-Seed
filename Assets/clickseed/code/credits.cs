using UnityEngine;
using UnityEngine.SceneManagement;

public class credits : MonoBehaviour
{
    public GameObject slime;


    public bool click;
    private void Start()
    {
        click = false;
        slime.SetActive(false); 

    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.name == "slimesleep")
                {
                    slime.SetActive(true);
                }
                if (hit.collider.name == "exit") {
                    slime.SetActive(false);
                }
            }
        }
    }
}