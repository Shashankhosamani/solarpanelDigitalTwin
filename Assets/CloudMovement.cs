

using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    public float speed;
    float loopPos = -11;
    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(0.005f, 0.02f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(transform.position.x - speed, transform.position.y);
        if(transform.position.x < loopPos)
        {
            Instantiate(gameObject, new Vector2(11f, transform.position.y), Quaternion.identity, transform.parent);
            Destroy(gameObject);
            
        }
    }
}
