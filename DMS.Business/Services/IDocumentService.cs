using System.Collections.Generic;
using Tangent.CeviriDukkani.Domain.Common;
using Tangent.CeviriDukkani.Domain.Dto.Document;
using Tangent.CeviriDukkani.Domain.Dto.Response;

namespace DMS.Business.Services {
    public interface IDocumentService {
        ServiceResult<TranslationDocumentDto> AddTranslationDocument(TranslationDocumentDto documentDto, int createdBy);
        ServiceResult<TranslationDocumentDto> GetTranslationDocument(int documentId);
        ServiceResult<TranslationDocumentDto> EditTranslationDocument(TranslationDocumentDto documentDto, int createdBy);
        ServiceResult<List<TranslationDocumentDto>> GetTranslationDocuments();
        ServiceResult<GeneralDocumentDto> AddGeneralDocument(GeneralDocumentDto documentDto, int createdBy);
        ServiceResult<GeneralDocumentDto> GetGeneralDocument(int documentId);
        ServiceResult<GeneralDocumentDto> EditGeneralDocument(GeneralDocumentDto documentDto, int createdBy);
        ServiceResult<List<GeneralDocumentDto>> GetGeneralDocuments();
        ServiceResult<UserDocumentDto> AddUserDocument(UserDocumentDto documentDto, int createdBy);
        ServiceResult<UserDocumentDto> GetUserDocument(int documentId);
        ServiceResult<UserDocumentDto> EditUserDocument(UserDocumentDto documentDto, int createdBy);
        ServiceResult<List<UserDocumentDto>> GetUserDocuments();
        ServiceResult<DocumentUploadResponseDto> AnalyzeDocument(string localFolder, string fileName);
        ServiceResult<List<TranslationDocumentPartDto>> GetDocumentPartsNormalized(int translationDocumentId, int partCount, int createdBy);
        ServiceResult<TranslationDocumentPartDto> GetTranslationDocumentPartById(int translationDocumentPartId);

    }
}
