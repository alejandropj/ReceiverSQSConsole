using Amazon.SQS.Model;
using Amazon.SQS;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ReceiverSQSConsole
{
    public class ServiceSQS
    {
        private IAmazonSQS clientSQS;
        private string UrlQueue;

        public ServiceSQS(IAmazonSQS client)
        {
            this.clientSQS = client;
            this.UrlQueue = "https://sqs.us-east-1.amazonaws.com/637423451273/queue-lunes-apj";
        }

        public async Task<List<Mensaje>>
            ReceiveMessagesAsync()
        {
            //CUANDO REALIZAMOS UN SONDEO, DEBEMOS INDICAR EL TIEMPO
            //EL NUMERO DE MENSAJES A RECUPERAR
            ReceiveMessageRequest request =
                new ReceiveMessageRequest
                {
                    QueueUrl = this.UrlQueue,
                    MaxNumberOfMessages = 5,
                    WaitTimeSeconds = 5
                };
            ReceiveMessageResponse response = await
                this.clientSQS.ReceiveMessageAsync(request);
            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                if (response.Messages.Count != 0)
                {
                    //TENEMOS UNA COLECCION LLAMADA Messages
                    //QUE CONTIENE TODAS LAS PROPIEDADES DE UN MENSAJE
                    List<Message> messages = response.Messages;
                    //CREAMOS NUESTRA COLECCION PARA DEVOLVER LOS 
                    //DATOS EXTRAIDOS
                    List<Mensaje> output = new List<Mensaje>();
                    foreach (Message msj in messages)
                    {
                        //DENTRO DE UNA MENSAJE TENEMOS VARIAS 
                        //CARACTERISTICAS
                        string json = msj.Body;
                        Mensaje data =
                            JsonConvert.DeserializeObject<Mensaje>(json);
                        output.Add(data);
                    }
                    return output;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

        }
    }
}
