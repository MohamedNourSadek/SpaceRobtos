using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimateEffect : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    [SerializeField] float speed;
    
    private Vector3 direction;
    
    private void Awake()
    {
        direction = (endPoint.position - startPoint.position).normalized;    
    }

    private void OnEnable()
    {
        StopCoroutine(AnimateMyEffect());
        StartCoroutine(AnimateMyEffect());
    }

    private void OnDisable()
    {
        StopCoroutine(AnimateMyEffect());
    }

    IEnumerator AnimateMyEffect()
    {
        float increment = 0.001f * speed * (endPoint.position - startPoint.position).magnitude;
        
        while(true) {

            if(HasReachedEndPoint() == false)
            {
                target.transform.position += (increment * direction);
            }
            else
            {
                target.transform.position = startPoint.position;
            }

            yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
        }; 
    }

    bool HasReachedEndPoint()
    {
        Vector3 directionFromMeToEnd = endPoint.transform.position - target.transform.position;

        
        if ((directionFromMeToEnd.x * direction.x) > 0 && 
            (directionFromMeToEnd.y * direction.y) > 0 &&
            (directionFromMeToEnd.y * direction.y) > 0)
            return false;
        else
            return true;
    }

}
