using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemybehaviorM1 : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] private GameObject mc;
    public float speed;
    private float distance;
    public float followRange;
    public Vector3 wpnoffset;
    //public equationMaker equationGenerator;
    //public chargeCounter chargeCounter;
    private SpriteRenderer childSpriteRenderer;
    private SpriteRenderer spriteRenderer;
    private Transform childTransform;
    [SerializeField] private Animator animator;
    public Vector2 direction1;
    

    // Start is called before the first frame update
    void Start()
    {
        Vector2 direction = direction1;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        childTransform = transform.GetChild(0);
        childSpriteRenderer = childTransform.GetComponent<SpriteRenderer>();
        wpnoffset = childTransform.localPosition;
        
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, mc.transform.position);
        Vector2 direction = mc.transform.position - transform.position;
        if (distance <= followRange)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, mc.transform.position, speed * Time.deltaTime);
            animator.SetTrigger("Run");
        }
        else
        {
            animator.SetTrigger("Stand");
        }
       
            if (direction.x < 0)
            {
                spriteRenderer.flipX = true; // Face left
                childSpriteRenderer.flipX = true;
                childTransform.localPosition = new Vector3(-wpnoffset.x, wpnoffset.y, wpnoffset.z);
            }
            else if (direction.x > 0)
            {
                spriteRenderer.flipX = false; // Face right
                childSpriteRenderer.flipX = false;
                childTransform.localPosition = wpnoffset;
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
