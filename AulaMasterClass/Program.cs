// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;

Console.WriteLine("Hello, World!");
ConnectionFactory conectionFactory = new ConnectionFactory()
{
    HostName = "localhost",
    Port = 5672,
    UserName = "guest",
    Password = "guest"
};

using var connection = conectionFactory.CreateConnection();

using var model = connection.CreateModel();

string exchangeName = "notify";
string routingKey = "teste.update";
int max = 10;

Console.WriteLine($"Inciando com {max}  mensagens");



for (int i = 0; i < max; i++)
{
    var prop = model.CreateBasicProperties();
    prop.DeliveryMode = 2;
    prop.ContentType = "application/json";
    prop.Headers = new Dictionary<string, object>()
    {
        { "DATA", DateTime.UtcNow.ToString("U") },
        { "MACHINEMANE", Environment.MachineName.ToString()},
        { "USERMANE", Environment.UserName.ToString()},
    };

    var serealized = System.Text.Json.JsonSerializer.Serialize(new { Value = i });

    var bytesToSend = System.Text.Encoding.UTF8.GetBytes(serealized);

    model.BasicPublish(exchangeName, routingKey, prop, bytesToSend);
}

