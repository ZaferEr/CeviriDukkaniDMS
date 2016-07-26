using RabbitMQ.Client;
using Tangent.CeviriDukkani.Event.DocumentEvents;
using Tangent.CeviriDukkani.Messaging.Consumer;

namespace DMS.Api {
    public class DmsEventProjection {
        private RabbitMqSubscription _consumer;

        public DmsEventProjection(IConnection connection) {
            _consumer = new RabbitMqSubscription(connection, "Cev-Exchange");
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
        }
    }
}