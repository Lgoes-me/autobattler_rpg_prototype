using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class SceneNodeView : Node
{
    public SceneGraphView SceneGraphView { get; private set; }
    public SceneNodeData SceneNodeData { get; private set; }
    
    public SceneNodeView(SceneGraphView sceneGraphView, SceneNodeData sceneNodeData)
    {
        SceneGraphView = sceneGraphView;
        SceneNodeData = sceneNodeData;
        
        title = sceneNodeData.RoomPrefab?.gameObject.name ?? "Scene";
        viewDataKey = sceneNodeData.Id;
        
        style.left = SceneNodeData.Position.x;
        style.top = sceneNodeData.Position.y;
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        SceneNodeData.Position = new Vector2(newPos.xMin, newPos.yMin);
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        evt.menu.AppendAction("Delete", (a) => SceneGraphView.DeleteNodeView(this));
    }
}