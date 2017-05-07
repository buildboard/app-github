namespace BB.App.Github.Commands
{
    using Boilerplate.AspNetCore;
    using BB.App.Github.ViewModels;

    public interface IGetCarPageCommand : IAsyncCommand<PageOptions>
    {
    }
}
