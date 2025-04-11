using UnityEngine.UI;

public class MyCell : Cell
{
    public Text text;

    public override void Enable(ICellData content)
    {
        base.Enable(content);
    }

    public override void UpdatePosition(float position)
    {
        base.UpdatePosition(position);
        text.text = Index+":"+position.ToString();;
    }
}
