using Microsoft.AspNetCore.Mvc.ModelBinding;
using Sqids;

namespace API.Binders;

public class CookBookIdBinder : IModelBinder
{
    private readonly SqidsEncoder<long> _idEncoder;

    public CookBookIdBinder(SqidsEncoder<long> idEncoder) => _idEncoder = idEncoder;
    
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        // config
        var modelName = bindingContext.ModelName;
        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

        if (valueProviderResult == ValueProviderResult.None) return Task.CompletedTask;
        
        bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

        var value = valueProviderResult.FirstValue;

        if (string.IsNullOrEmpty(value)) return Task.CompletedTask;
        
        // good stuff
        var id = _idEncoder.Decode(value).Single();
        bindingContext.Result = ModelBindingResult.Success(id);
        
        return Task.CompletedTask;
    }
}