namespace BB.App.Github
{
    using System.Threading.Tasks;

    public interface IConnectionTester
    {
        Task TestConnection();
    }
}
