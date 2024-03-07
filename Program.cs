//HTTPListener primero se instancia esta clase
using System.Net;
using System.Text;
HttpListener server = new();
var ip = Dns.GetHostAddresses(Dns.GetHostName())
    .Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
    .Select(x => x.ToString())
    .FirstOrDefault();
server.Prefixes.Add("http://localhost:10000/");//esta es la direccion por la cual va estar esperando y escuchando mi servidor
server.Prefixes.Add($"http://{ip}:10000/");
server.Start();//aqui lo inicio para que empiece a escuchar

//otras formas de verificar por donde me contactan = //Context.request.httpmethod == HttpMethod.Get;
while (true)
{
    HttpListenerContext context = server.GetContext(); //aqui empieza a escuchar 
    string respuesta = "";
    //preguntamos si existe la variable nombre

    var nombre= context.Request.QueryString[ "nombre"];
    if(nombre!=null)//manda una variable llamdada nombre
    {
        Console.WriteLine(nombre + "Ha echo una peticion");
         respuesta = $"<html><body><h1>saludos {nombre}</h1></body></html>";
    }
    else
    {
    //context.User = aqui me da que informacion tiene el usuario
     respuesta = "<html><body><h1>Respuesta desde el servidor</h1></body></html>";
    var ns = context.Response.OutputStream;//a esto le escribo lo que voy a dar de salida, //primero se indica la salida y despues el buffer
    byte[] buffer = Encoding.UTF8.GetBytes(respuesta);
    ns.Write(buffer, 0, buffer.Length);
    context.Response.StatusCode = 200; // OK   //esto tiene que llevarlo para ver que paso con la peticion
                                       //  context.Response.ContentLength64 = buffer.Length;//le tengo que indicar con el peso de contenido
    context.Response.Close();

    }



   
}