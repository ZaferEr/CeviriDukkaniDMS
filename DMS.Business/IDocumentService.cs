using System.Collections.Generic;
using Tangent.CeviriDukkani.Domain.Common;
using Tangent.CeviriDukkani.Domain.Dto.Document;
using Tangent.CeviriDukkani.Domain.Dto.Response;

namespace DMS.Business
{
    public interface IDocumentService
    {
        ServiceResult AddDocument(TranslationDocumentDto documentDto, int createdBy);
        ServiceResult<TranslationDocumentDto> GetDocument(int documentId);
        ServiceResult EditDocument(TranslationDocumentDto documentDto, int createdBy);
        ServiceResult<List<TranslationDocumentDto>> GetDocuments();
        ServiceResult<DocumentUploadResponseDto> AnalyzeDocument(string localFolder, string fileName);
        ServiceResult<List<TranslationDocumentPartDto>> GetDocumentPartsNormalized(int translationDocumentId, int partCount);
    }
}
