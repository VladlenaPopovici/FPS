namespace Ecs
{
    public struct LatestClickedSlotComponent
    {
        public SlotMetaData slotMetaData;
    }

    public class SlotMetaData
    {
        public byte index;
        public bool isHandled;
    }
}