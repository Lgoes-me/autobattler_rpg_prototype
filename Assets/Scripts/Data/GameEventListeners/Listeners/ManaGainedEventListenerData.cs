using System;

[Serializable]
public class ManaGainedEventListenerData : BaseEventListenerData<IResourceChangedValidator, IResourceChangedEffect>
{
    public void OnManaGained(Battle battle, PawnController pawnController, int value)
    {
        if (Validator != null && !Validator.Validate(battle, pawnController, value))
            return;

        Effect.OnResourceChanged(battle, pawnController, value);
    }
}

