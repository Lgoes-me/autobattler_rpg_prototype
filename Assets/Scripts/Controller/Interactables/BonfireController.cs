﻿public class BonfireController : InteractionListener
{
    public override void Interact()
    {
        Application.Instance.SceneManager.StartBonfireScene();
    }

    public override void UnSelect()
    {
        base.UnSelect();
        Application.Instance.SceneManager.EndBonfireScene();
    }
}