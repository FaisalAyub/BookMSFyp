﻿using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;

namespace ERP.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public AppAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public AppAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

            var orders = pages.CreateChildPermission(AppPermissions.Pages_Orders, L("Orders"));
            orders.CreateChildPermission(AppPermissions.Pages_Orders_Create, L("CreateNewOrder"));
            orders.CreateChildPermission(AppPermissions.Pages_Orders_Edit, L("EditOrder"));
            orders.CreateChildPermission(AppPermissions.Pages_Orders_Delete, L("DeleteOrder"));

            var orderItems = pages.CreateChildPermission(AppPermissions.Pages_OrderItems, L("OrderItems"), multiTenancySides: MultiTenancySides.Host);
            orderItems.CreateChildPermission(AppPermissions.Pages_OrderItems_Create, L("CreateNewOrderItem"), multiTenancySides: MultiTenancySides.Host);
            orderItems.CreateChildPermission(AppPermissions.Pages_OrderItems_Edit, L("EditOrderItem"), multiTenancySides: MultiTenancySides.Host);
            orderItems.CreateChildPermission(AppPermissions.Pages_OrderItems_Delete, L("DeleteOrderItem"), multiTenancySides: MultiTenancySides.Host);

            var books = pages.CreateChildPermission(AppPermissions.Pages_Books, L("Books"), multiTenancySides: MultiTenancySides.Host);
            books.CreateChildPermission(AppPermissions.Pages_Books_Create, L("CreateNewBook"), multiTenancySides: MultiTenancySides.Host);
            books.CreateChildPermission(AppPermissions.Pages_Books_Edit, L("EditBook"), multiTenancySides: MultiTenancySides.Host);
            books.CreateChildPermission(AppPermissions.Pages_Books_Delete, L("DeleteBook"), multiTenancySides: MultiTenancySides.Host);

            //var glacgrp = pages.CreateChildPermission(AppPermissions.Pages_GLACGRP, L("GLACGRP"));
            //glacgrp.CreateChildPermission(AppPermissions.Pages_GLACGRP_Create, L("CreateNewGLACGRP"));
            //glacgrp.CreateChildPermission(AppPermissions.Pages_GLACGRP_Edit, L("EditGLACGRP"));
            //glacgrp.CreateChildPermission(AppPermissions.Pages_GLACGRP_Delete, L("DeleteGLACGRP"));

            //var glCstCent = pages.CreateChildPermission(AppPermissions.Pages_GLCstCent, L("GLCstCent"));
            //glCstCent.CreateChildPermission(AppPermissions.Pages_GLCstCent_Create, L("CreateNewGLCstCent"));
            //glCstCent.CreateChildPermission(AppPermissions.Pages_GLCstCent_Edit, L("EditGLCstCent"));
            //glCstCent.CreateChildPermission(AppPermissions.Pages_GLCstCent_Delete, L("DeleteGLCstCent"));

            //var glbooks = pages.CreateChildPermission(AppPermissions.Pages_GLBOOKS, L("GLBOOKS"));
            //glbooks.CreateChildPermission(AppPermissions.Pages_GLBOOKS_Create, L("CreateNewGLBOOKS"));
            //glbooks.CreateChildPermission(AppPermissions.Pages_GLBOOKS_Edit, L("EditGLBOOKS"));
            //glbooks.CreateChildPermission(AppPermissions.Pages_GLBOOKS_Delete, L("DeleteGLBOOKS"));

            //var glsrce = pages.CreateChildPermission(AppPermissions.Pages_GLSRCE, L("GLSRCE"));
            //glsrce.CreateChildPermission(AppPermissions.Pages_GLSRCE_Create, L("CreateNewGLSRCE"));
            //glsrce.CreateChildPermission(AppPermissions.Pages_GLSRCE_Edit, L("EditGLSRCE"));
            //glsrce.CreateChildPermission(AppPermissions.Pages_GLSRCE_Delete, L("DeleteGLSRCE"));

            pages.CreateChildPermission(AppPermissions.Pages_DemoUiComponents, L("DemoUiComponents"));

            var administration = pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

            var roles = administration.CreateChildPermission(AppPermissions.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Delete, L("DeletingRole"));

            var users = administration.CreateChildPermission(AppPermissions.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Impersonation, L("LoginForUsers"));

            var languages = administration.CreateChildPermission(AppPermissions.Pages_Administration_Languages, L("Languages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Create, L("CreatingNewLanguage"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Edit, L("EditingLanguage"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Delete, L("DeletingLanguages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeTexts, L("ChangingTexts"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs, L("AuditLogs"));

            var organizationUnits = administration.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits, L("OrganizationUnits"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree, L("ManagingOrganizationTree"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers, L("ManagingMembers"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageRoles, L("ManagingRoles"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_UiCustomization, L("VisualSettings"));

            //TENANT-SPECIFIC PERMISSIONS

            pages.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Tenant);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement, L("Subscription"), multiTenancySides: MultiTenancySides.Tenant);

            //HOST-SPECIFIC PERMISSIONS

            var editions = pages.CreateChildPermission(AppPermissions.Pages_Editions, L("Editions"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Create, L("CreatingNewEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Edit, L("EditingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Delete, L("DeletingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_MoveTenantsToAnotherEdition, L("MoveTenantsToAnotherEdition"), multiTenancySides: MultiTenancySides.Host);

            var tenants = pages.CreateChildPermission(AppPermissions.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Impersonation, L("LoginForTenants"), multiTenancySides: MultiTenancySides.Host);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Host);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Maintenance, L("Maintenance"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_HangfireDashboard, L("HangfireDashboard"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Host);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ERPConsts.LocalizationSourceName);
        }
    }
}