using Photon.Pun;
using UnityEngine;

[System.Serializable]
public enum TestMouse { Locked, Confined}
public class TestScript : MonoBehaviour
{
    public TestMouse mouseBehaviour;
    public bool isOffline;

    private void Awake()
    {
        if (isOffline)
        {
            PhotonNetwork.OfflineMode = true;
        }
    }

    void Start()
    {
        switch (mouseBehaviour)
        {
            case TestMouse.Confined:
                Cursor.lockState = CursorLockMode.Confined;
                break;
            case TestMouse.Locked:
                Cursor.lockState = CursorLockMode.Locked;
                break;

        }
    }

}
