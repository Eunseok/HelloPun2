using UnityEngine;

namespace MiniGame
{
    public class Obstacle : MonoBehaviour
    {
        public float speed = 3.0f;
    
        // Update is called once per frame
        void Update()
        {
            transform.Translate(Vector2.left * (speed * Time.deltaTime));

            if (transform.position.x < -10) 
                Destroy(gameObject);
        }
    }
}
