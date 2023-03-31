using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public GameObject door;
    private Door doorScript;
    private SpriteRenderer buttonGlow;

    [SerializeField] private int activatorAmount = 0;
    [SerializeField] private float timer = 0;
    [SerializeField] private GameObject timerSprite;

    // Start is called before the first frame update
    void Start()
    {
        doorScript = door.GetComponent<Door>();
        buttonGlow = GetComponent<SpriteRenderer>();
        timerSprite = transform.parent.GetChild(1).GetChild(0).gameObject;
        if (timer > 0) {
            timerSprite.transform.parent.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        activatorAmount ++;
        UpdateButtonState();
        StopCoroutine("BeginTimer"); // stop coroutine to fill time.
        // reset size of the timer
        timerSprite.transform.localScale = new Vector3(0.95f, timerSprite.transform.localScale.y, timerSprite.transform.localScale.z);

    }
    
    public void OnTriggerExit2D(Collider2D other)
    {
        activatorAmount --;
        if (activatorAmount == 0) {
            StopCoroutine("BeginTimer"); // stop coroutine to reset timer
            StartCoroutine("BeginTimer"); // start the timer
            
        }
    }

    public void UpdateButtonState()
    {
        if (activatorAmount == 0)
        {
            buttonGlow.color = Color.blue;
            doorScript.updateTrigger(false);
        }
        else {
            buttonGlow.color = new Color(1, 0.5f, 0, 1);
            doorScript.updateTrigger(true);
        }
    }

    private IEnumerator BeginTimer() {
        float time = timer;
        while (time > 0) {
            time -= 0.1f;
            timerSprite.transform.localScale = new Vector3(0.95f * (time / timer), timerSprite.transform.localScale.y, timerSprite.transform.localScale.z);
            yield return new WaitForSeconds(0.1f);
        }
        UpdateButtonState();
        yield return null;
    }
}
