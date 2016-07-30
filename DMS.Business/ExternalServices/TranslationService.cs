using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using Tangent.CeviriDukkani.Domain.Common;
using Tangent.CeviriDukkani.Domain.Dto.Translation;

namespace DMS.Business.ExternalServices {
    public class TranslationService:ITranslationService {
        private readonly HttpClient _httpClient;

        public TranslationService() {
            var serviceEndpoint = ConfigurationManager.AppSettings["TranslationServiceEndpoint"];
            _httpClient = new HttpClient {
                BaseAddress = new Uri(serviceEndpoint)
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        #region Implementation of ITranslationService

        public ServiceResult SaveTranslationOperations(List<TranslationOperationDto> translationOperations) {
            var response = _httpClient.PostAsJsonAsync($"api/translationapi/saveTranslationOperations",translationOperations).Result;
            return response.Content.ReadAsAsync<ServiceResult>().Result;
        }

        #endregion
    }
}