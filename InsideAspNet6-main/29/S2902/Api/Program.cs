var builder = WebApplication.CreateBuilder();
builder.Services.AddCors();
var app = builder.Build();
app.UseCors(cors => cors.WithOrigins(
    "http://www.foo.com:3721",
    "http://www.bar.com:3721"));
app.MapGet("/contacts", GetContacts);
app.Run(url:"http://0.0.0.0:8080");

static IResult GetContacts()
{
    var contacts = new Contact[]
    {
        new Contact("张三", "123", "zhangsan@gmail.com"),
        new Contact("李四","456", "lisi@gmail.com"),
        new Contact("王五", "789", "wangwu@gmail.com")
    };
    return Results.Json(contacts);
}

public readonly record struct Contact(string Name, string PhoneNo, string EmailAddress);