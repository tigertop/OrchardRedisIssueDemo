using System;
using System.Threading.Tasks;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using ContractCreator.ContentTypes.Models;
using OrchardCore.DisplayManagement.Handlers;

namespace ContractCreator.ContentTypes.Settings
{
    public sealed class TestPartSettingsDisplayDriver : ContentTypePartDefinitionDisplayDriver
    {
        public override IDisplayResult Edit(ContentTypePartDefinition contentTypePartDefinition, BuildEditorContext context)
        {
            if (!string.Equals(nameof(TestPart), contentTypePartDefinition.PartDefinition.Name))
            {
                return null;
            }

            return Initialize<TestPartSettingsViewModel>("TestPartSettings_Edit", model =>
            {
                var settings = contentTypePartDefinition.GetSettings<TestPartSettings>();

                model.MySetting = settings.MySetting;
                model.TestPartSettings = settings;
            }).Location("Content");
        }

        public override async Task<IDisplayResult> UpdateAsync(ContentTypePartDefinition contentTypePartDefinition, UpdateTypePartEditorContext context)
        {
            if (!string.Equals(nameof(TestPart), contentTypePartDefinition.PartDefinition.Name))
            {
                return null;
            }

            var model = new TestPartSettingsViewModel();

            await context.Updater.TryUpdateModelAsync(model, Prefix, m => m.MySetting);

            context.Builder.WithSettings(new TestPartSettings { MySetting = model.MySetting });

            return Edit(contentTypePartDefinition, context);
        }
    }
}
