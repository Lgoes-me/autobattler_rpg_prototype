using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BaseNodeView: Node
{
    public Action<BaseNodeView> OnNodeSelected;
    public BaseNodeData NodeData { get; }

    public Dictionary<string, Port> Inputs { get; private set; }
    public Dictionary<string, Port> Outputs { get; private set; }

    protected BaseNodeView(BaseNodeData nodeData)
    {
        NodeData = nodeData;
        NodeData.OnNodeDataUpdated = UpdateView;
        
        title = NodeData.Name;
        viewDataKey = NodeData.Id;

        SetPosition(new Rect(NodeData.Position, Vector2.zero));

        CreateInputPorts();
        CreateOutputPorts();
    }

    private void UpdateView()
    {
        title = NodeData.Name;
    }
    
    private void CreateInputPorts()
    {
        Inputs = new Dictionary<string, Port>();

        foreach (var door in NodeData.Doors)
        {
            AddInput(door);
        }
    }

    private void CreateOutputPorts()
    {
        Outputs = new Dictionary<string, Port>();

        foreach (var door in NodeData.Doors)
        {
            AddOutput(door);
        }
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        
        NodeData.SetPosition(new Vector2(newPos.x, newPos.y));
        EditorUtility.SetDirty(NodeData);
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
        var door = NodeData.Doors.FirstOrDefault(d => d.Id == id);

        if (door != null)
        {
            AddInput(door);
        }
    }
    
    public void AddOutput(string id)
    {
        var door = NodeData.Doors.FirstOrDefault(d => d.Id == id);

        if (door != null)
        {
            AddOutput(door);
        }
    }

    private void AddInput(DoorData door)
    {
        if(Inputs.ContainsKey(door.Id))
            return;
        
        var input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(DoorData));
        input.userData = door;
        input.portName = door.Name;
        inputContainer.Add(input);

        Inputs.Add(door.Id, input);
    }

    private void AddOutput(DoorData door)
    {
        if(Outputs.ContainsKey(door.Id))
            return;
        
        var output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(DoorData));
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