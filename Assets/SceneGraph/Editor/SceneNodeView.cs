﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class SceneNodeView : Node
{
    public Action<SceneNodeView> OnNodeSelected;
    public SceneNodeData SceneNodeData { get; private set; }

    private Dictionary<string, Port> Inputs { get; set; }
    private Dictionary<string, Port> Outputs { get; set; }

    public SceneNodeView(SceneNodeData sceneNodeData)
    {
        SceneNodeData = sceneNodeData;

        title = sceneNodeData.name;
        viewDataKey = sceneNodeData.Id;

        SetPosition(new Rect(SceneNodeData.Position, Vector2.one));

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
        Inputs = new Dictionary<string, Port>();

        foreach (var door in SceneNodeData.Doors)
        {
            AddInput(door);
        }
    }

    private void CreateOutputPorts()
    {
        Outputs = new Dictionary<string, Port>();

        foreach (var door in SceneNodeData.Doors)
        {
            AddOutput(door);
        }
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        SceneNodeData.Position = new Vector2(newPos.x, newPos.y);
    }

    public void RemoveInput(string id)
    {
        inputContainer.Remove(Inputs[id]);
        Inputs.Remove(id);
    }

    public void RemoveOutput(string id)
    {
        outputContainer.Remove(Outputs[id]);
        Outputs.Remove(id);
    }

    public void AddInput(string id)
    {
        var door = SceneNodeData.Doors.FirstOrDefault(d => d.Id == id);

        if (door != null)
        {
            AddInput(door);
        }
    }
    
    public void AddOutput(string id)
    {
        var door = SceneNodeData.Doors.FirstOrDefault(d => d.Id == id);

        if (door != null)
        {
            AddOutput(door);
        }
    }

    private void AddInput(SpawnData door)
    {
        var input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(SpawnData));
        input.userData = door;
        input.portName = door.Name;
        inputContainer.Add(input);

        Inputs.Add(door.Id, input);
    }

    private void AddOutput(SpawnData door)
    {
        var output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(SpawnData));
        output.userData = door;
        output.portName = door.Name;
        outputContainer.Add(output);

        Outputs.Add(door.Id, output);
    }

    public override void OnUnselected()
    {
        base.OnUnselected();
        OnNodeSelected?.Invoke(null);
    }

    public override void OnSelected()
    {
        base.OnSelected();
        OnNodeSelected?.Invoke(this);
    }
}