using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dashGhost : MonoBehaviour
{
    public float timeSpent = 0f;
    public float totalTime = 1.5f;

    public SpriteRenderer _sprite;
    public Transform location;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        location = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        timeSpent = timeSpent + Time.deltaTime;
        Color temp = _sprite.color;
        temp.a = 1.0f - (timeSpent / totalTime);
        _sprite.color = temp;

        if(1.0f - (timeSpent / totalTime) <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
