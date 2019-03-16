using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    public float waitBetween;
    public float waitEnd;

    
    private List<Animator> _animators;
    
    void Start()
    {
        _animators = new List<Animator>(GetComponentsInChildren<Animator>());
        StartCoroutine(DoAnimation());
    }

    IEnumerator DoAnimation()
    {
        while (true)
        {
            foreach (var animator in _animators)
            {
                animator.SetTrigger("DoAnimation");
                yield return new WaitForSeconds(waitBetween);
            }
            
            yield return new WaitForSeconds(waitEnd);
            
        }
    }
    
}
