using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimEvent : MonoBehaviour
{
    public UnityEvent animComleted;

    public void AnimComplete()
    {
        animComleted.Invoke();
    }
}
