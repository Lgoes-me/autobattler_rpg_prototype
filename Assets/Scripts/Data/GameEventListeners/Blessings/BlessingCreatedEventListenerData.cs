using System;

[Serializable]
public class BlessingCreatedEventListenerData : BaseEventListenerData<IBlessingCreatedValidatorEffect, IBlessingCreatedEffect>
{
    public void OnBlessingCreated()
    {
        if (Validator != null && !Validator.Validate())
            return;

        Effect.OnBlessingCreated();
    }
}

public interface IBlessingCreatedValidatorEffect : IEventValidatorData
{
    bool Validate();
}

public interface IBlessingCreatedEffect : IEventEffectData
{
    void OnBlessingCreated();
}