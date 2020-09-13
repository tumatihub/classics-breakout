using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField] private float _secondsToSelfDestroy;
    void Start()
    {
        Destroy(gameObject, _secondsToSelfDestroy);
    }
}
