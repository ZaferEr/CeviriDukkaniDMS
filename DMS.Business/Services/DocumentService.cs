using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DMS.Business.Extensions;
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

namespace DMS.Business.Services {
    public class DocumentService : IDocumentService {

        private readonly CeviriDukkaniModel _model;
        private readonly CustomMapperConfiguration _customMapperConfiguration;
        private readonly ILog _logger;

        public DocumentService(CeviriDukkaniModel model, CustomMapperConfiguration customMapperConfiguration,ILog logger) {
            _model = model;
            _customMapperConfiguration = customMapperConfiguration;
            _logger = logger;
        }
        
        public ServiceResult<TranslationDocumentDto> AddTranslationDocument(TranslationDocumentDto documentDto, int createdBy) {
            var serviceResult = new ServiceResult<TranslationDocumentDto>();
            try {
                documentDto.CreatedBy = createdBy;
                documentDto.Active = true;
                var document = _customMapperConfiguration.GetMapEntity<TranslationDocument, TranslationDocumentDto>(documentDto);

                _model.TranslationDocuments.Add(document);
                if (_model.SaveChanges() <= 0) {
                    throw new BusinessException(ExceptionCodes.UnableToInsert);
                }
                serviceResult.ServiceResultType = ServiceResultType.Success;
                serviceResult.Data = _customMapperConfiguration.GetMapDto<TranslationDocumentDto, TranslationDocument>(document);
            } catch (Exception exc) {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                _logger.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
            }
            return serviceResult;
        }

        public ServiceResult<TranslationDocumentDto> GetTranslationDocument(int documentId) {
            var serviceResult = new ServiceResult<TranslationDocumentDto>();
            try {
                var document = _model.TranslationDocuments.Find(documentId);
                if (document == null) {
                    throw new DbOperationException(ExceptionCodes.NoRelatedData);
                }
                serviceResult.ServiceResultType = ServiceResultType.Success;
                serviceResult.Data = _customMapperConfiguration.GetMapDto<TranslationDocumentDto, TranslationDocument>(document);
            } catch (Exception exc) {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                _logger.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
            }
            return serviceResult;
        }

        public ServiceResult<TranslationDocumentDto> EditTranslationDocument(TranslationDocumentDto documentDto, int createdBy) {
            var serviceResult = new ServiceResult<TranslationDocumentDto>();
            try {
                var document = _model.TranslationDocuments.FirstOrDefault(f => f.Id == documentDto.Id);
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

                if (_model.SaveChanges() <= 0) {
                    throw new BusinessException(ExceptionCodes.UnableToUpdate);
                }
                serviceResult.ServiceResultType = ServiceResultType.Success;
                serviceResult.Data = _customMapperConfiguration.GetMapDto<TranslationDocumentDto, TranslationDocument>(document);
            } catch (Exception exc) {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                _logger.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
            }
            return serviceResult;
        }

        public ServiceResult<List<TranslationDocumentDto>> GetTranslationDocuments() {
            var serviceResult = new ServiceResult<List<TranslationDocumentDto>>();
            try {
                var documents = _model.TranslationDocuments.ToList();

                serviceResult.ServiceResultType = ServiceResultType.Success;
                serviceResult.Data = documents.Select(s => _customMapperConfiguration.GetMapDto<TranslationDocumentDto, TranslationDocument>(s)).ToList();
            } catch (Exception exc) {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                _logger.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
            }
            return serviceResult;
        }
        
        public ServiceResult<GeneralDocumentDto> AddGeneralDocument(GeneralDocumentDto documentDto, int createdBy) {
            var serviceResult = new ServiceResult<GeneralDocumentDto>();
            try {
                documentDto.CreatedBy = createdBy;
                documentDto.Active = true;
                var document = _customMapperConfiguration.GetMapEntity<GeneralDocument, GeneralDocumentDto>(documentDto);

                _model.GeneralDocuments.Add(document);
                if (_model.SaveChanges() <= 0) {
                    throw new BusinessException(ExceptionCodes.UnableToInsert);
                }
                serviceResult.ServiceResultType = ServiceResultType.Success;
                serviceResult.Data = _customMapperConfiguration.GetMapDto<GeneralDocumentDto, GeneralDocument>(document);
            } catch (Exception exc) {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                _logger.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
            }
            return serviceResult;
        }

        public ServiceResult<GeneralDocumentDto> GetGeneralDocument(int documentId) {
            var serviceResult = new ServiceResult<GeneralDocumentDto>();
            try {
                var document = _model.GeneralDocuments.Find(documentId);
                if (document == null) {
                    throw new DbOperationException(ExceptionCodes.NoRelatedData);
                }
                serviceResult.ServiceResultType = ServiceResultType.Success;
                serviceResult.Data = _customMapperConfiguration.GetMapDto<GeneralDocumentDto, GeneralDocument>(document);
            } catch (Exception exc) {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                _logger.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
            }
            return serviceResult;
        }

        public ServiceResult<GeneralDocumentDto> EditGeneralDocument(GeneralDocumentDto documentDto, int createdBy) {
            var serviceResult = new ServiceResult<GeneralDocumentDto>();
            try {
                var document = _model.GeneralDocuments.FirstOrDefault(f => f.Id == documentDto.Id);
                if (document == null) {
                    throw new DbOperationException(ExceptionCodes.NoRelatedData);
                }

                document.Path = documentDto.Path;
                document.Name = documentDto.Name;
                document.Active = documentDto.Active;

                document.UpdatedAt = DateTime.Now;
                document.UpdatedBy = createdBy;

                if (_model.SaveChanges() <= 0) {
                    throw new BusinessException(ExceptionCodes.UnableToUpdate);
                }
                serviceResult.ServiceResultType = ServiceResultType.Success;
                serviceResult.Data = _customMapperConfiguration.GetMapDto<GeneralDocumentDto, GeneralDocument>(document);
            } catch (Exception exc) {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                _logger.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
            }
            return serviceResult;
        }

        public ServiceResult<List<GeneralDocumentDto>> GetGeneralDocuments() {
            var serviceResult = new ServiceResult<List<GeneralDocumentDto>>();
            try {
                var documents = _model.GeneralDocuments.ToList();

                serviceResult.ServiceResultType = ServiceResultType.Success;
                serviceResult.Data = documents.Select(s => _customMapperConfiguration.GetMapDto<GeneralDocumentDto, GeneralDocument>(s)).ToList();
            } catch (Exception exc) {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                _logger.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
            }
            return serviceResult;
        }
        
        public ServiceResult<UserDocumentDto> AddUserDocument(UserDocumentDto documentDto, int createdBy) {
            var serviceResult = new ServiceResult<UserDocumentDto>();
            try {
                documentDto.CreatedBy = createdBy;
                documentDto.Active = true;
                var document = _customMapperConfiguration.GetMapEntity<UserDocument, UserDocumentDto>(documentDto);

                _model.UserDocuments.Add(document);
                if (_model.SaveChanges() <= 0) {
                    throw new BusinessException(ExceptionCodes.UnableToInsert);
                }
                serviceResult.ServiceResultType = ServiceResultType.Success;
                serviceResult.Data = _customMapperConfiguration.GetMapDto<UserDocumentDto, UserDocument>(document);
            } catch (Exception exc) {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                _logger.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
            }
            return serviceResult;
        }

        public ServiceResult<UserDocumentDto> GetUserDocument(int documentId) {
            var serviceResult = new ServiceResult<UserDocumentDto>();
            try {
                var document = _model.UserDocuments.Find(documentId);
                if (document == null) {
                    throw new DbOperationException(ExceptionCodes.NoRelatedData);
                }
                serviceResult.ServiceResultType = ServiceResultType.Success;
                serviceResult.Data = _customMapperConfiguration.GetMapDto<UserDocumentDto, UserDocument>(document);
            } catch (Exception exc) {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                _logger.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
            }
            return serviceResult;
        }

        public ServiceResult<UserDocumentDto> EditUserDocument(UserDocumentDto documentDto, int createdBy) {
            var serviceResult = new ServiceResult<UserDocumentDto>();
            try {
                var document = _model.UserDocuments.FirstOrDefault(f => f.Id == documentDto.Id);
                if (document == null) {
                    throw new DbOperationException(ExceptionCodes.NoRelatedData);
                }

                document.Path = documentDto.Path;
                document.Name = documentDto.Name;
                document.Active = documentDto.Active;

                document.UpdatedAt = DateTime.Now;
                document.UpdatedBy = createdBy;

                if (_model.SaveChanges() <= 0) {
                    throw new BusinessException(ExceptionCodes.UnableToUpdate);
                }
                serviceResult.ServiceResultType = ServiceResultType.Success;
                serviceResult.Data = _customMapperConfiguration.GetMapDto<UserDocumentDto, UserDocument>(document);
            } catch (Exception exc) {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                _logger.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
            }
            return serviceResult;
        }

        public ServiceResult<List<UserDocumentDto>> GetUserDocuments() {
            var serviceResult = new ServiceResult<List<UserDocumentDto>>();
            try {
                var documents = _model.UserDocuments.ToList();

                serviceResult.ServiceResultType = ServiceResultType.Success;
                serviceResult.Data = documents.Select(s => _customMapperConfiguration.GetMapDto<UserDocumentDto, UserDocument>(s)).ToList();
            } catch (Exception exc) {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                _logger.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
            }
            return serviceResult;
        }

        public ServiceResult<DocumentUploadResponseDto> AnalyzeDocument(string localFolder, string fileName) {
            var result = new DocumentUploadResponseDto();
            var serviceResult = new ServiceResult<DocumentUploadResponseDto>();
            try {
                result.FilePath = fileName;
                if (fileName.IsDocumentExtension()) {
                    IDocumentParser parser = GetDocumentParser(localFolder);
                    var parsedDoc = parser.Parse();
                    var stringParser = new StringParser(parsedDoc.ToString());
                    result.PageCount = parsedDoc.TotalPageNumber;
                    result.CharCountWithSpaces = stringParser.GenerateCharacterCount();
                    result.CharCount = stringParser.GenerateCharacterCount(withoutWhitespaces: true);
                } else {
                    ITextParser parser = GetTextParser(fileName);
                    var parsedDoc = parser.Parse();
                    var stringParser = new StringParser(parsedDoc);
                    result.PageCount = 1;
                    result.CharCountWithSpaces = stringParser.GenerateCharacterCount();
                    result.CharCount = stringParser.GenerateCharacterCount(withoutWhitespaces: true);
                }

                serviceResult.ServiceResultType = ServiceResultType.Success;
                serviceResult.Data = result;
            } catch (Exception exc) {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                _logger.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
            }
            return serviceResult;
        }

        public ServiceResult<List<TranslationDocumentPartDto>> GetDocumentPartsNormalized(int translationDocumentId, int partCount, int createdBy) {
            var serviceResult = new ServiceResult<List<TranslationDocumentPartDto>>();
            try {
                var translationDocument = _model.TranslationDocuments.Find(translationDocumentId);
                if (translationDocument == null) {
                    throw new BusinessException(ExceptionCodes.NoRelatedData);
                }

                string content = string.Empty;

                if (translationDocument.Path.IsDocumentExtension()) {
                    var parser = GetDocumentParser(translationDocument.Path);
                    content = parser.Parse().ToString();
                } else {
                    var parser = GetTextParser(translationDocument.Path);
                    content = parser.Parse();
                }

                var stringParser = new StringParser(content);
                var parts = stringParser.SplitByCount(partCount);

                var documentParts = parts.Select(x => new TranslationDocumentPart() {
                    TranslationDocumentId = translationDocumentId,
                    Path = translationDocument.Path,
                    Active = true,
                    Content = x,
                    CreatedAt = DateTime.Now,
                    CreatedBy = createdBy
                })
                .ToList();

                _model.TranslationDocumentParts.AddRange(documentParts);

                if (_model.SaveChanges() <= 0)
                    throw new BusinessException(ExceptionCodes.UnableToInsert);

                var documentPartsFromDb = _model.TranslationDocumentParts
                                                .Where(x => x.TranslationDocumentId == translationDocumentId)
                                                .ToList();

                serviceResult.ServiceResultType = ServiceResultType.Success;
                serviceResult.Data = documentPartsFromDb.Select(x => _customMapperConfiguration.GetMapDto<TranslationDocumentPartDto, TranslationDocumentPart>(x)).ToList();
            } catch (Exception exc) {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                _logger.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
            }
            return serviceResult;
        }

        public ServiceResult<TranslationDocumentPartDto> GetTranslationDocumentPartById(int translationDocumentPartId) {
            var serviceResult = new ServiceResult<TranslationDocumentPartDto>();
            try {
                var translationDocumentPart = _model.TranslationDocumentParts.Find(translationDocumentPartId);

                serviceResult.ServiceResultType = ServiceResultType.Success;
                serviceResult.Data = _customMapperConfiguration.GetMapDto<TranslationDocumentPartDto,TranslationDocumentPart>(translationDocumentPart);
            } catch (Exception exc) {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                _logger.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
            }
            return serviceResult;
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
    }
}