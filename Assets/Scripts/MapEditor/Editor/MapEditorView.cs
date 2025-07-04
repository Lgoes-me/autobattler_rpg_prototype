﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class MapEditorView : GraphView
{
    public Action<BaseNodeView> OnNodeSelected;
    public MapData MapData { get; private set; }

    public MapEditorView()
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/MapEditor/Editor/MapEditor.uss");
        styleSheets.Add(styleSheet);
    }

    public void PopulateView(MapData mapData)
    {
        MapData = mapData;

        graphViewChanged -= OnGraphViewChange;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChange;

        var nodeViews = new List<BaseNodeView>();
        
        foreach (var node in MapData.Nodes)
        {
            var nodeView = node.ToNodeView();
            
            nodeView.OnNodeSelected = OnNodeSelected;
            
            nodeViews.Add(nodeView);
            AddElement(nodeView);
        }

        foreach (var nodeView in nodeViews)
        {
            foreach (var (key, port) in nodeView.Inputs)
            {
                var data = (DoorData)port.userData;
                
                if(!data.SetUp)
                    continue;
                
                if(data.Port != "input")
                    continue;
                
                if(port.connected)
                {
                    nodeView.RemoveOutput(data.Id);
                    continue;
                }

                var otherNode = nodeViews.FirstOrDefault(n => n.viewDataKey == data.SceneDestination);
                
                if(otherNode == null)
                    continue;

                var otherPort =
                    otherNode.Outputs.FirstOrDefault(d => ((DoorData) d.Value.userData).Id == data.DoorDestination).Value;
                
                if(otherPort == null)
                    continue;
                
                var edge = port.ConnectTo(otherPort);
                nodeView.RemoveOutput(data.Id);
                AddElement(edge);
            }
            
            foreach (var (key, port) in nodeView.Outputs)
            {
                var data = (DoorData)port.userData;
                
                if(!data.SetUp)
                    continue;
                
                if(data.Port != "output")
                    continue;
                
                if(port.connected)
                {
                    nodeView.RemoveInput(data.Id);
                    continue;
                }
                
                var otherNode = nodeViews.FirstOrDefault(n => n.viewDataKey == data.SceneDestination);
                
                if(otherNode == null)
                    continue;

                var otherPort =
                    otherNode.Inputs.FirstOrDefault(d => ((DoorData) d.Value.userData).Id == data.DoorDestination).Value;
                
                if(otherPort == null)
                    continue;
                
                var edge = port.ConnectTo(otherPort);
                nodeView.RemoveInput(data.Id);
                AddElement(edge);
            }
            
            FrameAll();
        }
    }

    private GraphViewChange OnGraphViewChange(GraphViewChange graphViewChange)
    {
        if (graphViewChange.elementsToRemove != null)
        {
            foreach (var element in graphViewChange.elementsToRemove)
            {
                if (element is BaseNodeView nodeView)
                {
                    MapData.DeleteNode(nodeView.NodeData);
                }
                
                if (element is Edge edge)
                {
                    var input = edge.input;
                    var output = edge.output;

                    var spawnInputData = (DoorData)input.userData;
                    var spawnOutputData = (DoorData)output.userData;
                    
                    spawnInputData.Clear();
                    ((BaseNodeView) input.node).AddOutput(spawnInputData.Id);
                    EditorUtility.SetDirty(((BaseNodeView) input.node).NodeData);

                    spawnOutputData.Clear();
                    ((BaseNodeView) output.node).AddInput(spawnOutputData.Id);
                    EditorUtility.SetDirty(((BaseNodeView) output.node).NodeData);

                    AssetDatabase.SaveAssets();
                }
            }
            
        }
        
        if (graphViewChange.edgesToCreate != null)
        {
            foreach (var edge in graphViewChange.edgesToCreate)
            {
                var input = edge.input;
                var output = edge.output;

                var spawnInputData = (DoorData) input.userData;
                var spawnOutputData = (DoorData) output.userData;

                var outputNode = (BaseNodeView) output.node;
                var inputNode = (BaseNodeView) input.node;

                var outputNodeData = outputNode.NodeData;
                var inputNodeData = inputNode.NodeData;

                spawnInputData.Connect(outputNodeData.Id, spawnOutputData.Id, "input");
                inputNode.RemoveOutput(spawnInputData.Id);
                EditorUtility.SetDirty(inputNodeData);

                spawnOutputData.Connect(inputNodeData.Id, spawnInputData.Id, "output");
                outputNode.RemoveInput(spawnOutputData.Id);
                EditorUtility.SetDirty(outputNodeData);

                AssetDatabase.SaveAssets();
            }
        }
        
        return graphViewChange;
    }

    private void CreateSceneNode(Vector2 eventInfoLocalMousePosition)
    {
        var path = EditorUtility.OpenFilePanel("Choose prefab", "Assets/Prefabs/Rooms", "prefab");

        if (path != null)
        {
            string[] separatedPath = path.Split(new[] {"Assets"}, StringSplitOptions.None);
            
            var id = Guid.NewGuid().ToString();
            var prefab = AssetDatabase.LoadAssetAtPath<RoomController>("Assets" + separatedPath[1]);
            
            var nodeParams = new SceneNodeDataParams(id, prefab);
            
            CreateNode<SceneNodeData>(eventInfoLocalMousePosition, nodeParams);
        }
    }
    
    private void CreateCutsceneNode(Vector2 eventInfoLocalMousePosition)
    {
        var path = EditorUtility.OpenFilePanel("Choose prefab", "Assets/Prefabs/Rooms", "prefab");

        if (path != null)
        {
            string[] separatedPath = path.Split(new[] {"Assets"}, StringSplitOptions.None);
            
            var id = Guid.NewGuid().ToString();
            var prefab = AssetDatabase.LoadAssetAtPath<CutsceneRoomController>("Assets" + separatedPath[1]);
            
            var nodeParams = new CutsceneNodeDataParams(id, prefab);
            
            CreateNode<CutsceneNodeData>(eventInfoLocalMousePosition, nodeParams);
        }
    }

    private void CreateSpawnNode(Vector2 eventInfoLocalMousePosition)
    {
        var id = Guid.NewGuid().ToString();
        var nodeParams = new SpawnNodeDataParams(id);
        
        CreateNode<SpawnNodeData>(eventInfoLocalMousePosition, nodeParams);
    }
    
    private void CreateBlockedEventNode(Vector2 eventInfoLocalMousePosition)
    {
        var id = Guid.NewGuid().ToString();
        var nodeParams = new BlockedEventNodeDataParams(id);
        
        CreateNode<BlockedEventNodeData>(eventInfoLocalMousePosition, nodeParams);
    }
    
    private void CreateForkedNode(Vector2 eventInfoLocalMousePosition)
    {
        var id = Guid.NewGuid().ToString();
        var nodeParams = new ForkedNodeDataParams(id);
        
        CreateNode<ForkedNodeData>(eventInfoLocalMousePosition, nodeParams);
    }

    private void CreateNode<T>(Vector2 eventInfoLocalMousePosition, NodeDataParams nodeParams) where T : BaseNodeData
    {
        var node = MapData.AddNode<T>();
        
        node.Init(nodeParams);
        node.SetPosition(viewTransform.matrix.inverse.MultiplyPoint(eventInfoLocalMousePosition));
            
        var nodeView = node.ToNodeView();
        nodeView.OnNodeSelected = OnNodeSelected;
        AddElement(nodeView);
            
        ClearSelection();
        AddToSelection(nodeView);
        
        EditorUtility.SetDirty(node);
        AssetDatabase.SaveAssets();
    }
    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        var mousePosition = evt.mousePosition;
        
        evt.menu.AppendAction("Spawn", _ => CreateSpawnNode(mousePosition));
        evt.menu.AppendAction("Scene", _ => CreateSceneNode(mousePosition));
        evt.menu.AppendAction("Cutscene", _ => CreateCutsceneNode(mousePosition));
        evt.menu.AppendAction("BlockedByEvent", _ => CreateBlockedEventNode(mousePosition));
        evt.menu.AppendAction("Fork", _ => CreateForkedNode(mousePosition));
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList()
            .Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();
    }
}