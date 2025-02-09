# Extension

## Tổng quan

Mục đích tái sử dụng cho các Service cần cấu hình ở Program

## Cấu trúc

> Package bắt buộc phải có: <code>Microsoft.AspNetCore</code>

- _**Swagger**_: Dùng để sử dụng Swagger để test, kiểm tra API. Vì đã có Extension này nên khi tạo mới 1 template
  ASP.NET
  Core, ta có thể bỏ tick cho phần OpenAPI.
    - Thư viện cần có:
        - <code>Swashbuckle.AspNetCore.Swagger</code>
        - <code>Swashbuckle.AspNetCore.SwaggerGen</code>
        - <code>Swashbuckle.AspNetCore.SwaggerUI</code>
- _**Entity Framework Core**_: Dùng để đăng kí DbContext, với mô hình microservice chia ra nhiều service nhỏ khác nhau,
  do đó
  Extension này giúp có thể nhanh chóng đăng kí vào Program thay vì viết lại logic kết nối.
    - Thư viện cần có:
        - <code>Microsoft.Extensions.DependencyInjection</code>
        - <code>Microsoft.Extensions.Hosting</code>

*Khuyến nghị sử dụng 1 Project Library khác để quản lý DbContex kèm theo Entity của cơ sở dữ liệu*

- _**App Config**_: Dùng để đăng kí các setting, ví dụ như cấu hình của JWT, MailKit, Clould,... được ánh xạ qua các
  class.
    - Thư viện cần có:
        - <code>Microsoft.AspNetCore</code>
- _**IDentity**_: Dùng để đăng ký các cấu hình, DI cho Identity, vì ASP.NET Core đã có Identity phục vụ cho
  Authenticate, việc thêm vào dự án sẽ giảm bớt thời gian cho việc xử lý phân quyền, kèm theo là cấu hình cho JWT Token
    - Thư viện cần có:
        - <code>Microsoft.AspNetCore.Authentication.JwtBearer</code>

## Một số lưu ý:

Như đã nói phía trên, thư viện bắt buộc phải có là <code>Microsoft.AspNetCore</code> để triển khai.
Có 2 cách triển khai với 2 kiểu:

### Sử dụng <code>WebApplication</code>

Ta có 1 đoạn mã ví dụ:

```csharp
public static WebApplication ConfigureCORS(this WebApplication app, IConfiguration config)
{
    app.UseCors(options =>
    options.WithOrigins("http://localhost:4200")
    .AllowAnyMethod()
    .AllowAnyHeader());
    return app;
}
```

Lỗi xảy ra:

```log
The type or namespace name 'WebApplication' could not be found (are you missing a using directive or an assembly reference?)
```

Trong <code>Class Library</code>, bạn không thể trực tiếp sử dụng <code>WebApplication</code> vì Class Library không
phải là một ứng dụng <code>ASP.NETCore.WebApplication</code> thuộc về <code>Microsoft.AspNetCore.Builder</code>, nhưng
nó chỉ khả dụng trong một <code>Web Application</code> chứ không phải một Class Library.

#### 🔍 Cách khắc phục lỗi WebApplication không tìm thấy trong Class Library

Thêm reference đến <code>Microsoft.AspNetCore.App</code>

- Class Library của cần tham chiếu đến ASP.NET Core bằng cách thêm <code>Microsoft.AspNetCore.App</code> vào <code>
  .csproj</code>.

🔹 Chỉnh sửa file .csproj của Library:

```xml
  <ItemGroup>
  <!--Thêm Microsoft.AspNetCore.App vào đây-->
    <FrameworkReference Include="Microsoft.AspNetCore.App" />
  </ItemGroup>
```
➡ Lý do: Microsoft.AspNetCore.App chứa tất cả các thành phần ASP.NET Core, bao gồm <code>WebApplication</code>.
### Sử dụng <code>IApplicationBuilder</code>
Vì WebApplication kế thừa từ <code>IApplicationBuilder</code>, nên khi gọi extension từ một Web API, nó vẫn hoạt động.

Do đó, ta có thể viết như sau:
```
public static IApplicationBuilder ConfigureCORS(this IApplicationBuilder app, IConfiguration config)
{
    app.UseCors(options =>
    options.WithOrigins("http://localhost:4200")
    .AllowAnyMethod()
    .AllowAnyHeader());

    return app;
}
```
Tuy nhiên có 1 vài trường hợp, <code>IApplicationBuilder</code> sẽ viết logic khác với <code>WebApplication</code>.
Ví dụ:

<code>WebApplication</code> khi kiểm tra môi trường
```csharp
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```
<code>IApplicationBuilder</code> khi kiểm tra môi trường
```csharp
var ev = app.ApplicationServices.GetRequiredService<IHostEnvironment>();
if (ev.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```
