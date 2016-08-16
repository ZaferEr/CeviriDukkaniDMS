using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using DMS.Api.Helpers;
using DMS.Business;
using DMS.Business.Services;
using Tangent.CeviriDukkani.Domain.Common;
using Tangent.CeviriDukkani.Domain.Dto.Document;
using Tangent.CeviriDukkani.Domain.Dto.Response;
using Tangent.CeviriDukkani.WebCore.BaseControllers;

namespace DMS.Api.Controllers {
    [RoutePrefix("api/documentapi")]
    public class DocumentApiController : BaseApiController {
        private readonly IDocumentService _documentService;

        public DocumentApiController(IDocumentService documentService) {
            _documentService = documentService;
        }

        [HttpPost, Route("uploadTranslationDocument")]
        public HttpResponseMessage UploadTranslationDocument(HttpRequestMessage request)
        {
            try
            {
                var requestFromBase = Request;

                var multipartStream = requestFromBase.Content.ReadAsMultipartAsync().Result;
                foreach (var file in multipartStream.Contents)
                {
                    var filePath = SaveFile(file);

                    var uploadResponseDto = _documentService.AnalyzeDocument(filePath);

                    return Request.CreateResponse(HttpStatusCode.OK, uploadResponseDto);
                }

                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpPost, Route("uploadDocument")]
        public HttpResponseMessage UploadDocument(HttpRequestMessage request)
        {
            try
            {
                var requestFromBase = Request;

                var multipartStream = requestFromBase.Content.ReadAsMultipartAsync().Result;
                foreach (var file in multipartStream.Contents)
                {
                    var filePath = SaveFile(file);

                    return Request.CreateResponse(HttpStatusCode.OK, filePath);
                }

                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            } catch (System.Exception e) {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        private string SaveFile(HttpContent file)
        {
            var buffer = file.ReadAsByteArrayAsync().Result;
            var uploadPath = ConfigurationManager.AppSettings["UploadDocumentPath"];

            var fileExtension = file.Headers.ContentDisposition.FileName.GetExtensionOfFile().Replace("\"", string.Empty);
            
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var filePath = $"{uploadPath}/{Guid.NewGuid()}.{fileExtension}";
            File.WriteAllBytes(filePath, buffer);

            return filePath;
        }

        [HttpPost, Route("addTranslationDocument")]
        public HttpResponseMessage AddTranslationDocument(TranslationDocumentDto documentDto) {
            var serviceResult = _documentService.AddTranslationDocument(documentDto, 1);

            if (serviceResult.ServiceResultType != ServiceResultType.Success)
                return Error();

            return OK(serviceResult);
        }

        [HttpPost, Route("editTranslationDocument")]
        public HttpResponseMessage EditTranslationDocument(TranslationDocumentDto documentDto) {
            var serviceResult = _documentService.EditTranslationDocument(documentDto, SessionUser.User.Id);

            if (serviceResult.ServiceResultType != ServiceResultType.Success)
                return Error();

            return OK(serviceResult);
        }

        [HttpGet, Route("getTranslationDocuments")]
        public HttpResponseMessage GetTranslationDocuments() {
            var serviceResult = _documentService.GetTranslationDocuments();

            if (serviceResult.ServiceResultType != ServiceResultType.Success)
                return Error();

            return OK(serviceResult);
        }

        [HttpGet, Route("getTranslationDocument/{id}")]
        public HttpResponseMessage GetTranslationDocument(int id) {
            var serviceResult = _documentService.GetTranslationDocument(id);

            if (serviceResult.ServiceResultType != ServiceResultType.Success)
                return Error();

            return OK(serviceResult);
        }

        [HttpPost, Route("addGeneralDocument")]
        public HttpResponseMessage AddGeneralDocument(GeneralDocumentDto documentDto) {
            var serviceResult = _documentService.AddGeneralDocument(documentDto, SessionUser.User.Id);

            if (serviceResult.ServiceResultType != ServiceResultType.Success)
                return Error();

            return OK(serviceResult);
        }

        [HttpPost, Route("editGeneralDocument")]
        public HttpResponseMessage EditGeneralDocument(GeneralDocumentDto documentDto) {
            var serviceResult = _documentService.EditGeneralDocument(documentDto, SessionUser.User.Id);

            if (serviceResult.ServiceResultType != ServiceResultType.Success)
                return Error();

            return OK(serviceResult);
        }

        [HttpGet, Route("getGeneralDocuments")]
        public HttpResponseMessage GetGeneralDocuments() {
            var serviceResult = _documentService.GetGeneralDocuments();

            if (serviceResult.ServiceResultType != ServiceResultType.Success)
                return Error();

            return OK(serviceResult);
        }

        [HttpGet, Route("getGeneralDocument/{id}")]
        public HttpResponseMessage GetGeneralDocument(int id) {
            var serviceResult = _documentService.GetGeneralDocument(id);

            if (serviceResult.ServiceResultType != ServiceResultType.Success)
                return Error();

            return OK(serviceResult);
        }

        [HttpPost, Route("addUserDocument")]
        public HttpResponseMessage AddUserDocument(UserDocumentDto documentDto) {
            var serviceResult = _documentService.AddUserDocument(documentDto, SessionUser.User.Id);

            if (serviceResult.ServiceResultType != ServiceResultType.Success)
                return Error();

            return OK(serviceResult);
        }

        [HttpPost, Route("editUserDocument")]
        public HttpResponseMessage EditUserDocument(UserDocumentDto documentDto) {
            var serviceResult = _documentService.EditUserDocument(documentDto, SessionUser.User.Id);

            if (serviceResult.ServiceResultType != ServiceResultType.Success)
                return Error();

            return OK(serviceResult);
        }

        [HttpGet, Route("getUserDocuments")]
        public HttpResponseMessage GetUserDocuments() {
            var serviceResult = _documentService.GetUserDocuments();

            if (serviceResult.ServiceResultType != ServiceResultType.Success)
                return Error();

            return OK(serviceResult);
        }

        [HttpGet, Route("getUserDocument/{id}")]
        public HttpResponseMessage GetUserDocument(int id) {
            var serviceResult = _documentService.GetUserDocument(id);

            if (serviceResult.ServiceResultType != ServiceResultType.Success)
                return Error();

            return OK(serviceResult);
        }

        [HttpGet, Route("getDocumentPartsNormalized")]
        public HttpResponseMessage GetDocumentPartsNormalized(int translationDocumentId, int partCount) {
            var serviceResult = _documentService.GetDocumentPartsNormalized(translationDocumentId, partCount, SessionUser.User.Id);

            if (serviceResult.ServiceResultType != ServiceResultType.Success)
                return Error();

            return OK(serviceResult);
        }

        [HttpGet, Route("getTranslationDocumentPartById")]
        public HttpResponseMessage GetTranslationDocumentPartById([FromUri]int translationDocumentPartId) {
            var serviceResult = _documentService.GetTranslationDocumentPartById(translationDocumentPartId);

            if (serviceResult.ServiceResultType != ServiceResultType.Success)
                return Error();

            return OK(serviceResult);
        }

        [HttpGet, Route("getDocumentAudits")]
        public HttpResponseMessage GetDocumentAudits(int documentId) {
            var serviceResult = _documentService.GetDocumentAudits(documentId);

            if (serviceResult.ServiceResultType != ServiceResultType.Success)
                return Error();

            return OK(serviceResult);
        }

        [HttpGet, Route("analyzeDocument")]
        public HttpResponseMessage AnalyzeDocument([FromUri]string fileFullPath)
        {
            var serviceResult = _documentService.AnalyzeDocument(fileFullPath);

            if (serviceResult.ServiceResultType != ServiceResultType.Success)
                return Error();

            return OK(serviceResult);
        }
    }

}