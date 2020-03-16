using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton containing a list of all the listeners that might want to hear about any message
/// </summary>
public class EventSystemListeners : MonoBehaviour
{
    public static EventSystemListeners main;

    [Tooltip("Listeners that want to know about messages.  By default any object with tag = Listener is included, but you can add more here, or add at runtime with method EventSystemListeners.main.AddListener()")]
    public List<GameObject> listeners;

    /// <summary>
    /// Check we are singleton
    /// </summary>
    private void Awake()
    {
        if (main == null)
        {
            main = this;
        }
        else
        {
            Debug.LogWarning("EventSystemListeners re-creation attempted, destroying the new one");
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        // Look for every object tagged as a listener
        if (listeners == null)
        {
            listeners = new List<GameObject>();
        }

        GameObject[] gos = GameObject.FindGameObjectsWithTag("Listener");

        listeners.AddRange(gos);

    }

    public void AddListener(GameObject go)
    {
        // Don't add if already there
        if (!listeners.Contains(go))
        {
            listeners.Add(go);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}