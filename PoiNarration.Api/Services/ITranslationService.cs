using PoiNarration.Core.Models;

namespace PoiNarration.Api.Services;

public interface ITranslationService
{
    Task<List<BoothTranslationLocal>> BuildBoothTranslationsAsync(Booth booth);
    Task<List<BoothMenuItemTranslationLocal>> BuildMenuTranslationsAsync(BoothMenuItem menuItem);
}