using Chubb.Prueba.BL.Customer;
using Chubb.Prueba.BL.RelationCustomerInsurance;
using Chubb.Prueba.BL.TypeInsurance;
using Chubb.Prueba.Entities.Customer;
using Chubb.Prueba.Entities.RelationCustomerInsurance;
using Chubb.Prueba.Entities.TypeInsurance;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy(
         "MyAllowSpecificOrigins",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                              .AllowAnyHeader()
                              .AllowAnyMethod();
        });
});

// Add services to the container.
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
builder.Services.AddControllers();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IRelationCustomerInsuranceRepository, RelationCustomerInsuranceRepository>();
builder.Services.AddScoped<ITypeInsuranceRepository, TypeInsuranceRepository>();
builder.Services.AddScoped<ICustomerServices, CustomerServices>();
builder.Services.AddScoped<IRelationCustomerInsuranceServices, RelationCustomerInsuranceServices>();
builder.Services.AddScoped<ITypeInsuranceServices, TypeInsuranceServices>();
var app = builder.Build();
app.UseCors("MyAllowSpecificOrigins");

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
