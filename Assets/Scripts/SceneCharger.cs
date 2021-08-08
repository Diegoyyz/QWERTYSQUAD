using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneCharger : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.LoadScene(1);
    }
}
