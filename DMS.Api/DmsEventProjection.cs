﻿using System;
using System.Collections.Generic;
using System.Linq;
using DMS.Business.ExternalServices;
using DMS.Business.Services;
using log4net;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Tangent.CeviriDukkani.Domain.Common;
using Tangent.CeviriDukkani.Domain.Dto.Enums;
using Tangent.CeviriDukkani.Domain.Dto.Translation;
using Tangent.CeviriDukkani.Event.DocumentEvents;
using Tangent.CeviriDukkani.Event.OrderEvents;
using Tangent.CeviriDukkani.Messaging;
using Tangent.CeviriDukkani.Messaging.Consumer;
using Tangent.CeviriDukkani.Messaging.Producer;

namespace DMS.Api {
    public class DmsEventProjection {
        private readonly IDocumentService _documentService;
        private readonly IDispatchCommits _dispatcher;
        private readonly ITranslationService _translationService;
        private readonly RabbitMqSubscription _consumer;
        private readonly ILog _logger;

        public DmsEventProjection(IConnection connection, IDocumentService documentService, IDispatchCommits dispatcher, ILog logger, ITranslationService translationService) {
            _documentService = documentService;
            _dispatcher = dispatcher;
            _translationService = translationService;
            _logger = logger;
            _consumer = new RabbitMqSubscription(connection, "Cev-Exchange", logger);
            _consumer
                .WithAppName("dms-projection")
                .WithEvent<CreateDocumentPartEvent>(Handle);
        }

        public void Start() {
            _consumer.Subscribe();
        }

        public void Stop() {
            _consumer.StopSubscriptionTasks();
        }

        public void Handle(CreateDocumentPartEvent documentPartEvent) {
            var serviceResult = _documentService.GetDocumentPartsNormalized(documentPartEvent.TranslationDocumentId,
                documentPartEvent.PartCount, documentPartEvent.CreatedBy);
            if (serviceResult.ServiceResultType != ServiceResultType.Success) {
                Console.WriteLine("Error occure while normalizing document parts");
            } else {

                var analyzedDocument = serviceResult.Data;

                var translationOperations = analyzedDocument.Select(a => new TranslationOperationDto {
                    TranslationDocumentPartId = a.Id,
                    TranslationOperationStatusId = (int)TranslationOperationStatusEnum.Bid,
                    TranslationProgressStatusId = (int)TranslationProgressStatusEnum.Open
                }).ToList();

                var translationOperationSaveResult = _translationService.SaveTranslationOperations(translationOperations);
                if (translationOperationSaveResult.ServiceResultType != ServiceResultType.Success) {
                    Console.WriteLine("Error Occured");
                    _logger.Error($"Unable to save translation operations");
                } else {
                    var operations = translationOperationSaveResult.Data;

                    var createOrderDetails = new CreateOrderDetailEvent {
                        Id = Guid.NewGuid(),
                        CreatedBy = documentPartEvent.CreatedBy,
                        OrderId = documentPartEvent.OrderId,
                        TranslationOperations = operations
                    };

                    _dispatcher.Dispatch(new List<EventMessage> { createOrderDetails.ToEventMessage() });
                }


            }


        }
    }
}