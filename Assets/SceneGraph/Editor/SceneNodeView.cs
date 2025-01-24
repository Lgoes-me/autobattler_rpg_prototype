using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class SceneNodeView : Node
{
    public SceneNodeData SceneNodeData { get; private set; }
    private SceneGraphView SceneGraphView { get; set; }
    
    private List<Port> Inputs { get; set; }
    private List<Port> Outputs { get; set; }
    
    public SceneNodeView(SceneGraphView sceneGraphView, SceneNodeData sceneNodeData)
    {
        SceneGraphView = sceneGraphView;
        SceneNodeData = sceneNodeData;
        
        title = sceneNodeData.RoomPrefab?.gameObject.name ?? "Scene";
        viewDataKey = sceneNodeData.Id;
        
        style.left = SceneNodeData.Position.x;
        style.top = sceneNodeData.Position.y;

        CreateInputPorts();
        CreateOutputPorts();
        
        var preview = new Image
        {
            image = AssetPreview.GetAssetPreview(SceneNodeData.RoomPrefab) ?? 
                    AssetPreview.GetMiniThumbnail(SceneNodeData.RoomPrefab)
        };

        mainContainer.Add(preview);
        
    }
    
    private void CreateInputPorts()
    {
        Inputs = new List<Port>();

        foreach (var door in SceneNodeData.Doors)
        {
            var input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(SpawnData));
            input.portName = door.Name;
            inputContainer.Add(input);
            
            Inputs.Add(input);
        }
    }

    private void CreateOutputPorts()
    {
        Outputs = new List<Port>();
        
        foreach (var door in SceneNodeData.Doors)
        {
            var output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(SpawnData)); 
            output.portName = door.Name;
            outputContainer.Add(output);
            
            Outputs.Add(output);
        }
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