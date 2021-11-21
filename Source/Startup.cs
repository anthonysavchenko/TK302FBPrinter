using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TK302FBPrinter.Configuration;
using TK302FBPrinter.Device;
using TK302FBPrinter.Device.DeviceCommands.Beep;
using TK302FBPrinter.Device.DeviceCommands.CancelLastItem;
using TK302FBPrinter.Device.DeviceCommands.Connect;
using TK302FBPrinter.Device.DeviceCommands.Disconnect;
using TK302FBPrinter.Device.DeviceCommands.PrinterStatusGet;
using TK302FBPrinter.Device.DeviceCommands.PrintTextCommand;
using TK302FBPrinter.Device.DeviceCommands.ReceiptAddItem;
using TK302FBPrinter.Device.DeviceCommands.ReceiptCancel;
using TK302FBPrinter.Device.DeviceCommands.ReceiptClose;
using TK302FBPrinter.Device.DeviceCommands.ReceiptOpen;
using TK302FBPrinter.Device.DeviceCommands.ReportXPrint;
using TK302FBPrinter.Device.DeviceCommands.ShiftClose;
using TK302FBPrinter.Device.DeviceCommands.ShiftOpen;
using TK302FBPrinter.Device.DeviceCommands.TextDocClose;
using TK302FBPrinter.Device.DeviceCommands.TextDocOpen;
using TK302FBPrinter.Device.Operations.Beep;
using TK302FBPrinter.Device.Operations.GetStatusOperation;
using TK302FBPrinter.Device.Operations.PrintReceipt;
using TK302FBPrinter.Device.Operations.PrintReceiptReturn;
using TK302FBPrinter.Device.Operations.PrintReportX;
using TK302FBPrinter.Device.Operations.PrintSlip;
using TK302FBPrinter.Device.Operations.ShiftClose;
using TK302FBPrinter.Device.Operations.ShiftOpen;

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
            services.AddControllers().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            var opt = Configuration.GetSection(PrinterOptions.Printer);
            services.Configure<PrinterOptions>(opt);

            var emulationModeEnabled = opt.GetValue<bool>("EmulationMode", false);
            if (emulationModeEnabled)
            {
                services.AddScoped<IConnectCommand, ConnectMockCommand>();
                services.AddScoped<IDisconnectCommand, DisconnectMockCommand>();
                services.AddScoped<IBeepCommand, BeepMockCommand>();
                services.AddScoped<IShiftOpenCommand, ShiftOpenMockCommand>();
                services.AddScoped<IShiftCloseCommand, ShiftCloseMockCommand>();
                services.AddScoped<IReceiptOpenCommand, ReceiptOpenMockCommand>();
                services.AddScoped<IReceiptCloseCommand, ReceiptCloseMockCommand>();
                services.AddScoped<IReceiptAddItemCommand, ReceiptAddItemMockCommand>();
                services.AddScoped<IReceiptCancelCommand, ReceiptCancelMockCommand>();
                services.AddScoped<ICancelLastItemCommand, CancelLastItemMockCommand>();
                services.AddScoped<ITextDocOpenCommand, TextDocOpenMockCommand>();
                services.AddScoped<ITextDocCloseCommand, TextDocCloseMockCommand>();
                services.AddScoped<IPrintTextCommand, PrintTextMockCommand>();
                services.AddScoped<IReportXPrintCommand, ReportXPrintMockCommand>();
                services.AddScoped<IPrinterStatusGetCommand, PrinterStatusGetMockCommand>();
            }
            else
            {
                services.AddScoped<Connector>();

                services.AddScoped<IConnectCommand, ConnectCommand>();
                services.AddScoped<IDisconnectCommand, DisconnectCommand>();
                services.AddScoped<IBeepCommand, BeepCommand>();
                services.AddScoped<IShiftOpenCommand, ShiftOpenCommand>();
                services.AddScoped<IShiftCloseCommand, ShiftCloseCommand>();
                services.AddScoped<IReceiptOpenCommand, ReceiptOpenCommand>();
                services.AddScoped<IReceiptCloseCommand, ReceiptCloseCommand>();
                services.AddScoped<IReceiptAddItemCommand, ReceiptAddItemCommand>();
                services.AddScoped<IReceiptCancelCommand, ReceiptCancelCommand>();
                services.AddScoped<ICancelLastItemCommand, CancelLastItemCommand>();
                services.AddScoped<ITextDocOpenCommand, TextDocOpenCommand>();
                services.AddScoped<ITextDocCloseCommand, TextDocCloseCommand>();
                services.AddScoped<IPrintTextCommand, PrintTextCommand>();
                services.AddScoped<IReportXPrintCommand, ReportXPrintCommand>();
                services.AddScoped<IPrinterStatusGetCommand, PrinterStatusGetCommand>();
            }

            services.AddScoped<IBeepOperation, BeepOperation>();
            services.AddScoped<IShiftOpenOperation, ShiftOpenOperation>();
            services.AddScoped<IShiftCloseOperation, ShiftCloseOperation>();
            services.AddScoped<IPrintReceiptOperation, PrintReceiptOperation>();
            services.AddScoped<IPrintReceiptReturnOperation, PrintReceiptReturnOperation>();
            services.AddScoped<IPrintSlipOperation, PrintSlipOperation>();
            services.AddScoped<IPrintReportXOperation, PrintReportXOperation>();
            services.AddScoped<IGetStatusOperation, GetStatusOperation>();
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
