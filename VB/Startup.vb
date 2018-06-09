Imports DevExpress.AspNetCore
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Threading.Tasks
Imports Microsoft.AspNetCore.Builder
Imports Microsoft.AspNetCore.Hosting
Imports Microsoft.AspNetCore.Http
Imports Microsoft.Extensions.DependencyInjection

Namespace TagBoxWithCustomTags
    Public Class Startup
        ' This method gets called by the runtime. Use this method to add services to the container.
        ' For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        Public Sub ConfigureServices(ByVal services As IServiceCollection)
            services.AddDevExpressControls()
            services.AddMvc()
            services.AddSession()
        End Sub

        ' This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        Public Sub Configure(ByVal app As IApplicationBuilder, ByVal env As IHostingEnvironment)
            app.UseDevExpressControls()
            app.UseSession()
            app.UseMvcWithDefaultRoute()
            app.UseStaticFiles()
            If env.IsDevelopment() Then
                app.UseDeveloperExceptionPage()
            End If
        End Sub
    End Class
End Namespace
