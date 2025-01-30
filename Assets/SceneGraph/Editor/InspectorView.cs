using UnityEditor;
using UnityEngine.UIElements;

public class InspectorView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<InspectorView, UxmlTraits> { }
    private Editor Editor { get; set; }

    public void UpdateSelection(BaseNodeView nodeView)
    {
        Clear();
        
        UnityEngine.Object.DestroyImmediate(Editor);

        if (nodeView != null)
        {
            Editor = Editor.CreateEditor(nodeView.NodeData);
            IMGUIContainer container = new IMGUIContainer(() => Editor.OnInspectorGUI());
            Add(container);
        }
    }
}
