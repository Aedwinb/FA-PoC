using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PrototyeMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float speed;
    [SerializeField] float dash;
    [SerializeField] Transform angleReference;
    [SerializeField] float maxStamina;
    private float currStamina;
    [SerializeField] float dashStamCost;
    [SerializeField] float staminaRegenWait;
    [SerializeField] float staminaRegenPerSec;
    [SerializeField] Image staminaBar;
    private Coroutine _staminaRoutine;
    bool canDash=>currStamina>dashStamCost;
    private void Start()
    {
        SetStamina(maxStamina);
    }
    // Update is called once per frame
    void Update()
    {
        float xInput = Input.GetAxis("Horizontal");
        if (xInput != 0)
        {
            rb.AddForce(Vector2.right* xInput*speed*Time.deltaTime);
            /*
            Vector2 v = rb.velocity;
            v.x = xInput * speed * Time.deltaTime;
            rb.velocity = v;*/
        }
        if (Input.GetMouseButtonDown(1)&&canDash)
        {
            Vector2 v = rb.velocity;
            v = dash*angleReference.up;
            rb.velocity = v;
            SetStamina(currStamina-dashStamCost);
        }
    }
    private void SetStamina(float newValue)
    {
        if (newValue < currStamina)
        {
            if (_staminaRoutine != null) StopCoroutine(_staminaRoutine);
            _staminaRoutine = StartCoroutine(IE_StaminaWait());
        }
        currStamina = Mathf.Clamp(newValue,0,maxStamina);
        staminaBar.fillAmount = currStamina / maxStamina;
    }
    private IEnumerator IE_StaminaWait()
    {
        yield return new WaitForSeconds(staminaRegenWait);
        _staminaRoutine = StartCoroutine(IE_StamineRegen());
    }
    private IEnumerator IE_StamineRegen()
    {
        Debug.Log("Starting stamina regen");
        while (currStamina < maxStamina)
        {
            yield return null;
            SetStamina(currStamina + Time.deltaTime * staminaRegenPerSec);
        }

    }
}
