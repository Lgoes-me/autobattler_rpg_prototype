using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerCard : MonoBehaviour, IPointerClickHandler, IDragHandler, IEndDragHandler
{
    [field: SerializeField] private Image Image { get; set; }
    [field: SerializeField] private TextMeshProUGUI Name { get; set; }
    [field: SerializeField] private PlayerArenaController PlayerArenaController { get; set; }

    private PawnController Pawn { get; set; }

    private Vector3 StartingPosition { get; set; }
    private bool IsDragging { get; set; }

    public void Init(PawnController pawn)
    {
        Pawn = pawn;
        Name.SetText(pawn.PawnData.name);
        Image.color = Color.grey;
        StartingPosition = transform.position;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayerArenaController.SelectPlayerPawn(this);
        Image.color = Color.blue;
        IsDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        PlayerArenaController.SelectPlayerPawn(this);
        Image.color = Color.blue;
        IsDragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Image.color = Color.grey;
        IsDragging = false;
        transform.position = StartingPosition;

        PlayerArenaController.UnselectPlayerPawn();
    }

    private void Update()
    {
        if (!IsDragging)
            return;

        transform.position = Input.mousePosition;
    }

    public void Hide()
    {
        Image.color = new Color(0, 0, 0, 0);
    }

    public void Show()
    {
        Image.color = IsDragging ? Color.blue : Color.grey;
    }

    public PawnController GetPawnController(ArenaController arenaController, Transform spawnPosition)
    {
        enabled = false;

        var pawnController = Instantiate(
            Pawn,
            spawnPosition.position,
            spawnPosition.rotation,
            arenaController.transform);
        
        return pawnController.Init();
    }
}