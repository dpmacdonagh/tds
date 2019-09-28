using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Com.TDS {
  public class UiManager : MonoBehaviour {
    public static UiManager Instance;
    public Text healthText;

    void Start () {
      Instance = this;
    }

    public void SetHealth(float newHealth) {
      healthText.text = newHealth.ToString();
    }
  }

}