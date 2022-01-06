using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class BallHandler : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField] private GameObject ballprefab;
    [SerializeField] private Rigidbody2D pivot;
     
    [SerializeField] float letgotime = 0.1f;
    [SerializeField] private float respawntime;
    
    private Rigidbody2D currentball;
    private SpringJoint2D springJoint;
    private Camera mainCamera;
    bool isdragging;
    
    void Start()
    {
        mainCamera = Camera.main;
        spawnnewball();   
    }

    // Update is called once per frame
    void Update()
    {
        touch();
    }
        void touch()
    {   
        if(currentball == null){return;}
        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            if(isdragging)
            {
                launchball();
            }
            isdragging = false;
            return;
        }
        
        isdragging = true;
        currentball.isKinematic = true;

        Vector2 touchposition = Touchscreen.current.primaryTouch.position.ReadValue();

        Vector3 worldposition =  mainCamera.ScreenToWorldPoint(touchposition);

        currentball.position = worldposition;     
    }
        private void launchball()
        {
            currentball.isKinematic = false;
            currentball = null;
            Invoke(nameof(detachball),letgotime);
        }
        private void detachball()
        {
            springJoint.enabled = false;
            springJoint = null;
            
            Invoke(nameof(spawnnewball) ,respawntime);
        }

        private void spawnnewball()
        {
            GameObject ballinstance =  Instantiate(ballprefab, pivot.position, Quaternion.identity);
            currentball = ballinstance.GetComponent<Rigidbody2D>();
            springJoint = ballinstance.GetComponent<SpringJoint2D>();

            springJoint.connectedBody = pivot;

        }
}
