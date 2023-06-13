namespace Ecs.Components
{
    public struct LatestClickedSlotComponent
    {
        public SlotMetaData SlotMetaData;
    }

    public class SlotMetaData
    {
        public byte Index;
        public bool IsHandled;
    }
}