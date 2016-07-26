using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using DMS.Business;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Tangent.CeviriDukkani.Domain.Common;
using Tangent.CeviriDukkani.Domain.Dto.Document;

namespace DMS.Api.Controllers {
    [RoutePrefix("api/v1/documentapi")]
    public class DocumentApiController : ApiController {

        public JsonMediaTypeFormatter Formatter = new JsonMediaTypeFormatter {
            SerializerSettings = new JsonSerializerSettings {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }
        };

        private readonly IDocumentService _documentService;

        public DocumentApiController(IDocumentService documentService) {
            _documentService = documentService;
        }

        [HttpPost, Route("addDocument")]
        public HttpResponseMessage AddDocument(TranslationDocumentDto documentDto) {
            var response = new HttpResponseMessage();
            var serviceResult = _documentService.AddDocument(documentDto, 1);

            if (serviceResult.ServiceResultType != ServiceResultType.Success) {
                response.StatusCode = HttpStatusCode.InternalServerError;
                return response;
            }

            response.StatusCode = HttpStatusCode.OK;
            response.Content = new ObjectContent(serviceResult.Data.GetType(), serviceResult.Data, new JsonMediaTypeFormatter());
            return response;
        }

        [HttpPost, Route("uploadDocument")]
        public HttpResponseMessage UploadDocument(HttpRequestMessage request) {
            try {
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count != 1)
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

                var postedFile = httpRequest.Files[0];
                var fileExtension = postedFile.FileName.GetExtensionOfFile();
                var newGuid = Guid.NewGuid();
                var filePath = "~/Areas/Admin/Uploads/" + newGuid + "." + fileExtension;
                var localPath = HttpContext.Current.Server.MapPath(filePath);
                postedFile.SaveAs(localPath);
                // NOTE: To store in memory use postedFile.InputStream

                //ServiceResult<DocumentUploadResponseDto> uploadResponseDto = _documentService.AnalyzeDocument(localPath, filePath);

                // Send OK Response along with saved file names to the client.
                return Request.CreateResponse(HttpStatusCode.OK, filePath);
            } catch (System.Exception e) {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpPost, Route("editDocument")]
        public HttpResponseMessage EditDocument(TranslationDocumentDto documentDto) {
            var response = new HttpResponseMessage();
            var serviceResult = _documentService.EditDocument(documentDto, 1);

            if (serviceResult.ServiceResultType != ServiceResultType.Success) {
                response.StatusCode = HttpStatusCode.InternalServerError;
                return response;
            }

            response.StatusCode = HttpStatusCode.OK;
            response.Content = new ObjectContent(serviceResult.Data.GetType(), serviceResult.Data, new JsonMediaTypeFormatter());
            return response;
        }

        [HttpGet, Route("getDocuments")]
        public HttpResponseMessage GetDocuments() {
            var response = new HttpResponseMessage();
            var serviceResult = _documentService.GetDocuments();

            if (serviceResult.ServiceResultType != ServiceResultType.Success) {
                response.StatusCode = HttpStatusCode.InternalServerError;
                return response;
            }

            response.StatusCode = HttpStatusCode.OK;
            response.Content = new ObjectContent(serviceResult.Data.GetType(), serviceResult.Data, Formatter);
            return response;
        }
    }

}