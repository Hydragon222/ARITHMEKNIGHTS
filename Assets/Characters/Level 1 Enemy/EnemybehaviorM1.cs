using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemybehaviorM1 : MonoBehaviour
{
    Rigidbody2D rb;
    public GameObject mc;
    public float speed;
    private float distance;
    public float followRange;

    public equationMaker equationGenerator;
    //public chargeCounter chargeCounter;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, mc.transform.position);
        Vector2 direction = mc.transform.position - transform.position;
        if (distance < followRange)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, mc.transform.position, speed * Time.deltaTime);
        }
       
            if (direction.x < 0)
            {
                spriteRenderer.flipX = true; // Face left
            }
            else if (direction.x > 0)
            {
                spriteRenderer.flipX = false; // Face right
            }
    }

    //void OnMouseDown()
   // {
        // This function is called when the enemy is clicked
        //Debug.Log("Enemy clicked!");
        // Add your desired action here
        //if (chargeCounter.counter == equationGenerator.expectedAnswer)
        //{
            //Debug.Log("CORRECT");
            //equationGenerator.equationText.text = equationGenerator.expectedAnswer.ToString();
            //animator.SetTrigger("Death");
            //speed = 0;
        //}
        //else
        //{
            //Debug.Log("WRONG");
       // }
    //}

    public void GetRekt()
    {
        Destroy(gameObject);
    }
}
