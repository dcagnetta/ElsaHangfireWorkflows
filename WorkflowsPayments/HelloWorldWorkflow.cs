using Elsa.Builders;
using System.Net;
using Elsa.Activities.Http;

namespace WorkflowsPayments
{
    /// <summary>
    /// A workflow that is triggered when HTTP requests are made to /hello-world and writes a response.
    /// </summary>
    public class HelloWorldWorkflow : IWorkflow
    {
        public void Build(IWorkflowBuilder builder)
        {
            
            builder
                .HttpEndpoint("/hello-world")
                .WriteHttpResponse(HttpStatusCode.OK, "<h1>Hello World!</h1>", "text/html");
        }
    }
}
