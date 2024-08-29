using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerCard : MonoBehaviour, IPointerClickHandler, IDragHandler, IEndDragHandler
{
    [field: SerializeField] private PawnController Pawn { get; set; }
    [field: SerializeField] private PawnData PawnData { get; set; }
    [field: SerializeField] private Image Image { get; set; }
    [field: SerializeField] private PlayerArenaController PlayerArenaController { get; set; }

    private Vector3 StartingPosition { get; set; }
    private bool IsDragging { get; set; }

    private void Start()
    {
        StartingPosition = transform.position;
        Image.color = Color.grey;
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

        pawnController.Init(arenaController, PawnData.ToDomain());

        return pawnController;
    }
}