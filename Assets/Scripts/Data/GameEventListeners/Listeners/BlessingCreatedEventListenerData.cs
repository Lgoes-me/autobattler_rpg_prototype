using System;

[Serializable]
public class BlessingCreatedEventListenerData : BaseEventListenerData<IBlessingCreatedValidator, IBlessingCreatedEffect>
{
    public void OnBlessingCreated()
    {
        if (Validator != null && !Validator.Validate())
            return;

        Effect.OnBlessingCreated();
    }
}

public interface IBlessingCreatedValidator : IEventValidatorData
{
    bool Validate();
}

public interface IBlessingCreatedEffect : IEventEffectData
{
    void OnBlessingCreated();
}