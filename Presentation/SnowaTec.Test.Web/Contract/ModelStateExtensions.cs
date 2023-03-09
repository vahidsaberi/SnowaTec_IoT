using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SnowaTec.Test.Web.Contract
{
    public static class ModelStateExtensions
    {
        public static ModelStateDictionary MarkAllFieldsAsSkipped(this ModelStateDictionary modelState)
        {
            foreach (var state in modelState.Select(x => x.Value))
            {
                state.Errors.Clear();
                state.ValidationState = ModelValidationState.Skipped;
            }
            return modelState;
        }
    }
}
