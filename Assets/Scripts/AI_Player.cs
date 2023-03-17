using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AI_Player : MonoBehaviour
{
    public OverlayInfo activeTile = null;
    public Vector2 curPosition;
    public float speed = 4f;
    public GameObject player;


    private void Awake()
    {
       
        curPosition = player.transform.position;
       
    }
    private void Update()
    {
        Debug.Log(activeTile);
    }


}
