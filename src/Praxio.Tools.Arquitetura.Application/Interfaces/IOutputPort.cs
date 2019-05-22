namespace Praxio.Tools.Arquitetura.Application.Interfaces
{
    public interface IOutputPort<in TUseCaseResponse>
    {
        void Handler(TUseCaseResponse response);        
    }
}