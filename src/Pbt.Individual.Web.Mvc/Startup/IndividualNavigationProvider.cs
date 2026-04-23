using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using Pbt.Individual.Authorization;

namespace Pbt.Individual.Web.Startup;

/// <summary>
/// This class defines menus for the application.
/// </summary>
public class IndividualNavigationProvider : NavigationProvider
{
    public override void SetNavigation(INavigationProviderContext context)
    {
        context.Manager.MainMenu
            .AddItem(
                new MenuItemDefinition(
                    PageNames.Home,
                    L("Dashboard"),
                    url: "",
                    icon: "fas fa-home",
                    requiresAuthentication: true
                )
            )
            .AddItem(
                new MenuItemDefinition(
                    "Support",
                    L("Support"),
                    url: "Support",
                    icon: "fas fa-life-ring",
                    requiresAuthentication: true
                )
            )
            .AddItem( // Menu items below is just for demonstration!
                new MenuItemDefinition(
                    "Orders",
                    L("MyOrders"),
                    icon: "fas fa-truck" // Quản lý đơn hàng
                ).AddItem(
                        new MenuItemDefinition(
                            "OrdersCreate",
                            L("OrdersCreate"),
                            url: "/Orders/Create",
                            icon: "fas fa-plus-circle" 
                        )
                    ).AddItem(
                        new MenuItemDefinition(
                            "OrdersList",
                            L("OrdersList"),
                            url: "/Orders",
                            icon: "fas fa-list" 
                        )
                )
            ).AddItem(
                    new MenuItemDefinition(
                        "DeliveryRequests",
                        L("DeliveryRequests"),
                        icon: "fas fa-truck-loading"
                    ).AddItem(
                        new MenuItemDefinition(
                            "CreateDeliveryRequest",
                            L("CreateDeliveryRequest"),
                            url: "/DeliveryRequests/Create",
                            icon: "fas fa-plus-circle" 
                        )
                    ).AddItem(
                        new MenuItemDefinition(
                            "DeliveryRequestList",
                            L("DeliveryRequestList"),
                            url: "/DeliveryRequests",
                            icon: "fas fa-list" 
                        )
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        "Payments",
                        L("Payments"),
                        icon: "fas fa-file-invoice-dollar"
                    ) .AddItem(
                        new MenuItemDefinition(
                            "PaymentsList",
                            L("PaymentsList"),
                            url: "/Payments",
                            icon: "fas fa-list"
                             
                        )
                    )
                );
    }

    private static ILocalizableString L(string name)
    {
        return new LocalizableString(name, IndividualConsts.LocalizationSourceName);
    }
}