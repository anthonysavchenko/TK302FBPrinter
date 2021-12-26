using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TK302FBPrinter.Configuration;
using TK302FBPrinter.Device;
using TK302FBPrinter.Device.Commands.Beep;
using TK302FBPrinter.Device.Commands.Connect;
using TK302FBPrinter.Device.Commands.Disconnect;
using TK302FBPrinter.Device.Commands.PrinterStatusGet;
using TK302FBPrinter.Device.Commands.TextDocTextAdd;
using TK302FBPrinter.Device.Commands.ReceiptItemAdd;
using TK302FBPrinter.Device.Commands.ReceiptCancel;
using TK302FBPrinter.Device.Commands.ReceiptClose;
using TK302FBPrinter.Device.Commands.ReceiptOpen;
using TK302FBPrinter.Device.Commands.ReportXPrint;
using TK302FBPrinter.Device.Commands.ShiftClose;
using TK302FBPrinter.Device.Commands.ShiftOpen;
using TK302FBPrinter.Device.Commands.TextDocClose;
using TK302FBPrinter.Device.Commands.TextDocOpen;
using TK302FBPrinter.Business.Operations.Beep;
using TK302FBPrinter.Business.Operations.GetStatusOperation;
using TK302FBPrinter.Business.Operations.PrintReceipt;
using TK302FBPrinter.Business.Operations.PrintReportX;
using TK302FBPrinter.Business.Operations.PrintSlip;
using TK302FBPrinter.Business.Operations.ShiftClose;
using TK302FBPrinter.Business.Operations.ShiftOpen;
using TK302FBPrinter.Device.Commands.GraphicDocOpen;
using TK302FBPrinter.Device.Commands.GraphicDocClose;
using TK302FBPrinter.Device.Commands.GraphicDocTextAdd;
using TK302FBPrinter.Business.Operations.PrintTicket;
using TK302FBPrinter.Device.Commands.GraphicDocLineAdd;
using TK302FBPrinter.Device.Commands.GraphicDocQrCodeAdd;
using TK302FBPrinter.Device.Commands.GraphicDocBitmapAdd;
using TK302FBPrinter.Business.Operations.PrintComplexDoc;
using TK302FBPrinter.Device.Commands.Cut;
using TK302FBPrinter.Device.Commands.ReceiptItemAgentFlagSet;
using TK302FBPrinter.Device.Commands.ReceiptItemSupplierNameSet;
using TK302FBPrinter.Device.Commands.ReceiptItemSupplierINNSet;
using TK302FBPrinter.Device.Commands.ReceiptItemSupplierPhoneSet;

namespace TK302FBPrinter
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options => options.AllowEmptyInputInBodyModelBinding = true).AddNewtonsoftJson();

            var deviceConfig = Configuration.GetSection(DeviceConfig.SectionName);

            services.Configure<DeviceConfig>(deviceConfig);
            services.Configure<SlipConfig>(Configuration.GetSection(SlipConfig.SectionName));
            services.Configure<TicketConfig>(Configuration.GetSection(TicketConfig.SectionName));

            if (deviceConfig.GetValue<bool>("EmulationMode", false))
            {
                services.AddScoped<IConnectCommand, ConnectMockCommand>();
                services.AddScoped<IDisconnectCommand, DisconnectMockCommand>();
                services.AddScoped<IBeepCommand, BeepMockCommand>();
                services.AddScoped<IShiftOpenCommand, ShiftOpenMockCommand>();
                services.AddScoped<IShiftCloseCommand, ShiftCloseMockCommand>();
                services.AddScoped<IReceiptOpenCommand, ReceiptOpenMockCommand>();
                services.AddScoped<IReceiptCloseCommand, ReceiptCloseMockCommand>();
                services.AddScoped<IReceiptItemAddCommand, ReceiptItemAddMockCommand>();
                services.AddScoped<IReceiptItemAgentFlagSetCommand, ReceiptItemAgentFlagSetMockCommand>();
                services.AddScoped<IReceiptItemSupplierNameSetCommand, ReceiptItemSupplierNameSetMockCommand>();
                services.AddScoped<IReceiptItemSupplierINNSetCommand, ReceiptItemSupplierINNSetMockCommand>();
                services.AddScoped<IReceiptItemSupplierPhoneSetCommand, ReceiptItemSupplierPhoneSetMockCommand>();
                services.AddScoped<IReceiptCancelCommand, ReceiptCancelMockCommand>();
                services.AddScoped<ITextDocOpenCommand, TextDocOpenMockCommand>();
                services.AddScoped<ITextDocCloseCommand, TextDocCloseMockCommand>();
                services.AddScoped<ITextDocTextAddCommand, TextDocTextAddMockCommand>();
                services.AddScoped<IReportXPrintCommand, ReportXPrintMockCommand>();
                services.AddScoped<IPrinterStatusGetCommand, PrinterStatusGetMockCommand>();
                services.AddScoped<IGraphicDocOpenCommand, GraphicDocOpenMockCommand>();
                services.AddScoped<IGraphicDocCloseCommand, GraphicDocCloseMockCommand>();
                services.AddScoped<IGraphicDocTextAddCommand, GraphicDocTextAddMockCommand>();
                services.AddScoped<IGraphicDocLineAddCommand, GraphicDocLineAddMockCommand>();
                services.AddScoped<IGraphicDocQrCodeAddCommand, GraphicDocQrCodeAddMockCommand>();
                services.AddScoped<IGraphicDocBitmapAddCommand, GraphicDocBitmapAddMockCommand>();
                services.AddScoped<ICutCommand, CutMockCommand>();
            }
            else
            {
                services.AddScoped<DeviceConnector>();
                services.AddScoped<IConnectCommand, ConnectCommand>();
                services.AddScoped<IDisconnectCommand, DisconnectCommand>();
                services.AddScoped<IBeepCommand, BeepCommand>();
                services.AddScoped<IShiftOpenCommand, ShiftOpenCommand>();
                services.AddScoped<IShiftCloseCommand, ShiftCloseCommand>();
                services.AddScoped<IReceiptOpenCommand, ReceiptOpenCommand>();
                services.AddScoped<IReceiptCloseCommand, ReceiptCloseCommand>();
                services.AddScoped<IReceiptItemAddCommand, ReceiptItemAddCommand>();
                services.AddScoped<IReceiptItemAgentFlagSetCommand, ReceiptItemAgentFlagSetCommand>();
                services.AddScoped<IReceiptItemSupplierNameSetCommand, ReceiptItemSupplierNameSetCommand>();
                services.AddScoped<IReceiptItemSupplierINNSetCommand, ReceiptItemSupplierINNSetCommand>();
                services.AddScoped<IReceiptItemSupplierPhoneSetCommand, ReceiptItemSupplierPhoneSetCommand>();
                services.AddScoped<IReceiptCancelCommand, ReceiptCancelCommand>();
                services.AddScoped<ITextDocOpenCommand, TextDocOpenCommand>();
                services.AddScoped<ITextDocCloseCommand, TextDocCloseCommand>();
                services.AddScoped<ITextDocTextAddCommand, TextDocTextAddCommand>();
                services.AddScoped<IReportXPrintCommand, ReportXPrintCommand>();
                services.AddScoped<IPrinterStatusGetCommand, PrinterStatusGetCommand>();
                services.AddScoped<IGraphicDocOpenCommand, GraphicDocOpenCommand>();
                services.AddScoped<IGraphicDocCloseCommand, GraphicDocCloseCommand>();
                services.AddScoped<IGraphicDocTextAddCommand, GraphicDocTextAddCommand>();
                services.AddScoped<IGraphicDocLineAddCommand, GraphicDocLineAddCommand>();
                services.AddScoped<IGraphicDocQrCodeAddCommand, GraphicDocQrCodeAddCommand>();
                services.AddScoped<IGraphicDocBitmapAddCommand, GraphicDocBitmapAddCommand>();
                services.AddScoped<ICutCommand, CutCommand>();
            }

            services.AddScoped<IBeepOperation, BeepOperation>();
            services.AddScoped<IShiftOpenOperation, ShiftOpenOperation>();
            services.AddScoped<IShiftCloseOperation, ShiftCloseOperation>();
            services.AddScoped<IPrintReceiptOperation, PrintReceiptOperation>();
            services.AddScoped<IPrintSlipOperation, PrintSlipOperation>();
            services.AddScoped<IPrintReportXOperation, PrintReportXOperation>();
            services.AddScoped<IGetStatusOperation, GetStatusOperation>();
            services.AddScoped<IPrintTicketOperation, PrintTicketOperation>();
            services.AddScoped<IPrintComplexDocOperation, PrintComplexDocOperation>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
