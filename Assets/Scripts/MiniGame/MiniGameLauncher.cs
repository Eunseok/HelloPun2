using UnityEngine;

namespace MiniGame
{
    public class MiniGameLauncher : MonoBehaviour
    {
       public GameObject miniGame;

        public GameObject functionKey;

        bool isActive;


        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F) && isActive)
            {
                if (GameObject.FindGameObjectWithTag("MiniGame") == null) 
                    Instantiate(miniGame);
            }
        }
    
        void OnTriggerEnter2D(Collider2D other)
        {
        
            if (other.CompareTag("Player"))
            {
                if(!other.GetComponent<PlayerController>().photonView.IsMine) return;
                isActive = true;
                functionKey.SetActive(true);
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if(!other.GetComponent<PlayerController>().photonView.IsMine) return;
                isActive = false;
                functionKey.SetActive(false);
            }
        }
    }
}