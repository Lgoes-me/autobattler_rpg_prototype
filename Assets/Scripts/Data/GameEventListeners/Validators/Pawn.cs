using System;
using UnityEngine;

[Serializable]
public class PawnControllerTeamValidator :
    IAttackValidator,
    IHealthGainedValidator,
    IHealthLostValidator,
    IManaGainedValidator,
    IManaLostValidator,
    IPawnDeathValidator,
    ISpecialAttackValidator
{
    [field: SerializeField] private TeamType Team { get; set; }

    public bool Validate(Battle battle, PawnController pawnController, int value) => DoValidation(pawnController);
    public bool Validate(Battle battle, PawnController pawnController, DamageDomain damage) => DoValidation(pawnController);
    public bool Validate(Battle battle, PawnController abilityUser, Ability ability) => DoValidation(abilityUser);

    private bool DoValidation(PawnController pawnController)
    {
        return pawnController.Pawn.Team == Team;
    }
}

[Serializable]
public class PawnControllerTeamAndMetadataValidator : 
    IAttackValidator,
    IHealthGainedValidator,
    IHealthLostValidator,
    IManaGainedValidator,
    IManaLostValidator,
    IPawnDeathValidator,
    ISpecialAttackValidator
{
    [field: SerializeField] private TeamType Team { get; set; }
    [field: SerializeField] private string MetaDataKey { get; set; }

    public bool Validate(Battle battle, PawnController pawnController, int value) => DoValidation(pawnController);
    public bool Validate(Battle battle, PawnController pawnController, DamageDomain damage) => DoValidation(pawnController);
    public bool Validate(Battle battle, PawnController abilityUser, Ability ability) => DoValidation(abilityUser);

    private bool DoValidation(PawnController pawnController)
    {
        return pawnController.Pawn.Team == Team &&
               pawnController.Pawn.GetComponent<MetaDataComponent>().CheckMetaData(MetaDataKey);
    }
}