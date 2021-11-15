namespace Blorc.Tye.Components
{
    using System.Threading.Tasks;

    using Blorc.Components;
    using Blorc.Services;

    public class IndexComponent : BlorcComponentBase
    {
        protected IExecutionService SurveyExcutionService { get; set; }

        protected IUIVisualizationService SurveyVisualizationService { get; set; }

        public IndexComponent()
            :base(true)
        {
        }

        protected async Task OnButtonClickAsync()
        {
            if (SurveyExcutionService is not null)
            {
                await SurveyExcutionService.ExecuteAsync();
            }

            if (SurveyVisualizationService is not null)
            {
                await SurveyVisualizationService.ShowAsync();
            }
        }
    }
}
