var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:9000");

// Adicione os serviços ao contêiner.
builder.Services.AddRazorPages();
builder.Services.AddScoped<ProdutividadeService>();

var app = builder.Build();

// Configure o pipeline de requisições HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // O valor HSTS padrão é de 30 dias. Talvez você queira alterar isso para cenários de produção, consulte https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

// ADICIONE ESTA LINHA PARA REDIRECIONAR A ROTA RAIZ PARA /Produtividade
app.MapGet("/", context => {
    context.Response.Redirect("/Produtividade");
    return Task.CompletedTask;
});

app.Run();