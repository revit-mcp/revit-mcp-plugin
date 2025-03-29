using System;
using Autodesk.Revit.UI;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace Host.App
{
    public class RevitApplication : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            RibbonPanel ribbonPanel = application.CreateRibbonPanel("Revit MCP Plugin");
            PushButtonData pushButtonData = new PushButtonData("ID_EXCMD_REVIT_MCP_SWITCH", "MCP Server\r\n Switch",Assembly.GetExecutingAssembly().Location, "Host.App.MCPServiceConnection");
            pushButtonData.ToolTip = "Open / Close mcp server";
            pushButtonData.Image = new BitmapImage(new Uri("/Host;component/App/Ressources-icon-16.png", UriKind.RelativeOrAbsolute));
            pushButtonData.LargeImage = new BitmapImage(new Uri("/Host;component/App/Ressources/icon-32.png", UriKind.RelativeOrAbsolute));
            ribbonPanel.AddItem(pushButtonData);
            return Result.Succeeded;
        }


        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
