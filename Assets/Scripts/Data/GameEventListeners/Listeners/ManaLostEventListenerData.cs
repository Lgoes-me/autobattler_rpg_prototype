using System;

[Serializable]
public class ManaLostEventListenerData : BaseEventListenerData<IResourceChangedValidator, IResourceChangedEffect>
{
    public void OnManaLost(Battle battle, PawnController pawnController, int value)
    {
        if (Validator != null && !Validator.Validate(battle, pawnController, value))
            return;

        Effect.OnResourceChanged(battle, pawnController, value);
    }
}
