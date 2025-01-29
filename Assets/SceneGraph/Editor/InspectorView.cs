using UnityEditor;
using UnityEngine.UIElements;

public class InspectorView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<InspectorView, UxmlTraits> { }
    
    private Editor Editor { get; set; }

    public void UpdateSelection(SceneNodeView nodeView)
    {
        Clear();
        
        UnityEngine.Object.DestroyImmediate(Editor);
        Editor = Editor.CreateEditor(nodeView.SceneNodeData);
        IMGUIContainer container = new IMGUIContainer(() => Editor.OnInspectorGUI());
        Add(container);
    }
}
