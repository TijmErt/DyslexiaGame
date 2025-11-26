using System;
using UnityEngine;
using TMPro;

public class IngredientItem : MonoBehaviour
{
    [SerializeField] private string letter;
    [SerializeField] private TMP_Text text;
    [SerializeField] private GameObject playerPosPoint;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player1")
        {
            Debug.Log("Player 1 has touched an item");
        }
        if (collision.gameObject.tag == "Player2")
        {
            Debug.Log("Player 2 has touched an item");
        }
    }

    public Vector3 GetPlayerPosPoint()
    {
        return playerPosPoint.transform.position;
    }
}
