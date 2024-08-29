using UnityEngine;

public class PlayerArenaController : MonoBehaviour
{
    [field: SerializeField] private ArenaController ArenaController { get; set; }
    [field: SerializeField] private Transform SelectionFeedback { get; set; }
    
    [field: SerializeField] private Material Red { get; set; }
    [field: SerializeField] private Material Blue { get; set; }
    [field: SerializeField] private Renderer SelectionFeedbackRenderer { get; set; }
    private PlayerCard SelectedPlayerCard { get; set; }

    private Camera Camera { get; set; }
    private bool CanPlacePawn { get; set; }

    private void Start()
    {
        Camera = Camera.main;
    }

    public void SelectPlayerPawn(PlayerCard playerCard)
    {
        SelectedPlayerCard = playerCard;
    }

    public void UnselectPlayerPawn()
    {
        if (CanPlacePawn)
        {
            PlacePawn();
        }
        else
        {
            UnselectCard();
        }

        SelectedPlayerCard = null;
    }

    private void Update()
    {
        if (SelectedPlayerCard == null)
            return;

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
            UnselectCard();
        }
    }

    private void UnselectCard()
    {
        SelectedPlayerCard.Show();
        SelectionFeedback.gameObject.SetActive(false);
        CanPlacePawn = false;
    }

    private void PlacePawn()
    {
        if (SelectedPlayerCard == null)
            return;
        
        var pawn = SelectedPlayerCard.GetPawnController(ArenaController, SelectionFeedback);
        ArenaController.AddPlayerPawn(pawn);
        
        SelectedPlayerCard.gameObject.SetActive(false);
        SelectionFeedback.gameObject.SetActive(false);
        CanPlacePawn = false;
    }

    private void OnPointerHit(RaycastHit hit)
    {
        if (SelectedPlayerCard == null)
            return;
        
        SelectedPlayerCard.Hide();
        SelectionFeedback.gameObject.SetActive(true);
        SelectionFeedback.transform.position = hit.point;

        if ((Application.Instance.PlayerManager.PlayerController.transform.position - hit.point).magnitude > 3 ||
            hit.normal.y <= 0)
        {
            SelectionFeedbackRenderer.material = Red;
            CanPlacePawn = false;
            return;
        }
        
        SelectionFeedbackRenderer.material = Blue;
        CanPlacePawn = true;
    }
}