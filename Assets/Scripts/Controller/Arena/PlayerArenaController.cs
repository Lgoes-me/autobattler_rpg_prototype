using UnityEngine;

public class PlayerArenaController : MonoBehaviour
{
    [field: SerializeField] private ArenaController ArenaController { get; set; }
    [field: SerializeField] private Transform SelectionFeedback { get; set; }
    private PlayerCard SelectedPlayerCard { get; set; }

    private bool CanPlacePawn { get; set; }

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

        SelectedPlayerCard = null;
    }

    private void Update()
    {
        if (SelectedPlayerCard == null)
            return;

        if (Physics.Raycast(
            Camera.main.ScreenPointToRay(Input.mousePosition),
            out var hit,
            100f,
            LayerMask.GetMask("Ground")))
        {
            OnPointerHit(hit);
        }
        else
        {
            SelectedPlayerCard.Show();
            SelectionFeedback.gameObject.SetActive(false);
            CanPlacePawn = false;
        }
    }

    private void PlacePawn()
    {
        if (SelectedPlayerCard == null)
            return;

        var pawn = Instantiate(SelectedPlayerCard.Pawn, ArenaController.transform);
        pawn.transform.position = SelectionFeedback.position;
        pawn.Init(ArenaController, SelectedPlayerCard.PawnData.ToDomain());
        ArenaController.ActivePawns.Add(pawn);
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
        CanPlacePawn = true;
    }
}