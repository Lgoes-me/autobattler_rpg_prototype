using System.Collections.Generic;
using UnityEngine;

public class PlayerArenaController : MonoBehaviour
{
    [field: SerializeField] private ArenaController ArenaController { get; set; }
    [field: SerializeField] private Transform SelectionFeedback { get; set; }
    
    [field: SerializeField] private Material Red { get; set; }
    [field: SerializeField] private Material Blue { get; set; }
    [field: SerializeField] private Renderer SelectionFeedbackRenderer { get; set; }
    
    private PawnController SelectedPlayerPawn { get; set; }

    private Camera Camera { get; set; }
    private bool CanPlacePawn { get; set; }

    private void Start()
    {
        Camera = Camera.main;
    }

    public void SelectPlayerPawn(PawnController pawn)
    {
        SelectedPlayerPawn = pawn;
    }

    public void UnselectPlayerPawn()
    {
        if (CanPlacePawn)
        {
            PlacePawn();
        }
        else
        {
            UnselectPawn();
        }

        SelectedPlayerPawn = null;
    }

    private void Update()
    {
        if (SelectedPlayerPawn == null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(
                    Camera.ScreenPointToRay(Input.mousePosition),
                    out var pawnHit,
                    100f,
                    LayerMask.GetMask("PlayerPawn")))
                {
                    SelectPlayerPawn(pawnHit.collider.GetComponent<PawnController>());
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            UnselectPlayerPawn();
            return;
        }

        if (Physics.Raycast(
            Camera.ScreenPointToRay(Input.mousePosition),
            out var hit,
            100f,
            LayerMask.GetMask("Ground")))
        {
            OnPointerHit(hit);
        }
        else
        {
            UnselectPawn();
        }
    }

    private void UnselectPawn()
    {
        if (SelectedPlayerPawn != null)
        {
            SelectedPlayerPawn.gameObject.SetActive(true);
        }
        
        SelectionFeedback.gameObject.SetActive(false);
        CanPlacePawn = false;
    }

    private void PlacePawn()
    {
        if (SelectedPlayerPawn == null)
            return;
        
        SelectionFeedback.gameObject.SetActive(false);
        SelectedPlayerPawn.transform.position = SelectionFeedback.transform.position;
        SelectedPlayerPawn.gameObject.SetActive(true);
        CanPlacePawn = false;
    }

    private void OnPointerHit(RaycastHit hit)
    {
        if (SelectedPlayerPawn == null)
            return;
        
        SelectedPlayerPawn.gameObject.SetActive(false);
        SelectionFeedback.gameObject.SetActive(true);
        SelectionFeedback.transform.position = hit.point;

        if ((SelectedPlayerPawn.transform.position - hit.point).magnitude > 3 || hit.normal.y <= 0)
        {
            SelectionFeedbackRenderer.material = Red;
            CanPlacePawn = false;
            return;
        }
        
        SelectionFeedbackRenderer.material = Blue;
        CanPlacePawn = true;
    }
}