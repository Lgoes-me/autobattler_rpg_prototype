using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, IManager
{
    [field: SerializeField] private LayerMask LayerMask { get; set; }
    
    public Vector3 ForwardVector { get; private set; }
    public Vector3 RightVector { get; private set; }
    public Vector2 MoveInput { get; private set; }
    public Vector2 MousePosition { get; private set; }
    
    private Coroutine Coroutine { get; set; }
    private PlayerManager PlayerManager { get; set; }
    private CinemachineBrain CinemachineBrain { get; set; }
    private CinemachineVirtualCamera VirtualCamera { get; set; }

    private InputActionsSettings InputMap { get; set; }
    
    public void Prepare()
    {
        PlayerManager = Application.Instance.GetManager<PlayerManager>();
        CinemachineBrain = Application.Instance.MainCamera;
        InputMap = new InputActionsSettings();
        InputMap.Enable();
    }

    public void SetNewCameraPosition(CinemachineVirtualCamera virtualCamera, CinemachineBlendDefinition blend)
    {
        CinemachineBrain.m_DefaultBlend = blend;

        if (VirtualCamera != null)
        {
            VirtualCamera.Priority = 0;
        }

        virtualCamera.Priority = 10;

        VirtualCamera = virtualCamera;

        if (Coroutine != null)
        {
            StopCoroutine(Coroutine);
        }

        var cam = virtualCamera.transform;
        
        ForwardVector = Vector3.Dot(cam.forward, -transform.up) > 0.8f ? cam.up : cam.forward;
        RightVector = Vector3.Cross(Vector3.up, ForwardVector).normalized;
    }

    private void Update()
    {
        MoveInput = new Vector2(InputMap.Game.Horizontal.ReadValue<float>(), InputMap.Game.Vertical.ReadValue<float>());
        MousePosition = new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue());
        
        if (Mouse.current.leftButton.isPressed && 
            Physics.Raycast (Camera.main.ScreenPointToRay(MousePosition), out var hit, 100, LayerMask))
        {
            Application.Instance.GetManager<PlayerManager>().SetDestination(hit.point);
        }

        if (InputMap.Game.Pause.ReadValue<bool>())
        {
            Application.Instance.GetManager<PauseManager>().PauseInput();
        }
    }

    private void OnDestroy()
    {
        InputMap.Disable();
    }
}