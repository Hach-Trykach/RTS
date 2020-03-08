using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
    void Start() {
    }

    void Update() {
    }

    void OnMouseEnter() {
        transform.GetChild(0).GetComponent<Image>().color = Color.green;
    }

    void OnMouseExit() {
        transform.GetChild(0).GetComponent<Image>().color = Color.white;
    }
}
