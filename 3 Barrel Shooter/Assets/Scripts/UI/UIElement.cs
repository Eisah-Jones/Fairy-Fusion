public interface UIElement
{
    void Interact(int dir);

    void Select();

    string GetElementType();

    string GetElementName();

    //bool GetIsReady();
}
