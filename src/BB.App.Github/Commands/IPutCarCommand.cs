namespace BB.App.Github.Commands
{
    using Boilerplate.AspNetCore;
    using BB.App.Github.ViewModels;

    public interface IPutCarCommand : IAsyncCommand<int, SaveCar>
    {
    }
}
