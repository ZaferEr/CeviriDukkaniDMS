using System.Collections.Generic;
using Tangent.CeviriDukkani.Domain.Common;
using Tangent.CeviriDukkani.Domain.Dto.Translation;

namespace DMS.Business.ExternalServices {
    public interface ITranslationService {
        ServiceResult SaveTranslationOperations(List<TranslationOperationDto> translationOperations);
    }
}