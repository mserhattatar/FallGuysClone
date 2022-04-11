using UnityEngine;

public class MainComponent : MonoBehaviour
{
    private ComponentContainer componentContainer;

    private PlayerController playerController;

    private void Awake()
    {
        componentContainer = new ComponentContainer();

        SetPlayerController();


        InitializeComponents();
    }

    private void SetPlayerController()
    {
        playerController = FindObjectOfType<PlayerController>();
        componentContainer.AddComponent("PlayerController", playerController);
    }

    private void InitializeComponents()
    {
        //playerController.Initialize(componentContainer);
    }
}