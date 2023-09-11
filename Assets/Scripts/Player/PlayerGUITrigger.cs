using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGUITrigger : SingletonMonobehaviour<PlayerGUITrigger>
{
    [SerializeField] LayerMask GUILayer;
    [SerializeField] LayerMask obstacleLayer;
    public List<GUIObject> GUIObjects;
    [SerializeField] Transform playerTransform;

    Coroutine checkGUICor;

    GUIObject nearestGUIObject;
    float distanceToNearest;

    protected override void Awake()
    {
        base.Awake();

    }

    void Start()
    {
        Clear();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (GUILayer == (GUILayer | (1 << collision.gameObject.layer)))
        {
            GUIObject GUIobj = collision.gameObject.GetComponent<GUIObject>();
            if (GUIobj.itemChest != null && GUIobj.itemChest.isOpened) return;
                GUIObjects.Add(GUIobj);
            if (GUIObjects.Count == 1)
            {
                Player.I.onGUI = true;
                checkGUICor = StartCoroutine(CheckGUICor());
            }
        }
    }

    IEnumerator CheckGUICor()
    {
        while (true)
        {
            GUIObject newNearest = nearestGUIObject;
            float newDistanceToNearest = distanceToNearest;

            foreach (GUIObject GUIObj in GUIObjects)
            {
                if (GUIObj == null) continue;

                Vector2 directionToGUIObj = GUIObj.gameObject.transform.position- playerTransform.position;
                float distanceToGUIObj = directionToGUIObj.magnitude;
                Vector2 raycastStart = (Vector2)playerTransform.position + directionToGUIObj.normalized * 0.5f;
                
                RaycastHit2D hit = Physics2D.Raycast(raycastStart, directionToGUIObj.normalized, distanceToGUIObj, obstacleLayer);

                if (hit.collider == null)
                {
                    if (distanceToGUIObj < newDistanceToNearest)
                    {
                        newNearest = GUIObj;
                        newDistanceToNearest = distanceToGUIObj;
                    }
                }
            }

            if (newNearest != nearestGUIObject)
            {
                if (nearestGUIObject != null) nearestGUIObject.ToggleCanvas(false);
                nearestGUIObject = newNearest;
                nearestGUIObject.ToggleCanvas(true);

                distanceToNearest = newDistanceToNearest;
            }

            yield return null;
        }
    }

    public void UseGUIObject()
    {
        nearestGUIObject.UseGUIObject();
        RemoveNearestObject();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (GUILayer == (GUILayer | (1 << collision.gameObject.layer)))
        {
            RemoveObject(collision.gameObject.GetComponent<GUIObject>());
        }
    }

    public void RemoveNearestObject()
    {
        RemoveObject(nearestGUIObject);
        distanceToNearest = Mathf.Infinity;
    }

    void RemoveObject(GUIObject obj)
    {

        if (GUIObjects.Contains(obj))
        {
            obj.ToggleCanvas(false);
            GUIObjects.Remove(obj);
        }

        if (GUIObjects.Count == 0)
        {
            Player.I.onGUI = false;
            if (checkGUICor != null) StopCoroutine(checkGUICor);

            Clear();
        }
    }

    public void Clear()
    {
        nearestGUIObject = null;
        distanceToNearest = Mathf.Infinity;
    }
}
