using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using Tangent.CeviriDukkani.Data.Model;
using Tangent.CeviriDukkani.Domain.Common;
using Tangent.CeviriDukkani.Domain.Dto.Document;
using Tangent.CeviriDukkani.Domain.Dto.Response;
using Tangent.CeviriDukkani.Domain.Entities.Document;
using Tangent.CeviriDukkani.Domain.Exceptions;
using Tangent.CeviriDukkani.Domain.Exceptions.ExceptionCodes;
using Tangent.CeviriDukkani.Domain.Mappers;
using Toxy;
using Toxy.Parsers;

namespace DMS.Business {
    public class DocumentService : IDocumentService {
        internal ILog Log { get; } = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly CeviriDukkaniModel _ceviriDukkaniModel;
        private readonly ICustomMapperConfiguration _customMapperConfiguration;

        public DocumentService(CeviriDukkaniModel ceviriDukkaniModel, ICustomMapperConfiguration customMapperConfiguration) {
            _ceviriDukkaniModel = ceviriDukkaniModel;
            _customMapperConfiguration = customMapperConfiguration;
        }

        public ServiceResult AddDocument(TranslationDocumentDto documentDto, int createdBy) {
            var serviceResult = new ServiceResult();
            try {
                documentDto.CreatedBy = createdBy;
                documentDto.Active = true;
                var document = _customMapperConfiguration.GetMapEntity<TranslationDocument, TranslationDocumentDto>(documentDto);

                _ceviriDukkaniModel.TranslationDocuments.Add(document);
                if (_ceviriDukkaniModel.SaveChanges() <= 0) {
                    throw new BusinessException(ExceptionCodes.UnableToInsert);
                }
                serviceResult.ServiceResultType = ServiceResultType.Success;
                serviceResult.Data = _customMapperConfiguration.GetMapDto<TranslationDocumentDto, TranslationDocument>(document);
            } catch (Exception exc) {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                Log.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
            }
            return serviceResult;
        }

        public ServiceResult<TranslationDocumentDto> GetDocument(int documentId) {
            var serviceResult = new ServiceResult<TranslationDocumentDto>();
            try {
                var document = _ceviriDukkaniModel.TranslationDocuments.Find(documentId);
                if (document == null) {
                    throw new DbOperationException(ExceptionCodes.NoRelatedData);
                }
                serviceResult.ServiceResultType = ServiceResultType.Success;
                serviceResult.Data = _customMapperConfiguration.GetMapDto<TranslationDocumentDto, TranslationDocument>(document);
            } catch (Exception exc) {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                Log.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
            }
            return serviceResult;
        }

        public ServiceResult EditDocument(TranslationDocumentDto documentDto, int createdBy) {
            var serviceResult = new ServiceResult();
            try {
                var document = _ceviriDukkaniModel.TranslationDocuments.FirstOrDefault(f => f.Id == documentDto.Id);
                if (document == null) {
                    throw new DbOperationException(ExceptionCodes.NoRelatedData);
                }

                document.Path = documentDto.Path;
                document.Name = documentDto.Name;
                document.Active = documentDto.Active;

                document.PageCount = documentDto.PageCount;
                document.CharCount = documentDto.CharCount;
                document.CharCountWithSpaces = documentDto.CharCountWithSpaces;

                document.UpdatedAt = DateTime.Now;
                document.UpdatedBy = createdBy;

                if (_ceviriDukkaniModel.SaveChanges() <= 0) {
                    throw new BusinessException(ExceptionCodes.UnableToUpdate);
                }
                serviceResult.ServiceResultType = ServiceResultType.Success;
                serviceResult.Data = _customMapperConfiguration.GetMapDto<TranslationDocumentDto, TranslationDocument>(document);
            } catch (Exception exc) {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                Log.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
            }
            return serviceResult;
        }

        public ServiceResult<List<TranslationDocumentDto>> GetDocuments() {
            var serviceResult = new ServiceResult<List<TranslationDocumentDto>>();
            try {
                var documents = _ceviriDukkaniModel.TranslationDocuments.ToList();

                serviceResult.ServiceResultType = ServiceResultType.Success;
                serviceResult.Data = documents.Select(s => _customMapperConfiguration.GetMapDto<TranslationDocumentDto, TranslationDocument>(s)).ToList();
            } catch (Exception exc) {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                Log.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
            }
            return serviceResult;
        }

        public ServiceResult<DocumentUploadResponseDto> AnalyzeDocument(string localFolder, string fileName) {
            var result = new DocumentUploadResponseDto();
            var serviceResult = new ServiceResult<DocumentUploadResponseDto>();
            try {
                result.FilePath = fileName;
                if (IsDocumentExtension(fileName)) {
                    IDocumentParser parser = GetDocumentParser(localFolder);
                    var parsedDoc = parser.Parse();

                    result.PageCount = parsedDoc.TotalPageNumber;
                    result.CharCountWithSpaces = parsedDoc.ToString().Count(c => char.IsLetterOrDigit(c) || c == ' ');
                    result.CharCount = parsedDoc.ToString().Count(char.IsLetterOrDigit);
                } else {
                    ITextParser parser = GetTextParser(fileName);
                    var parsedDoc = parser.Parse();
                    result.PageCount = 1;
                    result.CharCountWithSpaces = parsedDoc.Count(c => char.IsLetterOrDigit(c) || c == ' ');
                    result.CharCount = parsedDoc.Count(char.IsLetterOrDigit);
                }

                serviceResult.ServiceResultType = ServiceResultType.Success;
                serviceResult.Data = result;
            } catch (Exception exc) {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                Log.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
            }
            return serviceResult;
        }

        public ServiceResult<List<TranslationDocumentPartDto>> GetDocumentPartsNormalized(int translationDocumentId, int partCount) {
            var translationDocument = _ceviriDukkaniModel.TranslationDocuments.Find(translationDocumentId);
            if (translationDocument == null)
                return new ServiceResult<List<TranslationDocumentPartDto>>() { ServiceResultType = ServiceResultType.NotKnown, Message = "Döküman bulunamadı." };

            string content = string.Empty;

            if (IsDocumentExtension(translationDocument.Path)) {
                var parser = GetDocumentParser(translationDocument.Path);
                content = parser.Parse().ToString();
            } else {
                var parser = GetTextParser(translationDocument.Path);
                content = parser.Parse();
            }

            var stringParser = new StringParser(content);
            var parts = stringParser.SplitByCount(partCount);
            // TODO : continue
            throw new NotImplementedException();
        }

        private ITextParser GetTextParser(string filePath) {
            if (filePath.GetExtensionOfFile() == "txt") {
                return new PlainTextParser(new ParserContext(filePath));
            }
            throw new NotSupportedException("Bu doküman tipi desteklenmiyor.");
        }

        private IDocumentParser GetDocumentParser(string filePath) {
            var fileExtension = filePath.GetExtensionOfFile();

            if (fileExtension == "pdf") {
                return new PDFDocumentParser(new ParserContext(filePath));
            } else if (fileExtension == "doc"
                       || fileExtension == "docx") {
                return new Word2007DocumentParser(new ParserContext(filePath));
            }

            throw new NotSupportedException("Bu doküman tipi desteklenmiyor.");
        }

        private bool IsDocumentExtension(string fileName) {
            return fileName.GetExtensionOfFile() != "txt";
        }
    }
}