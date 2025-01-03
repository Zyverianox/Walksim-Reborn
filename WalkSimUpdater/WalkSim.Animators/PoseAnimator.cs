private void RotateHand()
{
    eulerAngles.x -= Mouse.current.delta.ReadValue().y / 10f;
    eulerAngles.x = Mathf.Clamp(eulerAngles.x > 180f ? eulerAngles.x - 360f : eulerAngles.x, -85f, 85f);

    eulerAngles.y += Mouse.current.delta.ReadValue().x / 10f;
    eulerAngles.y = Mathf.Clamp(eulerAngles.y > 180f ? eulerAngles.y - 360f : eulerAngles.y, -85f, 85f);

    if (main.isLeft)
    {
        lookAtLeft = Quaternion.Euler(eulerAngles) * head.forward;
        zRotationLeft += Mouse.current.scroll.ReadValue().y / 5f;
    }
    else
    {
        lookAtRight = Quaternion.Euler(eulerAngles) * head.forward;
        zRotationRight += Mouse.current.scroll.ReadValue().y / 5f;
    }
}

private void PositionHand()
{
    Vector3 offset = main.isLeft ? offsetLeft : offsetRight;

    offset.z += Mouse.current.scroll.ReadValue().y / 1000f;
    if (Keyboard.current.upArrowKey.wasPressedThisFrame)
    {
        offset.z += 0.1f;
    }
    if (Keyboard.current.downArrowKey.wasPressedThisFrame)
    {
        offset.z -= 0.1f;
    }
    offset.z = Mathf.Clamp(offset.z, -0.25f, 0.75f);

    offset.x += Mouse.current.delta.ReadValue().x / 1000f;
    offset.x = Mathf.Clamp(offset.x, -0.5f, 0.5f);

    offset.y += Mouse.current.delta.ReadValue().y / 1000f;
    offset.y = Mathf.Clamp(offset.y, -0.5f, 0.5f);

    if (main.isLeft)
    {
        offsetLeft = offset;
    }
    else
    {
        offsetRight = offset;
    }
}
