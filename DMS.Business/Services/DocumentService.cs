﻿using System;
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
using DMS.Business.Extensions;
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


        public ServiceResult AddTranslationDocument(TranslationDocumentDto documentDto, int createdBy) {
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

        public ServiceResult<TranslationDocumentDto> GetTranslationDocument(int documentId) {
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

        public ServiceResult EditTranslationDocument(TranslationDocumentDto documentDto, int createdBy) {
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

        public ServiceResult<List<TranslationDocumentDto>> GetTranslationDocuments() {
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


        public ServiceResult AddGeneralDocument(GeneralDocumentDto documentDto, int createdBy) {
            var serviceResult = new ServiceResult();
            try {
                documentDto.CreatedBy = createdBy;
                documentDto.Active = true;
                var document = _customMapperConfiguration.GetMapEntity<GeneralDocument, GeneralDocumentDto>(documentDto);

                _ceviriDukkaniModel.GeneralDocuments.Add(document);
                if (_ceviriDukkaniModel.SaveChanges() <= 0) {
                    throw new BusinessException(ExceptionCodes.UnableToInsert);
                }
                serviceResult.ServiceResultType = ServiceResultType.Success;
                serviceResult.Data = _customMapperConfiguration.GetMapDto<GeneralDocumentDto, GeneralDocument>(document);
            } catch (Exception exc) {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                Log.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
            }
            return serviceResult;
        }

        public ServiceResult<GeneralDocumentDto> GetGeneralDocument(int documentId) {
            var serviceResult = new ServiceResult<GeneralDocumentDto>();
            try {
                var document = _ceviriDukkaniModel.GeneralDocuments.Find(documentId);
                if (document == null) {
                    throw new DbOperationException(ExceptionCodes.NoRelatedData);
                }
                serviceResult.ServiceResultType = ServiceResultType.Success;
                serviceResult.Data = _customMapperConfiguration.GetMapDto<GeneralDocumentDto, GeneralDocument>(document);
            } catch (Exception exc) {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                Log.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
            }
            return serviceResult;
        }

        public ServiceResult EditGeneralDocument(GeneralDocumentDto documentDto, int createdBy) {
            var serviceResult = new ServiceResult();
            try {
                var document = _ceviriDukkaniModel.GeneralDocuments.FirstOrDefault(f => f.Id == documentDto.Id);
                if (document == null) {
                    throw new DbOperationException(ExceptionCodes.NoRelatedData);
                }

                document.Path = documentDto.Path;
                document.Name = documentDto.Name;
                document.Active = documentDto.Active;

                document.UpdatedAt = DateTime.Now;
                document.UpdatedBy = createdBy;

                if (_ceviriDukkaniModel.SaveChanges() <= 0) {
                    throw new BusinessException(ExceptionCodes.UnableToUpdate);
                }
                serviceResult.ServiceResultType = ServiceResultType.Success;
                serviceResult.Data = _customMapperConfiguration.GetMapDto<GeneralDocumentDto, GeneralDocument>(document);
            } catch (Exception exc) {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                Log.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
            }
            return serviceResult;
        }

        public ServiceResult<List<GeneralDocumentDto>> GetGeneralDocuments() {
            var serviceResult = new ServiceResult<List<GeneralDocumentDto>>();
            try {
                var documents = _ceviriDukkaniModel.GeneralDocuments.ToList();

                serviceResult.ServiceResultType = ServiceResultType.Success;
                serviceResult.Data = documents.Select(s => _customMapperConfiguration.GetMapDto<GeneralDocumentDto, GeneralDocument>(s)).ToList();
            } catch (Exception exc) {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                Log.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
            }
            return serviceResult;
        }


        public ServiceResult AddUserDocument(UserDocumentDto documentDto, int createdBy) {
            var serviceResult = new ServiceResult();
            try {
                documentDto.CreatedBy = createdBy;
                documentDto.Active = true;
                var document = _customMapperConfiguration.GetMapEntity<UserDocument, UserDocumentDto>(documentDto);

                _ceviriDukkaniModel.UserDocuments.Add(document);
                if (_ceviriDukkaniModel.SaveChanges() <= 0) {
                    throw new BusinessException(ExceptionCodes.UnableToInsert);
                }
                serviceResult.ServiceResultType = ServiceResultType.Success;
                serviceResult.Data = _customMapperConfiguration.GetMapDto<UserDocumentDto, UserDocument>(document);
            } catch (Exception exc) {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                Log.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
            }
            return serviceResult;
        }

        public ServiceResult<UserDocumentDto> GetUserDocument(int documentId) {
            var serviceResult = new ServiceResult<UserDocumentDto>();
            try {
                var document = _ceviriDukkaniModel.UserDocuments.Find(documentId);
                if (document == null) {
                    throw new DbOperationException(ExceptionCodes.NoRelatedData);
                }
                serviceResult.ServiceResultType = ServiceResultType.Success;
                serviceResult.Data = _customMapperConfiguration.GetMapDto<UserDocumentDto, UserDocument>(document);
            } catch (Exception exc) {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                Log.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
            }
            return serviceResult;
        }

        public ServiceResult EditUserDocument(UserDocumentDto documentDto, int createdBy) {
            var serviceResult = new ServiceResult();
            try {
                var document = _ceviriDukkaniModel.UserDocuments.FirstOrDefault(f => f.Id == documentDto.Id);
                if (document == null) {
                    throw new DbOperationException(ExceptionCodes.NoRelatedData);
                }

                document.Path = documentDto.Path;
                document.Name = documentDto.Name;
                document.Active = documentDto.Active;

                document.UpdatedAt = DateTime.Now;
                document.UpdatedBy = createdBy;

                if (_ceviriDukkaniModel.SaveChanges() <= 0) {
                    throw new BusinessException(ExceptionCodes.UnableToUpdate);
                }
                serviceResult.ServiceResultType = ServiceResultType.Success;
                serviceResult.Data = _customMapperConfiguration.GetMapDto<UserDocumentDto, UserDocument>(document);
            } catch (Exception exc) {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                Log.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
            }
            return serviceResult;
        }

        public ServiceResult<List<UserDocumentDto>> GetUserDocuments() {
            var serviceResult = new ServiceResult<List<UserDocumentDto>>();
            try {
                var documents = _ceviriDukkaniModel.UserDocuments.ToList();

                serviceResult.ServiceResultType = ServiceResultType.Success;
                serviceResult.Data = documents.Select(s => _customMapperConfiguration.GetMapDto<UserDocumentDto, UserDocument>(s)).ToList();
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
                Log.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
            }
            return serviceResult;
        }

        public ServiceResult<List<TranslationDocumentPartDto>> GetDocumentPartsNormalized(int translationDocumentId, int partCount, int createdBy) {
            var serviceResult = new ServiceResult<List<TranslationDocumentPartDto>>();
            try {
                var translationDocument = _ceviriDukkaniModel.TranslationDocuments.Find(translationDocumentId);
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

                _ceviriDukkaniModel.TranslationDocumentParts.AddRange(documentParts);

                if (_ceviriDukkaniModel.SaveChanges() <= 0)
                    throw new BusinessException(ExceptionCodes.UnableToInsert);

                var documentPartsFromDb = _ceviriDukkaniModel.TranslationDocumentParts
                                                .Where(x => x.TranslationDocumentId == translationDocumentId)
                                                .ToList();

                serviceResult.ServiceResultType = ServiceResultType.Success;
                serviceResult.Data = documentPartsFromDb.Select(x => _customMapperConfiguration.GetMapDto<TranslationDocumentPartDto, TranslationDocumentPart>(x)).ToList();
            } catch (Exception exc) {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                Log.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
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