using OrchardCore.ContentManagement.Handlers;
using System.Threading.Tasks;
using ContractCreator.ContentTypes.Models;

namespace ContractCreator.ContentTypes.Handlers
{
    public class TestPartHandler : ContentPartHandler<TestPart>
    {
        public override Task InitializingAsync(InitializingContentContext context, TestPart part)
        {
            part.Show = true;

            return Task.CompletedTask;
        }
    }
}