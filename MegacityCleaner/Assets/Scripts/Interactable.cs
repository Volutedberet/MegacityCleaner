using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour{
    public string action;
    public UnityEvent onInteract;
    public Vector2 scale = new Vector2(100, 100);

    public bool ignoreInteract;
}