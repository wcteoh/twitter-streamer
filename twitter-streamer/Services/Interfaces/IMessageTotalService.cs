
namespace TwitterStreamer.Services.Interfaces
{
    public interface IMessageTotalService
    {
        int GetMessageTotal();
        void IncrementMessageTotal();
    }
}
