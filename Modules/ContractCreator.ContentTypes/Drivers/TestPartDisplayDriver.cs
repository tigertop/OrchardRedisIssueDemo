using System.Threading.Tasks;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using ContractCreator.ContentTypes.Models;
using ContractCreator.ContentTypes.Settings;
using ContractCreator.ContentTypes.ViewModels;
using OrchardCore;

namespace ContractCreator.ContentTypes.Drivers
{
    public sealed class TestPartDisplayDriver : ContentPartDisplayDriver<TestPart>
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;

        public TestPartDisplayDriver(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public override IDisplayResult Display(TestPart part, BuildPartDisplayContext context)
        {
            return Initialize<TestPartViewModel>(GetDisplayShapeType(context), m => BuildViewModel(m, part, context))
                .Location(OrchardCoreConstants.DisplayType.Detail, "Content:10")
                .Location(OrchardCoreConstants.DisplayType.Summary, "Content:10");
        }

        public override IDisplayResult Edit(TestPart part, BuildPartEditorContext context)
        {
            return Initialize<TestPartViewModel>(GetEditorShapeType(context), model =>
            {
                model.Show = part.Show;
                model.ContentItem = part.ContentItem;
                model.TestPart = part;
            });
        }
        
        public override async Task<IDisplayResult> UpdateAsync(TestPart part, UpdatePartEditorContext context)
        {
            await context.Updater.TryUpdateModelAsync(part, Prefix, t => t.Show);

            return Edit(part, context);
        }

        private static void BuildViewModel(TestPartViewModel model, TestPart part, BuildPartDisplayContext context)
        {
            var settings = context.TypePartDefinition.GetSettings<TestPartSettings>();

            model.ContentItem = part.ContentItem;
            model.MySetting = settings.MySetting;
            model.Show = part.Show;
            model.TestPart = part;
            model.Settings = settings;
        }
    }
}
